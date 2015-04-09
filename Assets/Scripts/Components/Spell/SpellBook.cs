using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SpellBook
{
    public Dictionary<int, Spell> spells = new Dictionary<int, Spell>();
    public int active;

    public void Choose(int num)
    {
        active = num;
    }

    public Spell Get(int number = -1)
    {
        if (number == -1)
            number = active;
        return spells[number];
    }

    public Spell Set(Spell spell, CastComponent owner, int number)
    {
        if (spells.ContainsKey(number))
            Spell.Destroy(spells[number]);
        spells[number] = spell;
        spells[number].castComponent = owner;
        spells[number].Initialise();
        return spell;
    }
}
