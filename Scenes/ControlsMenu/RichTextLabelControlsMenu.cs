using Godot;
using System;

public partial class RichTextLabelControlsMenu : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = "[wave amp = 20 freq = 5 connected = 0]Controls[/wave]";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
