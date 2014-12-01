using UnityEngine;
using System.Collections;

public class ProjectileHealthComponent : HealthComponent
{
    public ProjectileComponent projectileComponent { get; set; }

	// Use this for initialization
	protected override void Start ()
    {
        base.Start();

        dead = false;
        health = maxHealth;
	}

    protected override void Update()
    {
        base.Update();

        if (health <= 0 && !dead)
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
