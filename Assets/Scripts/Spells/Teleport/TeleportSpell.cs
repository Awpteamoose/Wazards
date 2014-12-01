using UnityEngine;
using System.Collections;

[System.Serializable]
public class TeleportSpell : Spell
{
	public float minDistance = 2f;

    public TeleportSoundProjectile prefab;

    public override void Initialise()
    {
        base.Initialise();

        prefab.CreatePool(1);
    }

	public override void Cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
		if (charged) 
			owner.transform.position = reticle;
		else
			owner.transform.position =(owner.transform.position + (reticle - owner.transform.position).normalized*minDistance);
        prefab.Spawn();
	}
}
