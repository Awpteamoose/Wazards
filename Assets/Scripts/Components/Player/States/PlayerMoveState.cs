﻿using UnityEngine;
using System.Collections;

public class PlayerMoveState: StateMachine.State
{
	private PlayerControl pc;
	private MoveComponent moveComponent;
	
	public PlayerMoveState(PlayerControl parent)
	{
		pc = parent;
		moveComponent = pc.GetComponent<MoveComponent>();
	}
	public override void Start()
	{
		#if UNITY_EDITOR
		//Debug.Log("Move: Enter");
		#endif
	}
	public override void Update()
	{
		for (int i = 0; i < 6; i++)
		{
			if (pc.inputComponent.getFire(i+1))
			{
				pc.castComponent.spellBook.Choose(i);
				if (pc.castComponent.spellBook.Get().CanCast())
					pc.sm.set(pc.sm.states[PlayerControl.States.Cast]);
			}
		}
	}
	public override void FixedUpdate()
	{
		if (pc.inputComponent.getHorizontal() != 0 || pc.inputComponent.getVertical() != 0 )
		{
            pc.animator.SetBool("Moving", true);
			moveComponent.direction.vector = new Vector3 (pc.inputComponent.getHorizontal(), pc.inputComponent.getVertical());
			
			pc.rigidbody2D.AddForce (moveComponent.direction.vector * moveComponent.speed * moveComponent.mod_speed);
		}
        else
        {
            pc.animator.SetBool("Moving", false);
        }
		
		float angle = Mathf.Round (pc.rigidbody2D.rotation);
		float difference = Mathf.Round(Mathf.Rad2Deg * Mathf.Atan2 (Mathf.Sin (Mathf.Deg2Rad * moveComponent.direction.angle - Mathf.Deg2Rad * angle), Mathf.Cos (Mathf.Deg2Rad * moveComponent.direction.angle - Mathf.Deg2Rad * angle)));
		pc.rigidbody2D.angularVelocity = 0;
		if (Mathf.Abs (difference) > 0.1f) {
			if (difference > 0)
				pc.rigidbody2D.AddTorque (Mathf.Abs (difference) * moveComponent.rotation_factor);
			else
				pc.rigidbody2D.AddTorque (Mathf.Abs (difference) * -moveComponent.rotation_factor);
		}
	}
	public override void Exit()
	{
		pc.rigidbody2D.angularVelocity = 0;
		moveComponent.direction.angle = pc.rigidbody2D.rotation;
		#if UNITY_EDITOR
		//Debug.Log("Move: Exit");
		#endif
	}
}