using UnityEngine;
using System.Collections;

[System.Serializable]
public class TeleportSpell : Spell
{
	public float minDistance = 2f;

	public override void cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
		//owner.collider2D.enabled = false;
		if (charged) 
			//owner.rigidbody2D.MovePosition(reticle);
			owner.transform.position = reticle;
		else
			//owner.rigidbody2D.MovePosition(owner.transform.position + (owner.moveComponent.direction * minDistance));
			//owner.transform.position = (owner.transform.position + (owner.moveComponent.direction * minDistance));
			owner.transform.position =(owner.transform.position + (reticle - owner.transform.position).normalized*minDistance);
		//owner.collider2D.enabled = true;
		Instantiate(prefab);
	}
}
