using Godot;
using System;

public partial class State_Crouch : StateCharacter
{

    public State_Crouch(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "CROUCH";
        type = "CROUCH";
    }

    public override void EnterState()
    {
        parent.additionalGravity = 0;
        parent.totalComboDecay = 0;
        if (parent.anim.IsPlaying()){
            parent.animQueue.Enqueue("crouch_idle");
        } else {
            parent.PlayAnimation("crouch_idle");
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

        if (!Input.IsActionPressed("down_" + parent.id)) {
            parent.PlayAnimation("uncrouch");
            return stateMachine.states["IDLE"];
        }

        if (parent.velocity.X > 0) parent.velocity.X = Math.Clamp(parent.velocity.X - (parent.traction * 1.5f), 0, parent.velocity.X);
        else if (parent.velocity.X < 0) parent.velocity.X = Math.Clamp(parent.velocity.X + (parent.traction * 1.5f), parent.velocity.X, 0);

        return null;
    }

}
