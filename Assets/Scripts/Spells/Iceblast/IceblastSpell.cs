using UnityEngine;
using System.Collections;

[System.Serializable]
public class IceblastSpell : Spell
{
    public float damage = 15f;
    public float damageCharged;
    public float speed = 1f;
    public float speedCharged;
    public float size = 1f;
    public float sizeCharged;
	public float t_duration;
    public float t_durationCharged;

    public IceblastProjectile prefab;

    public override void Initialise()
    {
        base.Initialise();

        prefab.CreatePool(10);
    }
	
	public override void Cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
        IceblastProjectile projectile = prefab.Spawn(owner.transform.position + (owner.moveComponent.direction.vector * 1f), Quaternion.identity);
        ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
		projectile.target = reticle;
		projectile.parent= owner.gameObject;
		
		if (charged) 
		{
			projectile.size = sizeCharged;
            projectile.speed = speedCharged;
            projectile.damage = damageCharged;
			projectile.t_duration = t_durationCharged;
		}
		else
		{
			projectile.size = size;
			projectile.speed = speed;
			projectile.damage = damage;
			projectile.t_duration = t_duration;
		}

        projectileHealth.maxHealth = damage;
        projectileHealth.projectileComponent = projectile;
	}
}
