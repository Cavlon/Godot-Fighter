using Godot;
using System;

public partial class HUDManager : CanvasLayer
{

    private Character player1;
    private Character player2;

    private HealthBar player1HealthBar;
    private HealthBar player2HealthBar;
    private RichTextLabel FPSCounter;

    public override void _Ready()
    {
        PlayerManager playerManager = GetParent().GetNode<PlayerManager>("PlayerManager");
        player1 = playerManager.GetNode<Character>("Player1");
        player2 = playerManager.GetNode<Character>("Player2");

        player1.Ready += Player1Ready;
        player2.Ready += Player2Ready;

        player1.Damaged += OnPlayer1Damage;
        player2.Damaged += OnPlayer2Damage;

        player1HealthBar = GetNode<HealthBar>("HealthBar1");
        player2HealthBar = GetNode<HealthBar>("HealthBar2");

        FPSCounter = GetNode<RichTextLabel>("FPSCounter");
    }

    public override void _Process(double delta)
    {
        FPSCounter.Text = "FPS: " + Engine.GetFramesPerSecond();
    }

    private void Player1Ready() {
        player1HealthBar.Initialise(player1.MAX_HEALTH);
    }

    private void Player2Ready() {
        player2HealthBar.Initialise(player2.MAX_HEALTH);
    }

    private void OnPlayer1Damage() {
        player1HealthBar.SetHealth(player1.health);
    }

    private void OnPlayer2Damage() {
        player2HealthBar.SetHealth(player2.health);
    }
}
