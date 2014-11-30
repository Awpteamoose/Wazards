using UnityEngine;
using System.Collections;

public class ProjectileComponent : MonoBehaviour
{
	public GameObject parent;

    public float damage;
	public float speed;
	public float size;
    public Vector3 target;
    public Direction direction;

    protected float killDistance = 50f;

    public Animator animator { get; set; }
    public new Transform transform { get; set; }
    public new SpriteRenderer renderer { get; set; }
    public new Rigidbody2D rigidbody { get; set; }
    public new Collider2D collider { get; set; }

    public virtual void Awake ()
    {
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }

	public virtual void Start ()
    {
        transform.localScale = new Vector2(size, size); //TODO: only the projectiles that want spell-controlled sizes should set this
        direction.vector = (target - parent.transform.position);
	}

    public virtual void FixedUpdate () { }

    public virtual void Update ()
    {
        if (transform.position.magnitude > killDistance)
            Destroy(gameObject);
    }

    protected HealthComponent c_healthComponent;
    public virtual void OnTriggerEnter2D(Collider2D collider) //TODO: should be public virtual
    {
        c_healthComponent = collider.gameObject.GetComponent<HealthComponent>();
        if (c_healthComponent)
        {
            if (collider.gameObject == parent)
                Collide(collider, c_healthComponent, true, false);
            else
            {
                ProjectileComponent pc = collider.gameObject.GetComponent<ProjectileComponent>();
                if (pc && pc.parent == parent)
                {
                    Collide(collider, c_healthComponent, false, true);
                }
                else
                {
                    Collide(collider, c_healthComponent, false, false);
                }
            }
        }
    }

    public virtual void Die()
    {
    }

    public virtual void Deactivate()
    {
    }

    public virtual void Collide(Collider2D collider, HealthComponent healthComponent, bool isParent, bool sameParent)
    {
    }
}