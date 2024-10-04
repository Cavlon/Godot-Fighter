using Godot;
using System;

public partial class State_Walk : StateCharacter
{

    public State_Walk(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "WALK";
        type = "STAND";
    }

    public override void EnterState()
    {
        if (parent.velocity.X * parent.dir < 0) {
            parent.PlayAnimation("walk_b");
            parent.UpdateSpriteDir();
        }
        else if (parent.velocity.X * parent.dir > 0) {
            parent.PlayAnimation("walk_f");
            parent.UpdateSpriteDir();
        }
    }

    public override void ExitState()
    {
        return;
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

        if (moveDir == 0) {
            parent.anim.Stop();
            return stateMachine.states["IDLE"];
        }

        parent.velocity.X = moveDir * parent.WALKSPEED;

        if (parent.anim.CurrentAnimation == "walk_f" && parent.velocity.X * parent.dir < 0) {
            parent.Frame();
            parent.PlayAnimation("walk_b");
        }
        else if (parent.anim.CurrentAnimation == "walk_b" && parent.velocity.X * parent.dir > 0) {
            parent.Frame();
            parent.PlayAnimation("walk_f");
        }

        return null;
    }

}
