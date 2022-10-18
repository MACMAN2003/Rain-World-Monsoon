using Godot;
using System;



namespace RainWorldMonsoon;
public partial class ApprenticeCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = Globals.CameraPosition;
		Zoom = Globals.CameraZoom;
	}
}
