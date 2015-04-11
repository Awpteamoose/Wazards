using UnityEngine;
using System.Collections;

[System.Serializable]
public class TeleportSpell : Spell
{
	public float minDistance;

    public TeleportSoundProjectile prefab;

    public override void Initialise()
    {
        base.Initialise();

        if (prefab.CountPooled() == 0)
            prefab.CreatePool(2);
    }

	public override void Cast(float charge, Vector3 reticle)
	{
		if (charge >= chargeDuration)
			castComponent.transform.position = reticle;
		else
			castComponent.transform.position =(castComponent.transform.position + (reticle - castComponent.transform.position).normalized*minDistance);
        prefab.Spawn();
        prefab.Activate();
        base.Cast(charge, reticle);
	}
}
