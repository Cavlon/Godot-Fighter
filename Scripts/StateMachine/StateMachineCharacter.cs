using Godot;
using System;
using System.Collections.Generic;

public abstract partial class StateMachineCharacter : StateMachine
{

    public Character parent;
    protected new StateCharacter state = null;
    public new Dictionary<string, StateCharacter> states = new Dictionary<string, StateCharacter>();
    private Vector2 effectiveVelocity = new Vector2();


    protected readonly string[] crouchStates = new string[] {"CROUCH"};
    protected readonly string[] groundStates = new string[] {"IDLE", "WALK", "DASH", "LANDING", "JUMPSQUAT", "DASHJUMPSQUAT", "BACKDASH"};

    public override void _Ready()
    {
        parent = (Character)GetParent();
        InitialiseStates();
        parent.Ready += ParentReady;
    }

    public override void InitialiseStates()
    {
        AddState(new State_Idle(this, parent));
        AddState(new State_Jump(this, parent));
        AddState(new State_Fall(this, parent));
        AddState(new State_Backdash(this, parent));
        AddState(new State_Dash(this, parent));
        AddState(new State_DashJumpSquat(this, parent));
        AddState(new State_JumpSquat(this, parent));
        AddState(new State_Landing(this, parent));
        AddState(new State_Walk(this, parent));
        AddState(new State_Crouch(this, parent));
        AddState(new State_AirJumpSquat(this, parent));
        AddState(new State_HitStun(this, parent));
        AddState(new State_CrouchHitStun(this, parent));
        AddState(new State_AirBackdash(this, parent));
        AddState(new State_AirDash(this, parent));
    }

    public void ParentReady() {
        state = states["IDLE"];
        parent.stateName = "IDLE";
        parent.Damaged += OnHit;
        state.EnterState();
    }

    public override void StateMachineProcess(double delta)
    {
        parent.invincible = false;
        
        // Perform state logic and transition checks
        if (state != null) {
            StateCharacter transition = state.StateLogic(delta);
            parent.UpdateFrames(delta);
            if (transition != null) {
                Transition(transition);
            }
        }

        parent.stateLabel.Text = state.stateName;
    }

    public void ConsolidateVelocities() {
        parent.apparentVelocity = parent.velocity;
        parent.apparentVelocity.X += parent.animVel * Engine.MaxFps;
    }

    public void VelocityLogic() {
        // The velocity the player thinks they have is different to the velocity they actually have
        // This is done to apply external forces on the velocity without affecting subsequent frames
        effectiveVelocity = parent.apparentVelocity;
        ushort appliedWeight = parent.opponent.effectiveWeight;

        // Logic to push players out of eachother
        foreach (Area2D area in parent.collisionArea.GetOverlappingAreas())
        {
            if (area.Owner is Character opponent)
            {

                appliedWeight = opponent.effectiveWeight;

                // Don't push players into the wall
                if (opponent.wallDir != 0) {
                    if (opponent.wallDir == -1) effectiveVelocity.X = Math.Clamp(effectiveVelocity.X, 0, GlobalVariables.Instance.HORIZ_MAX_SPEED);
                    else effectiveVelocity.X = Math.Clamp(effectiveVelocity.X, -GlobalVariables.Instance.HORIZ_MAX_SPEED, 0);
                    appliedWeight += parent.effectiveWeight;
                }

                // The ratio of how deep within the opponent the player is
                float factor = ((opponent.Position.X - parent.Position.X) * parent.dir * 2) / ((parent.collisionDims.X + opponent.collisionDims.X));
                // Add a small deadzone so intentional pushing is possible
                factor = Math.Clamp(0.9f - factor, 0, 1);

                // Apply internal pushing force according to the depth ratio, stronger force the closer they are
                effectiveVelocity.X += GlobalVariables.Instance.INTERNAL_FORCE * -parent.dir * factor * appliedWeight;

                if (opponent.wallDir == 0) {
                    // Conservation of momentum to model collisions with pushing
                    float relVelocity = Math.Abs((opponent.apparentVelocity.X - parent.apparentVelocity.X));
                    effectiveVelocity.X += relVelocity * (appliedWeight / (float)(parent.effectiveWeight + appliedWeight)) * -parent.dir;
                }
            }
        }

        // Forcibly stop the player from being pushed into the wall
        // if (parent.wallDir == -1) effectiveVelocity.X = Math.Clamp(effectiveVelocity.X, 0, GlobalVariables.Instance.HORIZ_MAX_SPEED);
        // else if (parent.wallDir == 1) effectiveVelocity.X = Math.Clamp(effectiveVelocity.X, -GlobalVariables.Instance.HORIZ_MAX_SPEED, 0);
    }

    public void ApplyVelocity() {
        parent.Position += effectiveVelocity / Engine.MaxFps;
    }

    public void Transition(StateCharacter new_state) {
        GD.Print(state.stateName + " to " + new_state.stateName);
        parent.animQueue.Clear();
        state.ExitState();
        state = new_state;
        parent.Frame();
        parent.stateName = state.stateName;
        state.EnterState();
    }

    public void AddState(StateCharacter new_state) {
        states.Add(new_state.stateName, new_state);
    }

    public void OnHit() {
        // Prevents multiple hits when a single hitbox touches multiple hurtboxes
        parent.invincible = true;
        if (state.type == "STAND") {
            Transition(states["HITSTUN"]);
        } else if (state.type == "CROUCH") {
            Transition(states["CROUCHHITSTUN"]);
        }
    }

    public abstract StateCharacter Attack();

}
