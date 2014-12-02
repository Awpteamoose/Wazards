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

	public override void Cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
		if (charged) 
			owner.transform.position = reticle;
		else
			owner.transform.position =(owner.transform.position + (reticle - owner.transform.position).normalized*minDistance);
        prefab.Spawn();
        prefab.Activate();
	}
}
