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
	[Export] private NodePath _ShopAvailableTimerPath;
	[Export] private NodePath _InventoryInGamePath;
	[Export] private NodePath _GamePausedLabelPath;

	private Timer _SpawningRoadTimer;
	private Timer _SpawningCarEnemyTimer;
	private Timer _CoinTimer;
	private Timer _ShopAvailableTimer;
	private Node2D _EnemyContainer;
	private Node2D _RoadContainer;
	private Node2D _CoinContainer;
	private InventoryInGame _InventoryInGame;
	private Marker2D _EnemyMarkerRight;
	private Marker2D _EnemyMarkerLeft;
	private Marker2D _SpawningRoadMarker;
	private Label _ScoreLabel;
	private Label _CoinLabel;
	private Label _GameOverLabel;
	private Label _GameOverStatsLabel;
	private Label _GameOverRestartLabel;
	private AnimationPlayer _AnimationPlayer;
	private AudioStreamPlayer _CarCrash;
	private AudioStreamPlayer _CarStarting;
	private AudioStreamPlayer _CoinSound;
	private AudioStreamPlayer _GameMusic;
	private RichTextLabel _GamePausedLabel;
	private Button _MainMenuButton;
	private SpeedingGame _SpeedingGame;
	private player _Player;
	
	// initialize all the variables used in the game
	private int _TotalScore;
	private int _EnemySpeed;
	private int _LeftCoins;
	private int _TotalCoins;
	private bool _ComingFromShop;
	private int[] _PlayerInventory = new int[] { -1, -1, -1 };
	private bool _ShopAvailable = false;
	private bool _GameOver = false;
	private bool _IsGamePaused = false;
	// variables for the timers. this is to keep track of the timers whenever we pause the game, so when we resume the game, we already have the time the timers were left im.

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
		_ShopAvailableTimer = GetNode<Timer>(_ShopAvailableTimerPath);
		_InventoryInGame = GetNode<InventoryInGame>(_InventoryInGamePath);
		_GamePausedLabel = GetNode<RichTextLabel>(_GamePausedLabelPath);

		_SpawningRoadTimer.Timeout += SpawnRoad;
		_SpawningCarEnemyTimer.Timeout += SpawnEnemy;
		_Player.PlayerHitEnemy += GameOver;
		_CoinTimer.Timeout += SpawnCoin;
		_SpeedingGame.SpeedingTheGame += OnSpeedingTheGame;
		_MainMenuButton.Pressed += OnMainMenuButtonPressed;
		_ShopAvailableTimer.Timeout += OnShopAvailable;

		GameStarted();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// restarting condition check and function
		if (_GameOver && Input.IsActionJustPressed("restart")) RestartGame();

		// going to shop if available
		if (_ShopAvailable && Input.IsActionJustPressed("use shop")) GameManager.Instance.ChangeSceneToShop();

		// pausing the game if _gameover is false
		if (!_GameOver && Input.IsActionJustPressed("pause")) PauseGame();

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
		GD.Print("game over!");
		_GameOver = true;

		// final stats message: 
		_GameOverStatsLabel.Text = "Total Score: " + _TotalScore.ToString() + "\n" + "Money Left: " + _LeftCoins + "\n" + "Total Money Earned: " + _TotalCoins;
		// change opacity of text to show the game over and stats
		_GameOverRestartLabel.Modulate = new Color(1, 1, 1, 1);
		_AnimationPlayer.Play("restart animation"); // animation for the restart label to play it
		_GameOverLabel.Modulate = new Color(1, 1, 1, 1);
		_GameOverStatsLabel.Modulate = new Color(1, 1, 1, 1);

		// modulate for the main menu button
		_MainMenuButton.Modulate = new Color(1, 1, 1, 1);

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

		PauseGame();

		_GameMusic.Stop();

		_CarCrash.Play(); // car crashing sound
	}

	// this function stops all the processes to show the game over screen. it's arranged to work too with the GameOver() function, so we use less lines of code.
	private void PauseGame()
	{
		bool set_process_bool; // this helps us to control the process of the nodes, which depends on the variable _isgamepaused

		if (_IsGamePaused)
		{
			set_process_bool = true;
			_IsGamePaused = false;
			_GameMusic.VolumeDb += 10; 
		}
		else // if it's game over, then this will pop up, pausing the game.
		{
			set_process_bool = false;
			_IsGamePaused = true;
			_GameMusic.VolumeDb -= 10; // with this we can make the volume in db of and audiostream lower, so if the game is paused, the db will lower, and if it's unpaused, the db will go up again.
		}

		if(!_GameOver && !_IsGamePaused) _GamePausedLabel.Modulate = new Color(0, 0, 0, 0); // if the game is not over and game is not paused, don't show the label
		else if (!_GameOver && _IsGamePaused) _GamePausedLabel.Modulate = new Color(1, 1, 1, 1); // otherwise, show it.
		else _GamePausedLabel.Modulate = new Color(0, 0, 0, 0); // if it's any other case (which is every time _gameover is true) then hide the label.

		// set the paused state of the timers to false, so they can follow in the time whey were left in or get paused.
		SetPausedStateTimers(_IsGamePaused);

		ShowPausedLabel(_IsGamePaused); // we send the paused state to this function to check wheter to show the paused label or not

		// stop each process for all the movable objects
		foreach (Node road in _RoadContainer.GetChildren()) road.SetProcess(set_process_bool);

		foreach (Node coin in _CoinContainer.GetChildren()) coin.SetProcess(set_process_bool);

		foreach (Node enemy in _EnemyContainer.GetChildren()) enemy.SetProcess(set_process_bool);

		// stop/start the timer for speeding game and the animation.
		foreach (Node speed in _SpeedingGame.GetChildren())
		{
			if (speed is Timer timer)
			{
				if (set_process_bool) timer.SetPaused(false); // if the process are turning back to true, then put the paused state to false, and viceversa. for the else below
				else timer.SetPaused(true);
			}

			if (speed is AnimationPlayer anim)
			{
				if (_GameOver) anim.Stop();
			}
		}

		// stop process for player
		_Player.SetProcess(set_process_bool);


	}

	private void OnEnemyDestroyed()
	{
		PlayerVariables.Instance.PlayerScore++;
		_TotalScore = PlayerVariables.Instance.PlayerScore; // every time a enemy dies/gets destroyed, a point gets added to the total score

		_ScoreLabel.Text = _TotalScore.ToString();

	}

	private void OnCoinHitsPlayer()
	{
		PlayerVariables.Instance.PlayerCoins++; // left coins are the actual coins in game, because with the coins you will be able to buy things in the future.
		_LeftCoins = PlayerVariables.Instance.PlayerCoins;

		PlayerVariables.Instance.PlayerTotalCoins++; // total coins are the coins obtained in general in all of the game
		_TotalCoins = PlayerVariables.Instance.PlayerTotalCoins++;

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
		_IsGamePaused = false;

		_GameMusic.VolumeDb = -23; // reassign the volume for the game music. -23 is the base value that is used in the editor (godot)

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

		SetPausedStateTimers(false); // put the state of pause of the timers in false when we start again

		_SpawningRoadTimer.Start();
		_SpawningCarEnemyTimer.Start();
		_CoinTimer.Start();
		_ShopAvailableTimer.Start();


		foreach (Node road in _RoadContainer.GetChildren()) road.QueueFree();

		foreach (Node coin in _CoinContainer.GetChildren()) coin.QueueFree();

		foreach (Node enemy in _EnemyContainer.GetChildren()) enemy.QueueFree();

		foreach (Node speed in _SpeedingGame.GetChildren()) if (speed is Timer timer)
			{
				// set the timer again for the next game. waittime is resetted and setpaused state is set to false to keep the timer going.
				timer.SetPaused(false);
				timer.WaitTime = 10;
				timer.Start();
			}

		_Player.SetProcess(true);
		_Player.Position = new Vector2(500, 530);
		GameStarted();

	}

	void GameStarted()
	{
		AssignGameVariables();
		UpdateLabels(); // this function is used for when the user comes from the shop, to re-update the labels that contain the user's score and money.

		SpawnRoad(); // spawn a road ahead of the timer to start the game earlier (should fix this later)
		_CarStarting.Play();
		_GameMusic.Play();

	}

	private void OnSpeedingTheGame()
	{
		GD.Print("speeding the game!");

		// increase the speeds for enemies.
		EnemyManager.Instance.EnemySpeed += 30;
		_EnemySpeed = EnemyManager.Instance.EnemySpeed;

		_SpawningCarEnemyTimer.WaitTime -= 0.2; // we keep lowing the timer

		// adjust the new speed for all the existing enemy objects
		foreach (CarEnemy enemy in _EnemyContainer.GetChildren()) enemy._CarEnemySpeed = _EnemySpeed;
	}

	private void OnMainMenuButtonPressed()
	{
		GameManager.Instance.ChangeSceneToMainMenu();
	}

	private void OnShopAvailable()
	{
		GD.Print("shop available!");
		_ShopAvailableTimer.Stop();
		_ShopAvailable = true;
	}

	// in the function below we initialize all the variables from the singleton/autoload
	private void AssignGameVariables()
	{
		if (CheckComingFromShop()) ; // if the function returns true (which is the case when the player is coming from the shop) the game doesn't initialize the variables again.
		else InitializeGameVariables(); // otherwise, the program will initialize (start from the predetermined start values) all the variables. and then, the predetermined values wiil be assigned again in this function.
										// at the same time, we update the inventory, the inventory's perks textures and the leftcoins if coming from the shop in this same function, without having to do another one.

		// player
		_TotalScore = PlayerVariables.Instance.PlayerScore;
		_EnemySpeed = EnemyManager.Instance.EnemySpeed;
		_LeftCoins = PlayerVariables.Instance.PlayerCoins;
		_TotalCoins = PlayerVariables.Instance.PlayerTotalCoins;

		// shop variables
		GameManager.Instance.RerollShopCost = 1;

		// game manager variables
		_ComingFromShop = GameManager.Instance.ComingFromShop;

		// player inventory
		for (int i = 0; i < _PlayerInventory.Length; i++) _PlayerInventory[i] = PlayerVariables.Instance.PlayerInventory[i];

		_InventoryInGame.UpdateInventoryPerksTextures(); // this updates the textures of the perks in the inventory
														 // enemy
		_EnemySpeed = EnemyManager.Instance.EnemySpeed;
	}

	private bool CheckComingFromShop()
	{
		_ComingFromShop = GameManager.Instance.ComingFromShop; // this is a variable to check whether the player is coming from the shop or not.

		if (_ComingFromShop)
		{
			GameManager.Instance.ComingFromShop = false; // we reassign the variable so there's no trouble if the game is restarted again.
			_ComingFromShop = GameManager.Instance.ComingFromShop;

			return true; // if the player is coming from shop, returns true
		}

		return false; // if the player doesn't come from shop, returns false.
	}

	private void InitializeGameVariables()
	{
		PlayerVariables.Instance.InitializePlayerVariables();
		EnemyManager.Instance.InitializeEnemyVariables();
		GameManager.Instance.InitializeGameManagerVariables();
	}

	// this function is used for when the user comes from the shop. 
	private void UpdateLabels()
	{
		_ScoreLabel.Text = _TotalScore.ToString();
		_CoinLabel.Text = "$" + _LeftCoins.ToString();
	}

	private void ShowPausedLabel(bool paused_state)
	{
		if (paused_state) ;
		else;
	}

	private void SetPausedStateTimers(bool paused_state)
	{
		_SpawningRoadTimer.SetPaused(paused_state);
		_SpawningCarEnemyTimer.SetPaused(paused_state);
		_CoinTimer.SetPaused(paused_state);
		_ShopAvailableTimer.SetPaused(paused_state);
	}
}
