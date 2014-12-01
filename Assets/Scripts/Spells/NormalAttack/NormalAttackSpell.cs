﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class NormalAttackSpell : Spell
{
    public float damage = 200f;
    public float damageCharged;
	public float delay = 0.2f;
	public float duration = 1f;
	public float size = 1f;

    public NormalAttackObject prefab;

    public override void Initialise()
    {
        base.Initialise();

        prefab.CreatePool(1);
    }
	
	public override void Cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
        NormalAttackObject attack = prefab.Spawn();

		if (charged)
		{
			attack.destroyProjectiles = true;
			attack.damage = damageCharged;
			attack.t_activation = Time.time + delay/2f;
			secondsCooldown = 0.1f;
		}
		else
		{
			attack.damage = damage;
			attack.t_activation = Time.time + delay;
			secondsCooldown = 0.5f;
		}

        attack.parent = owner.gameObject;
		attack.transform.parent = owner.transform;
		attack.transform.localPosition = new Vector3(0, 0.5f, 0);
		attack.transform.localRotation = Quaternion.identity;
		attack.t_deactivation = attack.t_activation + duration;
		attack.target = reticle;
		attack.size = size;

        attack.Activate();
	}
}
