using UnityEngine;
using System.Collections;

public class PlayerHealthComponent : HealthComponent {

    public float damageTaken = 0f;
    public float lavaDamagePerSec = 1f;

    protected Collider2D groundCollider;
    protected bool grounded = true;

	public Transform damageIndicator;

    protected override void Start()
    {
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
		typo.Text = (Mathf.Round(damage * 100f)/100f).ToString();
		newIndicator.position = new Vector3 (transform.position.x, transform.position.y, -0.5f);
		typo.Size = Mathf.Clamp((damage / 10f), 0.35f, 1.65f) * typo.Size;
		newIndicator.rigidbody2D.AddForce (-direction * 2.5f, ForceMode2D.Impulse);
    }
}
