using Godot;
using System;

public partial class CarEnemy : Area2D
{
	[Export] public int _CarEnemySpeed = 200;
	[Export] private Sprite2D _Sprite2D;
	[Signal] public delegate void EnemyDestroyedEventHandler();
	[Export] public Texture2D[] _EnemySkins; // array that contains all the textures for the enemy

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
		ChooseSkin();
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

	private void ChooseSkin()
	{
		int skin = GD.RandRange(0, 9); // choose a skin for the enemy. in total, there are 10 textures in the arra _EnemySkins, so we choose a number from 0 to 9.
		_Sprite2D.Texture = _EnemySkins[skin];
	}

	private void OnAreaEntered(Area2D node)
	{
		if (node is player) QueueFree(); // if it's the player that hit the enemy, destroy itself
		else if (node is Bullet) QueueFree(); // the same happens for when it hits a bullet.
	}
}
