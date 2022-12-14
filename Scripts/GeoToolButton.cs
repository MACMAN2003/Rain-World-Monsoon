using Godot;
using System;

namespace RainWorldMonsoon;
public partial class GeoToolButton : TextureButton
{
	[Export]GeometryTools toolToChangeTo;
	[Export] Vector2i ToolMatrixPosition;
	bool IsCurrentTool = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}


	public override void _Pressed()
	{
		Globals.CurrentTool = toolToChangeTo;
		//GD.Print("Changing geo tool to ", toolToChangeTo);
		IsCurrentTool = true;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Globals.ToolMatrixIndex == ToolMatrixPosition & IsCurrentTool == false)
		{
			Globals.CurrentTool = toolToChangeTo;
			//GD.Print("Changing geo tool to ", toolToChangeTo);
			IsCurrentTool = true;
		}
		else if (Globals.ToolMatrixIndex != ToolMatrixPosition)
		{
			IsCurrentTool = false;
		}



		if (Globals.CurrentTool == toolToChangeTo)
		{
			//this.Modulate = Colors.White;
			this.SelfModulate = Colors.Red;
		}
		else
		{
            //this.Modulate = Color.FromHSV(0, 0, 53);
            this.SelfModulate = Color.FromHSV(0,0,0.53f);
		}
	}
}
