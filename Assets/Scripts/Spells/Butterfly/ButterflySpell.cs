using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ButterflySpell : Spell
{
	public float damage = 75f;
	public float speed = 0.3f;
	public float size = 2f;
	public int amount = 3;
	public int angleEdge = 20;
	public int chargedAmount = 5;
	public int chargedAngleEdge = 20;
    public float t_delayMax;
	
	public override void cast(bool charged, Vector3 reticle, PlayerControl owner)
	{	
		int angle;
		int amt;
		if (charged) 
		{
			angle = chargedAngleEdge;
			amt = chargedAmount;
		}
		else
		{
			angle = angleEdge;
			amt = amount;
		}
		
		int step;
		if (amt > 2)
			step = angle/((amt-1)/2);
		else
		{
			step = 1;
			angle = 0;
		}
		for ( int i = -angle; i <= angle; i+=step )
		{
			Transform butterfly = Instantiate(prefab) as Transform;
			ButterflyProjectile projectile = butterfly.GetComponent<ButterflyProjectile>();
			
			projectile.shift=i+Random.Range (-10f, 10f);
			
			projectile.target = reticle;
			projectile.parent = owner.gameObject;
            //projectile.transform.localScale = new Vector2(size, size);
            projectile.size = size;
			projectile.speed = speed*Random.Range(0.3f, 1.5f);
			projectile.damage = damage;

			if (owner.player == "Player 1")
				projectile.color = new Color(0.37f, 0.47f, 1f);
			else
				projectile.color = new Color(1f, 0.97f, 0.3f);

            projectile.renderer.enabled = false;
            projectile.collider.enabled = false;
            projectile.t_activation = Time.time + Random.Range(0, t_delayMax);
		}
	}
}