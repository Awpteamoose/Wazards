#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptables
{
	[MenuItem("Assets/Create/Create Scriptables")]
	public static void CreateAsset()
	{
		ScriptableObject asset;
		
		/*asset = ScriptableObject.CreateInstance<TeleportSpell>();
		AssetDatabase.CreateAsset(asset, "Assets/Resources/Spells/Teleport/TeleportSpell.asset");*/

		/*asset = ScriptableObject.CreateInstance<FireballSpell>();
		AssetDatabase.CreateAsset(asset, "Assets/Resources/Spells/Fireball/FireballSpell.asset");*/

		/*asset = ScriptableObject.CreateInstance<ButterflySpell>();
		AssetDatabase.CreateAsset(asset, "Assets/Resources/Spells/Butterfly/ButterflySpell.asset");*/

		/*asset = ScriptableObject.CreateInstance<IceblastSpell>();
		AssetDatabase.CreateAsset(asset, "Assets/Resources/Spells/Iceblast/IceblastSpell.asset");*/

		/*asset = ScriptableObject.CreateInstance<ChargeSpell>();
		AssetDatabase.CreateAsset(asset, Assets/Resources/Spells/Charge/ChargeSpell.asset");*/

		/*asset = ScriptableObject.CreateInstance<NormalAttackSpell>();
		AssetDatabase.CreateAsset(asset, "Assets/Resources/Spells/NormalAttack/NormalAttackSpell.asset");*/

        /*asset = ScriptableObject.CreateInstance<LaserSpell>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/Spells/Laser/LaserSpell.asset");*/

        asset = ScriptableObject.CreateInstance<WallSpell>();
        AssetDatabase.CreateAsset(asset, "Assets/Resources/Spells/Wall/WallSpell.asset");

		PlayerControl[] players = ScriptableObject.FindObjectsOfType(typeof(PlayerControl)) as PlayerControl[];
		foreach (PlayerControl player in players)
		{
			if (!GameObject.Find(player.player+" Background Mana Bar"))
			{
				GameObject Bar = new GameObject(player.player+" Background Mana Bar");
				Bar.transform.parent = GameObject.Find (player.player+" interface").transform;
				Bar.AddComponent("GUITexture");
				
				Texture2D tex_Bar = new Texture2D(1,1);
				tex_Bar.SetPixel(1,1,Color.black);
				tex_Bar.Apply();
				
				Bar.guiTexture.texture = tex_Bar;
				
				Bar.transform.localScale = new Vector3(0,0,0);
				Bar.transform.position = new Vector3(0,0,0.1f);
			}
			
			if (!GameObject.Find(player.player+" Foreground Mana Bar"))
			{
				GameObject Bar = new GameObject(player.player+" Foreground Mana Bar");
				Bar.transform.parent = GameObject.Find (player.player+" interface").transform;
				Bar.AddComponent("GUITexture");
				
				Texture2D tex_Bar = new Texture2D(1,1);
				tex_Bar.SetPixel(1,1,Color.blue);
				tex_Bar.Apply();
				
				Bar.guiTexture.texture = tex_Bar;
				
				Bar.transform.localScale = new Vector3(0,0,0);
				Bar.transform.position = new Vector3(0,0,0.1f);
			}

			if (!GameObject.Find(player.player+" Background Health Bar"))
			{
				GameObject Bar = new GameObject(player.player+" Background Health Bar");
				Bar.transform.parent = GameObject.Find (player.player+" interface").transform;
				Bar.AddComponent("GUITexture");
				
				Texture2D tex_Bar = new Texture2D(1,1);
				tex_Bar.SetPixel(1,1,Color.black);
				tex_Bar.Apply();
				
				Bar.guiTexture.texture = tex_Bar;
				
				Bar.transform.localScale = new Vector3(0,0,0);
				Bar.transform.position = new Vector3(0,0,0);
			}

			if (!GameObject.Find(player.player+" Foreground Health Bar"))
			{
				GameObject Bar = new GameObject(player.player+" Foreground Health Bar");
				Bar.transform.parent = GameObject.Find (player.player+" interface").transform;
				Bar.AddComponent("GUITexture");
				
				Texture2D tex_Bar = new Texture2D(1,1);
				tex_Bar.SetPixel(1,1,Color.white);
				tex_Bar.Apply();
				
				Bar.guiTexture.texture = tex_Bar;
				
				Bar.transform.localScale = new Vector3(0,0,0);
				Bar.transform.position = new Vector3(0,0,0.1f);
			}

			for (int i = 1; i <= 3; i++)
			{
				if (!GameObject.Find (player.player+" Spell "+i+" icon"))
				{
					GameObject icon = new GameObject(player.player+" Spell "+i+" icon");
					icon.transform.parent = GameObject.Find (player.player+" interface").transform;
					icon.AddComponent("GUITexture");

					icon.transform.localScale = new Vector3(0,0,0);
					icon.transform.position = new Vector3(0,0,0);
				}

				if (!GameObject.Find (player.player+" Spell "+i+" icon Shadow"))
				{
					GameObject icon = new GameObject(player.player+" Spell "+i+" icon Shadow");
					icon.transform.parent = GameObject.Find (player.player+" interface").transform;
					icon.AddComponent("GUITexture");

					Texture2D shadow = new Texture2D(1,1);
					Color t_black = Color.black;
					t_black.a = 0.8f;
					shadow.SetPixel(1,1,t_black);
					shadow.Apply();

					icon.guiTexture.texture = shadow;
					
					icon.transform.localScale = new Vector3(0,0,0);
					icon.transform.localPosition = new Vector3(0,0,0.2f);
				}

				if (!GameObject.Find (player.player+" Spell "+i+" icon ManaShadow"))
				{
					GameObject icon = new GameObject(player.player+" Spell "+i+" icon ManaShadow");
					icon.transform.parent = GameObject.Find (player.player+" interface").transform;
					icon.AddComponent("GUITexture");
					
					Texture2D shadow = new Texture2D(1,1);
					Color t_blue = Color.blue;
					t_blue.a = 0.8f;
					shadow.SetPixel(1,1,t_blue);
					shadow.Apply();
					
					icon.guiTexture.texture = shadow;
					
					icon.transform.localScale = new Vector3(0,0,0);
					icon.transform.localPosition = new Vector3(0,0,0.1f);
				}
			}
		}
		
		CastComponent[] components = ScriptableObject.FindObjectsOfType(typeof(CastComponent)) as CastComponent[];
		foreach (CastComponent component in components)
		{
			if (!component.transform.Find("Background Cast Bar"))
			{
				GameObject Bar = new GameObject("Background Cast Bar");
				Bar.transform.parent = component.transform;
				Bar.AddComponent("SpriteRenderer");
				
				Texture2D tex_Bar = new Texture2D(1,1);
				tex_Bar.SetPixel(1,1,Color.red);
				tex_Bar.Apply();
				((SpriteRenderer)Bar.renderer).sprite = Sprite.Create(tex_Bar,new Rect(1,1,1,1),new Vector2(0.5f, 0.5f));
				
				Bar.transform.localScale = new Vector3(150, 20, 0);
				Bar.transform.localPosition = 0.75f * Vector3.down;
				Bar.transform.rotation = Quaternion.identity;
				Bar.SetActive(false);
			}
			
			if (!component.transform.Find("Foreground Cast Bar"))
			{
				GameObject Bar = new GameObject("Foreground Cast Bar");
				Bar.transform.parent = component.transform;
				Bar.AddComponent("SpriteRenderer");
				Bar.renderer.sortingOrder = 10;
				
				Texture2D tex_Bar = new Texture2D(1,1);
				tex_Bar.SetPixel(1,1,Color.green);
				tex_Bar.Apply();
				((SpriteRenderer)Bar.renderer).sprite = Sprite.Create(tex_Bar,new Rect(1,1,1,1),new Vector2(0.5f, 0.5f));
				
				Bar.transform.localScale = new Vector3(0, 20, 0);
				Bar.transform.localPosition = 0.75f * Vector3.down;
				Bar.transform.rotation = Quaternion.identity;
				Bar.SetActive(false);
			}

			if (!component.transform.Find("Partial Charge Cast Bar"))
			{
				GameObject Bar = new GameObject("Partial Charge Cast Bar");
				Bar.transform.parent = component.transform;
				Bar.AddComponent("SpriteRenderer");
				Bar.renderer.sortingOrder = 5;
				
				Texture2D tex_Bar = new Texture2D(1,1);
				tex_Bar.SetPixel(1,1,Color.black);
				tex_Bar.Apply();
				((SpriteRenderer)Bar.renderer).sprite = Sprite.Create(tex_Bar,new Rect(1,1,1,1),new Vector2(0.5f, 0.5f));
				
				Bar.transform.localScale = new Vector3(0, 20, 0);
				Bar.transform.localPosition = 0.75f * Vector3.down;
				Bar.transform.rotation = Quaternion.identity;
				Bar.SetActive(false);
			}
		}
	}
}
#endif