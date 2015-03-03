using UnityEngine;
using System.Collections;

public class NormalAttackObject : ProjectileComponent
{
	public float t_activation;
	public float t_deactivation;
	public bool destroyProjectiles;

	public override void Activate ()
    {
        base.Activate();
		parent = transform.parent.gameObject;
	}

	public override void Update ()
	{
        if ((Time.time > t_deactivation))
        {
            GetComponent<Collider2D>().enabled = false;
            if (!GetComponent<AudioSource>().isPlaying)
                Die();
        }
        else if ( Time.time > t_activation )
		{
			GetComponent<Collider2D>().enabled = true;
		}
        rigidbody.MoveRotation(parent.GetComponent<Rigidbody2D>().rotation);
        rigidbody.MovePosition(parent.GetComponent<Rigidbody2D>().position);
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

            if (destroyProjectiles && (healthComponent is ProjectileHealthComponent))
                healthComponent.TakeDamage(damage*3f, (healthComponent.transform.position - parent.transform.position).normalized);
            else
                healthComponent.TakeDamage(damage, (healthComponent.transform.position - parent.transform.position).normalized);
        }
    }
}
