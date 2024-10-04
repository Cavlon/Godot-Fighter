using Godot;
using System;

public partial class State_CrouchHitStun : StateCharacter
{
    public State_CrouchHitStun(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "CROUCHHITSTUN";
        type = "CROUCH";
    }


    public override void EnterState()
    {
        parent.PlayAnimation("hit_crouch");
    }

    public override void ExitState()
    {
        return;
    }

    public override StateCharacter StateLogic(double delta)
    {
        if (parent.frame > parent.hitstun-1) {
            if (Input.IsActionPressed("down_" + parent.id)) return stateMachine.states["CROUCH"];
            parent.PlayAnimation("uncrouch");
            return stateMachine.states["IDLE"];
        }

        if (parent.velocity.X > 0) parent.velocity.X = Math.Clamp(parent.velocity.X - (parent.traction * 1.5f), 0, parent.velocity.X);
        else if (parent.velocity.X < 0) parent.velocity.X = Math.Clamp(parent.velocity.X + (parent.traction * 1.5f), parent.velocity.X, 0);

        return null;
    }

}
