using UnityEngine;
using System.Collections;

[System.Serializable]
public class NormalAttackSpell : Spell
{
    public float damage = 200f;
    public float damageCharged;
	public float delay = 0.2f;
	public float duration = 1f;
	public float size = 1f;
	
	public override void cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
		Transform attack = Instantiate(prefab) as Transform;
		NormalAttackObject obj = attack.GetComponent<NormalAttackObject>();

		
		if (charged)
		{
			obj.destroyProjectiles = true;
			obj.damage = damageCharged;
			obj.t_activation = Time.time + delay/2f;
			secondsCooldown = 0.1f;
		}
		else
		{
			obj.damage = damage;
			obj.t_activation = Time.time + delay;
			secondsCooldown = 0.5f;
		}

		attack.transform.parent = owner.transform;
		attack.transform.localPosition = new Vector3(0, 0.5f, 0);
		attack.transform.localRotation = Quaternion.identity;
		obj.t_deactivation = obj.t_activation + duration;
		obj.target = reticle;
		obj.size = size;
	}
}
