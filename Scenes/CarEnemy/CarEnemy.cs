using Godot;
using System;

public partial class CarEnemy : Area2D
{
	[Export] private int _CarEnemySpeed = 200;

	[Signal] public delegate void EnemyDestroyedEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		Position += new Vector2(0, _CarEnemySpeed * (float)delta);

		if (Position.Y >= 800) DestroyEnemy();

	}

	private void DestroyEnemy()
	{
		QueueFree();
		EmitSignal(SignalName.EnemyDestroyed);

	}
}
