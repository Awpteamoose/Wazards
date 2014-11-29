using UnityEngine;
using System.Collections;

public class NormalAttackObject : ProjectileComponent
{
	public float t_activation;
	public float t_deactivation;
	public bool destroyProjectiles = false;

	public override void Start ()
	{
		parent = transform.parent.gameObject;
		base.Start();
	}

	public override void Update ()
	{
        if ((Time.time > t_deactivation))
        {
            collider2D.enabled = false;
            if (!audio.isPlaying)
                Destroy(gameObject);
        }
        else if ( Time.time > t_activation )
		{
			collider2D.enabled = true;
		}
	}

	/*void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject != parent)
		{
            if (destroyProjectiles)
            {
                ProjectileComponent proj = collider.gameObject.GetComponent<ProjectileComponent>();
                if (proj)
                {
                    Destroy(proj.gameObject);
                    return;
                    //TODO: make it bounce the projectile back?
                }
            }
            HealthComponent hc = collider.gameObject.GetComponent<HealthComponent>();
            if (hc)
            {
                collided = true;
                hc.TakeDamage(damage, (hc.transform.position - parent.transform.position).normalized);
            }
		}
	}*/

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
