using Godot;
using System;

public partial class Shop : Node2D
{
	// array for all the perks textures
	private static Texture2D[] _PerksTexture;
	// array for all the perks medium-size textures
	private static Texture2D[] _PerksTextureMedium;

	// array to choose the perks
	private int[] _PerksChosen = new int[3];
	// array of strings that are the perk's names

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
	[Export] private Label _Perk1CostLabel;
	[Export] private Label _Perk2CostLabel;
	[Export] private Label _Perk3CostLabel;
	[Export] private Label _ActualMoneyLabel;
	[Export] private Label _RerollShopCostLabel;
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

		GetGlobalPerks(); // in this function we get the perks from the playervariables autoload so we have them here in the shop.

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_ActualMoneyLabel.Text = "Money: " + PlayerVariables.Instance.PlayerCoins.ToString(); // we assign the actual coins the player has in this label. putting this in process so it gets reassigned automatically, same as the reroll label below.
		_RerollShopCostLabel.Text = "$" + GameManager.Instance.RerollShopCost; // we update the reroll cost label with the new cost.

	}

	private void ChoosePerks()
	{
		// the amount of perks per shop will be 3.
		for (int i = 0; i < 3; i++) _PerksChosen[i] = (int)GD.RandRange(0, GameManager.NumberOfPerks - 1); // this will choose between 1 of the six perks that are in the game.

		_Perk1.Texture = _PerksTexture[_PerksChosen[0]];
		_Perk2.Texture = _PerksTexture[_PerksChosen[1]];
		_Perk3.Texture = _PerksTexture[_PerksChosen[2]];
		_Perk1Label.Text = GameManager.PerksNames[_PerksChosen[0]];
		_Perk2Label.Text = GameManager.PerksNames[_PerksChosen[1]];
		_Perk3Label.Text = GameManager.PerksNames[_PerksChosen[2]];
		_Perk1CostLabel.Text = "$" + GameManager.PerksPrices[_PerksChosen[0]].ToString();
		_Perk2CostLabel.Text = "$" + GameManager.PerksPrices[_PerksChosen[1]].ToString();
		_Perk3CostLabel.Text = "$" + GameManager.PerksPrices[_PerksChosen[2]].ToString();
	}

	public void OnRerollShopButtonPressed()
	{
		// here should be the logic to substract certain amount of money to the user for each reroll.
		GD.Print("rerolling shop");
		if (CheckPlayerMoneyReroll())
		{
			ChoosePerks();

			// re-enable the buttons so they can be pressed again (in case they were pressed and bought some perks)
			_BuyPerk1Button.Disabled = false;
			_BuyPerk2Button.Disabled = false;
			_BuyPerk3Button.Disabled = false;

			PlayerVariables.Instance.PlayerCoins -= GameManager.Instance.RerollShopCost; // we substract the reroll cost to the player coins.

			GameManager.Instance.RerollShopCost++; // every reroll we increment the value of the rerolling cost.
		}
		else _NotEnoughAnimations.Play("not enough money");

		_RerollShopSound.Play();
	}

	public void OnLeaveShopButtonPressed()
	{
		// logic for when the shop is left
		GD.Print("leaving shop!");

		GameManager.Instance.ComingFromShop = true; // we are leaving the shop, so comingfromshop has to be true.
		UpdateGlobalPerks(); // now, with this function we update the global perks from what we got from the shop.

		_ClickButtonSound.Play();

		GameManager.Instance.ChangeSceneToGame();
	}

	public void OnBuyPerk1Button()
	{
		GD.Print("buying perk1");
		_ClickButtonSound.Play();

		if (CheckInventorySpace(_PerksChosen[0])) // if the perk is bought, change the text, id and texture.
		{
			_Perk1.Texture = null;
			_Perk1Label.Text = "Sold!";
			// the substraction of the price should be before the reassignment of the _perkschosen variable.
			PlayerVariables.Instance.PlayerCoins -= GameManager.PerksPrices[_PerksChosen[0]]; // here we substract the price from the player coins if the perk can be bought
			_PerksChosen[0] = -1;
			_BuyPerk1Button.Disabled = true; // this disables the button to be pressed.
		}
	}

	public void OnBuyPerk2Button()
	{
		GD.Print("buying perk2");
		_ClickButtonSound.Play();

		if (CheckInventorySpace(_PerksChosen[1]))
		{
			_Perk2.Texture = null;
			_Perk2Label.Text = "Sold!";
			PlayerVariables.Instance.PlayerCoins -= GameManager.PerksPrices[_PerksChosen[1]];
			_PerksChosen[1] = -1;
			_BuyPerk2Button.Disabled = true;
			
		}

	}

	public void OnBuyPerk3Button()
	{
		GD.Print("buying perk3");
		_ClickButtonSound.Play();

		if (CheckInventorySpace(_PerksChosen[2]))
		{
			_Perk3.Texture = null;
			_Perk3Label.Text = "Sold!";
			PlayerVariables.Instance.PlayerCoins -= GameManager.PerksPrices[_PerksChosen[2]];
			_PerksChosen[2] = -1;
			_BuyPerk3Button.Disabled = true;
			
		}
	}

	public bool CheckInventorySpace(int _PerkChosenInShop)
	{
		bool _SpaceNotAvailable = true;
		bool _NotEnoughMoney = true;

		for (int i = 0; i < 3; i++)
		{
			if (_PlayerInventory[i] == -1) // if the player inventory has a free slot
			{
				_SpaceNotAvailable = false; // this is not to trigger the notenoughspace animation
				if (CheckPlayerMoneyPerks(_PerkChosenInShop))
				{
					_NotEnoughMoney = false; // this is not to trigger the notenoughmoney animation.
					AssignInventoryPerk(_PerkChosenInShop, i); // here we assign the inventory perk id and texture
					break;
				}


			}
		}

		if (_SpaceNotAvailable) // not  enough space animation
		{
			_NotEnoughAnimations.Play("not enough space"); // if there's no space, then the animation will be played
			return false; // if the perk can't be bought, this will return 0.
		}

		if (_NotEnoughMoney) // not enough money animation
		{
			_NotEnoughAnimations.Play("not enough money");
			return false;
		}

		else return true; // in case the user can buy it, returns 1.
	}

	private void AssignInventoryPerk(int _PerkChosenInShop, int _InventorySlot)
	{
		// the assignment of the id of the perk will be redundant if called when entered the shop. (should fix this later)
		_PlayerInventory[_InventorySlot] = _PerkChosenInShop; // this assigns the id of the perk chosen in the shop. we need this here as playerinv[i] will not be the same as perkschosen[i]
		switch (_InventorySlot) // this is to assign the texture of the perk
		{
			case 0:
				_InventoryPerk1.Texture = _PerksTextureMedium[_PerkChosenInShop];
				_SellPerk1Button.Disabled = false;
				break;
			case 1:
				_InventoryPerk2.Texture = _PerksTextureMedium[_PerkChosenInShop];
				_SellPerk2Button.Disabled = false;
				break;
			case 2:
				_InventoryPerk3.Texture = _PerksTextureMedium[_PerkChosenInShop];
				_SellPerk3Button.Disabled = false;
				break;
		}
	}

	public void OnSellPerk1Button()
	{

		GD.Print("selling perk1");

		// in this part we sell the perk and add the money to the player coins. the adding of the money must be before the SellPerk function, as in the SellPerk function we get rid of the id of the _PlayerInventory, making unable to sell it or get the id after that.
		PlayerVariables.Instance.PlayerCoins += GameManager.PerksPricesSelling[_PlayerInventory[0]];

		SellPerk(0); // here we send the perk slot.
		
		_ClickButtonSound.Play();
	}

	public void OnSellPerk2Button()
	{

		GD.Print("selling perk2");

		PlayerVariables.Instance.PlayerCoins += GameManager.PerksPricesSelling[_PlayerInventory[1]];
		SellPerk(1);

		_ClickButtonSound.Play();
	}

	public void OnSellPerk3Button()
	{

		GD.Print("selling perk3");

		PlayerVariables.Instance.PlayerCoins += GameManager.PerksPricesSelling[_PlayerInventory[2]];
		SellPerk(2);

		_ClickButtonSound.Play();
	}

	private void SellPerk(int _PerkSlot)
	{
		_PlayerInventory[_PerkSlot] = -1; // assign the id to the player inventory, because it was sold. 

		switch (_PerkSlot)
		{
			case 0:
				_InventoryPerk1.Texture = null; // this puts no texture in the sprite. so when a perk is sold, there's no texture to show as there's no perk.
				_SellPerk1Button.Disabled = true;
				break;
			case 1:
				_InventoryPerk2.Texture = null;
				_SellPerk2Button.Disabled = true;
				break;
			case 2:
				_InventoryPerk3.Texture = null;
				_SellPerk3Button.Disabled = true;
				break;
		}
	}

	private void GetGlobalPerks()
	{
		for (int i = 0; i < _PlayerInventory.Length; i++)
		{
			_PlayerInventory[i] = PlayerVariables.Instance.PlayerInventory[i];
			// as we need to assign the texture of the perk too, we send it to this function:

			if (_PlayerInventory[i] == -1) ; // we do this to prevent errors in the assigninventoryperk function, as it can be that the inventory contains -1, and that's out of the index bound of any array.
			else AssignInventoryPerk(_PlayerInventory[i], i);
		}


	}

	private void UpdateGlobalPerks()
	{
		for (int i = 0; i < _PlayerInventory.Length; i++) PlayerVariables.Instance.PlayerInventory[i] = _PlayerInventory[i];
	}

	private bool CheckPlayerMoneyPerks(int perk_chosen) // in this function we check if the money the player has is greater or equal to the price of the perk chosen.
	{
		if (PlayerVariables.Instance.PlayerCoins >= GameManager.PerksPrices[perk_chosen]) return true;

		return false;
	}

	private bool CheckPlayerMoneyReroll() // we check if the money is equal or greater than the reroll cost.
	{
		if (PlayerVariables.Instance.PlayerCoins >= GameManager.Instance.RerollShopCost) return true;

		return false;
	}
}
