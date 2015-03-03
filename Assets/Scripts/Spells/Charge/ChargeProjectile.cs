using UnityEngine;
using System.Collections;

public class ChargeProjectile : ProjectileComponent {

	public float damageMod;
	public float t_terminate;
    public float maxDamage;
    public float minDamage;

	private Vector3 initial_pos;
	private AudioSource[] audioSources;

    public override void Awake ()
    {
        base.Awake();
        audioSources = GetComponents<AudioSource>();
    }

	public override void Activate ()
	{
		base.Activate();
		initial_pos = transform.position;
        damage = minDamage;
        collider.enabled = true;
	}

	// Update is called once per frame
	public override void FixedUpdate ()
	{
		if (collider.enabled)
		{
			base.FixedUpdate();
			if (Time.time < t_terminate)
			{
				parent.GetComponent<Rigidbody2D>().MovePosition(parent.transform.position + direction.vector*speed);
				GetComponent<Rigidbody2D>().MovePosition(parent.transform.position+direction.vector*speed);

                float distDamage = (transform.position - initial_pos).magnitude * damageMod;
                damage = Mathf.Clamp(distDamage, minDamage, maxDamage);
			}
			else
			{
                collider.enabled = false;
			}
		}
	}

	public override void Update()
	{
        base.Update();
        if (collider.enabled)
        {
            parent.GetComponent<Rigidbody2D>().velocity = direction.vector * 5f;
        }
        else
        {
            if (!audioSources[0].isPlaying && !audioSources[1].isPlaying)
                gameObject.Recycle();
        }
	}

    public override void Die()
    {
        base.Die();
        GetComponent<AudioSource>().Play();
        collider.enabled = false;
    }

    public override void Collide(Collider2D collider, HealthComponent healthComponent, bool isParent, bool sameParent)
    {
        if (!isParent && !sameParent)
        {
            base.Collide(collider, healthComponent, isParent, sameParent);

            float damage = (transform.position - initial_pos).magnitude * damageMod;
            c_healthComponent.TakeDamage(Mathf.Clamp(damage, minDamage, maxDamage), direction.vector);
            if (!(healthComponent is ProjectileHealthComponent))
            {
                Die();
            }
        }
    }
}
