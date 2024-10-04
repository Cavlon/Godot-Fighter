using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Hitbox : Area2D
{

    public Character parent;
    private CollisionShape2D hitbox;
    private uint frame = 0;
    private string state;

    public ushort width = 300;
    public ushort height = 300;
    public ushort damage = 50;
    public byte duration = 4;
    public byte hitlevel = 1;
    public sbyte type = 0;    //-1 = LOW  0 = MID  1 = OVERHEAD
    public short xLaunch = 0;
    public short yLaunch = 0;
    public byte decay = 5;
    public byte hitstun = 10;
    public byte blockstun = 10;

    public override void _Ready()
    {
        hitbox = GetNode<CollisionShape2D>("HitboxShape");
        hitbox.Shape = new RectangleShape2D();
        SetPhysicsProcess(false);
    }

    public override void _Process(double delta)
    {
        // If the parent's state changes, delete all these hitboxes
        if (parent != null) {
            if (parent.stateName != state) {
                Engine.TimeScale = 1;
                QueueFree();
                return;
            }
        }
    }

    public void SetParameters(ushort _width, ushort _height, ushort _damage, sbyte _type, Vector2 _position, byte _hitlevel, byte _hitstun, byte _blockstun, short _xlaunch, short _ylaunch, byte _decay) {
        state = parent.stateName;
        width = _width;
        height = _height;
        damage = _damage;
        type = _type;
        Position = _position;
        hitlevel = _hitlevel;
        hitstun = _hitstun;
        blockstun = _blockstun;
        xLaunch = _xlaunch;
        yLaunch = _ylaunch;
        decay = _decay;
        UpdateExtents();
        AreaEntered += HitboxCollision;
        SetPhysicsProcess(true);
    }

    private void UpdateExtents() {

        RectangleShape2D rect = (RectangleShape2D)hitbox.Shape;
        rect.Size = new Vector2(width, height);

    }

    private void HitboxCollision(Area2D body) {
        if (body is Hurtbox hurtbox) {
            Character target = hurtbox.parent;
            if (target.id == parent.id) return;

            // Prevents multiple hits when a single hitbox touches multiple hurtboxes
            if (!target.invincible) {
                target.velocity.Y = 0.15f * target.velocity.Y;

                if (target.wallDir != 0) parent.velocity.X += xLaunch * -parent.dir;    
                else target.velocity.X = xLaunch * parent.dir;

                if (target.totalComboDecay > 0) {
                    float launchFactor = target.totalComboDecay / 60;
                    target.velocity.Y += yLaunch * (1 - launchFactor);
                    target.velocity.X *= 1 + launchFactor;
                }

                if (target.totalComboDecay > 75) {
                    target.additionalGravity = (short)(target.totalComboDecay * 0.1f);
                }
                
                target.hitstun = hitstun;
                target.OnHit((short)damage);
                parent.DestroyHitBoxes();
            }
        }
    }
}
