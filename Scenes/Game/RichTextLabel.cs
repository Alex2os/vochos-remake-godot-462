using Godot;
using System;

public partial class RichTextLabel : Godot.RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = "\n    [wave amp = 20 freq = 5 connected = 0]Game paused...[/wave]"; // we can use this bbcode to modify a richtext label. search for bbcode in godot documentation for more about this bbcode.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
