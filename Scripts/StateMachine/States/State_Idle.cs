using Godot;
using System;

public partial class State_Idle : StateCharacter
{
    public State_Idle(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "IDLE";
        type = "STAND";
    }

    public override void EnterState()
    {
        parent.additionalGravity = 0;
        parent.totalComboDecay = 0;
        if (parent.anim.IsPlaying()) {
            parent.animQueue.Enqueue("idle");
        } else {
            parent.PlayAnimation("idle");
            parent.UpdateSpriteDir();
        }
    }

    public override void ExitState()
    {
        parent.animQueue.Clear();
    }

    public override StateCharacter StateLogic(double delta)
    {
        parent.UpdateSpriteDir();

        if (!parent.groundCheck.IsColliding()) return stateMachine.states["FALL"];

        if (InputBuffer.IsActionPressBuffered("up_" + parent.id)) return stateMachine.states["JUMPSQUAT"];

        StateCharacter attackTrans = stateMachine.Attack();
        if (attackTrans != null) return attackTrans;

        if (Input.IsActionPressed("down_" + parent.id)) {
            parent.PlayAnimation("crouch");
            return stateMachine.states["CROUCH"];
        }

        sbyte moveDir = 0;

        if (Input.IsActionPressed("right_" + parent.id)) moveDir += 1;
        if (Input.IsActionPressed("left_" + parent.id)) moveDir -= 1;

        if (Input.IsActionJustPressed("dash_" + parent.id)) {
            if (parent.dir * moveDir > -1) return stateMachine.states["DASH"]; 
            else return stateMachine.states["BACKDASH"];
        }

        if (moveDir != 0) {
            parent.velocity.X = moveDir * parent.WALKSPEED;
            return stateMachine.states["WALK"];
        }

        if (parent.velocity.X > 0) parent.velocity.X = Math.Clamp(parent.velocity.X - parent.traction, 0, GlobalVariables.Instance.HORIZ_MAX_SPEED);
        else if (parent.velocity.X < 0) parent.velocity.X = Math.Clamp(parent.velocity.X + parent.traction, -GlobalVariables.Instance.HORIZ_MAX_SPEED, 0);

        return null;
    }

}
