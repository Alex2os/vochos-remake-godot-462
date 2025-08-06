using Godot;
using System;

public partial class Shop : Node2D
{
	// array for all the perks textures
	private static Texture2D[] _PerksTexture;
	// this is the perk's frame
	private static Texture2D _PerksFrameTexture = GD.Load<Texture2D>("res://assets/perks/perks frame.png");
	// number of perks in the game
	private const int _NumberOfPerks = 6;
	// array to choose the perks
	private int[] _PerksChosen = new int[3];

	// array of strings that are the perk's names
	private string[] _PerksNames = new string[6] { "Shield",
		"Extra Life",
		"Double Points",
		"Bullet",
		"Double Money",
		"Slow Time" };

	// player inventory for the perks
	private int[] _PlayerInventory = new int[3] {-1, -1, -1}; // if any index = -1, then it means there's perk in that slot.

	[Export] private Sprite2D _Perk1;
	[Export] private Sprite2D _Perk2;
	[Export] private Sprite2D _Perk3;
	[Export] private Label _Perk1Label;
	[Export] private Label _Perk2Label;
	[Export] private Label _Perk3Label;
	[Export] private Button _RerollShopButton;
	[Export] private Button _LeaveShopButton;
	[Export] private Button _BuyPerk1Button;
	[Export] private Button _BuyPerk2Button;
	[Export] private Button _BuyPerk3Button;
	[Export] private AudioStreamPlayer _RerollShopSound;
	[Signal] public delegate void Perk1BoughtEventHandler(string _PerkName, int _PerkNumber);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// charge the perks' different skins. perks will be randomly chose, so we need to choose from between this textures.
		_PerksTexture = new Texture2D[]
			{
			GD.Load<Texture2D>("res://assets/perks/shield.png"),
			GD.Load<Texture2D>("res://assets/perks/extra life.png"),
			GD.Load<Texture2D>("res://assets/perks/double points.png"),
			GD.Load<Texture2D>("res://assets/perks/bullet.png"),
			GD.Load<Texture2D>("res://assets/perks/double money.png"),
			GD.Load<Texture2D>("res://assets/perks/time slow.png")

		};

		_RerollShopButton.Pressed += OnRerollShopButtonPressed;
		_LeaveShopButton.Pressed += OnLeaveShopButtonPressed;

		// linking alarms and functions for when a perk is bought
		_BuyPerk1Button.Pressed += OnBuyPerk1Button;
		_BuyPerk2Button.Pressed += OnBuyPerk2Button;
		_BuyPerk3Button.Pressed += OnBuyPerk3Button;

		ChoosePerks(); // choose perks that will randomnly appear on the shop
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ChoosePerks()
	{
		// the amount of perks per shop will be 3.
		for (int i = 0; i < 3; i++) _PerksChosen[i] = (int)GD.RandRange(0, _NumberOfPerks - 1); // this will choose between 1 of the six perks that are in the game.

		_Perk1.Texture = _PerksTexture[_PerksChosen[0]];
		_Perk2.Texture = _PerksTexture[_PerksChosen[1]];
		_Perk3.Texture = _PerksTexture[_PerksChosen[2]];
		_Perk1Label.Text = _PerksNames[_PerksChosen[0]];
		_Perk2Label.Text = _PerksNames[_PerksChosen[1]];
		_Perk3Label.Text = _PerksNames[_PerksChosen[2]];
	}

	public void OnRerollShopButtonPressed()
	{
		// here should be the logic to substract certain amount of money to the user for each reroll.
		GD.Print("rerolling shop");
		ChoosePerks();
		_RerollShopSound.Play();
	}

	public void OnLeaveShopButtonPressed()
	{
		// logic for when the shop is left
		GD.Print("leaving shop!");
	}

	public void OnBuyPerk1Button()
	{
		GD.Print("buying perk1");

		_PlayerInventory[0] = _PerksChosen[0];

		GD.Print("perk 2 parameters: perk_name: ", _PerksNames[_PerksChosen[0]], " perk id: ", _PerksChosen[0], " in inventory id: ", _PlayerInventory[0]);
	}

	public void OnBuyPerk2Button()
	{
		GD.Print("buying perk2");

		_PlayerInventory[1] = _PerksChosen[1];

		GD.Print("perk 2 parameters: perk_name: ", _PerksNames[_PerksChosen[1]], " perk id: ", _PerksChosen[1], " in inventory id: ", _PlayerInventory[1]);
	}

	public void OnBuyPerk3Button()
	{
		GD.Print("buying perk3");

		_PlayerInventory[2] = _PerksChosen[2];

		GD.Print("perk 2 parameters: perk_name: ", _PerksNames[_PerksChosen[2]], " perk id: ", _PerksChosen[2], " in inventory id: ", _PlayerInventory[2]);
	}
}
