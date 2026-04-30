using Godot;
using System;

public partial class SpeedingGame : Node2D
{
	[Export] Timer _SpeedingGameTimer;
	[Export] AnimationPlayer _AnimationPlayer;
	[Signal] public delegate void SpeedingTheGameEventHandler();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_SpeedingGameTimer.Timeout += OnTimeOut;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnTimeOut()
	{
		_AnimationPlayer.Play("speedinggameanim");
		EmitSignal(SignalName.SpeedingTheGame);
		_SpeedingGameTimer.Start();
	}
}