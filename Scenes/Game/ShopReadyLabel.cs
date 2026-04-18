using Godot;
using System;

public partial class ShopReadyLabel : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = "[wave amp = 1 freq = 1 connected = 0]Shop is ready! Press f to go[/wave]";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
