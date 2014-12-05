using UnityEngine;
using System.Collections;

public class Spell : ScriptableObject
{
	public string spellName;
	public string description;
	public Sprite icon;
	public float t_charge;
	public float t_minCharge;
	public float manacost;
	public float t_cooldown;
    public PlayerControl owner { get; set; }

    public float cooldown { get; set; }
    public float t_startCharge { get; set; }
    public float t_charged { get; set; }
    public bool charged { get; set; }
    protected int nextSeed;

    public virtual void Initialise() { }

    public virtual void Activate() { }

    public virtual void PlugNextWord()
    {
        if (!owner.audio.isPlaying)
        {
            Random.seed = nextSeed;
            nextSeed = Random.Range(-214748364, 214748364);
            owner.audio.clip = owner.words[Random.Range(0, owner.words.Length)];
            owner.audio.Play();
            Random.seed = Mathf.RoundToInt(Time.time * 100f);
        }
    }

    public virtual void Begin(Vector3 reticle)
    {
        t_startCharge = Time.time;
        t_charged = 0;
        charged = false;

        nextSeed = spellName.GetHashCode();
        PlugNextWord();
        if (OnBegin != null)
            OnBegin(this, reticle);
    }

    public virtual void Chant(Vector3 reticle)
    {
        t_charged += Time.deltaTime * owner.castComponent.mod_charge;
        PlugNextWord();
    }

    public virtual void Charge(Vector3 reticle)
    {
        charged = true;
        if (OnCharge != null)
            OnCharge(this, reticle);
    }

    public virtual void End(Vector3 reticle)
    {
        if (CanCast() && t_charged >= t_minCharge)
            Cast(t_charged, reticle);
        t_charged = 0;
        t_startCharge = 0;
        if (OnEnd != null)
            OnEnd(this, reticle);
    }

    public virtual void Update()
    {
        if (cooldown > 0)
            cooldown -= Time.deltaTime * owner.castComponent.mod_cooldown;
        else
            cooldown = 0;
    }

    public virtual bool EnoughMana()
    {
        if (owner.castComponent.mana > manacost)
            return true;
        else
            return false;
    }

    public virtual bool IsCooldown()
    {
        if (cooldown <= 0)
            return true;
        else
            return false;
    }

    public virtual bool CanCast()
    {
        if (IsCooldown() && EnoughMana())
            return true;
        else
            return false;
    }

	public virtual void Cast(float charge, Vector3 reticle)
    {
        cooldown += t_cooldown;
        owner.castComponent.mana -= manacost * owner.castComponent.mod_manacost;
        if (OnCast != null)
            OnCast(this, reticle);
    }

    public virtual void OnDestroy()
    {

    }

    public delegate void CastAction<SpellType>(SpellType spell, Vector3 target);
    public event CastAction<Spell> OnBegin;
    public event CastAction<Spell> OnCharge;
    public event CastAction<Spell> OnEnd;
    public event CastAction<Spell> OnCast;
}
