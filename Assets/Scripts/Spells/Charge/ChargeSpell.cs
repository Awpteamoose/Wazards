﻿using UnityEngine;
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

	private float _distance;

    public ChargeProjectile prefab;

    public override void Initialise()
    {
        base.Initialise();

        if (prefab.CountPooled() == 0)
            prefab.CreatePool(3);
    }

	public override void Cast(float charge, Vector3 reticle)
	{
        ChargeProjectile projectile = prefab.Spawn(owner.transform.position, Quaternion.identity);
        ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
		float _distance;
		if (charge >= t_charge)
		{
			_distance = (reticle - owner.transform.position).magnitude;
		}
		else
		{
			_distance = distance;
		}
		projectile.target = reticle;
		projectile.parent= owner.gameObject;
		projectile.speed = speed * owner.moveComponent.mod_speed;
        projectile.t_terminate = Time.time + _distance / (1f / Time.fixedDeltaTime * projectile.speed);
        projectile.damageMod = damageMod * owner.castComponent.mod_damage;
        projectile.minDamage = minDamage * owner.castComponent.mod_damage;
        projectile.maxDamage = maxDamage * owner.castComponent.mod_damage;
		projectile.size = size;

        projectileHealth.maxHealth = superArmour;
        projectileHealth.projectileComponent = projectile;

        projectile.Activate();
        base.Cast(charge, reticle);
	}
}
