using Godot;
using System;

public partial class ShopPickable : Area2D
{
	[Signal] public delegate void ShopPickableHitPlayerEventHandler();
	[Export] private int _ShopPickableYSpeed = 150;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Position += new Vector2(0, _ShopPickableYSpeed * (float)delta);
		
		if (Position.Y >= 800) QueueFree(); // we destroy the pickable if it reaches a certain y value

	}

	private void OnAreaEntered(Node2D node)
	{
		if(node is player) // we emit a signal when it's the player and also we destroy the pickable with QueueFree();
		{
			EmitSignal(SignalName.ShopPickableHitPlayer);
			QueueFree();
		}
	}
}
