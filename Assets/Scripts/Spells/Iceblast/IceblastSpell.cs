using UnityEngine;
using System.Collections;

[System.Serializable]
public class IceblastSpell : Spell
{
	public float damage;
	public float damageCharged;
	public float speed;
	public float speedCharged;
	public float size;
	public float sizeCharged;
	public float t_duration;
	public float t_durationCharged;

	public IceblastProjectile prefab;

	public override void Initialise()
	{
		base.Initialise();

		if (prefab.CountPooled() == 0)
			prefab.CreatePool(10);
	}
	
	public override void Cast(float charge, Vector3 reticle)
	{
		IceblastProjectile projectile = prefab.Spawn(castComponent.owner.transform.position + (castComponent.owner.moveComponent.direction.vector * 1f), Quaternion.identity);
		ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
		projectile.target = reticle;
		projectile.parent = castComponent.gameObject;

		if (charge >= chargeDuration)
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
		projectile.size *= castComponent.owner.castComponent.mod_size;
		projectile.speed *= castComponent.owner.castComponent.mod_speed;
		projectile.damage *= castComponent.owner.castComponent.mod_damage;

		projectileHealth.maxHealth = damage;
		projectileHealth.projectileComponent = projectile;

		projectile.Activate();
		base.Cast(charge, reticle);
	}
}
