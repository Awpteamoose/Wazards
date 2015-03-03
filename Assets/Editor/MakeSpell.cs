using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

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
            prefab.GetComponent<SpriteRenderer>().sprite = sprite;
            //UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(prefab, "Assets/Editor/MakeSpell.cs (63,13)", name + "Projectile");
            prefab.AddComponent(Type.GetType(name + "Projectile"));
            prefab.AddComponent<HealthComponent>();

            Spell spell = AssetDatabase.LoadAssetAtPath(resPath + "/" + name + "Spell.asset", typeof(Spell)) as Spell;
            spell.icon = AssetDatabase.LoadAssetAtPath(resPath + "/" + name + "_icon.png", typeof(Sprite)) as Sprite;
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

    public " + name + @"Projectile prefab;

    public override void Initialise()
    {
        base.Initialise();

        if (prefab.CountPooled() == 0)
            prefab.CreatePool(10);
    }
    
    public override void Cast(float charge, Vector3 reticle)
    {
        " + name + @"Projectile projectile = prefab.Spawn(owner.transform.position + (owner.moveComponent.direction.vector * 1f), Quaternion.identity);
        ProjectileHealthComponent projectileHealth = projectile.GetComponent<ProjectileHealthComponent>();
        projectile.target = reticle;
        projectile.parent= owner.gameObject;
        
        if (charge >= t_charge)
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

        projectile.Activate();
        base.Cast(charge, reticle);
    }
}";
        spellProjectileCode
            = @"using UnityEngine;
using System.Collections;

public class " + name + @"Projectile : ProjectileComponent
{

    public override void Awake()
    {
        base.Awake();
    }

    public override void Activate ()
    {
        base.Activate();
    }

    public override void FixedUpdate ()
    {
        base.FixedUpdate();
        transform.rotation = Quaternion.Euler (0, 0, direction.angle);
        transform.Translate(Vector3.up * speed * Time.fixedDeltaTime);
    }

    public override void Die()
    {
        base.Die();
        gameObject.Recycle();
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