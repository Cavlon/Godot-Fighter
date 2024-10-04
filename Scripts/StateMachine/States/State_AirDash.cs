using Godot;
using System;

public partial class State_AirDash : StateCharacter
{

    bool startup = true;

    public State_AirDash(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "AIRDASH";
        type = "AIR";
    }

    public override void EnterState()
    {
        parent.velocity.X = 0;
        parent.velocity.Y = 0;
        parent.airMoves++;
        parent.landing_lag_frames = 8;
        startup = true;
        parent.PlayAnimation("crouch");
    }

    public override void ExitState()
    {
        return;
    }

    public override StateCharacter StateLogic(double delta)
    {

        if (parent.frame > GlobalVariables.Instance.AIR_MOVE_ANTICIPATION-1) {

            if (startup) {
                parent.PlayAnimation("airdash");
                parent.velocity.X = parent.DASHSPEED * parent.dir;
                startup = false;
            }

            if (parent.frame > parent.AIR_DASH_DURATION-1) return stateMachine.states["FALL"];
        }
        return null;
    }

}
