using Godot;
using System;

public partial class Camera : Camera2D
{

    private Node2D target;
    private Area2D leftBound;
    private Area2D rightBound;
    private int width;
    private Vector2 screenCenter;

    public override void _Ready()
    {
        target = GetParent().GetNode<Node2D>("CameraTarget");
        leftBound = GetNode<Area2D>("LeftBound");
        rightBound = GetNode<Area2D>("RightBound");

        width = (int)(GetViewport().GetVisibleRect().Size.X / Zoom.X);
    }

    public override void _Process(double delta)
    {
        Position = target.Position;
        screenCenter = GetScreenCenterPosition();
        leftBound.GlobalPosition = new Vector2(screenCenter.X - width/2, 0);
        rightBound.GlobalPosition = new Vector2(screenCenter.X + width/2, 0);
    }

}
