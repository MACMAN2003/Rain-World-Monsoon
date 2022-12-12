using Godot;
using System;



namespace RainWorldMonsoon;
public partial class Dropdownbutton : Button
{
	[Export] public Control IDropdown;
	[Export] public string Undropped = "V";
	[Export] public string Dropped = "=";
	//[Export]bool isDropped = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Callable th = new Callable(this, nameof(Toggle));
		//Connect("toggled", th);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
	public override void _Toggled(bool Toggled)
	{
		//GD.Print(buttonPressed);
		if(Toggled == true)
		{
			Text = Dropped;
			IDropdown.Visible = true;
		}
		else
		{
			Text = Undropped;
			IDropdown.Visible = false;
		}
		//base._Toggled(buttonPressed);
	}
}
