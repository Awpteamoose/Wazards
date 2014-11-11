using UnityEngine;
using System.Collections;

[System.Serializable]
public class FireballSpell : Spell
{
    public float damage = 15f;
    public float damageCharged;
    public float speed = 1f;
    public float speedCharged;
	public float size = 1f;
    public float sizeCharged;
	
	public override void cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
		Transform fireball = Instantiate(prefab, owner.transform.position+(owner.moveComponent.direction.vector*1f), Quaternion.identity) as Transform;
		FireballProjectile projectile = fireball.GetComponent<FireballProjectile>();
		projectile.target = reticle;
		projectile.parent= owner.gameObject;
		
		if (charged) 
		{
			projectile.size = sizeCharged;
            projectile.speed = speedCharged;
            projectile.damage = damageCharged;
		}
		else
		{
			projectile.size = size;
			projectile.speed = speed;
			projectile.damage = damage;
		}
	}
}
