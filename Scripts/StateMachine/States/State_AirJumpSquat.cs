using Godot;
using System;

public partial class State_AirJumpSquat : StateCharacter
{

    public State_AirJumpSquat(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "AIRJUMPSQUAT";
        type = "AIR";
    }

    public override void EnterState()
    {
        parent.velocity.X = 0;
        parent.velocity.Y = 0;
        parent.airMoves++;
        parent.landing_lag_frames = 8;
        parent.PlayAnimation("jump");
        parent.UpdateSpriteDir();
    }

    public override void ExitState()
    {
        return;
    }

    public override StateCharacter StateLogic(double delta)
    {
        parent.UpdateSpriteDir();

        if (parent.frame > GlobalVariables.Instance.AIR_MOVE_ANTICIPATION-1) {
            sbyte moveDir = 0;

            if (Input.IsActionPressed("right_" + parent.id)) moveDir += 1;
            if (Input.IsActionPressed("left_" + parent.id)) moveDir -= 1;

            parent.velocity.X = parent.WALKSPEED * moveDir;
            parent.velocity.Y = -GlobalVariables.Instance.AIRJUMPFORCE;
            return stateMachine.states["JUMP"];
        }
        return null;
    }

}
