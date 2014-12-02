using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageBar : MonoBehaviour {

    public Image under100;
    public Image over100;

    private float _level;
    public float level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
            if (value < 100)
            {
                over100.fillAmount = 0;
                under100.fillAmount = value/100f;
            }
            else
            {
                over100.fillAmount = (value - 100f)/100f;
                under100.fillAmount = 1f;
            }
        }
    }
}
