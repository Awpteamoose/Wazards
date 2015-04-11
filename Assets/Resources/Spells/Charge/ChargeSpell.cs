using UnityEngine;
using System.Collections;

[System.Serializable]
public class ChargeSpell : Spell
{
	public float distance;
	public float speed;
	public float damageMod;
	public float size;
	public float minDamage;
	public float maxDamage;
	public float superArmour;

	public ChargeProjectile prefab;

	public override void Initialise()
	{
		base.Initialise();

		if (prefab.CountPooled() == 0)
			prefab.CreatePool(3);
	}

	public override void Cast(float charge, Vector3 reticle)
	{
		ChargeProjectile projectile = prefab.Spawn(castComponent.owner.transform.position, Quaternion.identity);
		ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
		float _distance;
		if (charge >= chargeDuration)
		{
			_distance = (reticle - castComponent.owner.transform.position).magnitude;
		}
		else
		{
			_distance = distance;
		}
		projectile.target = reticle;
		projectile.parent= castComponent.gameObject;
		projectile.speed = speed * castComponent.mod_speed;
		projectile.t_terminate = Time.time + _distance / (1f / Time.fixedDeltaTime * projectile.speed);
		projectile.damageMod = damageMod * castComponent.mod_damage;
		projectile.minDamage = minDamage * castComponent.mod_damage;
		projectile.maxDamage = maxDamage * castComponent.mod_damage;
		projectile.size = size;

		projectileHealth.maxHealth = superArmour;
		projectileHealth.projectileComponent = projectile;

		projectile.Activate();
		base.Cast(charge, reticle);
	}
}
