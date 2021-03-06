﻿using UnityEngine;
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
	public float reticle_distance;
	public float reticle_minimumDistance;
	public float reticle_speed;

    public float mod_damage = 1f;
    public float mod_size = 1f;
    public float mod_speed = 1f;
    public float mod_cooldown = 1f;
    public float mod_charge = 1f;
    public float mod_manacost = 1f;
    public float mod_regen = 1f;

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

	public class SpellBook
	{
		public Spell[] spells = new Spell[6];
		public int active;
		public void Choose(int num)
		{
			active = num;
		}
		public Spell Get(int number=-1)
		{
			if (number == -1)
				number = active;
			return spells[number];
		}
		public Spell Set(Spell spell, PlayerControl owner, int number)
		{
            if (spells[number] != null)
                Spell.Destroy(spells[number]);
            spells[number] = spell;
            spells[number].owner = owner;
            spells[number].Initialise();
			return spell;
		}
        public Spell Set(int listIndex, PlayerControl owner, int number)
        {
            return Set(ScriptableObject.Instantiate(SpellList.Get(listIndex)) as Spell, owner, number);
        }
        public void Activate(Spell spell)
        {
            spell.Activate();
        }
        public void Activate(int number)
        {
            spells[number].Activate();
        }
        public void ActivateAll()
        {
            foreach (Spell spell in spells)
            {
                Activate(spell);
            }
        }
	}

	void Update()
	{
        for (int i = 0; i < spellBook.spells.Length; i++ )
        {
            spellBook.Get(i).Update();
        }

        currentManaRegen = manaRegen;
        if (GetComponent<Rigidbody2D>().velocity.magnitude < 3f)
            currentManaRegen *= 3f;
        mana += currentManaRegen * mod_regen * Time.deltaTime;
        mana = Mathf.Clamp(mana, 0, maxMana);
	}

	private void Start()
	{
		bgBar = transform.Find("Background Cast Bar").gameObject;
		fgBar = transform.Find("Foreground Cast Bar").gameObject;
		pcBar = transform.Find("Partial Charge Cast Bar").gameObject;
	}
}