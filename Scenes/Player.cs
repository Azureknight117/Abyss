using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public int speed { get; set; } = 50;

    [Export]
    public int dashSpeed = 15;

    private bool moving = false;

    [Export]
    public int maxHP = 3;

    private int curHP = 0;

    [Export]
    public CollisionShape2D collider;

    [Export]
    public Label healthUI;

    [Export]
    public Sprite2D sprite;

    [Export]

    public AnimationTree animationTree;

    private AnimationNodeStateMachinePlayback stateMachine;

    [Export]
    public AnimationPlayer flashAnimation;

    public bool canControl { get; set; } = false;

    public bool canMove { get; set; } = false;


    private Vector2 knockBack = Vector2.Zero;
    private bool canBeHit = true;

    private bool knockBacked = false;

    private CharacterBody2D knockBackSource;

    [Export]
    private AudioStreamPlayer2D hurtSfx;

    [Export]
    private AudioStreamPlayer2D dieSfx;


    private Interactiblebase interactingObject;//Object the player can currently interact with (should only be one at a time)

    public override void _Ready()
    {
        curHP = maxHP;
        canControl = true;
        canMove = true;
        //healthUI.Text = "" + curHP;
        stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
    }

    public override void _Process(double delta)
    {
        if (GameManager.GamePaused)
            return;
        GetInput(delta);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (GameManager.GamePaused)
            return;
        Velocity = GetInput(delta);
        var collision = MoveAndCollide(Velocity * (float)delta);
        if (collision != null)
        {
            try
            {
                if (((CharacterBody2D)collision.GetCollider()).IsInGroup("Enemies"))
                {
                    HitWithKnockback(2, (CharacterBody2D)collision.GetCollider());
                }
            }
            catch
            {
                Velocity = Vector2.Zero;
            }

        }
        Position += knockBack * speed * (float)delta;
        knockBack = knockBack.Lerp(Vector2.Zero, 0.1f);
    }


    private Vector2 GetInput(double delta)
    {

        if (!canControl)
            return Vector2.Zero;

        int dashValue = 0;
        Vector2 velocity = Vector2.Zero; // The player's movement vector.
        if (canMove)
        {
            if (Input.IsActionPressed("move_right"))
            {
                velocity.X += 1;
            }

            if (Input.IsActionPressed("move_left"))
            {
                velocity.X -= 1;
            }

            if (Input.IsActionPressed("move_down"))
            {
                velocity.Y += 1;
            }

            if (Input.IsActionPressed("move_up"))
            {
                velocity.Y -= 1;
            }

            if (Input.IsActionJustPressed("attack"))
            {
                //Shoot();
            }

            if (Input.IsActionPressed("dash"))
            {
                dashValue = dashSpeed;
            }

            if (Input.IsActionJustPressed("interact"))
            {
                if (interactingObject != null)
                {
                    interactingObject.TriggerInteraction();
                }
            }
            if (velocity.Length() > 0)
            {
                velocity = velocity.Normalized();
            }
        }
        if (velocity == Vector2.Zero)
        {
            stateMachine.Travel("Idle");
        }
        else
        {
            stateMachine.Travel("Walk");
            animationTree.Set("parameters/Idle/BlendSpace2D/blend_position", velocity);
            animationTree.Set("parameters/Walk/BlendSpace2D/blend_position", velocity);
        }

        return velocity * (speed + dashValue);

    }

    public void AddInteractible(Interactiblebase interactible)
    {
        interactingObject = interactible;
    }

    public void RemoveInteractible()
    {
        interactingObject = null;
    }


    public void Die()
    {
        canControl = false;
        GD.Print("Dedge");
        GetNode<Node2D>("Sprite").Visible = false;
        collider.Disabled = true;
        dieSfx.Play();
        //GameManager.Instance.PlayerDeath();
    }



    public void Hit(int hitDamage)
    {
        //Flash damage and become immune for a sec
        if (canBeHit)
        {
            canBeHit = false;
            GD.Print("Hit for " + hitDamage);
            HitReact(hitDamage);
        }

    }

    public void HitWithKnockback(int hit, CharacterBody2D collidee)
    {
        Hit(hit);
        KnockBack(collidee);
    }

    private async void HitReact(int hitDamage)
    {
        ChangeHP(-1 * hitDamage);
        if (curHP == 0)
            Die();
        flashAnimation.Play("Flash");
        hurtSfx.Play();
        await ToSignal(GetTree().CreateTimer(0.4f), SceneTreeTimer.SignalName.Timeout);
        canBeHit = true;

    }
    public async void KnockBack(CharacterBody2D collidee)
    {

        if (knockBacked)
            return;
        knockBacked = true;
        knockBackSource = collidee;
        Vector2 directionAway = -1 * GlobalPosition.DirectionTo(knockBackSource.GlobalPosition);
        knockBack = directionAway * 2;
        canMove = false;
        GD.Print(knockBack);
        await ToSignal(GetTree().CreateTimer(0.5), SceneTreeTimer.SignalName.Timeout);
        knockBacked = false;
        canMove = true;
    }

    private void ChangeHP(int changeAmount)
    {
        curHP += changeAmount;
        if (curHP < 0)
            curHP = 0;
        else if (curHP > maxHP)
            curHP = maxHP;
        healthUI.Text = "" + curHP;
    }

    public void Heal(int healAmount)
    {
        ChangeHP(healAmount);
    }

    public void UpgradeHealth(int upgradeAmount)
    {
        maxHP += upgradeAmount;
        ChangeHP(maxHP - curHP);
    }

}

