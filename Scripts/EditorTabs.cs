using Godot;
using System;


namespace RainWorldMonsoon;

public partial class EditorTabs : TabContainer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	   Callable InSignal = new Callable(this, nameof(ChangeTab));
	   // Callable Killbox = new Callable(this, nameof(LevelFileSelectQuit));
	   this.Connect("tab_changed", InSignal);


	}

	public void ChangeTab(int index)
	{
		//GD.Print(index);
		Globals.PrevState = Globals.State;
		Globals.State = (EditorState)index + 1;
		//GD.Print(Globals.State);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int index = CurrentTab;
		if (index != (int)Globals.State - 1 && Globals.State != EditorState.LoadSaveIO)
		{
			TabReset();
		}
	}
	private void TabReset()
	{
		CurrentTab = (int)Globals.State - 1;
	}
}
