using UnityEngine;
using System.Collections;

public class Spell : ScriptableObject
{
	public string spellName;
	public string description;
	public Sprite icon;
	public Transform prefab;
	public float secondsToCharge = 1.5f;
	public float secondsMinCharge = 0.0f;
	public float manacost = 10f;
	public float secondsCooldown = 1f;
	public virtual void cast(bool charged, Vector3 reticle, PlayerControl owner) {}
}
