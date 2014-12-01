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

    public FireballProjectile prefab;

    public override void Initialise()
    {
        base.Initialise();

        prefab.CreatePool(10);
    }
	
	public override void Cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
        FireballProjectile projectile = prefab.Spawn(owner.transform.position + (owner.moveComponent.direction.vector * 1f), Quaternion.identity);
        ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
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


        projectileHealth.maxHealth = projectile.damage;
        projectileHealth.projectileComponent = projectile;

        projectile.Activate();
	}
}
