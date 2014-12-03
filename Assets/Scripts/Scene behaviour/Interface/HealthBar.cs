using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    private int _ticks;
    public int ticks
    {
        get
        {
            return _ticks;
        }
        set
        {
            _ticks = value;
            if (value < transform.childCount)
            {
                for (int i = value; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }
}
