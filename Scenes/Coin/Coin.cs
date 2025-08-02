using Godot;
using System;

public partial class Coin : Area2D
{
	[Export] private int _CoinSpeed = 100;
	[Signal] public delegate void CoinHitsPlayerEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		AreaEntered += OnAreaEntered;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Position += new Vector2(0, _CoinSpeed * (float)delta);

		if (Position.Y >= 800) QueueFree(); // destroy coins if the fall below viewport sight

	}

	private void OnAreaEntered(Area2D node)
	{
		if (node is player)
		{
			EmitSignal(SignalName.CoinHitsPlayer); // alarm to detect when a coin hits the player
			QueueFree(); // destroy the coin
		}

	}
}
