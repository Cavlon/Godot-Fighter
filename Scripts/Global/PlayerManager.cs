using Godot;
using System;

public partial class PlayerManager : Node2D
{

    private StateMachineCharacter player1State;
    private StateMachineCharacter player2State;
    private Character player1;
    private Character player2;

    public override void _Ready()
    {
        player1 = GetNode<Character>("Player1");
        player2 = GetNode<Character>("Player2");
        player1State = player1.GetNode<StateMachineCharacter>("StateMachine");
        player2State = player2.GetNode<StateMachineCharacter>("StateMachine");
    }

    // ORDER OF EXECUTION:
    // Process
    // Physics Process
    // Physics Object Update
    // Deferred Calls

    public override void _Process(double delta)
    {
        // Characters may read opponent's velocity so they need to be in sync before they are applied
        player1State.StateMachineProcess(delta);
        player2State.StateMachineProcess(delta);

        // Animations may affect velocity so they need to be done before they are processed
        player1.AdvanceAnimation();
        player2.AdvanceAnimation();

        player1State.ConsolidateVelocities();
        player2State.ConsolidateVelocities();

        player1State.VelocityLogic();
        player2State.VelocityLogic();

        player1State.ApplyVelocity();
        player2State.ApplyVelocity();

        CallDeferred("DeferredProcess");  
    }

    // Executed in deferred time after physics update (e.g. raycast update)
    public void DeferredProcess() {
        player1.WallCheck();
        player2.WallCheck();

        player1.UpdateDir();
        player2.UpdateDir();
    }

}
