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
	[Export] private Timer _ShieldPerkActiveTimer;
	[Signal] public delegate void PlayerHealthDepletedEventHandler();
	[Signal] public delegate void UpdateInventoryPerkTextureEventHandler();

	// variables
	private bool ShieldPerkActive = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		AreaEntered += OnAreaEntered;
		_ShieldPerkActiveTimer.Timeout += OnShieldPerkTimerTimeout;
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

		if (node is Coin) ; // if it's a coin, then don't do anything. 
		else LowerPlayerHealth(); // in any other case, it's an enemy what hit the player, so we lower the health

	}

	private void UsePerk(int inventory_index)
	{
		GD.Print(PlayerVariables.Instance.PlayerInventory[inventory_index]);
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
				UseDoublePointsPerk();
				break;
			case 3:
				UseBulletPerk();
				break;
			case 4:
				UseDoubleMoneyPerk();
				break;
			case 5:
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

	// this function is used when the game restarts. by default the car has the nohit health bar texture.
	public void SetDefaultHealthBar()
	{
		_HealthBar.Texture = _HealthBar_NoHit;
	}

	private void LowerPlayerHealth() // we lower the health of the player, and check if the health is equal or less than zero to send the game over signal.
	{
		// if the player currently has the shield, then we return and also play a special sound when crashing an enemy. the player does not get their health lowered
		if (ShieldPerkActive)
		{
			_ShieldPerkHitSound.Play();
			return;
		}

		PlayerVariables.Instance.PlayerHealth -= 1; // 40 is the health the player loses everytime it crashes with an enemy car

		// we update the health bar sprite depending on the value of the health

		UpdateHealthBarTexture();


		if (PlayerVariables.Instance.PlayerHealth <= 0) EmitSignal(SignalName.PlayerHealthDepleted);
		else _CarCrash.Play(); // if it's not game over yet, play de carcrash sound. otherwise the game over sound will be played
	}

	// the following functions are for the perks
	private void UseShieldPerk()
	{
		ShieldPerkActive = true;

		_ShieldPerkActiveTimer.Start();
		_ShieldPerkSound.Play();
		_ShieldPerkInUse.Modulate = new Color(1, 1, 1, 1); // we show the shield perk sprite on the player
	}

	// when the timer gets to the timeout, we stop the timer and also set the shield perk active variable to false.
	private void OnShieldPerkTimerTimeout()
	{
		ShieldPerkActive = false;
		_ShieldPerkActiveTimer.Stop();
		_ShieldPerkInUse.Modulate = new Color(0, 0, 0, 0); // we hide the shield perk sprite
	}

	private void UseExtraLifePerk()
	{
		PlayerVariables.Instance.PlayerHealth++;
		UpdateHealthBarTexture();
		_ExtraLifePerkSound.Play();
	}

	private void UseDoublePointsPerk()
	{

	}

	private void UseBulletPerk()
	{

	}

	private void UseDoubleMoneyPerk()
	{

	}

	private void UseSlowTimePerk()
	{

	}

	public void UpdateHealthBarTexture()
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

	// this is used to set the timers in the player to true or false. mostly used by perks.
	public void SetPlayerTimers(bool state)
	{
		if (ShieldPerkActive) _ShieldPerkActiveTimer.SetPaused(!state);
	}

	public void RestartPlayerVariables()
	{
		ShieldPerkActive = false;
		_ShieldPerkInUse.Modulate = new Color(0, 0, 0, 0);
		// in addition to the comment below, when the game is over this timer gets paused anyways, but we do it here too just so we have a control over the game.
		_ShieldPerkActiveTimer.SetPaused(false); // this pauses the shieldperkactive timer. when activating this timer again on the game, this restarts from its default value and waits until the timeout, so it's fine to just pause it here.
	}
}
