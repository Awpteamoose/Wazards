using UnityEngine;
using System.Collections;

public class ProjectileHealthComponent : HealthComponent
{
    public ProjectileComponent projectileComponent { get; set; }

	// Use this for initialization
	public override void Activate ()
    {
        base.Activate();

        health = maxHealth;
        dead = false;
	}

    protected override void Update()
    {
        if ((health <= 0 || transform.position.magnitude > killAt) && !dead)
        {
            dead = true;
            projectileComponent.Die();
        }
    }

    public override void TakeDamage(float damage, Vector2 direction, float scale = 1f)
    {
        base.TakeDamage(damage, direction, scale);

        health -= damage;
    }
}
