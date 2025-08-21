using Godot;
using System;

public partial class player : Area2D
{
	[Export] private int _MovingXAxis = 300;
	[Export] private int _MovingYAxis = 300;
	[Export] private AudioStreamPlayer _CarCrash;
	private int inventory_index = 0; // initialize the inventory index
	[Signal] public delegate void PlayerHealthDepletedEventHandler();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		AreaEntered += OnAreaEntered;

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
			inventory_index = 0;
			UsePerk(inventory_index); // here we send the inventory index that we want to use/check so the user can use the perk.
		}
		if (Input.IsActionJustPressed("perk2"))
		{
			inventory_index = 1;
			UsePerk(inventory_index);
		}
		if (Input.IsActionJustPressed("perk3"))
		{
			inventory_index = 2;
			UsePerk(inventory_index);
		}

	}

	private void OnAreaEntered(Area2D node)
	{

		if (node is Coin) ; // if it's a coin, then don't do anything. 
		else LowerPlayerHealth(); // in any other case, it's an enemy what hit the player, so we lower the health

	}

	private void UsePerk(int inventory_index)
	{
		switch (PlayerVariables.Instance.PlayerInventory[inventory_index])
		{
			case 0:
				UseShieldPerk();
				break;
			case 1:
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
				break;
		}

		PlayerVariables.Instance.PlayerInventory[inventory_index] = -1; // this is to reassign the inventory perk id when a perk is used. if there's no perk, this will axtivate too, so everytime we check this function the perk id of the inventory slot will be assigned to -1 at the ond of the function.
	}

	private void LowerPlayerHealth() // we lower the health of the player, and check if the health is equal or less than zero to send the game over signal.
	{
		PlayerVariables.Instance.PlayerHealth -= 40; // 40 is the health the player loses everytime it crashes with an enemy car
		if (PlayerVariables.Instance.PlayerHealth <= 0) EmitSignal(SignalName.PlayerHealthDepleted); 
		else _CarCrash.Play(); // if it's not game over yet, play de carcrash sound. otherwise the game over sound will be played
	}

	// the following functions are for the perks
	private void UseShieldPerk()
	{

	}

	private void UseExtraLifePerk()
	{

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
}
