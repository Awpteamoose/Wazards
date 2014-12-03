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
    #if UNITY_EDITOR
	[ReadOnly]
	#endif
    public float cooldown;
    public PlayerControl owner { get; set; }

    public virtual void Initialise() { }
    public virtual void Update()
    {
        if (cooldown > 0)
            cooldown -= Time.deltaTime;
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
        cooldown += t_cooldown * owner.castComponent.mod_cooldown;
        owner.castComponent.mana -= manacost * owner.castComponent.mod_cooldown;
    }
}
