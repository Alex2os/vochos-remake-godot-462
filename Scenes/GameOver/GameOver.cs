using Godot;

public partial class GameOver : Control
{
	
	[Export] private Button MainMenuButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MainMenuButton.Pressed += OnMainMenuButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnMainMenuButtonPressed() { GameManager.Instance.ChangeSceneToMainMenu(); }
}
