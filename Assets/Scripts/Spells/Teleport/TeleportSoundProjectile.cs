using UnityEngine;
using System.Collections;

public class TeleportSoundProjectile : ProjectileComponent {

	// Use this for initialization
	public override void Start ()
	{
		
	}
	
	// Update is called once per frame
	public override void Update ()
	{
		if (!audio.isPlaying)
			Destroy(gameObject);
	}
}
