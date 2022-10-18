using Godot;
//using RainWorldMonsoon;
using System;

namespace RainWorldMonsoon;

public partial class LECamera2D : Camera2D
{
	// 832 x 640 is 13 x 5 aspect ratio (????)
	public float MaxZoomOut = Globals.levelSize.x / 16f / 10;
	public float MinZoomIn = 0.4f;
	public float DefaultZoom = 1f;
	public float ZoomPercent = 1f;
	private float CurrentZoom = 1f;
	private SubViewport viewport;
	public Vector2 mouserelative;
	public override void _Ready()
	{
		CurrentZoom = DefaultZoom;
		viewport = (SubViewport)GetViewport();
	}


	public void UpdateCamera()
	{
		if (Globals.levelSize.x >= 72)
		{
            if (Globals.levelSize.y > Globals.levelSize.x)
            {
                MaxZoomOut = Globals.levelSize.y / 16;
            }
            else
            {
                MaxZoomOut = Globals.levelSize.x / 16;
            }
        }

		CenterCamera(true);
	}





	public override void _Input(InputEvent @event)
	{
		if (Globals.State != EditorState.LoadSaveIO)
		{
			if (@event is InputEventMouseMotion mouse)
			{
				if (mouse.ButtonMask == MouseButton.MaskMiddle)
				{
					//GD.Print(mouse.Relative);
					Position -= mouse.Relative * ZoomPercent;
					//GD.Print(mouse.Relative.x, mouse.Relative.y);
				}
				//GD.Print(mouse.Relative);
			}
		}
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Globals.State != EditorState.LoadSaveIO)
		{
			if (Globals.Settings.IsFreecam == false)
			{
				this.Position = new Vector2(Math.Clamp(this.Position.x, 0, Globals.levelSize.x * 16), Math.Clamp(this.Position.y, 0, Globals.levelSize.y * 16));
			}
			else this.Position = new Vector2(Math.Clamp(this.Position.x, -19384, 19384), Math.Clamp(this.Position.y, -19384, 19384)); // literall george orwell's 19384
			ZoomPercent = (float)DefaultZoom / (float)CurrentZoom;
			//GD.Print(ZoomPercent);
			Vector2 movedir = Input.GetVector("Camera LEFT", "Camera RIGHT", "Camera UP", "Camera DOWN");
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

			if (Input.IsActionJustPressed("Camera CENTER"))
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
				CurrentZoom += 0.3f;
				CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
				ZoomCamera(CurrentZoom);
			}
			if (Input.IsActionJustReleased("Camera ZOOMIN_MOUSE") && !(Input.IsKeyPressed(Key.Shift) || Input.IsKeyPressed(Key.Ctrl)))
			{
				CurrentZoom += MinZoomIn;
				CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
				ZoomCamera(CurrentZoom);
			}
			if (Input.IsActionJustReleased("Camera ZOOMOUT_MOUSE") && !(Input.IsKeyPressed(Key.Shift) || Input.IsKeyPressed(Key.Ctrl)))
			{
				CurrentZoom -= MinZoomIn;
				CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
				ZoomCamera(CurrentZoom);
			}
			if (Input.IsActionPressed("Camera ZOOMOUT"))
			{
				CurrentZoom -= 0.3f;
				CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
				ZoomCamera(CurrentZoom);
			}
			if (Input.IsMouseButtonPressed(MouseButton.Middle))
			{

			}





			movedir.Normalized();
			// GD.Print(Zoom);
			// GD.Print(ZoomPercent);
			Translate(new Vector2(movedir.x, movedir.y) * ZoomPercent * 16);



		}

		Globals.CameraPosition = Position;
		Globals.CameraZoom = Zoom;

	}


	public void ZoomCamera(float zoom)
	{
		Zoom = new Vector2(Math.Clamp(zoom, MinZoomIn, MaxZoomOut), Math.Clamp(zoom, MinZoomIn, MaxZoomOut)); //2D is fucky
	}


	public void CenterCamera(bool dontResetZoom = false)
	{
		if (dontResetZoom == true)
		{
			this.Position = new Vector2((Globals.levelSize.x * 16) / 2, (Globals.levelSize.y * 16) / 2);
			ZoomCamera(MinZoomIn);
			CurrentZoom = MinZoomIn;
		}
		else
		{
			this.Position = new Vector2((Globals.levelSize.x * 16) / 2, (Globals.levelSize.y * 16) / 2);
			ZoomCamera(DefaultZoom);
			CurrentZoom = DefaultZoom;
		}
	}




}
