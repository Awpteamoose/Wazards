using UnityEngine;
using System.Collections;

public class PlayerHealthComponent : HealthComponent {

    public float damageTaken = 0f;
    public float lavaDamagePerSec = 1f;

    protected Collider2D groundCollider;
    protected bool grounded = true;

	public Transform damageIndicator;

    public PlayerControl playerControl;

    protected override void Awake()
    {
        base.Awake();

        playerControl = GetComponent<PlayerControl>();
        groundCollider = GameObject.Find("Ground").collider2D;
    }

    protected override void Update()
    {
        if (health <= 0f || transform.position.magnitude > killAt)
			//HACK:
			Application.LoadLevel(Application.loadedLevel);
	}

    protected override void FixedUpdate()
    {
		if (!grounded)
			health-=lavaDamagePerSec * Time.fixedDeltaTime;
	}

    protected override void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider == groundCollider)
        {
            grounded = true;
            health += 10f - health % 10f;
            if (health > maxHealth)
                health = maxHealth;
        } 
	}

    protected override void OnTriggerExit2D(Collider2D collider)
	{
		if (collider == groundCollider)
			grounded = false;
	}

    public override void TakeDamage(float damage, Vector2 direction, float scale = 1f)
    {
        damageTaken += damage;
        //old knockback formula
        //rigidbody2D.AddForce(direction * (damageTaken * 0.4f + damage * 1.5f));

        //new knockback formula
        //7.5 - constant
        //500 - minimal knockback
        //TODO: take base knockback as a parameter?
        //http://www.ssbwiki.com/Knockback
        rigidbody2D.AddForce(direction * ( (((damageTaken * 0.1f) + ((damageTaken * damage) * 0.05f)) * 0.75f/*35f*/) + 4f/*250f*/ ) * scale, ForceMode2D.Impulse);

		Transform newIndicator = Instantiate (damageIndicator) as Transform;
		newIndicator.gameObject.SetActive (true);
		TypogenicText typo = newIndicator.GetComponent<TypogenicText> ();
        if (damage % 1 == 0)
            typo.Text = damage.ToString("F0");
        else
            typo.Text = damage.ToString("F2");
        float timesAverage = Mathf.Clamp((damage / 5f), 0.25f, 4f);
        float sizemod = (timesAverage - 1f) / 2f + 1f;
        typo.Size = sizemod * typo.Size;
        newIndicator.position = new Vector3(transform.position.x, transform.position.y + typo.Size / 14.05f, -0.5f);
		newIndicator.rigidbody2D.AddForce (-direction * 2.5f, ForceMode2D.Impulse);
    }
}
