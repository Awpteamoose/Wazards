using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEmitter : ProjectileComponent
{
    public float t_activation;
    public float t_tick;
    public float scale;
    public float width;
    public AudioClip chargeSound;
    public AudioClip fireSound;
    public LineRenderer laserBeam;

    private List<HealthComponent> damageList;
    private bool hasEndpoint;
    private float t_nextTick;
    private float height;
    private Vector2 infinite;

    public override void Awake()
    {
        base.Awake();
        laserBeam = GetComponent<LineRenderer>();

        damageList = new List<HealthComponent>();
    }

	public override void Activate ()
	{
        base.Activate();
        transform.rotation = Quaternion.Euler(0, 0, direction.angle);
        
        t_nextTick = t_activation;

        audio.clip = chargeSound;
        audio.time = chargeSound.length - (t_activation - Time.time);
        audio.Play();

        animator.speed = 1f / (t_activation - Time.time);

        infinite = new Vector2(0, 100f);
        laserBeam.SetWidth(width, width);
	}

	// Update is called once per frame
	public override void FixedUpdate ()
	{
        base.FixedUpdate();
        transform.position = parent.transform.position + direction.vector * 0.6f;
	}

	public override void Update()
	{
        base.Update();
        hasEndpoint = false;
        damageList.Clear();
        RaycastHit2D[] hits = Physics2D.BoxCastAll(laserBeam.transform.position, new Vector2(width, 0.1f), 0f, direction.vector);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != parent.collider2D)
            {
                HealthComponent hc = hit.collider.gameObject.GetComponent<HealthComponent>();
                if (hc)
                {
                    damageList.Add(hc);
                    float distance = (hit.point - new Vector2(transform.position.x, transform.position.y)).magnitude;
                    laserBeam.SetPosition(1, new Vector2(0, distance));
                    if (!(hc is ProjectileHealthComponent))
                    {
                        hasEndpoint = true;
                        break;
                    }
                }
            }
        }
        if (!hasEndpoint)
            laserBeam.SetPosition(1, infinite);

        if (Time.time > t_activation)
        {
            if (audio.clip == chargeSound)
            {
                audio.clip = fireSound;
                audio.loop = true;
                audio.time = 0;
                audio.Play();
            }
            if (damageList.Count > 0 && Time.time > t_nextTick)
            {
                foreach (HealthComponent hc in damageList)
                {
                    hc.TakeDamage(damage, direction.vector, scale);
                    t_nextTick = Time.time + t_tick;
                }
            }
        }
	}
}
