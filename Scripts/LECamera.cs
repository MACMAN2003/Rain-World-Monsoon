using Godot;
using System;




namespace RainWorldMonsoon;
public partial class LECamera : Camera3D
{
	[Export] public uint MaxZoomOut = Globals.levelSize.x;
	[Export] public uint MinZoomIn = 5;
	[Export] uint DefaultZoom = 52;
	float ZoomPercent = 1;
	private uint CurrentZoom;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CurrentZoom = DefaultZoom;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public void UpdateCamera()
	{
		if (Globals.levelSize.y > Globals.levelSize.x)
		{
			MaxZoomOut = Globals.levelSize.y;
		}
		else
		{
			MaxZoomOut = Globals.levelSize.x;
		}
		CenterCamera(true);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouse)
		{
			if (mouse.ButtonMask == MouseButton.MaskMiddle)
			{
				//GD.Print(mouse.Relative);
				Position -= new Vector3(mouse.Relative.x, mouse.Relative.y, 0) / ((Globals.levelSize.x + Globals.levelSize.y) / 2) * ZoomPercent * 8;
			}
		}
		

	}

	public override void _Process(double delta)
	{
		this.Position = new Vector3(Math.Clamp(this.Position.x, 0, Globals.levelSize.x), Math.Clamp(this.Position.y, 0, Globals.levelSize.y), Position.z);
		ZoomPercent = (float)CurrentZoom / (float)DefaultZoom;
		//GD.Print(ZoomPercent);
		Vector2 movedir = Input.GetVector("Camera LEFT", "Camera RIGHT", "Camera DOWN", "Camera UP");
		//movedir.Normalized();

		//Translate(new Vector3(movedir.x,movedir.y, 0) * ZoomPercent);

		if (Input.IsActionJustReleased("Camera ZOOMIN_MOUSE") && Input.IsKeyPressed(Key.Shift))
		{
			movedir.x -= 6;
		}
        if (Input.IsActionJustReleased("Camera ZOOMOUT_MOUSE") && Input.IsKeyPressed(Key.Shift))
        {
            movedir.x += 6;
        }
        if (Input.IsActionJustReleased("Camera ZOOMIN_MOUSE") && Input.IsKeyPressed(Key.Ctrl))
        {
            movedir.y += 6;
        }
        if (Input.IsActionJustReleased("Camera ZOOMOUT_MOUSE") && Input.IsKeyPressed(Key.Ctrl))
        {
            movedir.y -= 6;
        }

        if (Input.IsActionPressed("Camera CENTER"))
		{
			//this.Position = new Vector3(Globals.levelSize.x / 2, Globals.levelSize.y / 2, Position.z);
			CenterCamera(false);

		}
		if (Input.IsActionJustPressed("Camera FULLCENTER"))
		{
			CenterCamera(true);
		}
		if (Input.IsActionPressed("Camera ZOOMIN"))
		{
			CurrentZoom -= 1;
            CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
            ZoomCamera(CurrentZoom);
		}
		if (Input.IsActionJustReleased("Camera ZOOMIN_MOUSE") && !(Input.IsKeyPressed(Key.Shift) || Input.IsKeyPressed(Key.Ctrl)))
		{
            CurrentZoom -= MinZoomIn;
            CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
            ZoomCamera(CurrentZoom);
        }
        if (Input.IsActionJustReleased("Camera ZOOMOUT_MOUSE") && !(Input.IsKeyPressed(Key.Shift) || Input.IsKeyPressed(Key.Ctrl)))
        {
            CurrentZoom += MinZoomIn;
            CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
            ZoomCamera(CurrentZoom);
        }
        if (Input.IsActionPressed("Camera ZOOMOUT"))
		{
			CurrentZoom += 1;
			CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
			ZoomCamera(CurrentZoom);
		}
		if (Input.IsMouseButtonPressed(MouseButton.Middle))
		{

		}





        movedir.Normalized();

        Translate(new Vector3(movedir.x, movedir.y, 0) * ZoomPercent);

    }

	public void ZoomCamera(uint zoom)
	{
		Size = Math.Clamp(zoom, MinZoomIn, MaxZoomOut);
	}



	public void CenterCamera(bool fill)
	{
		if (fill == true)
		{
            this.Position = new Vector3(Globals.levelSize.x / 2, Globals.levelSize.y / 2, Position.z);
            ZoomCamera(MaxZoomOut);
            CurrentZoom = MaxZoomOut;
        }
		else
		{
			this.Position = new Vector3(Globals.levelSize.x / 2, Globals.levelSize.y / 2, Position.z);
			ZoomCamera(DefaultZoom);
			CurrentZoom = DefaultZoom;
		}

	}



}
