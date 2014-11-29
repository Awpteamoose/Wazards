using UnityEngine;
using System.Collections;

public class Spell : ScriptableObject
{
	public string spellName;
	public string description;
	public Sprite icon;
	public Transform prefab;
	public float secondsToCharge;
	public float secondsMinCharge;
	public float manacost;
	public float secondsCooldown;
    

	public virtual void cast(bool charged, Vector3 reticle, PlayerControl owner) {}
}
