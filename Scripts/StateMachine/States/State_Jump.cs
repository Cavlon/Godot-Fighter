using Godot;
using System;

public partial class State_Jump : StateCharacter
{

    public State_Jump(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "JUMP";
        type = "AIR";
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
        return;
    }

    public override StateCharacter StateLogic(double delta)
    {
        parent.UpdateSpriteDir();

        if (parent.velocity.Y > 0) {
            return stateMachine.states["FALL"];
        }

        if (parent.airMoves < parent.AIR_MOVES_MAX) {
            if (InputBuffer.IsActionPressBuffered("up_" + parent.id)) return stateMachine.states["AIRJUMPSQUAT"];
            if (Input.IsActionJustPressed("dash_" + parent.id)) {
                sbyte moveDir = 0;

                if (Input.IsActionPressed("right_" + parent.id)) moveDir += 1;
                if (Input.IsActionPressed("left_" + parent.id)) moveDir -= 1;

                if (parent.dir * moveDir > -1) return stateMachine.states["AIRDASH"];
                else return stateMachine.states["AIRBACKDASH"];
            }
        }

        parent.velocity.Y = Math.Clamp(parent.velocity.Y + parent.additionalGravity + GlobalVariables.Instance.GRAVITY, parent.velocity.Y, parent.effectiveFallSpeed);

        return null;
    }


}
