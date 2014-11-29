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

	private float _distance;

	public override void cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
		Transform charge = Instantiate(prefab, owner.transform.position, Quaternion.identity) as Transform;
		ChargeProjectile projectile = charge.GetComponent<ChargeProjectile>();
		float _distance;
		if (charged)
		{
			_distance = (reticle - owner.transform.position).magnitude;
		}
		else
		{
			_distance = distance;
		}
		projectile.t_terminate = Time.time + _distance/(1f/Time.fixedDeltaTime*speed);
		projectile.target = reticle;
		projectile.parent= owner.gameObject;
		projectile.speed = speed;
        projectile.damageMod = damageMod;
        projectile.minDamage = minDamage;
        projectile.maxDamage = maxDamage;
		projectile.size = size;

        projectile.GetComponent<ProjectileHealthComponent>().maxHealth = superArmour;
	}
}
