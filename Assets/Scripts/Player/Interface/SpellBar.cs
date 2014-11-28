using UnityEngine;
using System.Collections;

public class SpellBar : MonoBehaviour {

    public SpellIcon[] spellIcons;

	public void ReadSpells(CastComponent castComponent)
    {
        for (int i = 0; i < castComponent.spellBook.spells.Length; i++)
        {
            spellIcons[i].iconSprite.sprite = castComponent.spellBook.get(i).icon;

            float cooldownTime = ((castComponent.cooldowns[i] + castComponent.spellBook.get(i).secondsCooldown) - Time.time);
            if (cooldownTime > 0)
            {
                spellIcons[i].cooldownText.text = cooldownTime.ToString("00.00");
                spellIcons[i].cooldownShadow.fillAmount = cooldownTime / castComponent.spellBook.get(i).secondsCooldown;
            }
            else
            {
                spellIcons[i].cooldownText.text = "";
            }

            float manaNeeded = castComponent.spellBook.get(i).manacost - castComponent.mana;
            if (manaNeeded > 0)
            {
                spellIcons[i].manaShadow.fillAmount = manaNeeded / castComponent.spellBook.get(i).manacost;
            }
        }
    }
}
