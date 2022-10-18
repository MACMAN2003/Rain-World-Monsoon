using Godot;
using System;


namespace RainWorldMonsoon;
public partial class Compass : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		if (Globals.Settings.IsFreecam == true)
		{
			on();
        }
		else
		{
			Hide();
		}
	}


	public void on()
	{
		if (Globals.Settings.ShowCompass == true)
		{
			Show();
		}
		else Hide();
        Vector2 Destination = new Vector2((Globals.levelSize.x * 16) / 2, (Globals.levelSize.y * 16) / 2);
        Node2D a = (Node2D)GetParent();
        Vector2 b = a.Position;
        //b.x += 0.00001f; // to prevent dividing by 0
        //b.y += 0.00001f;

        Vector2 dir = Destination - b;
        dir.Normalized();
		Scale = new Vector2(GlobalPosition.DistanceTo(Destination) / 512, GlobalPosition.DistanceTo(Destination) / 512);
        Rotation = GlobalPosition.AngleToPoint(Destination) - (float)Math.PI / 2;
    }
}
