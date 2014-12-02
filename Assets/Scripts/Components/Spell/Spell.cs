using UnityEngine;
using System.Collections;

public class Spell : ScriptableObject
{
	public string spellName;
	public string description;
	public Sprite icon;
	public float secondsToCharge;
	public float secondsMinCharge;
	public float manacost;
	public float secondsCooldown;
    public PlayerControl owner { get; set; }

    public virtual void Initialise() { }
    public virtual void Update() { }
	public virtual void Cast(bool charged, Vector3 reticle) { }
}
