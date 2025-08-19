using Godot;
using System;

public partial class Credits : Control
{
	[Export] private Button _QuitCreditsButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_QuitCreditsButton.Pressed += OnQuitCreditsButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnQuitCreditsButtonPressed()
	{
		QueueFree(); // we free the current scene to go back to the menu screen
	}
}
