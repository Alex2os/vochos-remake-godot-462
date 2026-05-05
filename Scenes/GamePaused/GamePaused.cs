using Godot;
using System;

public partial class GamePaused : Control
{
	[Signal] public delegate void UserPressedRestartEventHandler();
	[Signal] public delegate void UserPressedResumeGameEventHandler();
	[Export] private Button _ResumeGameButton;
	[Export] private Button _MainMenuButton;
	[Export] private Button _RestartGameButton;
	[Export] private Button _MusicButton;
	[Export] private Button _SoundButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_ResumeGameButton.Pressed += OnResumeGameButtonPressed;
		_MainMenuButton.Pressed += OnMainMenuButtonPressed;
		_RestartGameButton.Pressed += OnRestartGameButtonPressed;
		_MusicButton.Pressed += OnMusicButtonPressed;
		_SoundButton.Pressed += OnSoundButtonPressed;

		// we assign the music and sound textures when entering the gamepaused scene
		GameManager.Instance.ChangeMusicButtonTextures(_MusicButton);
		GameManager.Instance.ChangeSoundButtonTextures(_SoundButton);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnResumeGameButtonPressed()
	{
		EmitSignal(SignalName.UserPressedResumeGame);
	}

	private void OnMainMenuButtonPressed(){ GameManager.Instance.ChangeSceneToMainMenu(); }

	private void OnRestartGameButtonPressed()
	{
		EmitSignal(SignalName.UserPressedRestart);
	}

	private void OnMusicButtonPressed()
	{
		GameManager.Instance.SwitchMusicActiveBool();
		GameManager.Instance.ChangeMusicButtonTextures(_MusicButton);
	}

	private void OnSoundButtonPressed()
	{
		GameManager.Instance.SwitchSoundActiveBool();
		GameManager.Instance.ChangeSoundButtonTextures(_SoundButton);
		
	}
}
