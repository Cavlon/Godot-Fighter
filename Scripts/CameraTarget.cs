using Godot;
using System;

public partial class CameraTarget : Node2D
{

    private Node2D player1;
    private Node2D player2;

    public override void _Ready()
    {
        Node2D playerManager = GetParent().GetNode<Node2D>("PlayerManager");
        player1 = playerManager.GetNode<Node2D>("Player1");
        player2 = playerManager.GetNode<Node2D>("Player2");
    }

    public override void _PhysicsProcess(double delta)
    {
        Position = (player1.Position + player2.Position) * 0.5f;
    }

}
