using UnityEngine;
using System.Collections;

public class PlayerMoveState : StateMachine.State
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
            if (pc.inputComponent.getFire(i + 1))
            {
                pc.castComponent.spellBook.Choose(i);
                if (pc.castComponent.spellBook.Get().CanCast())
                    pc.sm.set(pc.sm.states[PlayerControl.States.Cast]);
            }
        }
    }
    public override void FixedUpdate()
    {
        if (pc.inputComponent.getHorizontal() != 0 || pc.inputComponent.getVertical() != 0)
        {
            pc.animator.SetBool("Moving", true);
            moveComponent.direction.vector = new Vector3(pc.inputComponent.getHorizontal(), pc.inputComponent.getVertical());

            pc.rigidbody.AddForce(moveComponent.direction.vector * moveComponent.speed * moveComponent.mod_speed);
        }
        else
        {
            pc.animator.SetBool("Moving", false);
        }

        
    }
    public override void Exit()
    {
        pc.rigidbody.angularVelocity = 0;
        moveComponent.direction.angle = pc.rigidbody.rotation;
#if UNITY_EDITOR
        //Debug.Log("Move: Exit");
#endif
    }
}