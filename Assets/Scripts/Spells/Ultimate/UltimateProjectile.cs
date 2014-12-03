using UnityEngine;
using System.Collections;

public class UltimateProjectile : ProjectileComponent
{

    public override void Awake()
    {
        base.Awake();
    }

    public override void Activate ()
    {
        base.Activate();
    }

    public override void FixedUpdate ()
    {
        base.FixedUpdate();
        transform.rotation = Quaternion.Euler (0, 0, direction.angle);
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
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
            if (!(healthComponent is ProjectileHealthComponent))
            {
                Die();
            }
        }
    }
}