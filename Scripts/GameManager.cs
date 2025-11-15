using Godot;
using System;
using Godot.Collections;
public partial class GameManager : Node
{
	static public GameManager Instance;

	private Player playerRef;

	static public bool GamePaused = false;

	private bool dialogueBoxOpened;

	[Export]
	private DialogueManager dialogueManager;


	public override void _Ready()
	{
		if (Instance == null)
			Instance = this;
		else
			this.QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{



	}


	public void StartDialogue(DialogueResource dResource)
	{
		dialogueManager.BeginDialogue(dResource);
		dialogueBoxOpened = true;
		GamePaused = true;
	}

	public void EndDialogue()
	{
		dialogueBoxOpened = false;
		GamePaused = false;
	}
}
