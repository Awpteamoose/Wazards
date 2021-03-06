﻿using UnityEngine;
using System.Collections;

public class ButterflyProjectile : ProjectileComponent
{
	public float shift {get; set;}
	public Color color;
    public float t_activation;

    private bool ready;

    public override void Activate ()
    {
        base.Activate();
		renderer.color = color;
        ready = false;
    }

	public override void Update ()
	{
        base.Update();
        if (Time.time > t_activation && !ready)
        {
            direction.angle = parent.transform.rotation.eulerAngles.z + shift;
            transform.position = parent.transform.position + direction.vector;
            transform.rotation = direction.rotation;
            rigidbody.AddForce(direction.vector * speed);

            animator.Play("butterfly", -1, Random.Range(0.0f, 1.0f));
            animator.Play("butterfly_opacity", -1, 0);
            renderer.enabled = true;
            collider.enabled = true;
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().time = Random.Range(0f, GetComponent<AudioSource>().clip.length);
            GetComponent<AudioSource>().Play();
            ready = true;
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

            Vector3 knockbackDirection = (healthComponent.transform.position - transform.position).normalized;
            healthComponent.TakeDamage(damage, knockbackDirection);
            if (!(healthComponent is ProjectileHealthComponent))
            {
                Die();
            }
        }
    }
}
