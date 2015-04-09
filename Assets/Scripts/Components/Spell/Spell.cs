using UnityEngine;
using System.Collections;

public class Spell : ScriptableObject
{
	public string displayName;
	public string realName;
	public string description;
	public Sprite icon;
	public float chargeDuration;
	public float min_chargeDuration;
	public float manacost;
	public float cooldownDuration;
	public CastComponent castComponent { get; set; }

	public float cooldown { get; set; }
	public float charge { get; set; }
	public bool charged { get; set; }
	protected int nextSeed;

	public virtual void Initialise() {}

	public virtual void PlugNextWord()
	{
		if (!castComponent.GetComponent<AudioSource>().isPlaying)
		{
			Random.seed = nextSeed;
			nextSeed = Random.Range(-214748364, 214748364);
			castComponent.owner.GetComponent<AudioSource>().clip = castComponent.owner.words[Random.Range(0, castComponent.owner.words.Length)];
			castComponent.owner.GetComponent<AudioSource>().Play();
			Random.seed = Mathf.RoundToInt(Time.time * 100f);
		}
	}

	public virtual void Begin(Vector3 reticle)
	{
		charge = 0;
		charged = false;

		nextSeed = displayName.GetHashCode();
		PlugNextWord();
		//Beholder.Trigger(realName + "_begun", this, reticle);
	}

	public virtual void Chant(Vector3 reticle)
	{
		charge += Time.deltaTime * castComponent.mod_charge;
		PlugNextWord();
	}

	public virtual void Charge(Vector3 reticle)
	{
		charged = true;
		//Beholder.Trigger(realName + "_charged", this, reticle);
	}

	public virtual void End(Vector3 reticle)
	{
		if (CanCast() && charge >= min_chargeDuration)
			Cast(charge, reticle);
		charge = 0;
		//Beholder.Trigger(realName + "_ended", this, reticle);
	}

	public virtual void Update()
	{
		Mathf.Clamp(cooldown, 0, cooldown - Time.deltaTime * castComponent.mod_cooldown);
	}

	public virtual bool EnoughMana()
	{
		return castComponent.mana > manacost;
	}

	public virtual bool IsReady()
	{
		return cooldown <= 0;
	}

	public virtual bool CanCast()
	{
		return IsReady() && EnoughMana();
	}

	public virtual void Cast(float charge, Vector3 reticle)
	{
		cooldown += cooldownDuration;
		castComponent.mana -= manacost * castComponent.mod_manacost;
		Beholder.Trigger(realName + "_cast", this, reticle);
	}

	public virtual void OnDestroy() {}
}
