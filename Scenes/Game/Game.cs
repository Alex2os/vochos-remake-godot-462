using Godot;
using System;

public partial class Game : Node2D
{
	[Export] private PackedScene _RoadScene;
	[Export] private PackedScene _CarEnemyScene;
	[Export] private NodePath _SpawningRoadTimerPath;
	[Export] private NodePath _SpawningRoadMarkerPath;
	[Export] private NodePath _SpawningCarEnemyTimerPath;
	[Export] private NodePath _RoadContainerPath;
	[Export] private NodePath _EnemyContainerPath;
	[Export] private NodePath _EnemyMarkerRightPath;
	[Export] private NodePath _EnemyMarkerLeftPath;
	[Export] private NodePath _PlayerPath;

	[Export] private NodePath _ScoreLabelPath;

	private Marker2D _SpawningRoadMarker;
	private Timer _SpawningRoadTimer;
	private Timer _SpawningCarEnemyTimer;
	private Node2D _EnemyContainer;
	private Node2D _RoadContainer;
	private Marker2D _EnemyMarkerRight;
	private Marker2D _EnemyMarkerLeft;
	private player _Player;
	private Label _ScoreLabel;
	private int _TotalScore = 0; // score for the game

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		_SpawningRoadTimer = GetNode<Timer>(_SpawningRoadTimerPath);
		_SpawningRoadMarker = GetNode<Marker2D>(_SpawningRoadMarkerPath);
		_SpawningCarEnemyTimer = GetNode<Timer>(_SpawningCarEnemyTimerPath);
		_RoadContainer = GetNode<Node2D>(_RoadContainerPath);
		_EnemyContainer = GetNode<Node2D>(_EnemyContainerPath);
		_EnemyMarkerLeft = GetNode<Marker2D>(_EnemyMarkerLeftPath);
		_EnemyMarkerRight = GetNode<Marker2D>(_EnemyMarkerRightPath);
		_Player = GetNode<player>(_PlayerPath);
		_ScoreLabel = GetNode<Label>(_ScoreLabelPath);

		_SpawningRoadTimer.Timeout += SpawnRoad;
		_SpawningCarEnemyTimer.Timeout += SpawnEnemy;
		_Player.PlayerHitEnemy += GameOver;

		SpawnRoad(); // spawn a road ahead of the timer to start the game earlier (should fix this later)

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SpawnRoad()
	{

		Road road = (Road)_RoadScene.Instantiate();
		_RoadContainer.AddChild(road);
		road.Position = new Vector2(_SpawningRoadMarker.Position.X, _SpawningRoadMarker.Position.Y);
	}

	private void SpawnEnemy()
	{
		CarEnemy enemy = (CarEnemy)_CarEnemyScene.Instantiate();
		_EnemyContainer.AddChild(enemy);
		float x_position = (float)GD.RandRange(_EnemyMarkerLeft.Position.X, _EnemyMarkerRight.Position.X);
		float y_position = _EnemyMarkerRight.Position.Y;
		enemy.Position = new Vector2(x_position, y_position);
		enemy.EnemyDestroyed += OnEnemyDestroyed;
	}

	private void GameOver()
	{

		GD.Print("game over!");

	}

	private void OnEnemyDestroyed()
	{

		_TotalScore++; // every time a enemy dies/gets destroyed, a point gets added to the total score
		_ScoreLabel.Text = _TotalScore.ToString();

	}
}
