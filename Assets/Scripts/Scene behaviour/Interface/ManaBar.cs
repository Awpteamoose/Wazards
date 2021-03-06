﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaBar : MonoBehaviour {

    public Image image { get; set; }
    public Text text;

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
            image.fillAmount = value / 100f;
            text.text = Mathf.RoundToInt(value).ToString() + "/100";
        }
    }
}
