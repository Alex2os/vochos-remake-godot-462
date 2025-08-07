using Godot;
using System;

public partial class Shop : Node2D
{
	// array for all the perks textures
	private static Texture2D[] _PerksTexture;
	// array for all the perks medium-size textures
	private static Texture2D[] _PerksTextureMedium;

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
	private int[] _PlayerInventory = new int[3] { -1, -1, -1 }; // if any index = -1, then it means there's perk in that slot.

	[Export] private Sprite2D _Perk1;
	[Export] private Sprite2D _Perk2;
	[Export] private Sprite2D _Perk3;
	[Export] private Sprite2D _InventoryPerk1;
	[Export] private Sprite2D _InventoryPerk2;
	[Export] private Sprite2D _InventoryPerk3;
	[Export] private Label _Perk1Label;
	[Export] private Label _Perk2Label;
	[Export] private Label _Perk3Label;
	[Export] private Button _RerollShopButton;
	[Export] private Button _LeaveShopButton;
	[Export] private Button _BuyPerk1Button;
	[Export] private Button _BuyPerk2Button;
	[Export] private Button _BuyPerk3Button;
	[Export] private Button _SellPerk1Button;
	[Export] private Button _SellPerk2Button;
	[Export] private Button _SellPerk3Button;
	[Export] private AnimationPlayer _NotEnoughAnimations;
	[Export] private AudioStreamPlayer _RerollShopSound;
	[Export] private AudioStreamPlayer _ClickButtonSound;
	[Signal] public delegate void InventoryUpdatedEventHandler(int perk1, int perk2, int perk3);
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

		// medium size textures for the perks
		_PerksTextureMedium = new Texture2D[]
		{
			GD.Load<Texture2D>("res://assets/perks/medium sizes/shield medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/extra life medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/double points medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/bullet medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/double money medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/time slow medium.png")

	};

		_RerollShopButton.Pressed += OnRerollShopButtonPressed;
		_LeaveShopButton.Pressed += OnLeaveShopButtonPressed;

		// linking alarms and functions for when a perk is bought
		_BuyPerk1Button.Pressed += OnBuyPerk1Button;
		_BuyPerk2Button.Pressed += OnBuyPerk2Button;
		_BuyPerk3Button.Pressed += OnBuyPerk3Button;

		_SellPerk1Button.Pressed += OnSellPerk1Button;
		_SellPerk2Button.Pressed += OnSellPerk2Button;
		_SellPerk3Button.Pressed += OnSellPerk3Button;

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

		// re-enable the buttons so they can be pressed again (in case they were pressed and bought some perks)
		_BuyPerk1Button.Disabled = false;
		_BuyPerk2Button.Disabled = false;
		_BuyPerk3Button.Disabled = false;

		_RerollShopSound.Play();
	}

	public void OnLeaveShopButtonPressed()
	{
		// logic for when the shop is left
		GD.Print("leaving shop!");
		EmitSignal(SignalName.InventoryUpdated, _PlayerInventory[0], _PlayerInventory[1], _PlayerInventory[2]);
		_ClickButtonSound.Play();
	}

	public void OnBuyPerk1Button()
	{
		GD.Print("buying perk1");
		_ClickButtonSound.Play();

		if (CheckInventorySpace(_PerksChosen[0]) == 1) // if the perk is bought, change the text, id and texture.
		{
			_Perk1.Texture = null;
			_Perk1Label.Text = "Sold!";
			_PerksChosen[0] = -1;
			_BuyPerk1Button.Disabled = true; // this disables the button to be pressed.
		}
	}

	public void OnBuyPerk2Button()
	{
		GD.Print("buying perk2");
		_ClickButtonSound.Play();

		if (CheckInventorySpace(_PerksChosen[1]) == 1)
		{
			_Perk2.Texture = null;
			_Perk2Label.Text = "Sold!";
			_PerksChosen[1] = -1;
			_BuyPerk2Button.Disabled = true;
		}

	}

	public void OnBuyPerk3Button()
	{
		GD.Print("buying perk3");
		_ClickButtonSound.Play();

		if (CheckInventorySpace(_PerksChosen[2]) == 1)
		{
			_Perk3.Texture = null;
			_Perk3Label.Text = "Sold!";
			_PerksChosen[2] = -1;
			_BuyPerk3Button.Disabled = true;
		}
	}

	public int CheckInventorySpace(int _PerkChosenInShop)
	{
		bool _SpaceNotAvailable = true;

		for (int i = 0; i < 3; i++)
		{
			if (_PlayerInventory[i] == -1) // if the player inventory has a free slot
			{
				_SpaceNotAvailable = false; // this is not to trigger the notenoughspace animation
				AssignInventoryPerk(_PerkChosenInShop, i); // here we assign the inventory perk id and texture
				break;
			}
		}

		if (_SpaceNotAvailable)
		{
			_NotEnoughAnimations.Play("not enough space"); // if there's no space, then the animation will be played
			return 0; // if the perk can't be bought, this will return 0.
		}
		else return 1; // in case the user can buy it, returns 1.
	}

	private void AssignInventoryPerk(int _PerkChosenInShop, int _InventorySlot)
	{
		_PlayerInventory[_InventorySlot] = _PerkChosenInShop; // this assigns the id of the perk chosen in the shop. we need this here as playerinv[i] will not be the same as perkschosen[i]
		switch (_InventorySlot) // this is to assign the texture of the perk
		{
			case 0:
				_InventoryPerk1.Texture = _PerksTextureMedium[_PerkChosenInShop];
				_SellPerk1Button.Modulate = new Color(1, 1, 1, 1); // we show the buttons when a perk can be sold.
				_SellPerk1Button.Disabled = false;
				break;
			case 1:
				_InventoryPerk2.Texture = _PerksTextureMedium[_PerkChosenInShop];
				_SellPerk2Button.Modulate = new Color(1, 1, 1, 1);
				_SellPerk2Button.Disabled = false;
				break;
			case 2:
				_InventoryPerk3.Texture = _PerksTextureMedium[_PerkChosenInShop];
				_SellPerk3Button.Modulate = new Color(1, 1, 1, 1);
				_SellPerk3Button.Disabled = false;
				break;
		}
	}

	public void OnSellPerk1Button()
	{

		GD.Print("selling perk1");

		SellPerk(0); // here we send the perk slot.

		_ClickButtonSound.Play();
	}

	public void OnSellPerk2Button()
	{

		GD.Print("selling perk2");

		SellPerk(1);

		_ClickButtonSound.Play();
	}

	public void OnSellPerk3Button()
	{

		GD.Print("selling perk3");

		SellPerk(2);

		_ClickButtonSound.Play();
	}

	private void SellPerk(int _PerkSlot)
	{
		_PlayerInventory[_PerkSlot] = -1; // assign the id to the player inventory, because it was sold. 

		switch (_PerkSlot) {
			case 0:
				_InventoryPerk1.Texture = null; // this puts no texture in the sprite. so when a perk is sold, there's no texture to show as there's no perk.
				_SellPerk1Button.Modulate = new Color(0, 0, 0, 0);
				_SellPerk1Button.Disabled = true;
				break;
			case 1:
			    _InventoryPerk2.Texture = null;
				_SellPerk2Button.Modulate = new Color(0, 0, 0, 0);
				_SellPerk2Button.Disabled = true;
				break;
			case 2:
				_InventoryPerk3.Texture = null;
				_SellPerk3Button.Modulate = new Color(0, 0, 0, 0);
				_SellPerk3Button.Disabled = true;
				break;
		}
	}
}
