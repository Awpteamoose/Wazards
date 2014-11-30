using UnityEngine;
using System.Collections;

public class ChargeProjectile : ProjectileComponent {

	public float damageMod;
	public float t_terminate;
    public float maxDamage;
    public float minDamage;

	private Vector3 initial_pos;
	private bool collided = false;
	private AudioSource[] audioSources;

	// Use this for initialization
	public override void Start ()
	{
		base.Start();
		initial_pos = transform.position;
        audioSources = GetComponents<AudioSource>();
        damage = minDamage;
	}

	// Update is called once per frame
	public override void FixedUpdate ()
	{
		if (!collided)
		{
			base.FixedUpdate();
			if (Time.time < t_terminate)
			{
				parent.rigidbody2D.MovePosition(parent.transform.position + direction.vector*speed);
				rigidbody2D.MovePosition(parent.transform.position+direction.vector*speed);

                float distDamage = (transform.position - initial_pos).magnitude * damageMod;
                damage = Mathf.Clamp(distDamage, minDamage, maxDamage);
			}
			else
			{
				collider2D.enabled = false;
                collided = true;
			}
		}
	}

	public override void Update()
	{
        base.Update();
        if (collided)
        {
            if (!audioSources[0].isPlaying && !audioSources[1].isPlaying)
                Destroy(gameObject);
        }
        else
        {
            parent.rigidbody2D.velocity = direction.vector * 5f;
        }
	}

	/*public override void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject != parent)
		{
            base.OnTriggerEnter2D(collider);
            if (c_healthComponent)
			{
                float damage = (transform.position - initial_pos).magnitude * damageMod;
                c_healthComponent.TakeDamage(Mathf.Clamp(damage, minDamage, maxDamage), direction.vector);
				audio.Play ();
				collided = true;
                collider2D.enabled = false;
			}
		}
	}*/

    public override void Die()
    {
        base.Die();
        audio.Play();
        collided = true;
        collider2D.enabled = false;
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
