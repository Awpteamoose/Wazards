using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ButterflySpell : Spell
{
	public float damage;
	public float speed;
	public float size;
	public int amount;
	public int angleEdge;
	public int chargedAmount;
	public int chargedAngleEdge;
	public float t_delayMax;

	public ButterflyProjectile prefab;

	public override void Initialise ()
	{
		base.Initialise();
		if (prefab.CountPooled() == 0)
			prefab.CreatePool(100);
	}
	
	public override void Cast(float charge, Vector3 reticle)
	{	
		int angle;
		int amt;
		if (charge >= chargeDuration)
		{
			angle = chargedAngleEdge;
			amt = chargedAmount;
		}
		else
		{
			angle = angleEdge;
			amt = amount;
		}
		
		int step;
		if (amt > 2)
			step = angle/((amt-1)/2);
		else
		{
			step = 1;
			angle = 0;
		}
		for ( int i = -angle; i <= angle; i+=step )
		{
			ButterflyProjectile projectile = prefab.Spawn();
			ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
			
			projectile.shift=i+Random.Range (-10f, 10f);
			
			projectile.target = reticle;
			projectile.parent = castComponent.gameObject;
			projectile.size = size * castComponent.mod_size;
			projectile.speed = speed * Random.Range(0.3f, 1.5f) * castComponent.mod_speed;
			projectile.damage = damage * castComponent.mod_damage;
			projectileHealth.maxHealth = damage;
			projectileHealth.projectileComponent = projectile;

			if (castComponent.owner.player == "Player 1")
				projectile.color = new Color(0.37f, 0.47f, 1f);
			else
				projectile.color = new Color(1f, 0.97f, 0.3f);

			projectile.renderer.enabled = false;
			projectile.collider.enabled = false;
			projectile.t_activation = Time.time + Random.Range(0, t_delayMax);

			projectile.Activate();
		}
		base.Cast(charge, reticle);
	}
}