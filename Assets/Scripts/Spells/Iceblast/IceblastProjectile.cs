﻿using UnityEngine;
using System.Collections;

public class IceblastProjectile : ProjectileComponent
{
	public float t_duration;

	private float createdAt;

	private float t_collision;
	private PlayerControl victim;
	private float reducedBy;

    public AudioClip flySound;
    public AudioClip freezeSound;

    public override void Awake()
    {
        base.Awake();
    }

	public override void Activate ()
	{
		base.Activate();
		createdAt = Time.time-0.3f;
        audio.clip = flySound;
        audio.Play();
	}

	// Update is called once per frame
	public override void FixedUpdate ()
	{
		base.FixedUpdate();
		if (collider.enabled)
		{
			transform.rotation = Quaternion.Euler (0, 0, direction.angle);
			transform.Translate(Vector3.up*speed*Time.fixedDeltaTime*Mathf.Pow((Time.time - createdAt), 2));
		}
		else
		{
			if (victim.inputComponent._active)
				victim.inputComponent._active = false;
			transform.rigidbody2D.MovePosition(victim.transform.position);
            audio.volume = ((t_collision + t_duration) - Time.time) / t_duration;
			if (Time.time > t_collision + t_duration)
			{
				victim.inputComponent._active = true;
                Die();
			}
		}
	}

    public override void Die()
    {
        base.Die();
        gameObject.Recycle();
    }

    public override void Collide(Collider2D collider, HealthComponent healthComponent, bool isParent, bool sameParent)
    {
        if (!isParent && !sameParent)
        {
            base.Collide(collider, healthComponent, isParent, sameParent);

            healthComponent.TakeDamage(damage, direction.vector);
            if (healthComponent is PlayerHealthComponent)
            {
                PlayerControl pc = ((PlayerHealthComponent)healthComponent).playerControl;
                victim = pc;
                t_collision = Time.time;
                victim.inputComponent._active = false;
                collider.enabled = false;
                transform.rotation = Quaternion.identity;
                audio.clip = freezeSound;
                audio.Play();
                particleSystem.enableEmission = false;
                transform.localScale = new Vector3(3f, 3f, 0);
            }
            else if (!(healthComponent is ProjectileHealthComponent))
            {
                Die();
            }
        }
    }
}
