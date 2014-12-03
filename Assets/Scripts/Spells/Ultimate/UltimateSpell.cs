using UnityEngine;
using System.Collections;

[System.Serializable]
public class UltimateSpell : Spell
{
    public float duration = 5f;

    private float _duration;
    public bool used;
    public bool active;
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
        active = false;
        expired = false;
    }

    public override void PlugNextWord()
    {
    }

    public override void Begin(Vector3 reticle)
    {
        base.Begin(reticle);

        owner.audio.Stop();
        owner.audio.clip = owner.ultimateSounds[0];
        owner.audio.Play();
    }

    public override void Charge(Vector3 reticle)
    {
        base.Charge(reticle);

        owner.audio.Stop();
        owner.audio.clip = owner.ultimateSounds[1];
        owner.audio.Play();
    }

    public override void End(Vector3 reticle)
    {
        base.End(reticle);

        owner.audio.Stop();
        owner.audio.clip = owner.ultimateSounds[2];
        owner.audio.Play();
    }


    public override void Update()
    {
        base.Update();
        if (_duration > 0)
        {
            _duration -= Time.deltaTime;
        }
        else if (active)
        {
            if (!expired)
            {
                Buff(-1.5f);
                owner.audio.clip = owner.ultimateSounds[3];
                //owner.audio.time = 0;
                owner.audio.Play();
                _duration = duration;
                expired = true;
            }
            else
            {
                Buff(0.5f);
                active = false;
                expired = false;
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
        health_mod_knockback = cast_mod_damage;

        Buff(1);
        used = true;

        Camera.main.Shake(0.4f);
        active = true;
        base.Cast(charge, reticle);
    }
}