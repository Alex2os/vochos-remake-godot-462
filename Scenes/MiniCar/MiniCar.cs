using Godot;
using System;

public partial class MiniCar : Node2D
{
	[Export] private int _MiniCarSpeed = 100;
	[Export] private Sprite2D _MiniCarSprite;
	[Export] public Texture2D[] _MiniCarSkins;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ChooseSkin();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(_MiniCarSpeed * (float)delta, 0);
		if (Position.X >= 1100 || Position.X <= -100) QueueFree();
	}

	public void ChangeSpeedDirection()
	{
		_MiniCarSpeed = -100;
		// the FlipH is in true by default in the editor.
		_MiniCarSprite.FlipH = false; // with this we can flip the sprite of the the car horizontally (FlipH) or vertically (FlipV), so when it changes directions, the sprite of the car changes to the left.
	}

	private void ChooseSkin()
	{
		int skin = GD.RandRange(0, 9); // same amount of skins (and same colors) as the CarEnemy class/object
		_MiniCarSprite.Texture = _MiniCarSkins[skin];
	}
}
