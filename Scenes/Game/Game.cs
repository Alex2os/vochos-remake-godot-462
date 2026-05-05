using Godot;

public partial class Game : Node2D
{
	[Export] private PackedScene _RoadScene;
	[Export] private PackedScene _CarEnemyScene;
	[Export] private PackedScene _CoinScene;
	[Export] private PackedScene _BulletScene;
	[Export] private PackedScene _ParkingLotRoadScene;
	[Export] private PackedScene _ShopPickableScene;
	[Export] private NodePath _SpeedingGamePath;
	[Export] private NodePath _SpawningRoadTimerPath;
	[Export] private NodePath _SpawningRoadMarkerPath;
	[Export] private NodePath _SpawningCarEnemyTimerPath;
	[Export] private NodePath _RoadContainerPath;
	[Export] private NodePath _EnemyContainerPath;
	[Export] private NodePath _BulletContainerPath;
	[Export] private NodePath _SpawnMarkerRightPath;
	[Export] private NodePath _SpawnMarkerLeftPath;
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
	[Export] private NodePath _GameOverCrashPath;
	[Export] private NodePath _CarStartingPath;
	[Export] private NodePath _CoinSoundPath;
	[Export] private NodePath _GameMusicPath;
	[Export] private NodePath _ShopAvailableTimerPath;
	[Export] private NodePath _InventoryInGamePath;
	[Export] private NodePath _GamePausedLabelPath;
	[Export] private NodePath _ShopReadyLabelPath;
	[Export] private NodePath _BulletHitEnemySoundPath;
	[Export] private NodePath _PerkTimerTexture1Path;
	[Export] private NodePath _PerkTimerTexture3Path;
	[Export] private NodePath _PerkTimerTexture2Path;
	[Export] private NodePath _PerkTimerText1Path;
	[Export] private NodePath _PerkTimerText3Path;
	[Export] private NodePath _PerkTimerText2Path;
	[Export] private NodePath _PerkTimer1Path;
	[Export] private NodePath _PerkTimer2Path;
	[Export] private NodePath _PerkTimer3Path;
	[Export] private NodePath _ShopPickableContainerPath;
	[Export] private NodePath _TransparentBlackScreenPath;
	[Export] private NodePath _SoundButtonPath;
	[Export] private NodePath _MusicButtonPath;

	private Timer _SpawningRoadTimer;
	private Timer _SpawningCarEnemyTimer;
	private Timer _CoinTimer;
	private Timer _ShopAvailableTimer;
	private Node2D _EnemyContainer;
	private Node2D _RoadContainer;
	private Node2D _CoinContainer;
	private Node2D _BulletContainer;
	private Node2D _ShopPickableContainer;
	private InventoryInGame _InventoryInGame;
	private Marker2D _SpawnMarkerRight;
	private Marker2D _SpawnMarkerLeft;
	private Marker2D _SpawningRoadMarker;
	private Label _ScoreLabel;
	private Label _CoinLabel;
	private Label _GameOverLabel;
	private Label _GameOverStatsLabel;
	private Label _GameOverRestartLabel;
	private Label _PerkTimerText1;
	private Label _PerkTimerText2;
	private Label _PerkTimerText3;
	private AnimationPlayer _AnimationPlayer;
	private AudioStreamPlayer _GameOverCrash;
	private AudioStreamPlayer _CarStarting;
	private AudioStreamPlayer _CoinSound;
	private AudioStreamPlayer _BulletHitEnemySound;
	private AudioStreamPlayer _GameMusic;
	private RichTextLabel _GamePausedLabel;
	private Button _MainMenuButton;
	private SpeedingGame _SpeedingGame;
	private player _Player;
	private Sprite2D _PerkTimerTexture1;
	private Sprite2D _PerkTimerTexture2;
	private Sprite2D _PerkTimerTexture3;
	private Timer _PerkTimer1;
	private Timer _PerkTimer2;
	private Timer _PerkTimer3;
	private Button _SoundButton;
	private Button _MusicButton;
	private Sprite2D _TransparentBlackScreen;
	private Texture2D[] _PerksTextureTimers; // array for the perks' timers textures. we save the textures here so we can use them when a perk is activated.
	[Signal] public delegate void ShieldPerkEndedEventHandler();
	[Signal] public delegate void DoublePointsPerkEndedEventHandler();
	[Signal] public delegate void DoubleMoneyPerkEndedEventHandler();
	[Signal] public delegate void SlowTimePerkEndedEventHandler();

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
	private bool _DoublePointsPerkActive = false; // variable used to know if the double points perk is active, so we can give the player double points correctly
	private bool _DoubleMoneyPerkActive = false; // same as the double points perk, but for the double money perk
	private bool _SlowTimePerkActive = false; // variable for the slowtime perk, same as the other two above.
	private bool[] _PerksTimerSlot = [false, false, false];
	private int[] _PerkActiveInTimer = [-1, -1, -1];

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		_SpawningRoadTimer = GetNode<Timer>(_SpawningRoadTimerPath);
		_SpawningRoadMarker = GetNode<Marker2D>(_SpawningRoadMarkerPath);
		_SpawningCarEnemyTimer = GetNode<Timer>(_SpawningCarEnemyTimerPath);
		_RoadContainer = GetNode<Node2D>(_RoadContainerPath);
		_EnemyContainer = GetNode<Node2D>(_EnemyContainerPath);
		_BulletContainer = GetNode<Node2D>(_BulletContainerPath);
		_SpawnMarkerLeft = GetNode<Marker2D>(_SpawnMarkerLeftPath);
		_SpawnMarkerRight = GetNode<Marker2D>(_SpawnMarkerRightPath);
		_Player = GetNode<player>(_PlayerPath);
		_ScoreLabel = GetNode<Label>(_ScoreLabelPath);
		_CoinTimer = GetNode<Timer>(_CoinTimerPath);
		_CoinContainer = GetNode<Node2D>(_CoinContainerPath);
		_CoinLabel = GetNode<Label>(_CoinLabelPath);
		_GameOverLabel = GetNode<Label>(_GameOverLabelPath);
		_GameOverStatsLabel = GetNode<Label>(_GameOverStatsLabelPath);
		_GameOverRestartLabel = GetNode<Label>(_GameOverRestartLabelPath);
		_AnimationPlayer = GetNode<AnimationPlayer>(_AnimationPlayerPath);
		_GameOverCrash = GetNode<AudioStreamPlayer>(_GameOverCrashPath);
		_CarStarting = GetNode<AudioStreamPlayer>(_CarStartingPath);
		_CoinSound = GetNode<AudioStreamPlayer>(_CoinSoundPath);
		_GameMusic = GetNode<AudioStreamPlayer>(_GameMusicPath);
		_SpeedingGame = GetNode<SpeedingGame>(_SpeedingGamePath);
		_MainMenuButton = GetNode<Button>(_MainMenuButtonPath);
		_ShopAvailableTimer = GetNode<Timer>(_ShopAvailableTimerPath);
		_InventoryInGame = GetNode<InventoryInGame>(_InventoryInGamePath);
		_GamePausedLabel = GetNode<RichTextLabel>(_GamePausedLabelPath);
		_BulletHitEnemySound = GetNode<AudioStreamPlayer>(_BulletHitEnemySoundPath);
		_PerkTimerTexture1 = GetNode<Sprite2D>(_PerkTimerTexture1Path);
		_PerkTimerTexture2 = GetNode<Sprite2D>(_PerkTimerTexture2Path);
		_PerkTimerTexture3 = GetNode<Sprite2D>(_PerkTimerTexture3Path);
		_PerkTimer1 = GetNode<Timer>(_PerkTimer1Path);
		_PerkTimer2 = GetNode<Timer>(_PerkTimer2Path);
		_PerkTimer3 = GetNode<Timer>(_PerkTimer3Path);
		_PerkTimerText1 = GetNode<Label>(_PerkTimerText1Path);
		_PerkTimerText2 = GetNode<Label>(_PerkTimerText2Path);
		_PerkTimerText3 = GetNode<Label>(_PerkTimerText3Path);
		_ShopPickableContainer = GetNode<Node2D>(_ShopPickableContainerPath);
		_TransparentBlackScreen = GetNode<Sprite2D>(_TransparentBlackScreenPath);
		_SoundButton = GetNode<Button>(_SoundButtonPath);
		_MusicButton = GetNode<Button>(_MusicButtonPath);

		_SpawningRoadTimer.Timeout += SpawnRoad;
		_SpawningCarEnemyTimer.Timeout += SpawnEnemy;
		_Player.PlayerHealthDepleted += GameOver;
		_Player.BulletPerkUsed += SpawnBullet;
		_Player.UpdateInventoryPerkTexture += OnUpdateInventoryPerkTexture;
		_CoinTimer.Timeout += SpawnCoin;
		_SpeedingGame.SpeedingTheGame += OnSpeedingTheGame;
		_MainMenuButton.Pressed += OnMainMenuButtonPressed;
		_ShopAvailableTimer.Timeout += SpawnShopPickable;
		_Player.ShieldPerkUsed += OnPlayerUsedPerk;
		_Player.DoublePointsPerkUsed += OnPlayerUsedPerk;
		_Player.DoubleMoneyPerkUsed += OnPlayerUsedPerk;
		_SoundButton.Pressed += OnSoundButtonPressed;
		_MusicButton.Pressed += OnMusicButtonPressed;
		// we use a single function to control the timers' timeouts. depending on which perk was used, the function OnPerkTimerTimeout controls what to do next.
		// we use 3 timers as there can only be at maximum 3 perks active with timeouts or timers.
		_PerkTimer1.Timeout += () => OnPerkTimerTimeout(0, _PerkActiveInTimer[0]);
		_PerkTimer2.Timeout += () => OnPerkTimerTimeout(1, _PerkActiveInTimer[1]);
		_PerkTimer3.Timeout += () => OnPerkTimerTimeout(2, _PerkActiveInTimer[2]);

		// we subscribe the player's functions to the signals here in the game scene like the below lines of code.
		ShieldPerkEnded += _Player.OnShieldPerkEnded;
		DoublePointsPerkEnded += _Player.OnDoublePointsPerkEnded;
		DoubleMoneyPerkEnded += _Player.OnDoubleMoneyPerkEnded;
		SlowTimePerkEnded += _Player.OnSlowTimePerkEnded;

		// we load the medium size perks to use them when a perk is activated, and we assign its perk texture. we load all of them so there's no problem when assigning them, as we use the respective perk number to assign them, so it's better to have them this way.
		_PerksTextureTimers = new Texture2D[]
		{
			GD.Load<Texture2D>("res://assets/perks/medium sizes/shield medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/extra life medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/double points medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/bullet medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/double money medium.png"),
			GD.Load<Texture2D>("res://assets/perks/medium sizes/time slow medium.png"),

		};

		GameStarted();

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// restarting condition check and function
		if (_GameOver && Input.IsActionJustPressed("restart")) RestartGame();

		// pausing the game if _gameover is false
		if (!_GameOver && Input.IsActionJustPressed("pause")) PauseGame();

		if (_PerksTimerSlot[0]) _PerkTimerText1.Text = $"{_PerkTimer1.TimeLeft:F1}"; // we can short the amount of numbers after the . of a float/double number using $ to use the variable inside the string and then using :F1, :F2, etc., to shorten the amount of numbers. in this case we use :F1 to just have one number after the point.
		if (_PerksTimerSlot[1]) _PerkTimerText2.Text = $"{_PerkTimer2.TimeLeft:F1}";
		if (_PerksTimerSlot[2]) _PerkTimerText3.Text = $"{_PerkTimer3.TimeLeft:F1}";

	}

	private void OnSoundButtonPressed()
	{

		if (GameManager.Instance.SoundActive) GameManager.Instance.SoundActive = false;
		else GameManager.Instance.SoundActive = true;

		GameManager.Instance.ChangeSoundButtonTextures(_SoundButton);

	}

	private void OnMusicButtonPressed()
	{
		if (GameManager.Instance.MusicActive)
		{
			GameManager.Instance.MusicActive = false;
			_GameMusic.Stop();
		}
		else
		{
			GameManager.Instance.MusicActive = true;
			_GameMusic.Play();
		}

		GameManager.Instance.ChangeMusicButtonTextures(_MusicButton);
	}

	private void SpawnParkingLotRoad()
	{
		ParkingLotRoad parking_lot_road = (ParkingLotRoad)_ParkingLotRoadScene.Instantiate();
		_RoadContainer.AddChild(parking_lot_road);
		parking_lot_road.Position = new Vector2(_SpawningRoadMarker.Position.X, _SpawningRoadMarker.Position.Y + 1175);
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
		float enemy_x_position = (float)GD.RandRange(_SpawnMarkerLeft.Position.X, _SpawnMarkerRight.Position.X);
		float enemy_y_position = _SpawnMarkerRight.Position.Y;
		enemy.Position = new Vector2(enemy_x_position, enemy_y_position);
		enemy.EnemyDestroyed += OnEnemyDestroyed;
	}

	private void SpawnShopPickable()
	{
		GD.Print("spawning shop pickable");

		ShopPickable shop_pickable = (ShopPickable)_ShopPickableScene.Instantiate();
		_ShopPickableContainer.AddChild(shop_pickable);
		float shop_pickable_x_position = (float)GD.RandRange(_SpawnMarkerLeft.Position.X, _SpawnMarkerRight.Position.X);
		float shop_pickable_y_position = _SpawnMarkerRight.Position.Y;
		shop_pickable.Position = new Vector2(shop_pickable_x_position, shop_pickable_y_position);
		shop_pickable.ShopPickableHitPlayer += OnShopPickableHitPlayer;

	}


	private void OnShopPickableHitPlayer()
	{
		// in this case, we have to use CallDeferred to call the function, as it gives us an error when using the function normally.
		// CallDeferred() allows us to call a function when the physics frame has ended. we are working with physics in this case (in our ShopPickable for this case), so for this to not give error we use this.
		CallDeferred(nameof(ChangeSceneToShop)); // we cant put inside GameManager.Instance.ChangeSceneToShop(), as it will not be recognized.
	}

	private void ChangeSceneToShop() { GameManager.Instance.ChangeSceneToShop(); }

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

		_MainMenuButton.Disabled = false;

		PauseGame();

		_GameMusic.Stop();

		_GameOverCrash.Play(); // car crashing sound
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
			_TransparentBlackScreen.Modulate = new Color(0, 0, 0, 0); // we hide the transparent black screen every time the game is unpaused
			_MusicButton.Modulate = new Color(0, 0, 0, 0); // we hide the music and sound buttons when the game is unpaused
			_SoundButton.Modulate = new Color(0, 0, 0, 0);
			// we also disable them so the player cant use them
			_MusicButton.Disabled = true;
			_SoundButton.Disabled = true;
		}
		else // if it's game over, then this will pop up, pausing the game.
		{
			set_process_bool = false;
			_IsGamePaused = true;
			_GameMusic.VolumeDb -= 10; // with this we can make the volume in db of and audiostream lower, so if the game is paused, the db will lower, and if it's unpaused, the db will go up again.
			_TransparentBlackScreen.Modulate = new Color(0, 0, 0, (float)0.5); // we show the transparent black screen every time the game is paused
																			   // in the case of the transparent black screen, we assign a black color (first three zeros) and then the alpha, which in this case is 0.5 for 50% transparency.
			_MusicButton.Modulate = new Color(1, 1, 1, 1); // we show the music and sound button when pausing the game
			_SoundButton.Modulate = new Color(1, 1, 1, 1);
			// we allow the buttons to be used assigning disabled to false.
			_MusicButton.Disabled = false;
			_SoundButton.Disabled = false;
		}

		// in the paused menu we show the gamepausedlabel and the main menu button, besides the other options.
		if (!_GameOver && !_IsGamePaused) // if the game is not over and game is not paused, don't show the labels / options
		{
			_GamePausedLabel.Modulate = new Color(0, 0, 0, 0);
			_MainMenuButton.Modulate = new Color(0, 0, 0, 0);
			_MainMenuButton.Disabled = true;
		}
		else if (!_GameOver && _IsGamePaused) // otherwise, show them.
		{
			_GamePausedLabel.Modulate = new Color(1, 1, 1, 1);
			_MainMenuButton.Modulate = new Color(1, 1, 1, 1);
			_MainMenuButton.Disabled = false;
		}
		else
		{ // if it's any other case (which is every time _gameover is true) then hide the options, excepting the main menu button, as that is part of the game over screen too.
			_GamePausedLabel.Modulate = new Color(0, 0, 0, 0);
		}

		// set the paused state of the timers to false, so they can follow in the time whey were left in or get paused.
		SetPausedStateTimers(_IsGamePaused);

		ShowPausedLabel(_IsGamePaused); // we send the paused state to this function to check wheter to show the paused label or not

		// stop each process for all the movable objects
		foreach (Node road in _RoadContainer.GetChildren()) road.SetProcess(set_process_bool);

		foreach (Node coin in _CoinContainer.GetChildren()) coin.SetProcess(set_process_bool);

		foreach (Node enemy in _EnemyContainer.GetChildren()) enemy.SetProcess(set_process_bool);

		foreach (Node bullet in _BulletContainer.GetChildren()) bullet.SetProcess(set_process_bool);

		foreach (Node shop_pickable in _ShopPickableContainer.GetChildren()) shop_pickable.SetProcess(set_process_bool);


		// stop/start the timer for speeding game and the animation.
		foreach (Node speed in _SpeedingGame.GetChildren())
		{
			if (speed is Timer timer) timer.SetPaused(!set_process_bool); // if the process are turning back to true, then put the paused state to false, and viceversa.

			if (speed is AnimationPlayer anim)
			{
				if (_GameOver) anim.Stop();
			}
		}

		// stop process and timers for player
		_Player.SetProcess(set_process_bool);

		// perks' timers
		_PerkTimer1.SetPaused(!set_process_bool);
		_PerkTimer2.SetPaused(!set_process_bool);
		_PerkTimer3.SetPaused(!set_process_bool);
	}

	private void OnEnemyDestroyed()
	{

		if (_DoublePointsPerkActive) PlayerVariables.Instance.PlayerScore += 2;
		else PlayerVariables.Instance.PlayerScore++;

		_TotalScore = PlayerVariables.Instance.PlayerScore; // every time a enemy dies/gets destroyed, a point gets added to the total score. if the double points perk is active, then 2 points get added to the score.

		_ScoreLabel.Text = _TotalScore.ToString();

	}

	private void OnCoinHitsPlayer()
	{
		int money_gained = 1;

		// still needed to implement the double money perk correctly here in the game scene code
		if (_DoubleMoneyPerkActive) money_gained = money_gained * 2;

		PlayerVariables.Instance.PlayerCoins += money_gained;
		_LeftCoins = PlayerVariables.Instance.PlayerCoins; // left coins are the actual coins in game, because with the coins you will be able to buy things in the future.

		PlayerVariables.Instance.PlayerTotalCoins += money_gained;
		_TotalCoins = PlayerVariables.Instance.PlayerTotalCoins;  // total coins are the coins obtained in general in all of the game

		_CoinLabel.Text = "$" + _LeftCoins.ToString();
		_CoinSound.Play();
	}

	private void SpawnCoin()
	{
		Coin coin = (Coin)_CoinScene.Instantiate();
		_CoinContainer.AddChild(coin);
		float coin_x_position = (float)GD.RandRange(_SpawnMarkerLeft.Position.X, _SpawnMarkerRight.Position.X); // using the same markers as the enemies.
		float coin_y_position = _SpawnMarkerRight.Position.Y;
		coin.Position = new Vector2(coin_x_position, coin_y_position);
		coin.CoinHitsPlayer += OnCoinHitsPlayer;
	}

	// function used when the player uses the bullet perk
	private void SpawnBullet()
	{
		Bullet bullet = (Bullet)_BulletScene.Instantiate();
		_BulletContainer.AddChild(bullet);
		float bullet_x_position = _Player.Position.X;
		float bullet_y_position = _Player.Position.Y;
		bullet.Position = new Vector2(bullet_x_position, bullet_y_position);
		bullet.BulletHitEnemy += OnBulletHitEnemy;

	}


	private void OnBulletHitEnemy()
	{
		// we can't play the sound directly from the bullet object, because it gets destroyed when touching the enemy. so we play it from the game instead, using a signal or event handler that tells when the bullet hit an enemy.
		_BulletHitEnemySound.Play();
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

		// we assign the perks' timers texts and textures modulate so they are not shown if they were active before.
		_PerkTimerText1.Modulate = new Color(0, 0, 0, 0);
		_PerkTimerText2.Modulate = new Color(0, 0, 0, 0);
		_PerkTimerText3.Modulate = new Color(0, 0, 0, 0);

		_PerkTimerTexture1.Texture = null;
		_PerkTimerTexture2.Texture = null;
		_PerkTimerTexture3.Texture = null;

		// we also have to reassign the variables used for the timers
		_PerksTimerSlot = [false, false, false];
		_PerkActiveInTimer = [-1, -1, -1];

		// we set the perk timers paused state to false, so they can work properly.
		_PerkTimer1.SetPaused(false);
		_PerkTimer2.SetPaused(false);
		_PerkTimer3.SetPaused(false);

		// we set the player's perks to stop. when a perk is used, they get restarted and used correctly.
		_PerkTimer1.Stop();
		_PerkTimer2.Stop();
		_PerkTimer3.Stop();

		GameStarted();

	}

	void GameStarted()
	{
		AssignGameVariables();
		UpdateLabels(); // this function is used for when the user comes from the shop, to re-update the labels that contain the user's score and money.

		SpawnParkingLotRoad();
		SpawnRoad(); // spawn a road ahead of the timer to start the game earlier (should fix this later)
		_CarStarting.Play();
		if (GameManager.Instance.MusicActive) _GameMusic.Play(); // if the music is active, play the music. otherwise dont do anything.

		// we also check the buttons for the music and sound here, because maybe the user disabled the music/sound on the menu, and we solve any visual bugs doing this.
		GameManager.Instance.ChangeMusicButtonTextures(_MusicButton);
		GameManager.Instance.ChangeSoundButtonTextures(_SoundButton);
	}

	private void OnSpeedingTheGame()
	{
		GD.Print("speeding the game!");

		// increase the speeds for enemies, in the EnemyManager variable.
		EnemyManager.Instance.EnemySpeed += 30;

		if (GameManager.Instance.EnemyTimerWaitTime >= 0.4 && GameManager.Instance.EnemyTimerWaitTime <= 0.5) ; // if the waittime has reached a threshold, we stop lowering it.
																												// the reason we use an interval to check if we keep lowering the timer or not, is that the values are a little bit weird with the decimals. so we use this interval for the condition.
																												// another thing to note here is that if the timer keeps lowering to when it's zero or below zero weird stuff happens in the game, like for example the cars spawning way too fast, that haundreds of them spawn in seconds.
		else GameManager.Instance.EnemyTimerWaitTime -= 0.2; // we keep lowering the timer. we use the gamemanager as when changing to the shop, the timer gets restarted, so we dont want that to happen.		

		if (_SlowTimePerkActive) return; // if the slowtimeperk is active, we return and dont update the enemies' speed, including the _EnemySpeed variable, as we use that one to spawn enemies.
										 // by letting the other variables be updated each time the speeding the game function is activated, we have no trouble managing this function when the-
										 // slow time perk is active, and we let the variables update properly. the only difference is that the new speed is stored and not updated directly to the enemies,-
										 // until the slow time perk is done.
		else
		{
			_EnemySpeed = EnemyManager.Instance.EnemySpeed;
			_SpawningCarEnemyTimer.WaitTime = GameManager.Instance.EnemyTimerWaitTime;
		}

		GD.Print(_SpawningCarEnemyTimer.WaitTime);

		// adjust the new speed for all the existing enemy objects
		foreach (CarEnemy enemy in _EnemyContainer.GetChildren()) enemy._CarEnemySpeed = _EnemySpeed;
	}

	private void OnMainMenuButtonPressed()
	{
		GameManager.Instance.ChangeSceneToMainMenu();
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

		// player variables that need to be restarted each time the game starts
		_Player.RestartPlayerVariables();

		_InventoryInGame.UpdateInventoryPerksTextures(); // this updates the textures of the perks in the inventory

		_EnemySpeed = EnemyManager.Instance.EnemySpeed; // enemy

		_GameMusic.PitchScale = 1; // music for the game. could be that the pitchscale was left on a different value than the normal, which is 1

		_TransparentBlackScreen.Modulate = new Color(0, 0, 0, 0); // we assign the transparent black screen to not visible when starting the game.

		// we assign again the timer to the gamemanager variable. with this, when coming back from shop this will have the value it was left in before going to the shop.
		_SpawningCarEnemyTimer.WaitTime = GameManager.Instance.EnemyTimerWaitTime;

		_MainMenuButton.Disabled = true; // we dusable the main menu button when the game starts, so the player cant click it. we reenable it when the player loses.
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

	// ==============
	// !!!! check if this is really needed in the game
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

	private void OnUpdateInventoryPerkTexture()
	{
		_InventoryInGame.UpdateInventoryPerksTextures();
	}

	// the both functions below (slowenemies and normalspeedenemies) are used for the slow time perk when it's activated and when it ended.
	private void SlowEnemies()
	{

		_EnemySpeed = 70;
		_SpawningCarEnemyTimer.WaitTime = 3.0;
		foreach (CarEnemy enemy in _EnemyContainer.GetChildren()) enemy._CarEnemySpeed = _EnemySpeed;

	}

	private void NormalSpeedEnemies()
	{

		_EnemySpeed = EnemyManager.Instance.EnemySpeed;
		_SpawningCarEnemyTimer.WaitTime = GameManager.Instance.EnemyTimerWaitTime;
		foreach (CarEnemy enemy in _EnemyContainer.GetChildren()) enemy._CarEnemySpeed = _EnemySpeed;

	}

	private void OnPlayerUsedPerk(int perk_number)
	{
		int i;

		// we search for an empty spot on the perks' timers
		for (i = 0; i < 3; i++)
		{
			// if there's a slot that's empty (so that the index of the array has a value of -1) then we assign the perk number to it and we break
			if (_PerkActiveInTimer[i] == -1)
			{
				_PerkActiveInTimer[i] = perk_number;
				break;
			}
		}

		// we use this variable to assign the perk duration depending on the perk number on the following ifs
		// also, on the ifs we do any necessary operations for the perks being used
		int perk_duration_seconds = 0;

		// it's still needed to add all the durations and all the methods or actions to be done in the ifs.
		// also, it's needed to control the perks' timers textures and texts (modulate and assign them) so they are shown properly in the game when a perk is activated.
		if (perk_number == GameManager.PerksNumbers["shield"]) perk_duration_seconds = 15;
		else if (perk_number == GameManager.PerksNumbers["double-points"])
		{
			perk_duration_seconds = 20;
			_DoublePointsPerkActive = true;
		}
		else if (perk_number == GameManager.PerksNumbers["double-money"])
		{
			perk_duration_seconds = 20;
			_DoubleMoneyPerkActive = true;
		}
		else if (perk_number == GameManager.PerksNumbers["slow-time"])
		{
			perk_duration_seconds = 15;
			_SlowTimePerkActive = true;
			_GameMusic.PitchScale = (float)0.74; // we pitch down the game music so it feels slowed, as we are slowing the time with this perk.
			SlowEnemies();
		}

		// depending on the value of i is the timer that we will use and start
		switch (i)
		{
			case 0:
				_PerkTimer1.WaitTime = perk_duration_seconds;

				_PerkTimerText1.Modulate = new Color(1, 1, 1, 1);

				_PerkTimerTexture1.Texture = _PerksTextureTimers[perk_number];
				_PerkTimerTexture1.Modulate = new Color(1, 1, 1, 1);

				_PerkTimer1.Start();
				break;
			case 1:
				_PerkTimer2.WaitTime = perk_duration_seconds;

				_PerkTimerText2.Modulate = new Color(1, 1, 1, 1);

				_PerkTimerTexture2.Texture = _PerksTextureTimers[perk_number];
				_PerkTimerTexture2.Modulate = new Color(1, 1, 1, 1);

				_PerkTimer2.Start();
				break;
			case 2:
				_PerkTimer3.WaitTime = perk_duration_seconds;

				_PerkTimerText3.Modulate = new Color(1, 1, 1, 1);

				_PerkTimerTexture3.Texture = _PerksTextureTimers[perk_number];
				_PerkTimerTexture3.Modulate = new Color(1, 1, 1, 1);

				_PerkTimer3.Start();
				break;
		}

		GD.Print(i);

		// finally, we update the perkstimerslot to true with the i value, so we know is active.
		_PerksTimerSlot[i] = true;
	}

	private void OnPerkTimerTimeout(int timer_number, int perk_number)
	{
		// in this case only 4 out of the 6 perks can have timers. in this case, we use their original perk number, so there is no confusion in the code. the extralife and bullet perks are not here, as those don't use timers.
		if (perk_number == GameManager.PerksNumbers["shield"]) EmitSignal(SignalName.ShieldPerkEnded);
		else if (perk_number == GameManager.PerksNumbers["double-points"])
		{
			_DoublePointsPerkActive = false;
			EmitSignal(SignalName.DoublePointsPerkEnded);
		}
		else if (perk_number == GameManager.PerksNumbers["double-money"])
		{
			_DoubleMoneyPerkActive = false;
			EmitSignal(SignalName.DoubleMoneyPerkEnded);
		}
		else if (perk_number == GameManager.PerksNumbers["slow-time"])
		{
			_SlowTimePerkActive = false;
			EmitSignal(SignalName.SlowTimePerkEnded);
			NormalSpeedEnemies();
			_GameMusic.PitchScale = 1; // we return the pitch of the game music to its original pitch, which is 1
		}

		switch (timer_number)
		{
			case 0:
				_PerkTimer1.Stop();

				_PerkTimerText1.Modulate = new Color(0, 0, 0, 0);

				_PerkTimerTexture1.Texture = null;
				_PerkTimerTexture1.Modulate = new Color(0, 0, 0, 0);

				_PerkActiveInTimer[0] = -1; // we assign the perk active in timer (its number) to -1, to say that there's no perk active there when the timer is freed. we have to do this for the OnPlayerUsedPerk function.
				break;
			case 1:
				_PerkTimer2.Stop();

				_PerkTimerText2.Modulate = new Color(0, 0, 0, 0);

				_PerkTimerTexture2.Texture = null;
				_PerkTimerTexture2.Modulate = new Color(0, 0, 0, 0);

				_PerkActiveInTimer[1] = -1;
				break;
			case 2:
				_PerkTimer3.Stop();

				_PerkTimerText3.Modulate = new Color(0, 0, 0, 0);

				_PerkTimerTexture3.Texture = null;
				_PerkTimerTexture3.Modulate = new Color(0, 0, 0, 0);

				_PerkActiveInTimer[2] = -1;
				break;
		}


	}
}
