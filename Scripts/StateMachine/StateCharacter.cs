using Godot;
using System;

public abstract partial class StateCharacter : State
{

    public Character parent;
    public string type;
    public new StateMachineCharacter stateMachine;

    protected StateCharacter(StateMachineCharacter stateMachine, Character parent) : base(stateMachine)
    {
        this.stateMachine = stateMachine;
        this.parent = parent;
    }

    public override abstract StateCharacter StateLogic(double delta);

}
