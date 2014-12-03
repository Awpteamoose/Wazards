using UnityEngine;
using System.Collections;

[System.Serializable]
public class UltimateSpell : Spell
{
    public float duration = 5f;

    private float _duration;
    public bool used;
    public bool expired;

    private float cast_mod_damage;
    private float cast_mod_size;
    private float cast_mod_speed;
    private float cast_mod_cooldown;
    private float cast_mod_manacost;
    private float cast_mod_regen;
    private float move_mod_speed;
    private float health_mod_damage;
    private float health_mod_knockback;

    public override void Initialise()
    {
        base.Initialise();

        used = false;
        expired = false;
    }

    public override void Update()
    {
        base.Update();
        if (used && !expired)
        {
            if (_duration > 0)
            {
                _duration -= Time.deltaTime;
            }
            else
            {
                Buff(-1);
                owner.audio.clip = owner.ultimateSounds[3];
                owner.audio.time = 0;
                owner.audio.Play();
                expired = true;
            }
        }
    }

    public override bool CanCast()
    {
        if (used)
            return false;
        else
            return base.CanCast();
    }

    public void Buff(float sign)
    {
        owner.castComponent.mod_damage += cast_mod_damage * sign;
        owner.castComponent.mod_size += cast_mod_size * sign;
        owner.castComponent.mod_speed += cast_mod_speed * sign;
        owner.castComponent.mod_cooldown += cast_mod_cooldown * sign;
        owner.castComponent.mod_manacost += cast_mod_manacost * sign;
        owner.castComponent.mod_regen += cast_mod_regen * sign;
        owner.moveComponent.mod_speed += move_mod_speed * sign;
        owner.healthComponent.mod_damage += health_mod_damage * sign;
        owner.healthComponent.mod_knockback += health_mod_knockback * sign;
    }
    
    public override void Cast(float charge, Vector3 reticle)
    {
        _duration += duration;

        cast_mod_damage = owner.healthComponent.totalDamage / 200f;
        cast_mod_size = cast_mod_damage * 0.5f;
        cast_mod_speed = cast_mod_damage * 0.5f;
        cast_mod_cooldown = 1f;
        cast_mod_manacost = 0f;
        cast_mod_regen = 1f;
        move_mod_speed = 1f;
        health_mod_damage = 0f;
        health_mod_knockback = cast_mod_damage * 0.5f;

        Buff(1);

        Camera.main.Shake(0.4f);
        used = true;
        base.Cast(charge, reticle);
    }
}