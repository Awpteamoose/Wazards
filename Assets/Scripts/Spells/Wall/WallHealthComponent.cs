﻿using UnityEngine;
using System.Collections;

public class WallHealthComponent : HealthComponent
{
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (health <= 0)
            Destroy(gameObject);
    }

    public override void TakeDamage(float damage, Vector2 direction, float scale = 1f)
    {
        base.TakeDamage(damage, direction, scale);
        health -= damage;
    }
}