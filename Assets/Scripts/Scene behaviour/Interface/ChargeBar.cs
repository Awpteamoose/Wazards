using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChargeBar : MonoBehaviour
{

    public Image image { get; set; }

    void Awake()
    {
        image = GetComponent<Image>();
    }

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
            image.fillAmount = value;
        }
    }
}
