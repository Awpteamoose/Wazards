using UnityEngine;
using System.Collections;

public class ProjectileHealthComponent : HealthComponent
{
    public ProjectileComponent projectileComponent;

	// Use this for initialization
	void Start ()
    {
        dead = false;
        health = maxHealth;
	}

    void Update()
    {
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
