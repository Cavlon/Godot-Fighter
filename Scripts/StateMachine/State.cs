using Godot;
using System;

public abstract partial class State : Node
{

    public StateMachine stateMachine;
    public string stateName;

    public State(StateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract State StateLogic(double delta);

}
