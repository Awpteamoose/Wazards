using UnityEngine;
using System.Collections;

public class ChargeProjectile : Projectile {

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

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject != parent)
		{
            HealthComponent hc = collider.gameObject.GetComponent<HealthComponent>();
			if (hc)
			{
                float damage = (transform.position - initial_pos).magnitude * damageMod;
                hc.TakeDamage(Mathf.Clamp(damage, minDamage, maxDamage), direction.vector);
				audio.Play ();
				collided = true;
                collider2D.enabled = false;
			}
		}
	}
}
