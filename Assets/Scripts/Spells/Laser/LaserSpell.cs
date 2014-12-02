using UnityEngine;
using System.Collections;

[System.Serializable]
public class LaserSpell : Spell
{
	public float damage;
    public float chargedDamage;
    public float width;
    public float chargedWidth;
    public float t_delay;
    public float t_chargedDelay;
    public float t_active;
    public float t_chargedActive;
    public float t_tick;
    public float knockbackScale;

    public LaserEmitter prefab;

    public override void Initialise()
    {
        base.Initialise();

        if (prefab.CountPooled() == 0)
            prefab.CreatePool(3);
    }
	
	public override void Cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
        LaserEmitter emitter = prefab.Spawn(owner.transform.position + (owner.moveComponent.direction.vector * 0.6f), Quaternion.identity);
		emitter.target = reticle;
		emitter.parent = owner.gameObject;
        emitter.size = 1f;
		
		if (charged)
        {
            emitter.damage = chargedDamage;
            emitter.t_activation = Time.time + t_chargedDelay;
            emitter.laserBeam.localScale = new Vector2(chargedWidth, emitter.laserBeam.localScale.y);
            emitter.t_death = emitter.t_activation + t_chargedActive;
		}
		else
        {
            emitter.damage = damage;
            emitter.t_activation = Time.time + t_delay;
            emitter.laserBeam.localScale = new Vector2(width, emitter.laserBeam.localScale.y);
            emitter.t_death = emitter.t_activation + t_active;
		}

        emitter.t_tick = t_tick;
        emitter.scale = knockbackScale;

        emitter.Activate();
	}
}
