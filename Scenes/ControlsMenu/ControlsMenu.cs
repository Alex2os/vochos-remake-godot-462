using Godot;
using System;

public partial class ControlsMenu : Control
{

	[Export] private Button CloseSceneButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CloseSceneButton.Pressed += OnCloseSceneButtonPressed;
	}

	private void OnCloseSceneButtonPressed(){ QueueFree(); }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
