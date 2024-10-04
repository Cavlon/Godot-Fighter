using Godot;
using System;

public partial class State_Backdash : StateCharacter
{

    public State_Backdash(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "BACKDASH";
        type = "STAND";
    }

    public override void EnterState()
    {
        parent.velocity.X = parent.DASHSPEED * -parent.dir;
        parent.PlayAnimation("backdash");
    }

    public override void ExitState()
    {
        parent.velocity.X = 0;
    }

    public override StateCharacter StateLogic(double delta)
    {
        if (parent.frame > parent.BACKDASH_DURATION-1) return stateMachine.states["IDLE"];
        return null;
    }

}
