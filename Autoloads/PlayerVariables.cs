using Godot;
using System;

public partial class PlayerVariables : Node
{
	public static PlayerVariables Instance { get; private set; }

	public int PlayerCoins = 0; // 0 by default
	public int PlayerTotalCoins = 0;
	public int PlayerScore = 0; // 0 by default
	public int PlayerHealth = 3;
	public static int PlayerInventorySize = 3;
	public int[] PlayerInventory = new int[] { -1, -1, -1 }; // main player inventory.

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this; // remember to initialize the instance, otherwise it will not work as an object/instance itself.
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InitializePlayerVariables()
	{
		PlayerCoins = 1000;
		PlayerTotalCoins = 0;
		PlayerScore = 0;
		PlayerHealth = 3;

		// we empty the player inventory
		for (int i = 0; i < PlayerInventory.Length; i++) PlayerInventory[i] = 2;

		// for testing only, we assign the perk numbers directly from here to not wait until the shop when testing the game.
		/*
		PlayerInventory[0] = 5;
		PlayerInventory[1] = 5;
		PlayerInventory[2] = 5;
		*/
	}
}
