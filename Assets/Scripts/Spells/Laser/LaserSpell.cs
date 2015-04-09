using UnityEngine;
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

		_t_charge = chargeDuration;
		_t_minCharge = min_chargeDuration;
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
				castComponent.mana -= manaSpent;
				if (castComponent.mana <= manaSpent)
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
		castComponent.mod_regen += 1f;
		chargeDuration = _t_charge;
		min_chargeDuration = _t_minCharge;
		icon = _icon;
		cooldown += cooldownDuration;
	}

	void TurnOn ()
	{
		emitter.Activate();
		castComponent.mod_regen -= 1f;
		chargeDuration = 0;
		min_chargeDuration = 0;
		icon = iconToOff;
		cooldown -= cooldownDuration;
	}

	public override bool EnoughMana()
	{
		if (emitter && !emitter.gameObject.activeSelf)
			return base.EnoughMana();
		else
			return true;
	}

	public override bool IsReady()
	{
		if (emitter && !emitter.gameObject.activeSelf)
			return base.IsReady();
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
			emitter = prefab.Spawn(castComponent.transform.position + (castComponent.owner.moveComponent.direction.vector * 0.6f), Quaternion.identity);
			emitter.target = reticle;
			emitter.parent = castComponent.gameObject;
			emitter.size = 1f;

			if (charge >= chargeDuration)
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
			emitter.damage *= castComponent.mod_damage;

			emitter.t_tick = t_tick;
			emitter.scale = knockbackScale;

			TurnOn();
			base.Cast(charge, reticle);
		}
	}
}
