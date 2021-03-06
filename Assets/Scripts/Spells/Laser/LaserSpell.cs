﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class LaserSpell : Spell
{
    public float manaPerSec;
    public float manaPerSecCharged;
	public float damage;
    public float chargedDamage;
    public float width;
    public float chargedWidth;
    public float t_delay;
    public float t_chargedDelay;
    public float t_tick;
    public float knockbackScale;

    public LaserEmitter prefab;
    public Sprite iconToOff;

    private LaserEmitter emitter;
    private float _manaPerSec;
    private float _t_charge;
    private float _t_minCharge;
    private Sprite _icon;

    public override void Initialise()
    {
        base.Initialise();

        _t_charge = t_charge;
        _t_minCharge = t_minCharge;
        _icon = icon;
        if (prefab.CountPooled() == 0)
            prefab.CreatePool(3);
    }

    public override void PlugNextWord()
    {
        if (!emitter || !emitter.gameObject.activeSelf)
            base.PlugNextWord();
    }

    public override void Update()
    {
        if (emitter && emitter.gameObject.activeSelf)
        {
            if (Time.time > emitter.t_activation)
            {
                float manaSpent = (_manaPerSec) * Time.deltaTime;
                owner.castComponent.mana -= manaSpent;
                if (owner.castComponent.mana <= manaSpent)
                    TurnOff();
            }
        }
        else
        {
            base.Update();
        }
    }

    void TurnOff ()
    {
        emitter.Recycle();
        owner.castComponent.mod_regen += 1f;
        t_charge = _t_charge;
        t_minCharge = _t_minCharge;
        icon = _icon;
        cooldown += t_cooldown;
    }

    void TurnOn ()
    {
        emitter.Activate();
        owner.castComponent.mod_regen -= 1f;
        t_charge = 0;
        t_minCharge = 0;
        icon = iconToOff;
        cooldown -= t_cooldown;
    }

    public override bool EnoughMana()
    {
        if (emitter && !emitter.gameObject.activeSelf)
            return base.EnoughMana();
        else
            return true;
    }

    public override bool IsCooldown()
    {
        if (emitter && !emitter.gameObject.activeSelf)
            return base.IsCooldown();
        else
            return true;
    }
	
	public override void Cast(float charge, Vector3 reticle)
	{
        if (emitter && emitter.gameObject.activeSelf)
        {
            TurnOff();
        }
        else
        {
            emitter = prefab.Spawn(owner.transform.position + (owner.moveComponent.direction.vector * 0.6f), Quaternion.identity);
            emitter.target = reticle;
            emitter.parent = owner.gameObject;
            emitter.size = 1f;

            if (charge >= t_charge)
            {
                emitter.damage = chargedDamage;
                emitter.t_activation = Time.time + t_chargedDelay;
                emitter.width = chargedWidth;
                _manaPerSec = manaPerSecCharged;
            }
            else
            {
                emitter.damage = damage;
                emitter.t_activation = Time.time + t_delay;
                emitter.width = width;
                _manaPerSec = manaPerSec;
            }
            emitter.damage *= owner.castComponent.mod_damage;

            emitter.t_tick = t_tick;
            emitter.scale = knockbackScale;

            TurnOn();
            base.Cast(charge, reticle);
        }
	}
}
