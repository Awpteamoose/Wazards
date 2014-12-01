using UnityEngine;
using System.Collections;

public class TeleportSoundProjectile : ProjectileComponent
{
	
	// Update is called once per frame
	public override void Update ()
	{
        if (!audio.isPlaying)
            gameObject.Recycle();
	}
}
