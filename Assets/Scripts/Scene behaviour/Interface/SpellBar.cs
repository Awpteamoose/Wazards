using UnityEngine;
using System.Collections;

public class SpellBar : MonoBehaviour {

	public SpellIcon[] spellIcons;

	public void ReadSpells(CastComponent castComponent)
	{
		for (int i = 0; i < castComponent.spellBook.spells.Count; i++)
		{
			spellIcons[i].iconSprite.sprite = castComponent.spellBook.Get(i).icon;

			if (castComponent.spellBook.Get(i).cooldown > 0)
			{
				spellIcons[i].cooldownText.text = castComponent.spellBook.Get(i).cooldown.ToString("00.00");
				spellIcons[i].cooldownShadow.fillAmount = castComponent.spellBook.Get(i).cooldown / castComponent.spellBook.Get(i).cooldownDuration;
			}
			else
			{
				spellIcons[i].cooldownText.text = "";
				spellIcons[i].cooldownShadow.fillAmount = 0;
			}

			float manaNeeded = castComponent.spellBook.Get(i).manacost - castComponent.mana;
			if (manaNeeded > 0)
			{
				spellIcons[i].manaShadow.fillAmount = manaNeeded / castComponent.spellBook.Get(i).manacost;
			}
			else
			{
				spellIcons[i].manaShadow.fillAmount = 0;
			}
		}
	}
}
