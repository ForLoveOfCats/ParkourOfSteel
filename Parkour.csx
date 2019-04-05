private class CustomCommands
{
	private static Parkour Self;
	public CustomCommands(Parkour SelfArg)
	{
		Self = SelfArg;
	}
}



public class Parkour : Gamemode
{
	public Label AlertLabel = new Label();

	public bool IsPlaying = false;
	public float OldY = 0;
	public float OldZ = 0;
	public float Timer = 0;


	public override void _Ready()
	{
		if(Net.Work.IsNetworkServer())
			Net.SteelRpc(Scripting.Self, nameof(Scripting.RequestGmLoad), "Parkour");

		Game.PossessedPlayer.HUDInstance.AddChild(AlertLabel);

		Console.Log("Hello world");
		API.Gm = new CustomCommands(this);
	}


	public override void _Process(float Delta)
	{
		if(IsPlaying)
		{
			Timer += Delta;
			AlertLabel.Text = Timer.ToString();
		}
	}


	public override void OnPlayerConnect(int Id)
	{
		if(Net.Work.IsNetworkServer())
			Scripting.Self.RpcId(Id, nameof(Scripting.RequestGmLoad), "Parkour");
	}


	public override void OnUnload()
	{
		if(Net.Work.IsNetworkServer())
			Net.SteelRpc(Scripting.Self, nameof(Scripting.RequestGmUnload));

		AlertLabel.QueueFree();
	}


	public override bool ShouldPlayerMove(Vector3 Position)
	{
		if(!IsPlaying && (OldZ < 100 && Position.z > 100))
		{
			IsPlaying = true;
			Timer = 0;
			AlertLabel.Text = Timer.ToString();
		}


		if(IsPlaying && Mathf.Abs(Position.x) <= 6 && Mathf.Abs(Position.z) <= 6 && (OldY > 45 && Position.y < 45))
		{
			AlertLabel.Text = $"Finished with a time of: {Timer} seconds";
			IsPlaying = false;
		}

	OldY = Position.y;
	OldZ = Position.z;
	return true;
	}
}


return new Parkour();
