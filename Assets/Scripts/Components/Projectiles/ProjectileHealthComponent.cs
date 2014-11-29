using UnityEngine;
using System.Collections;

public class ProjectileHealthComponent : HealthComponent
{
    ProjectileComponent projectileComponent;

	// Use this for initialization
	void Start ()
    {
        health = maxHealth;
	}

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public override void TakeDamage(float damage, Vector2 direction, float scale = 1f)
    {
        base.TakeDamage(damage, direction, scale);

        health -= damage;
    }
}
