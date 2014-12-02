using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CastComponent: MonoBehaviour
{
	public GameObject reticle {get; set;}
	public GameObject bgBar {get; set;}
	public GameObject fgBar {get; set;}
	public GameObject pcBar {get; set;}
	public float reticle_distance = 2f;
	public float reticle_minimumDistance = 1f;
	public float reticle_speed;

	public float maxMana;
    #if UNITY_EDITOR
	[ReadOnly]
	#endif
	public float mana;

    public float manaRegen;
    #if UNITY_EDITOR
	[ReadOnly]
	#endif
	public float currentManaRegen;
    public bool altAimMode;

	public SpellBook spellBook = new SpellBook();

	#if UNITY_EDITOR
	[ReadOnly]
	#endif
	public List<float> cooldowns = Enumerable.Repeat(-1000f, 4).ToList();

	public class SpellBook
	{
		public Spell[] spells = new Spell[4];
		public int active;
		public void choose(int num)
		{
			active = num;
		}
		public Spell get(int number=-1)
		{
			if (number == -1)
				number = active;
			return spells[number];
		}
		public Spell set(PlayerControl owner, Spell spell, int number)
		{
            spells[number] = Object.Instantiate(spell) as Spell;
            spells[number].owner = owner;
            spells[number].Initialise();
			return spell;
		}
	}

	void Update()
	{
        for (int i = 0; i < spellBook.spells.Length; i++ )
        {
            spellBook.get(i).Update();
        }

        if (rigidbody2D.velocity.magnitude < 3f)
        {
            currentManaRegen = manaRegen * 3f;
        }
        else
        {
            currentManaRegen = manaRegen;
        }
        mana += currentManaRegen * Time.deltaTime;
        mana = Mathf.Clamp(mana, 0, maxMana);
	}

	private void Start()
	{
		bgBar = transform.Find("Background Cast Bar").gameObject;
		fgBar = transform.Find("Foreground Cast Bar").gameObject;
		pcBar = transform.Find("Partial Charge Cast Bar").gameObject;
	}
	
	public bool is_cooldown(int number=-1)
	{
		if (number == -1)
			number=spellBook.active;
		if (Time.time >= cooldowns[number] + spellBook.get(number).secondsCooldown)
			return true;
		else
			return false;
	}
	public bool enough_mana(int number=-1)
	{
		if (number == -1)
			number=spellBook.active;
		if (mana >= spellBook.get(number).manacost)
			return true;
		else
			return false;
	}
	public bool can_cast(int number=-1)
	{
		if (number == -1)
			number=spellBook.active;
		if (is_cooldown (number) && enough_mana (number))
			return true;
		else
			return false;
	}
	public void Cast(bool charged, Vector3 reticle, int number=-1)
	{
		if (number == -1)
			number=spellBook.active;
        cooldowns[spellBook.active] = Time.time;
        mana -= spellBook.get().manacost;
		spellBook.get().Cast(charged, reticle);
	}
}