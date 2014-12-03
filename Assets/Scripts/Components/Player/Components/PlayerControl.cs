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
    public Animator animator { get; set; }
    public SpriteRenderer spriteRenderer { get; set; }

    public GameObject arrow;
    public UltimateSpell ultimate;
	public string player = "Player 1";
    public AudioClip[] words;
    public AudioClip[] ultimateSounds;

    public static List<PlayerControl> activePlayers;
	
	public StateMachine.Machine<States> sm = new StateMachine.Machine<States> ();
	public enum States
	{
		Move,
		Cast
	}

	// Use this for initialization
	void Start () {
        if (PlayerControl.activePlayers == null)
            PlayerControl.activePlayers = new List<PlayerControl>();
        PlayerControl.activePlayers.Add(this);

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
            castComponent.spellBook.Set(this, ScriptableObject.Instantiate(SpellList.spells[spellNumber]) as Spell, i);
		}
        castComponent.altAimMode = (PlayerPrefs.GetInt(player + " Aim Mode", 0) == 0)?false:true;
        castComponent.reticle_speed = PlayerPrefs.GetFloat(player + " Reticle Speed", 3f);
        castComponent.spellBook.Set(this, ultimate, 4);
        castComponent.spellBook.Set(this, ScriptableObject.Instantiate(SpellList.normalAttack) as Spell, 5);
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
                    ScriptableObject.Destroy(castComponent.spellBook.Get(i));
                    castComponent.spellBook.Set(this, ScriptableObject.Instantiate(SpellList.spells[newSpell]) as Spell, i);
                    PlayerPrefs.Save();
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