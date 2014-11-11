/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ButterflySpell : Spell
{
	public float damage = 75f;
	public float speed = 0.3f;
	public float size = 2f;
	public int chargedAmount = 5;
	public int chargedAngleEdge = 20;
	public Transform prefab;
	
	public override void cast(bool charged, Vector3 reticle, PlayerControl owner)
	{	
		if (charged) 
		{
			int step;
			if (chargedAmount > 2)
				step = chargedAngleEdge/((chargedAmount)/2);
			else
			{
				step = 1;
				chargedAngleEdge = 0;
			}
			for ( int i = -chargedAngleEdge; i < chargedAngleEdge; i+=step )
			{
				Transform butterfly = (Instantiate(prefab, reticle + (Quaternion.Euler(0, 0, i) * Vector3.right).normalized, Quaternion.identity) as Transform);
				ButterflyProjectile projectile = butterfly.GetComponent<ButterflyProjectile>();

				projectile.shift = 180;
				Debug.Log ((Quaternion.Euler(0, 0, i) * Vector3.right).normalized);
				projectile.target = reticle;
				projectile.parent = projectile.gameObject;
				projectile.size = size;
				projectile.speed = speed*0.75f+Random.Range(-0.08f, 0.08f);
				projectile.damage = damage*0.35f;
				
				projectile.startPosition = reticle;
			}
		}
		else
		{
			Transform butterfly = (Instantiate(prefab, owner.transform.position+(owner.moveComponent.direction*0.3f), Quaternion.identity) as Transform);
			ButterflyProjectile projectile = butterfly.GetComponent<ButterflyProjectile>();
			
			projectile.target = reticle;
			projectile.parent= owner.gameObject;
			projectile.size = size;
			projectile.speed = speed;
			projectile.damage = damage;
			
			projectile.startPosition = owner.transform.position;
		}
	}
}*/
