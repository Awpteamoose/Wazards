using UnityEngine;
using System.Collections;

[System.Serializable]
public class WallSpell : Spell
{
	public float minDistance;
	public float width;
	public float height;
	public float health;

	public Transform prefab;

	public override void Initialise()
	{
		base.Initialise();

		if (prefab.CountPooled() == 0)
			prefab.CreatePool(10);
	}

	public override void Cast(float charge, Vector3 reticle)
	{
		Transform wall = prefab.Spawn();
		if (charge >= chargeDuration)
			wall.position = reticle;
		else
			wall.position = castComponent.transform.position + (reticle - castComponent.transform.position).normalized * minDistance;
		wall.rotation = castComponent.transform.rotation;
		wall.localScale = new Vector3(width, height, 1f);
		//wall.renderer.material.mainTextureScale = wall.localScale;
		WallHealthComponent whc = wall.GetComponent<WallHealthComponent>();
		whc.health = health * castComponent.mod_damage;
		whc.gameObject.SetActive(true);
		base.Cast(charge, reticle);
	}
}
