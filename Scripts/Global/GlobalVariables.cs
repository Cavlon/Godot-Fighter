using Godot;
using System;

public partial class GlobalVariables : Node
{

    public static GlobalVariables Instance { get; private set;}
    public readonly ushort FPS = 60;
    public readonly byte GRAVITY = 80;
    public readonly ushort FALLSPEED = 2000;
    public readonly ushort JUMPFORCE = 2000;
    public readonly ushort AIRJUMPFORCE = 1750;
    public readonly ushort HORIZ_MAX_SPEED = 2500;
    public readonly ushort INTERNAL_FORCE = 6;
    public readonly byte JUMP_SQUAT_DURATION = 3;
    public readonly byte AIR_MOVE_ANTICIPATION = 7;
    public readonly PackedScene hitbox = ResourceLoader.Load<PackedScene>("res://Scenes/hitbox.tscn");
    public readonly PackedScene hurtbox = ResourceLoader.Load<PackedScene>("res://Scenes/hurtbox.tscn");

    public override void _Ready()
    {
        Instance = this;
    }

}
