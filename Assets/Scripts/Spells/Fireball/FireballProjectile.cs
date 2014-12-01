using UnityEngine;
using System.Collections;

public class FireballProjectile : ProjectileComponent
{
    public AudioClip flySound;
    public AudioClip explodeSound;

    private ParticleSystem ps;

    public override void Awake()
    {
        base.Awake();
        ps = GetComponent<ParticleSystem>();
    }

	public override void Activate ()
	{
		base.Activate();
        renderer.enabled = true;
        collider.enabled = true;
        ps.enableEmission = true;
        audio.clip = flySound;
        audio.Play();
	}

	// Update is called once per frame
	public override void FixedUpdate ()
	{
		base.FixedUpdate();
		transform.rotation = Quaternion.Euler (0, 0, direction.angle);
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
	}

	public override void Update()
	{
        base.Update();
		if (animator.GetCurrentAnimatorStateInfo(0).IsName("fireball_destroy") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
		{
			renderer.enabled = false;
            if (!audio.isPlaying)
                gameObject.Recycle();
		}
	}

    public override void Die()
    {
        base.Die();
        speed = 0;
        collider.enabled = false;
        ps.enableEmission = false;
        animator.SetTrigger("Destroy");
        audio.clip = explodeSound;
        audio.Play();
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
