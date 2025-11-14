using Godot;
using System;

public partial class Interactiblebase : Area2D
{
	bool canBeInteracted = false;
	public void _on_body_entered(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("Can Interact with " + this.GetParent().Name);
			canBeInteracted = true;
			Player playerRef = (Player)body;
			playerRef.AddInteractible(this);
		}
	}

	public void _on_body_exited(Node2D body)
	{
		if (body.IsInGroup("Player"))
		{
			GD.Print("Can't Interact with " + this.GetParent().Name + " anymore");
			canBeInteracted = false;
			Player playerRef = (Player)body;
			playerRef.RemoveInteractible();
		}
	}


	public virtual void TriggerInteraction()
	{
		GameManager.Instance.ShowDialogueBox("Greetings traveler!");
	}
}
