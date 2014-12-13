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
            moveComponent.direction.vector = new Vector3(pc.inputComponent.getHorizontal(), pc.inputComponent.getVertical());

            pc.rigidbody.AddForce(moveComponent.direction.vector * moveComponent.speed * moveComponent.mod_speed);
        }
    }
    public override void Exit()
    {
        pc.rigidbody.angularVelocity = 0;
        moveComponent.direction.angle = pc.rigidbody.rotation;
    }
}