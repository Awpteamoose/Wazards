﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class FireballSpell : Spell
{
	public float damage;
	public float damageCharged;
	public float speed;
	public float speedCharged;
	public float size;
	public float sizeCharged;

	public FireballProjectile prefab;

	public override void Initialise()
	{
		base.Initialise();

		if (prefab.CountPooled() == 0)
			prefab.CreatePool(10);
	}
	
	public override void Cast(float charge, Vector3 reticle)
	{
		FireballProjectile projectile = prefab.Spawn(castComponent.transform.position + (castComponent.owner.moveComponent.direction.vector * 1f), Quaternion.identity);
		ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
		projectile.target = reticle;
		projectile.parent= castComponent.gameObject;
		
		if (charge >= chargeDuration)
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
		projectile.size *= castComponent.mod_size;
		projectile.speed *= castComponent.mod_speed;
		projectile.damage *= castComponent.mod_damage;

		projectileHealth.maxHealth = projectile.damage;
		projectileHealth.projectileComponent = projectile;

		projectile.Activate();
		base.Cast(charge, reticle);
	}
}
