using UnityEngine;
using System.Collections;

public class WallHealthComponent : HealthComponent
{
    public float t_death;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (Time.time > t_death)
            Destroy(gameObject);
    }
}