using Godot;
//using RainWorldMonsoon;
using System;
using System.Diagnostics;

namespace RainWorldMonsoon;

public partial class LECamera2D : Camera2D
{
	// 832 x 640 is 13 x 5 aspect ratio (????)
	public float MaxZoomOut = Globals.levelSize.x / 16f / 10;
	public float MinZoomIn = 0.4f;
	public float DefaultZoom = 1f;
	public float ZoomPercent = 1f;
	public Vector2 ZoomDelta;
	private float CurrentZoom = 1f;
	private SubViewport viewport;
	public Vector2 mouserelative;
	public CursorController cursorcontroller;
	public CanvasItem Mainviewport;
	public CanvasItem Geoviewport;
	public CanvasItem Tileviewport;


	public override void _Ready()
	{
		cursorcontroller = (CursorController)GetTree().Root.FindChild("Cursor Controller", true, false);
		CurrentZoom = DefaultZoom;
		viewport = (SubViewport)GetViewport();
		Mainviewport = (CanvasItem)GetTree().Root.FindChild("Matrix Display Master 2D", true, false);
		Geoviewport = (CanvasItem)GetTree().Root.FindChild("Matrix Display Apprentice Geo", true, false);
		Tileviewport = (CanvasItem)GetTree().Root.FindChild("Matrix Display Apprentice Tile", true, false);
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

	[Signal]
	public delegate void ZoomUpdatedEventHandler();


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
			
			if (Input.IsActionJustReleased("Camera ZOOMIN_MOUSE") && Input.IsKeyPressed(Key.Ctrl))
			{
				switch (Globals.State)
				{
					case EditorState.LoadSaveIO:
						break;
					case EditorState.Main:
						movedir.x -= 6;
						break;
					case EditorState.Geometry:
						break;
					case EditorState.Tile:
						if (Globals.TileCatIndex.x + 1 > Globals.GTiles.Count - 1)
						{
							Globals.TileCatIndex.x = 0;
						}
						else
						{
							Globals.TileCatIndex.x += 1;
						}
						break;
					case EditorState.Camera:
						break;
					case EditorState.Light:
						break;
					case EditorState.Properties:
						break;
					case EditorState.Effects:
						break;
					case EditorState.Props:
						break;
					case EditorState.Environment:
						break;
					default:
						break;
				}
				
			}
			if (Input.IsActionJustReleased("Camera ZOOMOUT_MOUSE") && Input.IsKeyPressed(Key.Ctrl))
			{
				switch (Globals.State)
				{
					case EditorState.LoadSaveIO:
						break;
					case EditorState.Main:
						movedir.x += 6;
						break;
					case EditorState.Geometry:
						break;
					case EditorState.Tile:


						if (Globals.TileCatIndex.x - 1 < 0)
						{
							Globals.TileCatIndex.x = Globals.GTiles.Count - 1;
						}
						else
						{
							Globals.TileCatIndex.x -= 1;
						}
						break;
					case EditorState.Camera:
						break;
					case EditorState.Light:
						break;
					case EditorState.Properties:
						break;
					case EditorState.Effects:
						break;
					case EditorState.Props:
						break;
					case EditorState.Environment:
						break;
					default:
						break;
				}
				
			}
			if (Input.IsActionJustReleased("Camera ZOOMIN_MOUSE") && Input.IsKeyPressed(Key.Shift))
			{
				switch (Globals.State)
				{
					case EditorState.LoadSaveIO:
						break;
					case EditorState.Main:
						movedir.y += 6;
						break;
					case EditorState.Geometry:
						break;
					case EditorState.Tile:
						if (Globals.TileCatIndex.y - 1 < 0)
						{
							Globals.TileCatIndex.y = Globals.GTiles[Globals.TileCatIndex.x].tiles.Count - 1;
						}
						else
						{
							Globals.TileCatIndex.y -= 1;
						}
						break;
					case EditorState.Camera:
						break;
					case EditorState.Light:
						break;
					case EditorState.Properties:
						break;
					case EditorState.Effects:
						break;
					case EditorState.Props:
						break;
					case EditorState.Environment:
						break;
					default:
						break;
				}
				
			}
			if (Input.IsActionJustReleased("Camera ZOOMOUT_MOUSE") && Input.IsKeyPressed(Key.Shift))
			{
				switch (Globals.State)
				{
					case EditorState.LoadSaveIO:
						break;
					case EditorState.Main:
						movedir.y -= 6;
						break;
					case EditorState.Geometry:
						break;
					case EditorState.Tile:
						if (Globals.TileCatIndex.y + 1 > Globals.GTiles[Globals.TileCatIndex.x].tiles.Count - 1)
						{
							Globals.TileCatIndex.y = 0;
						}
						else
						{
							Globals.TileCatIndex.y += 1;
						}
						break;
					case EditorState.Camera:
						break;
					case EditorState.Light:
						break;
					case EditorState.Properties:
						break;
					case EditorState.Effects:
						break;
					case EditorState.Props:
						break;
					case EditorState.Environment:
						break;
					default:
						break;
				}
				
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
			Rect2 rect = Globals.ActiveViewport.GetVisibleRect();
			if (Input.IsActionJustReleased("Camera ZOOMIN_MOUSE") && !(Input.IsKeyPressed(Key.Shift) || Input.IsKeyPressed(Key.Ctrl)) && rect.HasPoint(Globals.ActiveViewport.GetMousePosition()))
			{
				//idea:
				//move camera to cursor position, strength is inverse to zoom%
				//so the farther you're zoomed out, the more it'll "snap" to the cursor's position
				// or "fake zoom" the camera and adjust the position accordingly so that the in editor cursor-
				//- remains in the same spot on the screen
				//barycentric coordinates?
				//actually nvm i got it working lol
				//Vector2 mouseTrue = new Vector2(Globals.MousePosition.x / CurrentZoom + )
				//Vector2 dir = (Globals.MousePosition - Position).Normalized();
				//GD.Print(GetScreenCenterPosition(), " screencent");
				//GD.Print(Position, " pos");
				//GD.Print(GetTargetPosition(), " targpos");
				//GD.Print(dir);
				//GD.Print(Globals.ActiveViewport.GetChild<CanvasItem>(0).GetGlobalMousePosition(), " old");
				//GD.Print(Globals.CursorPosition, " oldgl");
				Vector2 oldzpos = Globals.ActiveViewport.GetChild<CanvasItem>(0).GetGlobalMousePosition();


				ZoomDelta = Zoom;
				CurrentZoom += MinZoomIn;
				CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
				ZoomCamera(CurrentZoom);
				Vector2 newzpos = Globals.ActiveViewport.GetChild<CanvasItem>(0).GetGlobalMousePosition();
				Vector2 difference = oldzpos - newzpos;
				Translate(difference);
				//GD.Print(Globals.MousePosition, " new");
				//GD.Print(Globals.ActiveViewport.GetChild<CanvasItem>(0).GetGlobalMousePosition(), " new");


			}
			if (Input.IsActionJustReleased("Camera ZOOMOUT_MOUSE") && !(Input.IsKeyPressed(Key.Shift) || Input.IsKeyPressed(Key.Ctrl)) && rect.HasPoint(Globals.ActiveViewport.GetMousePosition()))
			{
				Vector2 oldzpos = Globals.ActiveViewport.GetChild<CanvasItem>(0).GetGlobalMousePosition();
				CurrentZoom -= MinZoomIn;
				CurrentZoom = Math.Clamp(CurrentZoom, MinZoomIn, MaxZoomOut);
				ZoomCamera(CurrentZoom);
				Vector2 newzpos = Globals.ActiveViewport.GetChild<CanvasItem>(0).GetGlobalMousePosition();
				Vector2 difference = oldzpos - newzpos;
				Translate(difference);
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
		//ZoomDelta = Zoom;
		Zoom = new Vector2(Math.Clamp(zoom, MinZoomIn, MaxZoomOut), Math.Clamp(zoom, MinZoomIn, MaxZoomOut)); //2D is fucky
		Globals.CameraZoom = Zoom;
		EmitSignal(SignalName.ZoomUpdated);
		//ctor2 newZoomPos = Globals.CursorPosition;
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
