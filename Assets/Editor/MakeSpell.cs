// C#
// Creates a simple wizard that lets you create a Light GameObject
// or if the user clicks in "Apply", it will set the color of the currently
// object selected to red

using UnityEditor;
using UnityEngine;
using System.Collections;

public class MakeSpell : EditorWindow
{
    public new string name = "";
    private string spellCode;
    private string spellProjectileCode;
    private string resPath;
    private bool wantRefresh = false;
    private bool _wantRefresh = false;
    private bool refreshing;

    [MenuItem("Assets/Create/Make a spell")]
    static void ShowWindow()
    {
        EditorWindow t_window = EditorWindow.GetWindow(typeof(MakeSpell), true, "Make a spell", true);
        t_window.maxSize = new Vector2(250f, 40f);
        t_window.minSize = new Vector2(250f, 40f);
    }

    void OnGUI ()
    {
        if (_wantRefresh)
        {
            GUILayout.Label("Please wait...");
        }
        else
        {
            name = EditorGUILayout.TextField("Spell name: ", name);

            if (GUILayout.Button("Create"))
            {
                UpdateSpellName();
                System.IO.Directory.CreateDirectory("Assets/Scripts/Spells/" + name);
                System.IO.File.WriteAllText("Assets/Scripts/Spells/" + name + "/" + name + "Spell.cs", spellCode);
                System.IO.File.WriteAllText("Assets/Scripts/Spells/" + name + "/" + name + "Projectile.cs", spellProjectileCode);

                resPath = "Assets/Resources/Spells/" + name;
                System.IO.Directory.CreateDirectory(resPath);
                AssetDatabase.CopyAsset("Assets/Resources/Placeholder/genericIcon.png", resPath + "/" + name + "_icon.png");
                AssetDatabase.CopyAsset("Assets/Resources/Placeholder/genericProjectile.png", resPath + "/" + name + ".png");
                AssetDatabase.CopyAsset("Assets/Resources/Placeholder/Generic Projectile.prefab", resPath + "/" + name + ".prefab");
                refreshing = false;
                wantRefresh = true;
                AssetDatabase.Refresh();
            }
        }

        if (wantRefresh && EditorApplication.isCompiling)
            refreshing = true;

        if (wantRefresh && refreshing && !EditorApplication.isCompiling)
        {
            wantRefresh = false;
            refreshing = false;
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance(name + "Spell"), resPath + "/" + name + "Spell.asset");

            GameObject prefab = AssetDatabase.LoadAssetAtPath(resPath + "/" + name + ".prefab", typeof(GameObject)) as GameObject;
            Sprite sprite = AssetDatabase.LoadAssetAtPath(resPath + "/" + name + ".png", typeof(Sprite)) as Sprite;
            ///Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(texture.width / 2f, texture.height / 2f));
            prefab.GetComponent<SpriteRenderer>().sprite = sprite;
            prefab.AddComponent(name + "Projectile");
            prefab.AddComponent<HealthComponent>();

            Spell spell = AssetDatabase.LoadAssetAtPath(resPath + "/" + name + "Spell.asset", typeof(Spell)) as Spell;
            spell.icon = AssetDatabase.LoadAssetAtPath(resPath + "/" + name + "_icon.png", typeof(Sprite)) as Sprite;
            spell.prefab = prefab.transform;
            AssetDatabase.Refresh();
        }

        if (Event.current.type == EventType.Repaint)
        {
            _wantRefresh = wantRefresh;
        }
        
        this.Repaint();
    }

    void UpdateSpellName()
    {
        spellCode
            = @"using UnityEngine;
using System.Collections;

[System.Serializable]
public class " + name + @"Spell : Spell
{
    public float damage;
    public float damageCharged;
    public float speed;
    public float speedCharged;
	public float size;
    public float sizeCharged;
	
	public override void cast(bool charged, Vector3 reticle, PlayerControl owner)
	{
		Transform transform = Instantiate(prefab, owner.transform.position+(owner.moveComponent.direction.vector*1f), Quaternion.identity) as Transform;
		" + name + @"Projectile projectile = transform.GetComponent<" + name + @"Projectile>();
        ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
		projectile.target = reticle;
		projectile.parent= owner.gameObject;
		
		if (charged) 
		{
			projectile.size = sizeCharged;
            projectile.speed = speedCharged;
            projectile.damage = damageCharged;
		}
		else
		{
			projectile.size = size;
			projectile.speed = speed;
			projectile.damage = damage;
		}


        projectileHealth.maxHealth = projectile.damage;
        projectileHealth.projectileComponent = projectile;
	}
}";
        spellProjectileCode
            = @"using UnityEngine;
using System.Collections;

public class " + name + @"Projectile : ProjectileComponent
{
    // Use this for initialization
    public override void Start ()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate ()
    {
        base.FixedUpdate();
        transform.rotation = Quaternion.Euler (0, 0, direction.angle);
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Die()
    {
        base.Die();
    }

    public override void Collide(Collider2D collider, HealthComponent healthComponent, bool isParent, bool sameParent)
    {
        if (!isParent && !sameParent)
        {
            base.Collide(collider, healthComponent, isParent, sameParent);

            healthComponent.TakeDamage(damage, direction.vector);
            if (!(healthComponent is ProjectileHealthComponent))
            {
                Die();
            }
        }
    }
}";
    }
}