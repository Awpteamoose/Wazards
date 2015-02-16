using UnityEngine;
using System.Collections;

public class PlayerHealthComponent : HealthComponent {

    public float totalDamage = 0f;
    public float lavaDamagePerSec = 1f;

    protected Collider2D groundCollider;
    protected bool grounded = true;

	public static TypogenicText damageIndicator;

    public PlayerControl playerControl { get; set; }

    protected override void Awake()
    {
        base.Awake();

        playerControl = GetComponent<PlayerControl>();
        groundCollider = GameObject.Find("Ground").collider2D;
        if (!damageIndicator)
            damageIndicator = Resources.Load("Placeholder/Damage Indicator", typeof(TypogenicText)) as TypogenicText;
        damageIndicator.CreatePool(25);
    }

    protected override void Update()
    {
        if (!grounded)
            health -= lavaDamagePerSec * Time.deltaTime;
        if (health <= 0f || transform.position.magnitude > killAt)
        {
            PlayerControl.activePlayers.Remove(this.playerControl);
            transform.parent.gameObject.SetActive(false);
        }
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
        totalDamage += damage;
        //old knockback formula
        //rigidbody2D.AddForce(direction * (damageTaken * 0.4f + damage * 1.5f));

        //http://www.ssbwiki.com/Knockback
        rigidbody2D.AddForce(direction * ( (((totalDamage * 0.1f) + ((totalDamage * damage) * 0.05f)) * 0.75f) + 4f ) * scale, ForceMode2D.Impulse);

        TypogenicText indicator = damageIndicator.Spawn();
        if (damage % 1f < 0.01 || damage % 1f > 0.99)
            indicator.Text = damage.ToString("F0");
        else
            indicator.Text = damage.ToString("F2");
        float timesAverage = Mathf.Clamp((damage / 5f), 0.25f, 4f);
        float sizemod = (timesAverage - 1f) / 2f + 1f;
        indicator.Size = sizemod * indicator.Size;
        indicator.transform.position = new Vector3(transform.position.x, transform.position.y + indicator.Size / 14.05f, -0.5f);
        Direction adjustedDirection = new Direction(direction);
        adjustedDirection.angle += Random.Range(-30, 30);
        indicator.rigidbody2D.AddForce(-adjustedDirection.vector * 2.5f, ForceMode2D.Impulse);

        Camera.main.Shake((damage / 40f) * (totalDamage / 100f));
    }
}
