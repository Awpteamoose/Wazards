using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerControl: MonoBehaviour
{
	public MoveComponent moveComponent {get; set;}
	public CastComponent castComponent {get; set;}
	public PlayerHealthComponent healthComponent {get; set;}
	public PlayerInputComponent inputComponent {get; set;}
    public Animator animator {get; set;}

    public GameObject arrow;
    public SpriteRenderer spriteRenderer;

	public string player = "Player 1";

    public AudioClip[] words;
	
	public StateMachine.Machine<States> sm = new StateMachine.Machine<States> ();
	public enum States
	{
		Move,
		Cast
	}

    private bool stopChant = false;
    private bool chanting = false;
    private int lastRandom;
    public void StartChant()
    {
        if (castComponent.spellBook.active != 3)
        {
            Random.seed = castComponent.spellBook.get().spellName.GetHashCode();
            lastRandom = Random.Range(-214748364, 214748364);
            Random.seed = Mathf.RoundToInt(Time.time*100f);
            chanting = true;
            stopChant = false;
        }
    }

    public void StopChant()
    {
        stopChant = true;
    }

	// Use this for initialization
	void Start () {
		moveComponent = GetComponent<MoveComponent>();
		castComponent = GetComponent<CastComponent>();
		healthComponent = GetComponent<PlayerHealthComponent>();
		inputComponent = GetComponent<PlayerInputComponent>();
		inputComponent.playername = player;
        animator = GetComponent<Animator>();

		for (int i = 0; i<3; i++)
		{
            //PlayerPrefs.DeleteAll();
            string spellName = player + " SelectedSpell " + i.ToString();
            int spellNumber = PlayerPrefs.GetInt(spellName, -1);
            if (spellNumber == -1 || spellNumber >= SpellList.spells.Count)
            {
                spellNumber = Random.Range(0, SpellList.spells.Count);
                PlayerPrefs.SetInt(spellName, spellNumber);
            }
            castComponent.spellBook.set(SpellList.spells[spellNumber], i);
		}
        castComponent.altAimMode = (PlayerPrefs.GetInt(player + " Aim Mode", 0) == 0)?false:true;
        castComponent.reticle_speed = PlayerPrefs.GetFloat(player + " Reticle Speed", 3f);
		castComponent.spellBook.set (SpellList.normalAttack, 3);
        PlayerPrefs.Save();

		sm.states.Add(States.Move,new PlayerMoveState(this));
		sm.states.Add(States.Cast,new PlayerCastState(this));

		sm.set (sm.states [States.Move]);
	}
	
	void Update()
	{
        if (!PauseMenu.paused)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Input.GetButtonDown(player + " Switch Spell " + (i + 1)))
                {
                    string spellName = player + " SelectedSpell " + i.ToString();
                    int newSpell = PlayerPrefs.GetInt(spellName) + 1;
                    if (newSpell >= SpellList.spells.Count)
                        newSpell = 0;
                    PlayerPrefs.SetInt(spellName, newSpell);
                    castComponent.spellBook.set(SpellList.spells[newSpell], i);
                    PlayerPrefs.Save();
                }
            }

            if (chanting)
            {
                if (!audio.isPlaying)
                {
                    if (stopChant)
                    {
                        audio.Stop();
                    }
                    else
                    {
                        Random.seed = lastRandom;
                        lastRandom = Random.Range(-214748364, 214748364);
                        audio.clip = words[Random.Range(0, words.Length)];
                        Random.seed = Mathf.RoundToInt(Time.time * 100f);
                        audio.Play();
                    }
                }
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