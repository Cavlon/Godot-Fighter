using Godot;
using System;

public partial class HealthBar : ProgressBar
{

    private ProgressBar damageBar;
    private int damageTimer;

    [Export]
    private int damageDelay = 60;

    public override void _Ready()
    {
        damageBar = GetNode<ProgressBar>("DamageBar");
    }

    public override void _Process(double delta)
    {
        if (damageTimer > 0) damageTimer -= 1;
        else if (damageBar.Value != Value) damageBar.Value -= 1;
    }

    public void SetHealth(int new_health) {
        Value = Math.Clamp(new_health, 0, (int)MaxValue);
        damageTimer = damageDelay;
    }

    public void Initialise(int max_health) {
        MaxValue = max_health;
        Value = max_health;
        damageBar.MaxValue = max_health;
        damageBar.Value = max_health;
    }

}
