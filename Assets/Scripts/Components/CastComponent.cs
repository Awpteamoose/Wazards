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

	public class SpellBook
	{
		public Spell[] spells = new Spell[4];
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
		public Spell Set(PlayerControl owner, Spell spell, int number)
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
            spellBook.Get(i).Update();
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
}