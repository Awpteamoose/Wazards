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

    private LaserEmitter emitter;
    private float _manaPerSec;
    private float _secondsToCharge;
    private float _secondsCooldown;
    private float _manacost;

    public override void Initialise()
    {
        base.Initialise();

        _secondsToCharge = secondsToCharge;
        _secondsCooldown = secondsCooldown;
        _manacost = manacost;
        if (prefab.CountPooled() == 0)
            prefab.CreatePool(3);
    }

    public override void Update()
    {
        base.Update();

        if (emitter && emitter.gameObject.activeSelf && Time.time > emitter.t_activation)
        {
            float manaSpent = (_manaPerSec + owner.castComponent.manaRegen * 3f) * Time.deltaTime;
            Debug.Log(_manaPerSec);
            owner.castComponent.mana -= manaSpent;
            if (owner.castComponent.mana <= manaSpent)
                TurnOff();
        }
    }

    void TurnOff ()
    {
        emitter.Recycle();
        secondsToCharge = _secondsToCharge;
        secondsCooldown = _secondsCooldown;
        manacost = _manacost;
    }

    void TurnOn ()
    {
        emitter.Activate();
        secondsToCharge = 0;
        secondsCooldown = 0;
        manacost = 0;
    }
	
	public override void Cast(bool charged, Vector3 reticle)
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

            if (charged)
            {
                emitter.damage = chargedDamage;
                emitter.t_activation = Time.time + t_chargedDelay;
                emitter.laserBeam.localScale = new Vector2(chargedWidth, emitter.laserBeam.localScale.y);
                _manaPerSec = manaPerSecCharged;
            }
            else
            {
                emitter.damage = damage;
                emitter.t_activation = Time.time + t_delay;
                emitter.laserBeam.localScale = new Vector2(width, emitter.laserBeam.localScale.y);
                _manaPerSec = manaPerSec;
            }

            emitter.t_tick = t_tick;
            emitter.scale = knockbackScale;

            TurnOn();
        }
	}
}
