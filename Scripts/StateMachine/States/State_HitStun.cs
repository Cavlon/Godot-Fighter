using Godot;
using System;

public partial class State_HitStun : StateCharacter
{
    public State_HitStun(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "HITSTUN";
        type = "STAND";
    }


    public override void EnterState()
    {
        parent.PlayAnimation("hit");
    }

    public override void ExitState()
    {
        return;
    }

    public override StateCharacter StateLogic(double delta)
    {
        if (parent.frame > parent.hitstun-1) {
            return stateMachine.states["IDLE"];
        }

        if (parent.velocity.X > 0) parent.velocity.X = Math.Clamp(parent.velocity.X - parent.traction, 0, GlobalVariables.Instance.HORIZ_MAX_SPEED);
        else if (parent.velocity.X < 0) parent.velocity.X = Math.Clamp(parent.velocity.X + parent.traction, -GlobalVariables.Instance.HORIZ_MAX_SPEED, 0);

        return null;
    }

}
