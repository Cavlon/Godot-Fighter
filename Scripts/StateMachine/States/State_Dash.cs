using Godot;
using System;

public partial class State_Dash : StateCharacter
{

    private sbyte dashDir;

    public State_Dash(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "DASH";
        type = "STAND";
    }

    public override void EnterState()
    {
        parent.velocity.X = parent.RUNSPEED * parent.dir;
        dashDir = parent.dir;
        parent.PlayAnimation("dash");
    }

    public override void ExitState()
    {
        return;
    }

    public override StateCharacter StateLogic(double delta)
    {

        sbyte moveDir = 0;

        if (!parent.groundCheck.IsColliding()) {
            return stateMachine.states["FALL"];
        }

        if (InputBuffer.IsActionPressBuffered("up_" + parent.id)) return stateMachine.states["DASHJUMPSQUAT"];

        if (parent.frame > parent.DASH_DURATION-1) {

            StateCharacter attackTrans = stateMachine.Attack();
            if (attackTrans != null) return attackTrans;

            if (Input.IsActionPressed("down_" + parent.id)) {
                parent.PlayAnimation("crouch");
                return stateMachine.states["CROUCH"];
            }

            if (Input.IsActionPressed("dash_" + parent.id)) return null;

            if (Input.IsActionPressed("right_" + parent.id)) moveDir += 1;
            if (Input.IsActionPressed("left_" + parent.id)) moveDir -= 1;

            if (parent.dir * dashDir == -1) {
                if (moveDir != 0) {
                    parent.velocity.X = moveDir * parent.WALKSPEED;
                    return stateMachine.states["WALK"];
                } else {
                    parent.anim.Stop();
                    return stateMachine.states["IDLE"];
                }
            }

            if (moveDir * dashDir == -1) {
                parent.velocity.X = moveDir * parent.WALKSPEED;
                return stateMachine.states["WALK"];
            } else if (moveDir == 0) {
                parent.anim.Stop();
                return stateMachine.states["IDLE"];
            }

        }

        return null;
    }

}
