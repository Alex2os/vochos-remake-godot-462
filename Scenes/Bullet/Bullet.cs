using Godot;
using System;

public partial class Bullet : Area2D
{
	[Export] public float _BulletSpeed = 250;
	[Signal] public delegate void BulletHitEnemyEventHandler();
	
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}
	
	public override void _Process(double delta)
	{
		Position -= new Vector2(0, _BulletSpeed * (float)delta);

		// we erase the object if it gets to a certain Y value
		if(Position.Y <= -50) QueueFree();
	}

	private void OnAreaEntered(Node2D node)
	{
		// if the bullet hits an enemy, then destroy the bullet object
		if(node is CarEnemy)
		{
			EmitSignal(SignalName.BulletHitEnemy);
			QueueFree();
		}
	}
}
