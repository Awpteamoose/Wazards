﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserEmitter : ProjectileComponent
{
    public float t_activation;
    public float t_death;
    public float t_tick;
    public float scale;
    public AudioClip chargeSound;
    public AudioClip fireSound;

    public Transform laserBeam;

    private List<HealthComponent> damageList = new List<HealthComponent>();
    private Vector3 startScale;
    private bool hasEndpoint;
    private float t_nextTick;
    private float height;
    private float damagePerTick;

	// Use this for initialization
	public override void Start ()
	{
        base.Start();
        transform.rotation = Quaternion.Euler(0, 0, direction.angle);
        height = laserBeam.GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        startScale = new Vector2(laserBeam.transform.localScale.x, 100f);
        Vector3 position = laserBeam.transform.position;
        laserBeam.parent = transform;
        laserBeam.localPosition = position;
        laserBeam.localRotation = Quaternion.identity;
        t_nextTick = t_activation;
        damagePerTick = damage / ((t_death - t_activation) / t_tick);

        audio.clip = chargeSound;
        audio.time = chargeSound.length - (t_activation - Time.time);
        audio.Play();

        laserBeam.GetComponent<Animator>().speed /= t_activation - Time.time;
	}

	// Update is called once per frame
	public override void FixedUpdate ()
	{
		base.FixedUpdate();
	}

	public override void Update()
	{
        base.Update();
        transform.position = parent.transform.position + direction.vector * 0.6f;
        hasEndpoint = false;
        damageList.Clear();
        RaycastHit2D[] hits = Physics2D.BoxCastAll(laserBeam.transform.position, new Vector2(laserBeam.localScale.x, 0.1f), 0f, direction.vector);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != parent.collider2D)
            {
                HealthComponent hc = hit.collider.gameObject.GetComponent<HealthComponent>();
                if (hc)
                {
                    damageList.Add(hc);
                    laserBeam.localScale = new Vector2(
                        laserBeam.localScale.x,
                        (hit.point - (Vector2)laserBeam.transform.position).magnitude / height
                    );
                    if (!(hc is ProjectileHealthComponent))
                    {
                        hasEndpoint = true;
                        break;
                    }
                }
            }
        }
        if (!hasEndpoint)
            laserBeam.localScale = startScale;

        if (Time.time > t_death)
        {
            laserBeam.gameObject.Recycle();
            gameObject.Recycle();
        }

        if (Time.time > t_activation)
        {
            if (audio.clip == chargeSound)
            {
                audio.clip = fireSound;
                audio.Play();
            }
            if (damageList.Count > 0 && Time.time > t_nextTick)
            {
                foreach (HealthComponent hc in damageList)
                {
                    hc.TakeDamage(damagePerTick, direction.vector, scale);
                    t_nextTick = Time.time + t_tick;
                }
            }
        }
	}
}
