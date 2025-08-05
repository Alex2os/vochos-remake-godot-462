using Godot;
using System;

public partial class MainMenu : Node2D
{
	[Export] private Button _PlayButton;
	[Export] private Button _ExitButton;
	[Export] private PackedScene _GameScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_PlayButton.Pressed += OnPlayButtonPressed;
		_ExitButton.Pressed += OnExitButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnPlayButtonPressed()
	{
		GetTree().ChangeSceneToPacked(_GameScene); // change the scene to the game scene
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit(); // leave the game or quit the scene, so it leaves the game when pressed 
	}
}
