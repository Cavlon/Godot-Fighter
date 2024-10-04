using Godot;
using System;

public partial class StateHyde_5H : StateCharacter
{

    Hyde hyde;

    public StateHyde_5H(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        hyde = (Hyde)parent;
        stateName = "5H";
    }

    public override void EnterState()
    {
        parent.PlayAnimation("5H");
        parent.effectiveWeight = 500;
    }

    public override void ExitState()
    {
        parent.effectiveWeight = parent.WEIGHT;
    }

    public override StateCharacter StateLogic(double delta)
    {
        

        if (parent.frame > 35) {
            return stateMachine.states["IDLE"];
        }

        if (parent.velocity.X > 0) parent.velocity.X = Math.Clamp(parent.velocity.X - parent.traction, 0, parent.velocity.X);
        else if (parent.velocity.X < 0) parent.velocity.X = Math.Clamp(parent.velocity.X + parent.traction, parent.velocity.X, 0);

        return null;
    }

}
