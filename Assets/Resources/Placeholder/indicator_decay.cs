using UnityEngine;
using System.Collections;

public class indicator_decay : MonoBehaviour {

	private TypogenicText typo;
    private float startSize;

	void Awake()
	{
		typo = GetComponent<TypogenicText> ();
        startSize = typo.Size;
	}

    void OnEnable()
    {
        typo.ColorTopLeft.a = 1f;
        typo.Size = startSize;
    }

	// Update is called once per frame
	void Update () {

		typo.ColorTopLeft.a -= 1f * Time.deltaTime;
		renderer.material.SetColor ("_GlowColor", new Color (1f, 1f, 1f, typo.ColorTopLeft.a));
        if (typo.ColorTopLeft.a <= 0f)
            gameObject.Recycle();
	}
}
