using Godot;
using System;

public partial class MainMenu : Node2D
{
	[Export] private Marker2D _Marker1;
	[Export] private Marker2D _Marker2;
	[Export] private Marker2D _Marker3;
	[Export] private Marker2D _Marker4;
	[Export] private Marker2D _Marker5;
	[Export] private Marker2D _CloudMarker1;
	[Export] private Marker2D _CloudMarker2;
	[Export] private Button _PlayButton;
	[Export] private Button _ExitButton;
	[Export] private PackedScene _MiniCarScene;
	[Export] private PackedScene _CloudScene;
	[Export] private Timer _SpawnMiniCarTimer;
	[Export] private Timer _SpawnCloudTimer;
	[Export] private Node2D _MiniCarContainer;
	[Export] private Node2D _CloudContainer;
	[Export] private PackedScene _CreditsScene;
	[Export] private Button _CreditsButton;
	[Export] private Button _SoundButton;
	[Export] private Button _MusicButton;
	[Export] private AudioStreamPlayer _MainMenuMusic;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_PlayButton.Pressed += OnPlayButtonPressed;
		_ExitButton.Pressed += OnExitButtonPressed;
		_CreditsButton.Pressed += OnCreditsButtonPressed;
		_SpawnMiniCarTimer.Timeout += SpawnMiniCar;
		_SpawnCloudTimer.Timeout += SpawnCloud;
		_SoundButton.Pressed += OnSoundButtonPressed;
		_MusicButton.Pressed += OnMusicButtonPressed;

		// we update the music and sound buttons when the scene is ready. it could be that the player disabled the music or sound inside the game, so to not have visual bugs when returning to the main menu, we do this.
		GameManager.Instance.ChangeSoundButtonTextures(_SoundButton);
		GameManager.Instance.ChangeMusicButtonTextures(_MusicButton);

		if(GameManager.Instance.MusicActive) _MainMenuMusic.Play(); // we also check for the music active to play the main menu music for the same reason we update the button textures.

		SpawnCloud(); // we spawn a cloud in the start so the sky doesn't seem to lonely
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// we assign the soundactive or musicactive variables from the gamemanager autoload here, and also change the textures of the buttons at the same time.
	private void OnMusicButtonPressed()
	{
		GameManager.Instance.SwitchMusicActiveBool();
		GameManager.Instance.ChangeMusicButtonTextures(_MusicButton);

		if(GameManager.Instance.MusicActive) _MainMenuMusic.Play();
		else _MainMenuMusic.Stop();
	}

	private void OnSoundButtonPressed()
	{
		GameManager.Instance.SwitchSoundActiveBool();
		GameManager.Instance.ChangeSoundButtonTextures(_SoundButton);
	}

	private void OnPlayButtonPressed()
	{
		GameManager.Instance.ChangeSceneToGame(); // change the scene to the game scene
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit(); // leave the game or quit the scene, so it leaves the game when pressed 
	}

	private void OnCreditsButtonPressed()
	{
		Credits _credits = (Credits)_CreditsScene.Instantiate();
		AddChild(_credits);

		_credits.Position = new Vector2(500, 0); // positioning the credits scene to the center.

	}

	private void SpawnMiniCar()
	{
		MiniCar mini_car = (MiniCar)_MiniCarScene.Instantiate();
		_MiniCarContainer.AddChild(mini_car);
		int _MarkerToChoose = GD.RandRange(0, 4); // choose a random marker so the car can spawn.
		switch (_MarkerToChoose) // assign the random marker's position to the mini_car object.
		{
			case 0:
				mini_car.Position = new Vector2(_Marker1.Position.X, _Marker1.Position.Y);
				break;
			case 1:
				mini_car.Position = new Vector2(_Marker2.Position.X, _Marker2.Position.Y);
				break;
			case 2:
				mini_car.Position = new Vector2(_Marker3.Position.X, _Marker3.Position.Y);
				mini_car.ChangeSpeedDirection(); // we change the speed direction of the mini car if spawned in the right side of the screen, which are markers 3 and 5.
				break;
			case 3:
				mini_car.Position = new Vector2(_Marker4.Position.X, _Marker4.Position.Y);
				break;
			case 4:
				mini_car.Position = new Vector2(_Marker5.Position.X, _Marker5.Position.Y);
				mini_car.ChangeSpeedDirection();
				break;
		}
	}

	private void SpawnCloud()
	{
		Cloud cloud = (Cloud)_CloudScene.Instantiate();
		_CloudContainer.AddChild(cloud);
		float cloud_y = (float)GD.RandRange(_CloudMarker1.Position.Y, _CloudMarker2.Position.Y); // this will be a random position between the two markers in the y axis.
		cloud.Position = new Vector2(_CloudMarker1.Position.X, cloud_y); // the x of the cloud object will be the same as the markers for the clouds.
	}
}
