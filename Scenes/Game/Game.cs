using Godot;
using System;

public partial class Game : Node2D
{
	[Export] private PackedScene _RoadScene;
	[Export] private PackedScene _CarEnemyScene;
	[Export] private PackedScene _CoinScene;
	[Export] private NodePath _SpeedingGamePath;
	[Export] private NodePath _SpawningRoadTimerPath;
	[Export] private NodePath _SpawningRoadMarkerPath;
	[Export] private NodePath _SpawningCarEnemyTimerPath;
	[Export] private NodePath _RoadContainerPath;
	[Export] private NodePath _EnemyContainerPath;
	[Export] private NodePath _EnemyMarkerRightPath;
	[Export] private NodePath _EnemyMarkerLeftPath;
	[Export] private NodePath _PlayerPath;
	[Export] private NodePath _ScoreLabelPath;
	[Export] private NodePath _CoinContainerPath;
	[Export] private NodePath _CoinTimerPath;
	[Export] private NodePath _CoinLabelPath;
	[Export] private NodePath _MainMenuButtonPath;
	[Export] private NodePath _GameOverLabelPath;
	[Export] private NodePath _GameOverStatsLabelPath;
	[Export] private NodePath _AnimationPlayerPath;
	[Export] private NodePath _GameOverRestartLabelPath;
	[Export] private NodePath _CarCrashPath;
	[Export] private NodePath _CarStartingPath;
	[Export] private NodePath _CoinSoundPath;
	[Export] private NodePath _GameMusicPath;

	private Marker2D _SpawningRoadMarker;
	private Timer _SpawningRoadTimer;
	private Timer _SpawningCarEnemyTimer;
	private Node2D _EnemyContainer;
	private Node2D _RoadContainer;
	private Marker2D _EnemyMarkerRight;
	private Marker2D _EnemyMarkerLeft;
	private player _Player;
	private Label _ScoreLabel;
	private Node2D _CoinContainer;
	private Timer _CoinTimer;
	private Label _CoinLabel;
	private bool _GameOver = false;
	private Label _GameOverLabel;
	private Label _GameOverStatsLabel;
	private Label _GameOverRestartLabel;
	private AnimationPlayer _AnimationPlayer;
	private AudioStreamPlayer _CarCrash;
	private AudioStreamPlayer _CarStarting;
	private AudioStreamPlayer _CoinSound;
	private AudioStreamPlayer _GameMusic;
	private SpeedingGame _SpeedingGame;
	private int _EnemySpeed = 200; // default enemy speed
	private int _TotalScore = 0; // score for the game
	private int _TotalCoins = 0;// total coins that the player has collected throughout the game
	private int _LeftCoins = 0; // coins that the user actually has. this value can be modified if the user buys things in the (future) shop
	private Button _MainMenuButton;
	private string _MainMenuScene = "res://Scenes/MainMenu/main_menu.tscn"; // should fix this later --> changing this to an export of type packedscene will cause trouble. so better to use this string as path to the scene.

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
		_CoinTimer = GetNode<Timer>(_CoinTimerPath);
		_CoinContainer = GetNode<Node2D>(_CoinContainerPath);
		_CoinLabel = GetNode<Label>(_CoinLabelPath);
		_GameOverLabel = GetNode<Label>(_GameOverLabelPath);
		_GameOverStatsLabel = GetNode<Label>(_GameOverStatsLabelPath);
		_GameOverRestartLabel = GetNode<Label>(_GameOverRestartLabelPath);
		_AnimationPlayer = GetNode<AnimationPlayer>(_AnimationPlayerPath);
		_CarCrash = GetNode<AudioStreamPlayer>(_CarCrashPath);
		_CarStarting = GetNode<AudioStreamPlayer>(_CarStartingPath);
		_CoinSound = GetNode<AudioStreamPlayer>(_CoinSoundPath);
		_GameMusic = GetNode<AudioStreamPlayer>(_GameMusicPath);
		_SpeedingGame = GetNode<SpeedingGame>(_SpeedingGamePath);
		_MainMenuButton = GetNode<Button>(_MainMenuButtonPath);

		_SpawningRoadTimer.Timeout += SpawnRoad;
		_SpawningCarEnemyTimer.Timeout += SpawnEnemy;
		_Player.PlayerHitEnemy += GameOver;
		_CoinTimer.Timeout += SpawnCoin;
		_SpeedingGame.SpeedingTheGame += OnSpeedingTheGame;
		_MainMenuButton.Pressed += OnMainMenuButtonPressed;

		GameStarted();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// restarting condition check and function
		if (_GameOver && Input.IsActionJustPressed("restart")) RestartGame();

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
		enemy._CarEnemySpeed = _EnemySpeed; // adjust the speed for the new enemy objects that are being generated
		float enemy_x_position = (float)GD.RandRange(_EnemyMarkerLeft.Position.X, _EnemyMarkerRight.Position.X);
		float enemy_y_position = _EnemyMarkerRight.Position.Y;
		enemy.Position = new Vector2(enemy_x_position, enemy_y_position);
		enemy.EnemyDestroyed += OnEnemyDestroyed;
	}

	private void GameOver()
	{
		// this function stops all the processes to show the game over screen

		GD.Print("game over!");
		_GameOver = true;

		// stop timers
		_SpawningRoadTimer.Stop();
		_SpawningCarEnemyTimer.Stop();
		_CoinTimer.Stop();

		_GameMusic.Stop();

		// final stats message: 
		_GameOverStatsLabel.Text = "Total Score: " + _TotalScore.ToString() + "\n" + "Money Left: " + _LeftCoins + "\n" + "Total Money Earned: " + _TotalCoins;
		// change opacity of text to show the game over and stats
		_GameOverRestartLabel.Modulate = new Color(1, 1, 1, 1);
		_AnimationPlayer.Play("restart animation"); // animation for the restart label to play it
		_GameOverLabel.Modulate = new Color(1, 1, 1, 1);
		_GameOverStatsLabel.Modulate = new Color(1, 1, 1, 1);

		// modulate for the main menu button
		_MainMenuButton.Modulate = new Color(1, 1, 1, 1);

		// stop each process for all the movable objects
		foreach (Node road in _RoadContainer.GetChildren()) road.SetProcess(false);

		foreach (Node coin in _CoinContainer.GetChildren()) coin.SetProcess(false);

		foreach (Node enemy in _EnemyContainer.GetChildren()) enemy.SetProcess(false);
		// change the timer for the speeding game scene and stopping it in case it's active when the game over screen is presented
		foreach (Node speed in _SpeedingGame.GetChildren())
		{
			if (speed is Timer timer)
			{
				timer.WaitTime = 10;
				timer.Stop();
			}

			if (speed is AnimationPlayer anim) anim.Stop();
		}

		// stop process for player
		_Player.SetProcess(false);
		_CarCrash.Play(); // car crashing sound

	}

	private void OnEnemyDestroyed()
	{

		_TotalScore++; // every time a enemy dies/gets destroyed, a point gets added to the total score
		_ScoreLabel.Text = _TotalScore.ToString();

	}

	private void OnCoinHitsPlayer()
	{
		_TotalCoins++; // total coins are the coins obtained in general in all of the game
		_LeftCoins++; // left coins are the actual coins in game, because with the coins you will be able to buy things in the future.
		_CoinLabel.Text = "$" + _LeftCoins.ToString();
		_CoinSound.Play();
	}

	private void SpawnCoin()
	{
		Coin coin = (Coin)_CoinScene.Instantiate();
		_CoinContainer.AddChild(coin);
		float coin_x_position = (float)GD.RandRange(_EnemyMarkerLeft.Position.X, _EnemyMarkerRight.Position.X); // using the same markers as the enemies.
		float coin_y_position = _EnemyMarkerRight.Position.Y;
		coin.Position = new Vector2(coin_x_position, coin_y_position);
		coin.CoinHitsPlayer += OnCoinHitsPlayer;
	}

	private void RestartGame()
	{

		_GameOver = false;
		_EnemySpeed = 200; // reset enemy speed

		// cleaning the labels so they don't show the prior score
		_CoinLabel.Text = "$0";
		_ScoreLabel.Text = "0";

		// color function/struct only accepts values from 0 to 1.
		_GameOverRestartLabel.Modulate = new Color(0, 0, 0, 0);
		_AnimationPlayer.Stop(); // stop the restart game animation
		_GameOverLabel.Modulate = new Color(0, 0, 0, 0); // change opacity of text to quit the game over and stats
		_GameOverStatsLabel.Modulate = new Color(0, 0, 0, 0);

		// modulate for the mainmenu button
		_MainMenuButton.Modulate = new Color(0, 0, 0, 0);

		_SpawningRoadTimer.Start();
		_SpawningCarEnemyTimer.Start();
		_CoinTimer.Start();


		foreach (Node road in _RoadContainer.GetChildren()) road.QueueFree();

		foreach (Node coin in _CoinContainer.GetChildren()) coin.QueueFree();

		foreach (Node enemy in _EnemyContainer.GetChildren()) enemy.QueueFree();

		foreach (Node speed in _SpeedingGame.GetChildren()) if (speed is Timer timer) timer.Start();

		_Player.SetProcess(true);
		_Player.Position = new Vector2(500, 530);
		GameStarted();

	}

	void GameStarted()
	{
		_TotalCoins = 0;
		_LeftCoins = 0;
		_TotalScore = 0;

		SpawnRoad(); // spawn a road ahead of the timer to start the game earlier (should fix this later)
		_CarStarting.Play();
		_GameMusic.Play();

	}

	private void OnSpeedingTheGame()
	{
		GD.Print("speeding the game!");

		// increase the speeds for enemies.
		_EnemySpeed += 30;
		// adjust the new speed for all the existing enemy objects
		foreach (CarEnemy enemy in _EnemyContainer.GetChildren()) enemy._CarEnemySpeed = _EnemySpeed;
	}

	private void OnMainMenuButtonPressed()
	{
		GetTree().ChangeSceneToFile(_MainMenuScene);
	}
}
