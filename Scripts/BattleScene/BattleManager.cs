using Godot;
using System;

public partial class BattleManager : Node2D
{
    [Export]
    public VBoxContainer options;

    public override void _Ready()
    {
        base._Ready();
        options.Visible = true;
    }


    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            Key input = keyEvent.Keycode;

            switch (input)
            {
                case Key.Escape:
                    GetTree().Quit();
                    break;
                case Key.R:
                    GetTree().ReloadCurrentScene();
                    break;

            }

        }
    }
}
