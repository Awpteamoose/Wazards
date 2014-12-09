using UnityEngine;
using System.Collections;

[System.Serializable]
public class NormalAttackSpell : Spell
{
    public float damage;
    public float damageCharged;
	public float delay;
	public float duration;
	public float size;

    public NormalAttackObject prefab;

    public override void Initialise()
    {
        base.Initialise();

        if (prefab.CountPooled() == 0)
            prefab.CreatePool(3);
    }

    public override void PlugNextWord()
    {
    }
	
	public override void Cast(float charge, Vector3 reticle)
	{
        NormalAttackObject attack = prefab.Spawn();

		if (charge >= t_charge)
		{
			attack.destroyProjectiles = true;
			attack.damage = damageCharged;
			attack.t_activation = Time.time/* + delay/2f */;
			t_cooldown = 0.1f;
		}
		else
		{
            attack.destroyProjectiles = false;
			attack.damage = damage;
			attack.t_activation = Time.time + delay / owner.castComponent.mod_speed;
			t_cooldown = 0.5f;
		}
        attack.damage *= owner.castComponent.mod_damage;

        attack.parent = owner.gameObject;
		attack.transform.parent = owner.transform;
		attack.transform.localPosition = new Vector3(0, 0.5f, 0);
		attack.transform.localRotation = Quaternion.identity;
		attack.t_deactivation = attack.t_activation + duration;
		attack.target = reticle;
		attack.size = size;

        attack.Activate();
        base.Cast(charge, reticle);
	}
}
