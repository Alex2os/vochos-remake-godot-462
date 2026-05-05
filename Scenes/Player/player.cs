using Godot;
using System;

public partial class player : Area2D
{
	[Export] private int _MovingXAxis = 300;
	[Export] private int _MovingYAxis = 300;
	[Export] private AudioStreamPlayer _CarCrash;
	[Export] private Texture2D _HealthBar_NoHit;
	[Export] private Texture2D _HealthBar_1Hit;
	[Export] private Texture2D _HealthBar_2Hit;
	[Export] private Texture2D _HealthBar_NoHealth;
	[Export] private Sprite2D _HealthBar;
	[Export] private Sprite2D _ShieldPerkInUse;
	[Export] private AudioStreamPlayer _ExtraLifePerkSound;
	[Export] private AudioStreamPlayer _ShieldPerkSound;
	[Export] private AudioStreamPlayer _ShieldPerkHitSound;
	[Export] private AudioStreamPlayer _BulletSpawnSound;
	[Export] private AudioStreamPlayer _DoublePointsPerkSound;
	[Export] private AudioStreamPlayer _DoubleMoneyPerkSound;
	[Export] private AudioStreamPlayer _SlowTimePerkSound;
	[Signal] public delegate void PlayerHealthDepletedEventHandler();
	[Signal] public delegate void UpdateInventoryPerkTextureEventHandler();

	// perk signals
	[Signal] public delegate void BulletPerkUsedEventHandler();
	[Signal] public delegate void ShieldPerkUsedEventHandler(int value);
	[Signal] public delegate void DoublePointsPerkUsedEventHandler(int value);
	[Signal] public delegate void DoubleMoneyPerkUsedEventHandler(int value);
	[Signal] public delegate void SlowTimePerkUsedEventHandler(int value);

	// variables
	private bool ShieldPerkActive = false;
	private bool DoublePointsPerkActive = false;
	private bool DoubleMoneyPerkActive = false;
	private bool SlowTimePerkActive = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		AreaEntered += OnAreaEntered;
		UpdateHealthBarTexture(); // this is used here so when coming from shop the health bar is updated here, when the car or player object is ready again.

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		// player movement
		if (Input.IsActionPressed("left"))
		{
			if (Position.X <= 150) ;
			else Position -= new Vector2(_MovingXAxis * (float)delta, 0);
		}

		if (Input.IsActionPressed("right"))
		{
			if (Position.X >= 850) ;
			else Position += new Vector2(_MovingXAxis * (float)delta, 0);
		}

		if (Input.IsActionPressed("up"))
		{
			if (Position.Y <= 50) ;
			else Position -= new Vector2(0, _MovingYAxis * (float)delta);
		}
		if (Input.IsActionPressed("down"))
		{
			if (Position.Y >= 650) ;
			else Position += new Vector2(0, _MovingYAxis * (float)delta);
		}

		// perks input management.
		if (Input.IsActionJustPressed("perk1"))
		{
			UsePerk(0); // here we send the inventory index that we want to use/check so the user can use the perk.
		}
		if (Input.IsActionJustPressed("perk2"))
		{
			UsePerk(1);
		}
		if (Input.IsActionJustPressed("perk3"))
		{
			UsePerk(2);
		}

	}

	private void OnAreaEntered(Area2D node)
	{

		if (node is CarEnemy) LowerPlayerHealth(); // if it's an enemy what hit the player, we lower the player's health. 
		// if anything else hit the player it won't lower their health, like coins or the shopPickable.

	}

	private void UsePerk(int inventory_index)
	{
		switch (PlayerVariables.Instance.PlayerInventory[inventory_index])
		{
			case 0: 
				if (ShieldPerkActive) return;
				UseShieldPerk();
				break;
			case 1: 
					// if health is already full, then just return.
				if (PlayerVariables.Instance.PlayerHealth == 3) return;
				// in any other case, use the extra life perk
				UseExtraLifePerk();
				break;
			case 2: 
				if (DoublePointsPerkActive) return;
				UseDoublePointsPerk();
				break;
			case 3: 
				UseBulletPerk();
				break;
			case 4: 
				if (DoubleMoneyPerkActive) return;
				UseDoubleMoneyPerk();
				break;
			case 5:
				if (SlowTimePerkActive) return;
				UseSlowTimePerk();
				break;
			case -1:
				GD.Print("No perk to use.");
				return;
		}

		// if any of the perks is used then we assign the inventory index to -1. 
		PlayerVariables.Instance.PlayerInventory[inventory_index] = -1; // this is to reassign the inventory perk id when a perk is used. if there's no perk, this will axtivate too, so everytime we check this function the perk id of the inventory slot will be assigned to -1 at the ond of the function.
																		// we then update the texture of the perk in the game inventory, sending a signal the game scene can use and successfully update it.
		EmitSignal(SignalName.UpdateInventoryPerkTexture);
	}

	private void LowerPlayerHealth() // we lower the health of the player, and check if the health is equal or less than zero to send the game over signal.
	{
		// if the player currently has the shield, then we return and also play a special sound when crashing an enemy. the player does not get their health lowered
		if (ShieldPerkActive)
		{
			GameManager.Instance.PlaySound(_ShieldPerkHitSound);
			return;
		}

		PlayerVariables.Instance.PlayerHealth -= 1; // 40 is the health the player loses everytime it crashes with an enemy car

		// we update the health bar sprite depending on the value of the health

		UpdateHealthBarTexture();

		if (PlayerVariables.Instance.PlayerHealth <= 0) EmitSignal(SignalName.PlayerHealthDepleted);
		else GameManager.Instance.PlaySound(_CarCrash); // if it's not game over yet, play de carcrash sound. otherwise the game over sound will be played
	}

	// the following functions are for the perks
	private void UseShieldPerk()
	{
		ShieldPerkActive = true;
		EmitSignal(SignalName.ShieldPerkUsed, GameManager.PerksNumbers["shield"]); // remember that for the signals that activate a perk, we need to send the respective perk number associated to that perk.

		GameManager.Instance.PlaySound(_ShieldPerkSound);
		_ShieldPerkInUse.Modulate = new Color(1, 1, 1, 1); // we show the shield perk sprite on the player

	}

	// when the timer gets to the timeout, we stop the timer and also set the shield perk active variable to false.
	public void OnShieldPerkEnded()
	{
		ShieldPerkActive = false;
		_ShieldPerkInUse.Modulate = new Color(0, 0, 0, 0); // we hide the shield perk sprite
	}

	private void UseExtraLifePerk()
	{
		PlayerVariables.Instance.PlayerHealth++;
		UpdateHealthBarTexture();
		GameManager.Instance.PlaySound(_ExtraLifePerkSound);
	}

	private void UseDoublePointsPerk()
	{
		DoublePointsPerkActive = true;
		EmitSignal(SignalName.DoublePointsPerkUsed, GameManager.PerksNumbers["double-points"]);
		GameManager.Instance.PlaySound(_DoublePointsPerkSound);
	}

	public void OnDoublePointsPerkEnded() { DoublePointsPerkActive = false; }

	private void UseBulletPerk()
	{
		EmitSignal(SignalName.BulletPerkUsed);
		GameManager.Instance.PlaySound(_BulletSpawnSound); // we reproduce the bullet spawn sound here instead of the bullet, as the bullet could be destroyed pretty soon in some cases.
	}

	private void UseDoubleMoneyPerk()
	{
		DoubleMoneyPerkActive = true;
		EmitSignal(SignalName.DoubleMoneyPerkUsed, GameManager.PerksNumbers["double-money"]);
		GameManager.Instance.PlaySound(_DoubleMoneyPerkSound);
	}

	public void OnDoubleMoneyPerkEnded() { DoubleMoneyPerkActive = false; }

	private void UseSlowTimePerk()
	{
		SlowTimePerkActive = true;
		EmitSignal(SignalName.DoubleMoneyPerkUsed, GameManager.PerksNumbers["slow-time"]);
		GameManager.Instance.PlaySound(_SlowTimePerkSound);
	}

	public void OnSlowTimePerkEnded() { SlowTimePerkActive = false; }

	private void UpdateHealthBarTexture()
	{
		switch (PlayerVariables.Instance.PlayerHealth)
		{
			case 3:
				_HealthBar.Texture = _HealthBar_NoHit;
				break;
			case 2:
				_HealthBar.Texture = _HealthBar_1Hit;
				break;
			case 1:
				_HealthBar.Texture = _HealthBar_2Hit;
				break;
			case 0:
				_HealthBar.Texture = _HealthBar_NoHealth;
				break;
		}
	}
	public void RestartPlayerVariables()
	{
		ShieldPerkActive = false;
		DoublePointsPerkActive = false;
		DoubleMoneyPerkActive = false;
		SlowTimePerkActive = false;
		UpdateHealthBarTexture();

		_ShieldPerkInUse.Modulate = new Color(0, 0, 0, 0);
	}
}
