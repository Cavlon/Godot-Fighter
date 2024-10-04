using Godot;
using System;

public partial class State_DashJumpSquat : StateCharacter
{

    public State_DashJumpSquat(StateMachineCharacter stateMachine, Character parent) : base(stateMachine, parent)
    {
        stateName = "DASHJUMPSQUAT";
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

            if (moveDir == 1) {
                if (parent.dir == 1) parent.velocity.X = parent.RUNSPEED;
                else parent.velocity.X = parent.WALKSPEED;
            } else if (moveDir == -1) {
                if (parent.dir == -1) parent.velocity.X = -parent.RUNSPEED;
                else parent.velocity.X = -parent.WALKSPEED;
            } else {
                parent.velocity.X = 0;
            }
            parent.velocity.Y = -GlobalVariables.Instance.JUMPFORCE;
            return stateMachine.states["JUMP"];
        }
        return null;
    }

}
