using Godot;
using System;

public partial class State_Landing : StateCharacter
{

    public State_Landing(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "LANDING";
        type = "STAND";
    }

    public override void EnterState()
    {
        parent.PlayAnimation("land");
        parent.velocity.Y = 0;
        parent.Position = new Vector2(parent.Position.X, 605 - parent.collisionPosY - parent.collisionDims.Y / 2);
        parent.airMoves = 0;
    }

    public override void ExitState()
    {
        parent.landing_lag_frames = 0;
    }

    public override StateCharacter StateLogic(double delta)
    {

        if (parent.frame < 1 + parent.landing_lag_frames) {
            if (parent.velocity.X > 0) {
                parent.velocity.X = Math.Clamp(parent.velocity.X - parent.traction, 0, GlobalVariables.Instance.HORIZ_MAX_SPEED);
            } else if (parent.velocity.X < 0) {
                parent.velocity.X = Math.Clamp(parent.velocity.X + parent.traction, -GlobalVariables.Instance.HORIZ_MAX_SPEED, 0);
            }
        } else {

            sbyte moveDir = 0;

            if (Input.IsActionPressed("right_" + parent.id)) moveDir += 1;
            if (Input.IsActionPressed("left_" + parent.id)) moveDir -= 1;

            if (InputBuffer.IsActionPressBuffered("dash_" + parent.id)) {
                if (parent.dir * moveDir > -1) return stateMachine.states["DASH"]; 
                else return stateMachine.states["BACKDASH"];
            }

            if (Input.IsActionPressed("down_" + parent.id)) {
                parent.anim.Stop();
                return stateMachine.states["CROUCH"];
            }

            return stateMachine.states["IDLE"];
        }
        return null;
    }

}
