using Godot;
using System;

public partial class StateHyde_5L : StateCharacter
{

    Hyde hyde;

    public StateHyde_5L(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        hyde = (Hyde)parent;
        stateName = "5L";
    }

    public override void EnterState()
    {
        parent.PlayAnimation("5L");
    }

    public override void ExitState()
    {
        return;
    }

    public override StateCharacter StateLogic(double delta)
    {
        if (parent.frame > 29) {
            return stateMachine.states["IDLE"];
        }

        if (parent.velocity.X > 0) parent.velocity.X = Math.Clamp(parent.velocity.X - parent.traction, 0, parent.velocity.X);
        else if (parent.velocity.X < 0) parent.velocity.X = Math.Clamp(parent.velocity.X + parent.traction, parent.velocity.X, 0);

        return null;
    }

}
