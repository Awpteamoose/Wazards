using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class FireBuffSpell : Spell
{
	public float damage;
	public float t_lifespan;
	public Sprite iconActive;
	public Sprite iconActiveCharged;

	private FireballSpell fireball;
	//private NormalAttackSpell normalAttack;
	private float t_life;
	private bool active;

	private Sprite _icon;

	Beholder.Subscription sub;

	public override void Initialise()
	{
		base.Initialise();

		fireball = SpellList.Get("FireballSpell") as FireballSpell;
		fireball.castComponent = castComponent;
		fireball.Initialise();
		active = false;
		_icon = icon;
	}

	public override void OnDestroy()
	{
		//Disable();
		if (active) {
			Disable();
		} else {
			Beholder.Cancel(sub);
		}
	}

	public override bool CanCast()
	{
		if (active)
			return false;
		else
			return base.CanCast();
	}

	public void Hit(NormalAttackSpell spell, Vector3 target)
	{
		if (charged)
		{
			fireball.Cast(0, target);
		}
		Disable();
	}

	public void Enable()
	{
		//normalAttack.damage += damage;
		//normalAttack.damageCharged += damage;
		Beholder.Callback<Spell, Vector3> hit = (spell, target) =>
		{
			if (charged)
			{
				fireball.Cast(0, target);
			}
			Disable();
		};
		sub = Beholder.Observe("attack_cast", hit);
		if (charged)
			icon = iconActiveCharged;
		else
			icon = iconActive;
		t_life = 0;
		active = true;
	}

	public void Disable()
	{
		//normalAttack.damage -= damage;
		//normalAttack.damageCharged -= damage;
		Beholder.Cancel(sub);
		cooldown = cooldownDuration;
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
					fireball.Cast(0, castComponent.transform.position + castComponent.owner.moveComponent.direction.vector);
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