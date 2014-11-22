using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerControl: MonoBehaviour
{
	public MoveComponent moveComponent {get; set;}
	public CastComponent castComponent {get; set;}
	public HealthComponent healthComponent {get; set;}
	public PlayerInputComponent inputComponent {get; set;}
    public Animator animator {get; set;}

	private GameObject fgManaBar {get; set;}
	private GameObject bgManaBar {get; set;}
	public GameObject bgHealthBar {get; set;}
	public GameObject fgHealthBar {get; set;}

    public GameObject arrow;
    public SpriteRenderer spriteRenderer;

	public List<GameObject> spellIcons;
	public List<GameObject> spellIconShadows;
	public List<GameObject> spellIconManaShadows;

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


		//HACK:
		bgManaBar = GameObject.Find(player+" Background Mana Bar");
		fgManaBar = GameObject.Find(player+" Foreground Mana Bar");
		bgHealthBar = GameObject.Find(player+" Background Health Bar");
		fgHealthBar = GameObject.Find(player+" Foreground Health Bar");

		bgManaBar.guiTexture.pixelInset = new Rect(0,0,Screen.width/2.5f/Camera.main.aspect,Screen.height/40f);
		fgManaBar.guiTexture.pixelInset = bgManaBar.guiTexture.pixelInset;
		
		bgHealthBar.guiTexture.pixelInset = new Rect(0,Screen.height/38f,Screen.width/2.5f/Camera.main.aspect,Screen.height/40f);
		fgHealthBar.guiTexture.pixelInset = bgHealthBar.guiTexture.pixelInset;

		for (int i = 0; i<3; i++)
		{
			spellIcons.Add(GameObject.Find(player+" Spell "+(i+1)+" icon"));
			spellIconShadows.Add(GameObject.Find(player+" Spell "+(i+1)+" icon Shadow"));
			spellIconManaShadows.Add(GameObject.Find(player+" Spell "+(i+1)+" icon ManaShadow"));

			spellIcons[i].guiTexture.texture = castComponent.spellBook.get(i).icon;
			spellIcons[i].guiTexture.pixelInset = new Rect((Screen.width/6.3f/Camera.main.aspect)*i, Screen.height/18f, Screen.width/12f/Camera.main.aspect, Screen.height/12f);

			spellIconShadows[i].guiTexture.pixelInset = new Rect(spellIcons[i].guiTexture.pixelInset.x, spellIcons[i].guiTexture.pixelInset.y, spellIcons[i].guiTexture.pixelInset.width, 0);
			spellIconManaShadows[i].guiTexture.pixelInset = spellIcons[i].guiTexture.pixelInset;
			spellIconManaShadows[i].SetActive(false);
		}
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
                    spellIcons[i].guiTexture.texture = castComponent.spellBook.get(i).icon;
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
		fgManaBar.guiTexture.pixelInset = new Rect(0, 0, bgManaBar.guiTexture.pixelInset.width * castComponent.mana/castComponent.maxMana, bgManaBar.guiTexture.pixelInset.height);
		fgHealthBar.guiTexture.pixelInset = new Rect(0, bgHealthBar.guiTexture.pixelInset.y, bgHealthBar.guiTexture.pixelInset.width * healthComponent.health/healthComponent.maxHealth, bgHealthBar.guiTexture.pixelInset.height);
		for (int i = 0; i<3; i++)
		{
			if (!castComponent.enough_mana(i))
			{
				spellIconManaShadows[i].SetActive(true);
			}
			else if (spellIconManaShadows[i].activeSelf)
			{
				spellIconManaShadows[i].SetActive(false);
			}

			float height;
			if (!castComponent.is_cooldown(i))
			{
				height = ((castComponent.cooldowns[i]+castComponent.spellBook.get(i).secondsCooldown) - Time.time)/castComponent.spellBook.get(i).secondsCooldown;
			}
			else
				height = 0;
			spellIconShadows[i].guiTexture.pixelInset = new Rect(spellIcons[i].guiTexture.pixelInset.x, spellIcons[i].guiTexture.pixelInset.y, spellIcons[i].guiTexture.pixelInset.width, spellIcons[i].guiTexture.pixelInset.height*height);
		}
	}

	void OnGUI ()
	{
		sm.OnGUI ();
	}
}