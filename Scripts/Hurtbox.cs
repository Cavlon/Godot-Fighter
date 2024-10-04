using Godot;
using System;

public partial class Hurtbox : Area2D
{

    public Character parent;
    private CollisionShape2D hurtbox;
    private string state;

    public ushort width = 300;
    public ushort height = 300;

    public override void _Ready()
    {
        hurtbox = GetNode<CollisionShape2D>("HurtboxShape");
        hurtbox.Shape = new RectangleShape2D();
        SetPhysicsProcess(false);
    }

    public void SetParameters(ushort _width, ushort _height, Vector2 _position) {
        state = parent.stateName;
        width = _width;
        height = _height;
        Position = _position;
        UpdateExtents();
        SetPhysicsProcess(true);
    }

    private void UpdateExtents() {
        RectangleShape2D rect = (RectangleShape2D)hurtbox.Shape;
        rect.Size = new Vector2(width, height);
    }

}
