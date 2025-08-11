using Godot;
using System;

public partial class player : Area2D
{
	[Export] private int _MovingXAxis = 300;
	[Export] private int _MovingYAxis = 300;
	// this nodepath is referenced in the game scene. in the car scene itself is not referenced, as it's better to have the node in the gamescene rather than in the playerscene
	// [Export] private NodePath _ShopScenePath;
	[Signal] public delegate void PlayerHitEnemyEventHandler();
	// private Shop _ShopScene;
	// inventory for the car scene. this inventory will be updated when the shop is used or the user uses a perk in-game
	public int[] _PlayerInventoryCarScene = new int[3] { -1, -1, -1 };

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		// _ShopScene.InventoryUpdated += OnInventoryUpdated;
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

	}

	private void OnAreaEntered(Area2D node)
	{
		
		if (node is Coin) ; // if it's a coin, then don't send the signal that the player hit an enemy.
		else EmitSignal(SignalName.PlayerHitEnemy);

	}

	private void OnInventoryUpdated(int perk1, int perk2, int perk3) // here are sent all the perks in order.
	{
		// update the new perks (if changed) so now we have them inside the player scene
		_PlayerInventoryCarScene[0] = perk1;
		_PlayerInventoryCarScene[1] = perk2;
		_PlayerInventoryCarScene[2] = perk3;
	}
}
