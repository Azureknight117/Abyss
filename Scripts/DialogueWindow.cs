using Godot;
using System;

public partial class DialogueWindow : NinePatchRect
{
	[Export]
	public Label textBox;


	public void SetText(string newText)
	{
		textBox.Text = newText;
	}
}
