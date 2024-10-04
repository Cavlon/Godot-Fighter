using Godot;
using System;

public partial class State_JumpSquat : StateCharacter
{

    public State_JumpSquat(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "JUMPSQUAT";
        type = "STAND";
    }

    public override void EnterState()
    {
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

        if (parent.frame > GlobalVariables.Instance.JUMP_SQUAT_DURATION-1) {

            sbyte moveDir = 0;

            if (Input.IsActionPressed("right_" + parent.id)) moveDir += 1;
            if (Input.IsActionPressed("left_" + parent.id)) moveDir -= 1;

            parent.velocity.X = parent.WALKSPEED * moveDir;
            parent.velocity.Y = -GlobalVariables.Instance.JUMPFORCE;
            return stateMachine.states["JUMP"];
        }
        return null;
    }

}
