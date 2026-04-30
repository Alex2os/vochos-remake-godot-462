using Godot;
using System.Collections.Generic; // used for dictionaries

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	private PackedScene _MainMenuScene = GD.Load<PackedScene>("res://Scenes/MainMenu/main_menu.tscn");
	private PackedScene _GameScene = GD.Load<PackedScene>("res://Scenes/Game/game.tscn");
	private PackedScene _ShopScene = GD.Load<PackedScene>("res://Scenes/Shop/shop.tscn");

	public bool ComingFromShop = false;
	// number of perks in the game
	public const int NumberOfPerks = 6;

	// a dictionary for the perks' numbers
	public static readonly Dictionary<string, int> PerksNumbers = new Dictionary<string, int>
{
	{ "shield", 0 },
	{ "health", 1 },
	{ "double-points", 2 },
	{ "bullet", 3 },
	{ "double-money", 4 },
	{ "slow-time", 5 }
};

	// names of the perks
	public static readonly string[] PerksNames = new string[6] { "Shield",
		"Health",
		"Double Points",
		"Bullet",
		"Double Money",
		"Slow Time" };


	// the prices of the perks
	public static readonly int[] PerksPrices = new int[] {
		7,
		8,
		6,
		6,
		10,
		7
	};

	public static readonly int[] PerksPricesSelling = new int[] {
		3,
		4,
		3,
		3,
		5,
		3
	};

	public int RerollShopCost = 1;


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
