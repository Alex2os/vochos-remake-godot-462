using Godot;

public partial class GameOver : Control
{
	[Export] private Button _MainMenuButton;
	[Export] private Label _GameOverStatsLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_MainMenuButton.Pressed += OnMainMenuButtonPressed;

		_MainMenuButton.MouseEntered += GameManager.Instance.PlayButtonHoverSound;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _ExitTree(){ _MainMenuButton.MouseEntered -= GameManager.Instance.PlayButtonHoverSound; }

	// through this function we can pass this values to the scene so we can update the game over stats label from outside the scene.
	public void AssignGameOverStatsLabel(string score, string money_left, string total_money)
	{
		_GameOverStatsLabel.Text = $"Total score: {score}\nMoney left: {money_left}\nTotal money: {total_money}";
	}

	private void OnMainMenuButtonPressed() { GameManager.Instance.ChangeSceneToMainMenu(); }
}
