using UnityEngine;
using System.Collections;

[System.Serializable]
public class WallSpell : Spell
{
	public float minDistance = 2f;
    public float width;
    public float height;
    public float health;

    public Transform prefab;

    public override void Initialise()
    {
        base.Initialise();

        prefab.CreatePool(10);
    }

	public override void Cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
        Transform wall = prefab.Spawn();
        if (charged)
            wall.position = reticle;
        else
            wall.position = owner.transform.position + (reticle - owner.transform.position).normalized * minDistance;
        wall.rotation = owner.transform.rotation;
        wall.localScale = new Vector2(width, height);
        wall.renderer.material.mainTextureScale = wall.localScale;
        WallHealthComponent whc = wall.GetComponent<WallHealthComponent>();
        whc.health = health;
	}
}
