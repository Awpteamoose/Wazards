using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

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
            text.text = value.ToString();
            float green = _ticks / 5f;
            text.color = new Color(2f - green, green, 0);
        }
    }
}
