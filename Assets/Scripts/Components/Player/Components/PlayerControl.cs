﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerControl: MonoBehaviour
{
	public MoveComponent moveComponent {get; set;}
	public CastComponent castComponent {get; set;}
	public PlayerHealthComponent healthComponent {get; set;}
	public PlayerInputComponent inputComponent {get; set;}
    public Animator animator { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }

    public GameObject arrow;
    public Spell ultimate;
	public string player = "Player 1";
    public AudioClip[] words;
    public AudioClip[] ultimateSounds;
	
	public StateMachine.Machine<States> sm = new StateMachine.Machine<States> ();
	public enum States
	{
		Move,
		Cast
	}

    private bool chanting;
    private bool ultiChant;
    private bool ultiCharged;
    private int lastRandom;
    public void StartChant()
    {
        Spell s_active = castComponent.spellBook.Get();
        if (!(s_active is NormalAttackSpell))
        {
            if (s_active is UltimateSpell)
            {
                audio.clip = ultimateSounds[0];
                audio.Play();
                ultiChant = true;
                ultiCharged = false;
            }
            else
            {
                Random.seed = castComponent.spellBook.Get().spellName.GetHashCode();
                lastRandom = Random.Range(-214748364, 214748364);
                Random.seed = Mathf.RoundToInt(Time.time * 100f);
                chanting = true;
            }
        }
    }

    public void StopChant()
    {
        if (ultiChant)
        {
            audio.clip = ultimateSounds[2];
            audio.time = 0;
            audio.Play();
            ultiChant = false;
        }
        else
        {
            audio.Stop();
            chanting = false;
        }
    }

	// Use this for initialization
	void Start () {
        chanting = false;
        ultiChant = false;

		moveComponent = GetComponent<MoveComponent>();
		castComponent = GetComponent<CastComponent>();
		healthComponent = GetComponent<PlayerHealthComponent>();
		inputComponent = GetComponent<PlayerInputComponent>();
		inputComponent.playername = player;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

		for (int i = 0; i<4; i++)
		{
            //PlayerPrefs.DeleteAll();
            string spellName = player + " SelectedSpell " + i.ToString();
            int spellNumber = PlayerPrefs.GetInt(spellName, -1);
            if (spellNumber == -1 || spellNumber >= SpellList.spells.Count)
            {
                spellNumber = Random.Range(0, SpellList.spells.Count);
                PlayerPrefs.SetInt(spellName, spellNumber);
            }
            castComponent.spellBook.Set(this, SpellList.spells[spellNumber], i);
		}
        castComponent.altAimMode = (PlayerPrefs.GetInt(player + " Aim Mode", 0) == 0)?false:true;
        castComponent.reticle_speed = PlayerPrefs.GetFloat(player + " Reticle Speed", 3f);
        castComponent.spellBook.Set (this, ultimate, 4);
		castComponent.spellBook.Set (this, SpellList.normalAttack, 5);
        PlayerPrefs.Save();

		sm.states.Add(States.Move,new PlayerMoveState(this));
		sm.states.Add(States.Cast,new PlayerCastState(this));

		sm.set (sm.states [States.Move]);
	}
	
	void Update()
	{
        if (!PauseMenu.paused)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Input.GetButtonDown(player + " Switch Spell " + (i + 1)))
                {
                    string spellName = player + " SelectedSpell " + i.ToString();
                    int newSpell = PlayerPrefs.GetInt(spellName) + 1;
                    if (newSpell >= SpellList.spells.Count)
                        newSpell = 0;
                    PlayerPrefs.SetInt(spellName, newSpell);
                    castComponent.spellBook.Set(this, SpellList.spells[newSpell], i);
                    PlayerPrefs.Save();
                }
            }

            if (chanting)
            {
                if (!audio.isPlaying)
                {
                    Random.seed = lastRandom;
                    lastRandom = Random.Range(-214748364, 214748364);
                    audio.clip = words[Random.Range(0, words.Length)];
                    Random.seed = Mathf.RoundToInt(Time.time * 100f);
                    audio.Play();
                }
            }

            if (ultiChant && castComponent.t_charged > castComponent.spellBook.Get().t_charge && !ultiCharged)
            {
                audio.clip = ultimateSounds[1];
                audio.time = 0;
                audio.Play();
                ultiCharged = true;
            }
            sm.Update();
        }
	}

    void OnBecameInvisible()
    {
        arrow.SetActive(true);
    }

    void OnBecameVisible()
    {
        arrow.SetActive(false);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
		sm.FixedUpdate ();
	}

	void OnGUI ()
	{
		sm.OnGUI ();
	}
}