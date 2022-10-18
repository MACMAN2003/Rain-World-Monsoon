using Godot;
using RainWorldMonsoon;
using System;


namespace RainWorldMonsoon;
public partial class LevelName : Label
{


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = Globals.gLoadedName;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Text != "Current Level: " + Globals.gLoadedName)
		{
			GD.Print("Setting the level name to ", Globals.gLoadedName, "!");
			ChangeTheLevelNamePlease();
		}
	}

	public void ChangeTheLevelNamePlease()
	{

        Text = "Current Level: " + Globals.gLoadedName;


    }

}
