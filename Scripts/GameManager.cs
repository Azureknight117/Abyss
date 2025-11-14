using Godot;
using System;

public partial class GameManager : Node
{
	static public GameManager Instance;

	private Player playerRef;

	[Export]
	private DialogueWindow dialogueWindow;


	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else
			this.QueueFree();


		dialogueWindow.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void ShowDialogueBox(string text)
	{
		dialogueWindow.Visible = true;
		dialogueWindow.SetText(text);
	}
}
