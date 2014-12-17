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
        if (spell.t_charged < spell.t_minCharge)
        {
            red.fillAmount = spell.t_charged / spell.t_minCharge;
        }
        else if (spell.t_charged < spell.t_charge)
        {
            red.fillAmount = 1;
            yellow.fillAmount = (spell.t_charged - spell.t_minCharge) / (spell.t_charge - spell.t_minCharge);
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
