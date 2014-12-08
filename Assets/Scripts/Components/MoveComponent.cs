using UnityEngine;
using System.Collections;

public class MoveComponent: MonoBehaviour
{
	public float speed = 40f;
	public float rotation_factor = 1f;

    public float mod_speed = 1f;

    public Direction direction;
    public PlayerControl owner { get; set; }

	void Start()
	{
		direction.angle = transform.rotation.eulerAngles.z;
        owner = GetComponent<PlayerControl>();
	}

    void FixedUpdate()
    {
        float angle = Mathf.Round(owner.rigidbody.rotation);
        float difference = Mathf.Round(Mathf.Rad2Deg * Mathf.Atan2(Mathf.Sin(Mathf.Deg2Rad * direction.angle - Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * direction.angle - Mathf.Deg2Rad * angle)));
        owner.rigidbody.angularVelocity = 0;
        if (Mathf.Abs(difference) > 0.1f)
        {
            if (difference > 0)
                owner.rigidbody.AddTorque(Mathf.Abs(difference) * rotation_factor);
            else
                owner.rigidbody.AddTorque(Mathf.Abs(difference) * -rotation_factor);
        }
        else
        {
            owner.rigidbody.MoveRotation(direction.angle);
        }
    }
}