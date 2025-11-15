using System.Linq.Expressions;
using Godot;
using Godot.Collections;

public partial class DialogueManager : Node
{
	[Export]
	private DialogueWindow dialogueWindow;


	private DialogueResource currentDialogue;

	private int dialougeIndex = -1;

	bool dialogueBegun;

	public override void _Ready()
	{
		dialogueWindow.Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (dialogueBegun)
		{
			if (Input.IsActionJustReleased("confirm"))
			{
				dialougeIndex++;
				if (dialougeIndex < currentDialogue.dialogueLines.Count)
					DisplayDialogueLine(dialougeIndex);
				else
					CloseDialougeBox();
			}
		}
	}

	public void DisplayDialogueLine(int index)
	{
		dialogueWindow.SetText(currentDialogue.dialogueLines[index]);
		//Do something that can parse the dialogue, so you can have things like give items and or initiate battles

		if (currentDialogue.characterName != null)
		{
			//Do something
		}
		if (currentDialogue.characterImage != null)
		{
			//Do something
		}
	}

	public void BeginDialogue(DialogueResource newDialogue)
	{
		currentDialogue = newDialogue;
		if (newDialogue.characterName != null)
		{
			//Do something
		}
		if (newDialogue.characterImage != null)
		{
			//Do something
		}

		ShowDialogueBox();
	}

	private void ShowDialogueBox()
	{
		if (currentDialogue == null)
		{
			GD.PrintErr("No dialogue set!");
			return;
		}
		dialogueWindow.Visible = true;
		dialougeIndex = 0;
		dialogueBegun = true;
		dialogueWindow.SetText(currentDialogue.dialogueLines[dialougeIndex]);
	}

	public void CloseDialougeBox()
	{
		dialogueWindow.Visible = false;
		dialougeIndex = -1;
		dialogueBegun = false;
		GameManager.Instance.EndDialogue();
	}
}
