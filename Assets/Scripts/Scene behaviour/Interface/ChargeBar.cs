using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChargeBar : MonoBehaviour
{

    public Image red;
    public Image yellow;
    public Image green;

    public void GetCharge(Spell spell)
    {
        if (spell.charge < spell.min_chargeDuration)
        {
            red.fillAmount = spell.charge / spell.min_chargeDuration;
        }
        else if (spell.charge < spell.chargeDuration)
        {
            red.fillAmount = 1;
            yellow.fillAmount = (spell.charge - spell.min_chargeDuration) / (spell.chargeDuration - spell.min_chargeDuration);
        }
        else
        {
            green.fillAmount = 1;
        }
    }
    public void Reset()
    {
        red.fillAmount = 0;
        yellow.fillAmount = 0;
        green.fillAmount = 0;
    }
}
