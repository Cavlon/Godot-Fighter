using Godot;
using System;
using System.Linq;

public partial class StateMachineHyde : StateMachineCharacter
{

    public override void InitialiseStates()
    {
        base.InitialiseStates();
        AddState(new StateHyde_5H(this, parent));
        AddState(new StateHyde_5L(this, parent));
    }

    public override StateCharacter Attack()
    {
        if (InputBuffer.IsActionPressBuffered("heavy_" + parent.id)) {
            return states["5H"];
        }
        if (InputBuffer.IsActionPressBuffered("light_" + parent.id)) {
            return states["5L"];
        }
        return null;
    }

}
