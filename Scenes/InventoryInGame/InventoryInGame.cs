using Godot;
using System;

public partial class InventoryInGame : Node2D
{

	private static Texture2D[] _PerksTexturesSmall;
	[Export] private Sprite2D Perk1;
	[Export] private Sprite2D Perk2;
	[Export] private Sprite2D Perk3;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		// load the textures for the small icons
		_PerksTexturesSmall = new Texture2D[]
		{
			GD.Load<Texture2D>("res://assets/perks/small sizes/shield small.png"),
			GD.Load<Texture2D>("res://assets/perks/small sizes/extra life small.png"),
			GD.Load<Texture2D>("res://assets/perks/small sizes/double points small.png"),
			GD.Load<Texture2D>("res://assets/perks/small sizes/bullet small.png"),
			GD.Load<Texture2D>("res://assets/perks/small sizes/double money small.png"),
			GD.Load<Texture2D>("res://assets/perks/small sizes/time slow small.png")
		};

		UpdateInventoryPerksTextures();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// in this function we assign the textures of the inventory.
	public void UpdateInventoryPerksTextures()
	{
		// int inventory_size = PlayerVariables.PlayerInventorySize; // as it's an static variables (playerinventorysize), we have to access it from the class itself, and not the instance. that's why whe don't do playervariables.instance.playerinventorysize

		if (PlayerVariables.Instance.PlayerInventory[0] == -1) Perk1.Texture = null;
		else Perk1.Texture = _PerksTexturesSmall[PlayerVariables.Instance.PlayerInventory[0]];

		if (PlayerVariables.Instance.PlayerInventory[1] == -1) Perk2.Texture = null;
		else Perk2.Texture = _PerksTexturesSmall[PlayerVariables.Instance.PlayerInventory[1]];

		if (PlayerVariables.Instance.PlayerInventory[2] == -1) Perk3.Texture = null;
		else Perk3.Texture = _PerksTexturesSmall[PlayerVariables.Instance.PlayerInventory[2]];
	}
}
