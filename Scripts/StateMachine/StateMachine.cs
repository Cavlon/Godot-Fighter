using Godot;
using System;
using System.Collections.Generic;
using System.Dynamic;

public abstract partial class StateMachine : Node
{

    protected State state = null;
    public Dictionary<string, State> states = new Dictionary<string, State>();

    public virtual void StateMachineProcess(double delta)
    {
        if (state != null) {
            State transition = state.StateLogic(delta);
            if (transition != null) {
                Transition(transition);
            }
        }
    }

    public void Transition(State new_state) {
        state.ExitState();
        state = new_state;
        state.EnterState();
    }

    public void AddState(State new_state) {
        states.Add(new_state.stateName, new_state);
    }

    public abstract void InitialiseStates();
}
