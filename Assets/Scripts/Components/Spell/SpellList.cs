using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpellList : MonoBehaviour {

	private static List<Spell> spells = new List<Spell>();
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

	public static int Count
	{
		get
		{
			return spells.Count;
		}
	}

	public static Spell Get(int i)
	{
		return spells[i];
	}

	public static Spell Get(string name)
	{
		foreach (Spell spell in spells)
		{
			if (spell.name == name)
				return spell;
		}
		return null;
	}
}
