using Godot;
using System;

public partial class State_AirBackdash : StateCharacter
{

    private sbyte dashDir;

    public State_AirBackdash(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "AIRBACKDASH";
        type = "AIR";
    }

    public override void EnterState()
    {
        dashDir = (sbyte)-parent.dir;
        parent.velocity.X = parent.DASHSPEED * dashDir;
        parent.velocity.Y = 0;
        parent.airMoves++;
        parent.landing_lag_frames = 8;
        parent.PlayAnimation("backdash");
    }

    public override void ExitState()
    {
        parent.velocity.X = parent.RUNSPEED * dashDir;
    }

    public override StateCharacter StateLogic(double delta)
    {
        if (parent.frame > parent.AIR_BACKDASH_DURATION-1) return stateMachine.states["FALL"];
        return null;
    }

}
