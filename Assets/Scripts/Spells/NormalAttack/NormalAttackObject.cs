using UnityEngine;
using System.Collections;

public class NormalAttackObject : Projectile
{
	public float damage;
	public float t_activation;
	public float t_deactivation;
	public bool destroyProjectiles = false;

	private bool collided = false;

	public override void Start ()
	{
		parent = transform.parent.gameObject;
		base.Start();
	}

	public override void FixedUpdate ()
	{
		if ( Time.time > t_activation && !collided )
		{
			collider2D.enabled = true;
		}

		if ( (Time.time > t_deactivation) || collided )
		{
			collider2D.enabled = false;
			if (!audio.isPlaying)
				Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject != parent)
		{
            if (destroyProjectiles)
            {
                Projectile proj = collider.gameObject.GetComponent<Projectile>();
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
	}
}
