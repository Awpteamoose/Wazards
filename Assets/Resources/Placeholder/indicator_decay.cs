using UnityEngine;
using System.Collections;

public class indicator_decay : MonoBehaviour {

	TypogenicText typo;

	void Start()
	{
		typo = GetComponent<TypogenicText> ();
	}

	// Update is called once per frame
	void Update () {
		//Color color = renderer.material.GetColor ("_GlobalMultiplierColor");
		//color.a -= 1f*Time.deltaTime;
		//Debug.Log (color.a);
		//renderer.material.SetColor ("_GlobalMultiplierColor", color);
		//if (color.a <= 0f)
		//	Destroy (gameObject);

		typo.ColorTopLeft.a -= 1f * Time.deltaTime;
		renderer.material.SetColor ("_GlowColor", new Color (1f, 1f, 1f, typo.ColorTopLeft.a));
		if (typo.ColorTopLeft.a <= 0f)
			Destroy (this.gameObject);
	}
}
