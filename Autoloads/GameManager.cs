using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	private PackedScene _MainMenuScene = GD.Load<PackedScene>("res://Scenes/MainMenu/main_menu.tscn");
	private PackedScene _GameScene = GD.Load<PackedScene>("res://Scenes/Game/game.tscn");
	private PackedScene _ShopScene = GD.Load<PackedScene>("res://Scenes/Shop/shop.tscn");

	public bool ComingFromShop = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeSceneToGame()
	{
		GetTree().ChangeSceneToPacked(_GameScene);
	}

	public void ChangeSceneToShop()
	{
		GetTree().ChangeSceneToPacked(_ShopScene);
	}

	public void ChangeSceneToMainMenu()
	{
		GetTree().ChangeSceneToPacked(_MainMenuScene);
	}

	public void InitializeGameManagerVariables()
	{
		ComingFromShop = false;
	}
}
