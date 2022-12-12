using Godot;
using System;



namespace RainWorldMonsoon;
public partial class ApprenticeCamera : Camera2D
{
	LECamera2D master;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		master = (LECamera2D)GetTree().Root.FindChild("Master Camera", true, false);
		Callable InSignal = new Callable(this, nameof(ForceUpdate));
		master.Connect("ZoomUpdated", InSignal);
	}
	public void ForceUpdate()
	{
		Position = Globals.CameraPosition;
		Zoom = Globals.CameraZoom;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = Globals.CameraPosition;
		Zoom = Globals.CameraZoom;
	}
}
