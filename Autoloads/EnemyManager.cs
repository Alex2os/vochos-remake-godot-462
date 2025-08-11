using Godot;
using System;

public partial class EnemyManager : Node
{

	public static EnemyManager Instance { get; private set; }
	public int EnemySpeed = 200; // default enemy speed

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void InitializeEnemyVariables()
	{
		EnemySpeed = 200;
	}
}
