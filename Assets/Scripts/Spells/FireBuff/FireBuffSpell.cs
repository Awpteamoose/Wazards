using UnityEngine;
using System.Collections;

[System.Serializable]
public class FireBuffSpell : Spell
{
    public float damage;
    public float t_lifespan;
    public Sprite iconActive;
    public Sprite iconActiveCharged;

    private FireballSpell fireball;
    private NormalAttackSpell normalAttack;
    private float t_life;
    private bool active;

    private Sprite _icon;

    public override void Initialise()
    {
        base.Initialise();

        fireball = SpellList.Get("FireballSpell") as FireballSpell;
        fireball.owner = owner;
        fireball.Initialise();
        fireball.Activate();
        active = false;
        _icon = icon;
    }

    public override void Activate()
    {
        foreach (Spell spell in owner.castComponent.spellBook.spells)
        {
            if (spell is NormalAttackSpell)
            {
                normalAttack = spell as NormalAttackSpell;
            }
        }
    }

    public override void OnDestroy()
    {
        Disable();
    }

    public override bool CanCast()
    {
        if (active)
            return true;
        else
            return base.CanCast();
    }

    public void Hit(Spell spell, Vector3 target)
    {
        if (charged)
        {
            fireball.Cast(0, target);
        }
        Disable();
    }

    public void Enable()
    {
        normalAttack.damage += damage;
        normalAttack.damageCharged += damage;
        normalAttack.OnCast += Hit;
        if (charged)
            icon = iconActiveCharged;
        else
            icon = iconActive;
        t_life = 0;
        active = true;
    }

    public void Disable()
    {
        normalAttack.damage -= damage;
        normalAttack.damageCharged -= damage;
        normalAttack.OnCast -= Hit;
        cooldown = t_cooldown;
        active = false;
        icon = _icon;
    }

    public override void Update()
    {
        base.Update();
        if (active)
        {
            t_life += Time.deltaTime;
            if (t_life >= t_lifespan)
            {
                Disable();
                if (charged)
                    fireball.Cast(0, owner.transform.position + owner.moveComponent.direction.vector);
            }
        }
    }
    
    public override void Cast(float charge, Vector3 reticle)
    {
        Enable();
        base.Cast(charge, reticle);
        cooldown = 0;
    }
}