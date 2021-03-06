﻿using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour {

    public float mod_damage = 1f;
    public float mod_knockback = 1f;

	#if UNITY_EDITOR
	[ReadOnly]
	#endif
	public float health = 100f;
	public float maxHealth = 100f;
    protected float killAt { get; set; }
    #if UNITY_EDITOR
    [ReadOnly]
    #endif
    public bool dead;

    protected virtual void Awake() 
    {
        killAt = 50f;
    }
	
    protected virtual void Update()
    {
        if (health <= 0f || transform.position.magnitude > killAt)
            gameObject.Recycle();
    }

    protected virtual void FixedUpdate() { }

    protected virtual void OnTriggerEnter2D(Collider2D collider) { }

    protected virtual void OnTriggerExit2D(Collider2D collider) { }

    public virtual void Activate() { }

    public virtual void TakeDamage(float damage, Vector2 direction, float scale = 1f) { }
}
