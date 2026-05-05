using Godot;
using System.Collections.Generic; // used for dictionaries

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	private PackedScene _MainMenuScene = GD.Load<PackedScene>("res://Scenes/MainMenu/main_menu.tscn");
	private PackedScene _GameScene = GD.Load<PackedScene>("res://Scenes/Game/game.tscn");
	private PackedScene _ShopScene = GD.Load<PackedScene>("res://Scenes/Shop/shop.tscn");

	public bool ComingFromShop = false;
	public double EnemyTimerWaitTime = 2.0; // 2 seconds 
											// number of perks in the game
	public const int NumberOfPerks = 6;
	// variables for the music and sound. used to turn on and off the music and sound respectively.
	public bool MusicActive = true;
	public bool SoundActive = true;

	// a dictionary for the perks' numbers
	public static readonly Dictionary<string, int> PerksNumbers = new Dictionary<string, int>
	{
	{ "shield", 0 },
	{ "health", 1 },
	{ "double-points", 2 },
	{ "bullet", 3 },
	{ "double-money", 4 },
	{ "slow-time", 5 }
	};

	// a dictionary for the music and sfx buttons' textures. we use these textures in the menu and in game, so it's better to load them once here and let the autoload use them, because as we can see it's a private variable for the autoload only.
	private static readonly Dictionary<string, Texture2D> SoundAndMusicButtonTextures = new Dictionary<string, Texture2D>
	{
		{"sfx-active-button", GD.Load<Texture2D>("res://assets/buttons/sfx-enabled-button.png")},
		{"sfx-active-button-pressed", GD.Load<Texture2D>("res://assets/buttons/sfx-enabled-button-pressed.png")},
		{"sfx-active-button-hover", GD.Load<Texture2D>("res://assets/buttons/sfx-enabled-button-hover.png")},
		{"sfx-disabled-button", GD.Load<Texture2D>("res://assets/buttons/sfx-disabled-button.png")},
		{"sfx-disabled-button-pressed", GD.Load<Texture2D>("res://assets/buttons/sfx-disabled-button-pressed.png")},
		{"sfx-disabled-button-hover", GD.Load<Texture2D>("res://assets/buttons/sfx-disabled-button-hover.png")},
		{"music-active-button", GD.Load<Texture2D>("res://assets/buttons/music-active-button.png")},
		{"music-active-button-pressed", GD.Load<Texture2D>("res://assets/buttons/music-active-button-pressed.png")},
		{"music-active-button-hover", GD.Load<Texture2D>("res://assets/buttons/music-active-button-hover.png")},
		{"music-disabled-button", GD.Load<Texture2D>("res://assets/buttons/music-disabled-button.png")},
		{"music-disabled-button-pressed", GD.Load<Texture2D>("res://assets/buttons/music-disabled-button-pressed.png")},
		{"music-disabled-button-hover", GD.Load<Texture2D>("res://assets/buttons/music-disabled-button-hover.png")},
	};

	// names of the perks
	public static readonly string[] PerksNames = new string[6] { "Shield",
		"Health",
		"Double Points",
		"Bullet",
		"Double Money",
		"Slow Time" };


	// the prices of the perks
	public static readonly int[] PerksPrices = new int[] {
		7,
		8,
		6,
		6,
		10,
		7
	};

	public static readonly int[] PerksPricesSelling = new int[] {
		3,
		4,
		3,
		3,
		5,
		3
	};

	public int RerollShopCost = 1;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void ChangeSceneToGame()
	{
		GetTree().ChangeSceneToPacked(_GameScene);
	}

	public void ChangeSceneToShop()
	{
		GetTree().ChangeSceneToPacked(_ShopScene);
	}

	public void ChangeSceneToMainMenu()
	{
		GetTree().ChangeSceneToPacked(_MainMenuScene);
	}

	// we basically change the music and sound button textures from here. we can just send a button and it will be sent by reference, so we can change whatever we want inside here, in the autoload.
	// we could use texturebuttons instead of normal buttons so the code would be cleaner and shorter, but in this case we stick to the buttons we were already using.
	public void ChangeMusicButtonTextures(Button button)
	{
		// the reason we use these many variables is that if we just use one, all the styles will accumulate over each other, causing bugs. so we better just have them separately.
		var styleNormal = new StyleBoxTexture(); // we need a styleboxtexture to save the style for the button
		var stylePressed = new StyleBoxTexture();
		var styleHover = new StyleBoxTexture();

		if (MusicActive)
		{
			// we assign each texture to the style properly, and update the button with the AddThemeStyleboxOverride, specifying the style and the button state we want to change.
			styleNormal.Texture = SoundAndMusicButtonTextures["music-active-button"];
			button.AddThemeStyleboxOverride("normal", styleNormal);

			stylePressed.Texture = SoundAndMusicButtonTextures["music-active-button-pressed"];
			button.AddThemeStyleboxOverride("pressed", stylePressed);

			styleHover.Texture = SoundAndMusicButtonTextures["music-active-button-hover"];
			button.AddThemeStyleboxOverride("hover", styleHover);

		}
		else
		{

			styleNormal.Texture = SoundAndMusicButtonTextures["music-disabled-button"];
			button.AddThemeStyleboxOverride("normal", styleNormal);

			stylePressed.Texture = SoundAndMusicButtonTextures["music-disabled-button-pressed"];
			button.AddThemeStyleboxOverride("pressed", stylePressed);

			styleHover.Texture = SoundAndMusicButtonTextures["music-disabled-button-hover"];
			button.AddThemeStyleboxOverride("hover", styleHover);

		}
	}

	// this is the same function as the above one for the music, but for the sound button
	public void ChangeSoundButtonTextures(Button button)
	{
		var styleNormal = new StyleBoxTexture();
		var stylePressed = new StyleBoxTexture();
		var styleHover = new StyleBoxTexture();

		if (SoundActive)
		{

			styleNormal.Texture = SoundAndMusicButtonTextures["sfx-active-button"];
			button.AddThemeStyleboxOverride("normal", styleNormal);

			stylePressed.Texture = SoundAndMusicButtonTextures["sfx-active-button-pressed"];
			button.AddThemeStyleboxOverride("pressed", stylePressed);

			styleHover.Texture = SoundAndMusicButtonTextures["sfx-active-button-hover"];
			button.AddThemeStyleboxOverride("hover", styleHover);

		}
		else
		{

			styleNormal.Texture = SoundAndMusicButtonTextures["sfx-disabled-button"];
			button.AddThemeStyleboxOverride("normal", styleNormal);

			stylePressed.Texture = SoundAndMusicButtonTextures["sfx-disabled-button-pressed"];
			button.AddThemeStyleboxOverride("pressed", stylePressed);

			styleHover.Texture = SoundAndMusicButtonTextures["sfx-disabled-button-hover"];
			button.AddThemeStyleboxOverride("hover", styleHover);

		}
	}

	// we can send an audiostreamplayer to this function from anywhere in the scenes and play the audio depending if the sound or sfx is active or not. in this way we can control the sound for all the scenes in one place, instead of checking the condition everywhere.
	public void PlaySound(AudioStreamPlayer audio) { if (SoundActive) audio.Play(); }

	public void InitializeGameManagerVariables()
	{
		ComingFromShop = false;
		EnemyTimerWaitTime = 2.0;
	}
}
