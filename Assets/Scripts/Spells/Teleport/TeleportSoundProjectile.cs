﻿using UnityEngine;
using System.Collections;

public class TeleportSoundProjectile : ProjectileComponent
{
    public override void Activate()
    {
    }

	// Update is called once per frame
	public override void Update ()
	{
        if (!GetComponent<AudioSource>().isPlaying)
            gameObject.Recycle();
	}
}
