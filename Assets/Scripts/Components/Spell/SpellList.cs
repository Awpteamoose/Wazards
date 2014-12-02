using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellList : MonoBehaviour {

	public static List<Spell> spells = new List<Spell>();
	public static Spell normalAttack;

	void Awake ()
	{
		Object[] _spells = Resources.LoadAll("Spells");
		foreach (Object spell in _spells)
		{
			if (spell is Spell)
			{
				if (spell is NormalAttackSpell)
					normalAttack = (Spell)spell;
				else
					spells.Add((Spell)spell);
			}
		}
	}
}
