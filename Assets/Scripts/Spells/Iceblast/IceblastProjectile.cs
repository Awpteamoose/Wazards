using UnityEngine;
using System.Collections;

public class IceblastProjectile : ProjectileComponent
{
	public float t_duration;

	private float createdAt;

	private bool collided;
	private float t_collision;
	private PlayerControl victim;
	private float reducedBy;

    public AudioClip flySound;
    public AudioClip freezeSound;

	// Use this for initialization
	public override void Start ()
	{
		base.Start();
		createdAt = Time.time-0.3f;
        audio.clip = flySound;
        audio.Play();
	}

	// Update is called once per frame
	public override void FixedUpdate ()
	{
		base.FixedUpdate();
		if (!collided)
		{
			transform.rotation = Quaternion.Euler (0, 0, direction.angle);
			transform.Translate(Vector3.up*speed*Time.fixedDeltaTime*Mathf.Pow((Time.time - createdAt), 2));
		}
		else
		{
			if (victim.inputComponent._active)
				victim.inputComponent._active = false;
			transform.rigidbody2D.MovePosition(victim.transform.position);
            audio.volume = ((t_collision + t_duration) - Time.time) / t_duration;
			if (Time.time > t_collision + t_duration)
			{
				victim.inputComponent._active = true;
				Destroy (gameObject);
			}
		}
	}

	/*void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.gameObject != parent)
		{
			PlayerControl pc = collider.gameObject.GetComponent<PlayerControl>();
			if (pc)
			{
				pc.healthComponent.TakeDamage(damage, direction.vector);
				collided = true;
				victim = pc;
				t_collision = Time.time;
				victim.inputComponent._active = false;
				this.collider2D.enabled = false;
				transform.rotation = Quaternion.identity;
                audio.clip = freezeSound;
				audio.Play ();
                particleSystem.enableEmission = false;
                this.transform.localScale = new Vector3(3f, 3f, 0);
                return;
			}
            HealthComponent hc = collider.gameObject.GetComponent<HealthComponent>();
            if (hc)
            {
                hc.TakeDamage(damage, direction.vector);
                Destroy(gameObject);
                return;
            }
		}
	}*/

    public override void Collide(Collider2D collider, HealthComponent healthComponent, bool isParent, bool sameParent)
    {
        if (!isParent && !sameParent)
        {
            base.Collide(collider, healthComponent, isParent, sameParent);

            healthComponent.TakeDamage(damage, direction.vector);
            if (healthComponent is PlayerHealthComponent)
            {
                PlayerControl pc = ((PlayerHealthComponent)healthComponent).playerControl;
                collided = true;
                victim = pc;
                t_collision = Time.time;
                victim.inputComponent._active = false;
                collider2D.enabled = false;
                transform.rotation = Quaternion.identity;
                audio.clip = freezeSound;
                audio.Play();
                particleSystem.enableEmission = false;
                transform.localScale = new Vector3(3f, 3f, 0);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
