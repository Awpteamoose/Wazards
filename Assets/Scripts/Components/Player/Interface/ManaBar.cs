using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaBar : MonoBehaviour {

    public Image filler;

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
            filler.fillAmount = value / 100f;
        }
    }
}
