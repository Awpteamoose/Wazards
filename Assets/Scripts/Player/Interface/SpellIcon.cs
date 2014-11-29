using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpellIcon : MonoBehaviour {

    public Image iconSprite { get; set; }
    public Image cooldownShadow;
    public Text cooldownText;
    public Image manaShadow;

    private void Awake()
    {
        iconSprite = GetComponent<Image>();
    }
}
