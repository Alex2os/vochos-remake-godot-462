using Godot;
using System;

public partial class Cloud : Node2D
{
	[Export] private Sprite2D _CloudSprite;
	[Export] private int _CloudSpeed = 30;
	[Export] public Texture2D[] _CloudSkins;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ChooseSkin(); // choose from the different skins the cloud has
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(_CloudSpeed * (float)delta, 0);
		if (Position.X >= 1100) QueueFree();
	}

	private void ChooseSkin()
	{
		int skin = GD.RandRange(0, 2);
		_CloudSprite.Texture = _CloudSkins[skin];
	}
}
