using Godot;
using System;
using System.Collections.Generic;

public abstract partial class Character : CharacterBody2D
{

    // Meta Constants
    [Export]
    public byte id = 1;


    // Nodes
    public Label stateLabel;
    public RayCast2D leftWallCheck;
    public RayCast2D rightWallCheck;
    public RayCast2D groundCheck;
    public AnimationPlayer anim;
    private AnimatedSprite2D sprite;
    public Character opponent;
    private CollisionShape2D collisionBox;
    public Area2D collisionArea;
    public Node hitboxes;
    public Node hurtboxes;


    // Lists
    public Queue<string> animQueue = new Queue<string>();


    // Auxillary Variables
    public uint frame = 0;
    public short health;
    public string stateName;

    public Vector2 collisionDims = new Vector2();
    public int collisionPosY;

    public Vector2 apparentVelocity = Vector2.Zero;
    public Vector2 velocity = Vector2.Zero;
    public int animVel = 0;

    public sbyte dir = 1;
    public sbyte visualDir = 1;
    public sbyte wallDir = 0;

    public bool onWall = false;
    public bool invincible = false;

    public byte airMoves = 0;
    public ushort effectiveFallSpeed = 0;
    public byte landing_lag_frames = 0;
    public ushort totalComboDecay = 0;
    public byte combo = 0;
    public short additionalGravity = 0;
    public byte hitstun = 0;
    public byte traction = 0;
    public ushort effectiveWeight = 0;
    

    // Exportable Variables

    [ExportCategory("System Stats")]
    [Export]
    public short MAX_HEALTH = 300;
    [Export]
    public byte WEIGHT = 50;

    [ExportCategory("Grounded")]
    [Export]
    public byte DASH_DURATION = 10;
    [Export]
    public byte BACKDASH_DURATION = 10;
    [Export]
    public ushort RUNSPEED = 340;
    [Export]
    public ushort DASHSPEED = 390;
    [Export]
    public ushort WALKSPEED = 200;

    [ExportCategory("Air")]
    [Export]
    public byte AIR_MOVES_MAX = 1;
    [Export]
    public ushort AIR_DASH_SPEED = 500;
    [Export]
    public ushort AIR_DASH_DURATION = 16;
    [Export]
    public ushort AIR_BACKDASH_DURATION = 6;


    [Signal]
    public delegate void DamagedEventHandler();

    public void UpdateFrames(double delta) {
        frame++;
    }
    public void Frame() {
        frame = 0;
    }

    public override void _Ready()
    {
        // Get Child Nodes
        stateLabel = GetNode<Label>("State");

        Node rays = GetNode<Node>("Raycasts");
        leftWallCheck = rays.GetNode<RayCast2D>("LeftWallRay");
        rightWallCheck = rays.GetNode<RayCast2D>("RightWallRay");
        groundCheck = rays.GetNode<RayCast2D>("GroundRay");

        sprite = GetNode<AnimatedSprite2D>("Sprite");
        anim = sprite.GetNode<AnimationPlayer>("AnimationPlayer");

        collisionArea = GetNode<Area2D>("CollisionArea");
        collisionBox = collisionArea.GetNode<CollisionShape2D>("CollisionBox");

        hitboxes = GetNode<Node>("Hitboxes");
        hurtboxes = GetNode<Node>("Hurtboxes");

        traction = WEIGHT;
        effectiveFallSpeed = GlobalVariables.Instance.FALLSPEED;

        anim.AnimationFinished += OnAnimationFinished;

        collisionBox.Shape = new RectangleShape2D();
        health = MAX_HEALTH;
        effectiveWeight = WEIGHT;

        // Determine whether this is player 1 or 2
        if (id == 1) {
            opponent = GetParent().GetNode<Character>("Player2");
        } else {
            opponent = GetParent().GetNode<Character>("Player1");
        }
    }

    public override void _Process(double delta)
    {
        // Visualise info for debugging
        GetNode<Label>("Frames").Text = frame.ToString();
        GetNode<Label>("Health").Text = health.ToString() + " / " + MAX_HEALTH.ToString();
    }

    // Performed after process and physics process
    public void WallCheck() {
        // Ray checks are done here because they are only calculated at the end of the physics process
        // If not done here, they are desynced by 1 frame
        if (leftWallCheck.IsColliding()) {
            wallDir = -1;
            // Correct player's position to not intersect with the wall
            Area2D leftWall = (Area2D)leftWallCheck.GetCollider();
            int leftBound = (int)(leftWall.GlobalPosition.X + ((RectangleShape2D)leftWall.GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size.X / 2);
            Position = new Vector2(Math.Max(leftBound + collisionDims.X / 2, Position.X), Position.Y);
        }
        else if (rightWallCheck.IsColliding()) {
            wallDir = 1;
            // Correct player's position to not intersect with the wall
            Area2D rightWall = (Area2D)rightWallCheck.GetCollider();
            int rightBound = (int)(rightWall.GlobalPosition.X - ((RectangleShape2D)rightWall.GetNode<CollisionShape2D>("CollisionShape2D").Shape).Size.X / 2);
            Position = new Vector2(Math.Min(rightBound - collisionDims.X / 2, Position.X), Position.Y);
        }
        else {
            wallDir = 0;
            onWall = false;
        }      
    }

    public void UpdateDir() {
        // Determine which way the player faces to face the opponent
        float xDiff = opponent.Position.X - Position.X;
        if (xDiff > 0) {
            dir = 1;
        } else if (xDiff < 0) {
            dir = -1;
        }
    }

    public void UpdateSpriteDir() {
        if (dir == 1) {
            sprite.FlipH = false;
            visualDir = 1;
        } else if (dir == -1) {
            sprite.FlipH = true;
            visualDir = -1;
        }
    }

    public void AdvanceAnimation() {
        anim.Advance(1/(float)Engine.MaxFps);
        sprite.Position = new Vector2(sprite.Position.X * visualDir, sprite.Position.Y);
    }

    public void PlayAnimation(string anim_name) {
        anim.Play(anim_name);
        sprite.Position = Vector2.Zero;
    }

    private void OnAnimationFinished(StringName animName)
    {
        if (animQueue.Count > 0) {
            PlayAnimation(animQueue.Dequeue());
        }
    }

    public void UpdateCollision(ushort width, ushort height, Vector2 pos) {
        collisionDims.X = width;
        collisionDims.Y = height;
        collisionPosY = (int)pos.Y;

        collisionBox.Position = pos;
        ((RectangleShape2D)collisionBox.Shape).Size = new Vector2(width, height);
        leftWallCheck.Position = new Vector2(-width/2, pos.Y);
        rightWallCheck.Position = new Vector2(width/2, pos.Y);
    }

    public void OnHit(short damage) {
        health -= damage;

        if (totalComboDecay > 75) {
            additionalGravity = (short)(totalComboDecay * 0.1f);
        }

        EmitSignal(SignalName.Damaged);
    }

    public Node CreateHitBox(ushort _width, ushort _height, ushort _damage, sbyte _type, Vector2 _position, byte _hitlevel, byte _hitstun, byte _blockstun, short _xlaunch = 0, short _ylaunch = 0, byte _decay = 5) {
        Hitbox hitbox_instance = (Hitbox)GlobalVariables.Instance.hitbox.Instantiate();
        hitboxes.AddChild(hitbox_instance);

        Vector2 pos = new Vector2(_position.X * visualDir, _position.Y);

        hitbox_instance.parent = this;
        hitbox_instance.SetParameters(_width, _height, _damage, _type, pos, _hitlevel, _hitstun, _blockstun, _xlaunch, _ylaunch, _decay);
        return hitbox_instance;
    }

    public Node CreateHurtBox(ushort _width, ushort _height, Vector2 _position) {
        Hurtbox hurtbox_instance = (Hurtbox)GlobalVariables.Instance.hurtbox.Instantiate();
        hurtboxes.AddChild(hurtbox_instance);

        Vector2 pos = new Vector2(_position.X * visualDir, _position.Y);

        hurtbox_instance.parent = this;
        hurtbox_instance.SetParameters(_width, _height, pos);
        return hurtbox_instance;
    }

    public void DestroyHitBoxes() {
        foreach (Node hitbox in hitboxes.GetChildren()) {
            hitbox.QueueFree();
        }
    }

    public void DestroyHurtBoxes() {
        foreach (Node hurtbox in hurtboxes.GetChildren()) {
            hurtbox.QueueFree();
        }
    }

    public void SetAnimVel(int additionalVelocity, sbyte direction) {
        animVel = additionalVelocity * direction * dir;
    }
}
