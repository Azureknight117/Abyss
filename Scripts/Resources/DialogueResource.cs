using Godot;
using System;
using Godot.Collections;
[GlobalClass]
public partial class DialogueResource : Resource
{
    [Export] public Array<string> dialogueLines;

    [Export] public string characterName;

    [Export] public Texture2D characterImage;
}
