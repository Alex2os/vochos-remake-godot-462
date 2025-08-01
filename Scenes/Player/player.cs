using Godot;
using System;

public partial class player : Area2D
{
	[Export] private int _MovingXAxis = 300;
	[Export] private int _MovingYAxis = 300;

	[Signal] public delegate void PlayerHitEnemyEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		AreaEntered += OnAreaEntered;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		// player movement
		if (Input.IsActionPressed("left")) Position -= new Vector2(_MovingXAxis * (float)delta, 0);

		if (Input.IsActionPressed("right")) Position += new Vector2(_MovingXAxis * (float)delta, 0);

		if (Input.IsActionPressed("up")) Position -= new Vector2(0, _MovingYAxis * (float)delta);

		if (Input.IsActionPressed("down")) Position += new Vector2(0, _MovingYAxis * (float)delta);

	}

	private void OnAreaEntered(Area2D node)
	{

		EmitSignal(SignalName.PlayerHitEnemy);

	}
}
