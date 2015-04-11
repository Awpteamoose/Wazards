using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class CastComponent: MonoBehaviour
{
	public GameObject reticle { get; set; }
	public GameObject bgBar { get; set; }
	public GameObject fgBar { get; set; }
	public GameObject pcBar { get; set; }
	public PlayerControl owner { get; set; }
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

	public void RecordSpell(Spell spell, int index)
	{
		spellBook.Set(ScriptableObject.Instantiate(spell) as Spell, this, index);
	}

	private void Update()
	{
		for (int i = 0; i < spellBook.spells.Count; i++ )
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