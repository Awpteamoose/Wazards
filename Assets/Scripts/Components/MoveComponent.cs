using UnityEngine;
using System.Collections;

public class MoveComponent: MonoBehaviour
{
	public float speed = 40f;
	public float rotation_factor = 1f;

    public float mod_speed = 1f;

    public Direction direction;

	void Start()
	{
		direction.angle = transform.rotation.eulerAngles.z;
	}
}