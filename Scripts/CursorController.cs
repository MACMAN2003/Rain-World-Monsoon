using Godot;
using System;

namespace RainWorldMonsoon;


public partial class CursorController : Node
{
	// Called when the node enters the scene tree for the first time.
	public CanvasItem Mainviewport;
	public CanvasItem Geoviewport;
	public CanvasItem Tileviewport;
	public Sprite2D cursortemp;
	public Sprite2D cursortile;
	public Label tilelabel1;
	public Label tilelabel2;
	public Label cursorlabel;
	public MatrixMaster Matrix;
	public TileController TileMatrix;

	public override void _Ready()
	{
		Mainviewport = (CanvasItem)GetTree().Root.FindChild("Matrix Display Master 2D", true, false);
		Geoviewport = (CanvasItem)GetTree().Root.FindChild("Matrix Display Apprentice Geo", true, false);
		Tileviewport = (CanvasItem)GetTree().Root.FindChild("Matrix Display Apprentice Tile", true, false);
		Matrix = (MatrixMaster)GetTree().Root.FindChild("Matrix Master", true, false);
		TileMatrix = (TileController)GetTree().Root.FindChild("Tile Controller", true, false);
		// replace moving the temporary cursor with drawing a rect where its position is
		// lol nvm
		cursortemp = (Sprite2D)Geoviewport.FindChild("Cursor", true);
		cursortile = (Sprite2D)Tileviewport.FindChild("Cursor", true);
		tilelabel1 = (Label)cursortile.GetNode("Label1");
		tilelabel2 = (Label)cursortile.GetNode("Label2");
		cursorlabel = (Label)cursortemp.GetNode("Label");
		//RNG.Seed = 64;



	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		/*if (Input.IsKeyPressed(Key.K))
		{
			//RNG.Seed = Globals.CursorPosition.y;
			string s = RNG.Seed.ToString();
			s += " <- seed|output -> ";
			s += RNG.Random(0).ToString();
			s += " next seed ->";
			s += RNG.Seed.ToString();
			GD.Print(s);
		}*/
		if (Globals.State != EditorState.LoadSaveIO)
		{
			CacheCompute();
		}
		if (Globals.State == EditorState.Main)
		{
			CursorCompute();
		}
		if (Globals.State == EditorState.Geometry)
		{
			GeoCursorCompute();
			//GeoToolCursorCompute();
		}
		if (Globals.State == EditorState.Tile)
		{
			TileCursorCompute();
		}
	}

	public void CacheCompute()
	{
		if (!Input.IsActionJustPressed("Redo") && Input.IsActionJustPressed("Undo"))
		{
			Undo();
		}
		if (Input.IsActionJustPressed("Redo"))
		{
			Redo();
		}
	}
	public void Undo()
	{
		Globals.Undo();
		TileMatrix.ReloadMatrix(false);
		Matrix.ReloadMatrix(false);
	}
	public void Redo()
	{
		Globals.Redo();
		TileMatrix.ReloadMatrix(false);
		Matrix.ReloadMatrix(false);
	}

	public void CursorCompute()
	{
		Vector2i roundCoord;
		roundCoord = new Vector2i((int)Math.Floor(Mainviewport.GetGlobalMousePosition().x / 16), (int)Math.Floor(Mainviewport.GetGlobalMousePosition().y / 16));
		Vector2 realCoord;
		realCoord = Mainviewport.GetViewport().GetMousePosition();
		Globals.MousePosition = realCoord;
		Globals.UnroundedCursorPosition = Mainviewport.GetGlobalMousePosition();
		Globals.ActiveViewportSize = Mainviewport.GetViewport().GetVisibleRect().Size;
		Globals.ActiveViewport = Mainviewport.GetViewport();

		roundCoord.x = Math.Clamp(roundCoord.x, 1, (int)Globals.levelSize.x);
		roundCoord.y = Math.Clamp(roundCoord.y, 1, (int)Globals.levelSize.y);

		Globals.CursorPosition = roundCoord;
	}

	//public void GeoToolCursorCompute()
	//{
//
   // }

	public void TileCursorCompute()
	{
		Vector2i roundCoord;
		roundCoord = new Vector2i((int)Math.Floor(Tileviewport.GetGlobalMousePosition().x / 16), (int)Math.Floor(Tileviewport.GetGlobalMousePosition().y / 16));
		if (Globals.TileCatIndex.x > Globals.GTiles.Count - 1)
		{
			Globals.TileCatIndex.x = Globals.GTiles.Count - 1;
		}
		if (Globals.TileCatIndex.y > Globals.GTiles[Globals.TileCatIndex.x].tiles.Count - 1)
		{
			Globals.TileCatIndex.y = Globals.GTiles[Globals.TileCatIndex.x].tiles.Count - 1;
		}

		Vector2 realCoord;
		realCoord = Tileviewport.GetViewport().GetMousePosition();
		Globals.MousePosition = realCoord;
		Globals.UnroundedCursorPosition = Tileviewport.GetGlobalMousePosition();
		Globals.ActiveViewportSize = Tileviewport.GetViewport().GetVisibleRect().Size;
		Globals.ActiveViewport = Tileviewport.GetViewport();
		Vector2i mouseCoord = roundCoord;
		if (Input.IsActionJustPressed("CatIndex LEFT") | Input.IsActionJustPressed("ToolMatrix LEFT"))
		{
			Control focus = (Control)Tileviewport.GetParent().GetParent();
			focus.GrabFocus();
			if (Globals.TileCatIndex.x - 1 < 0)
			{
				Globals.TileCatIndex.x = Globals.GTiles.Count - 1;
			}
			else
			{
				Globals.TileCatIndex.x -= 1;
			}
			//GD.Print(Globals.TileCatIndex.x);
		}
		if (Input.IsActionJustPressed("CatIndex RIGHT") | Input.IsActionJustPressed("ToolMatrix RIGHT"))
		{
			Control focus = (Control)Tileviewport.GetParent().GetParent();
			focus.GrabFocus();
			if (Globals.TileCatIndex.x + 1 > Globals.GTiles.Count - 1)
			{
				Globals.TileCatIndex.x = 0;
			}
			else
			{
				Globals.TileCatIndex.x += 1;
			}
			//GD.Print(Globals.TileCatIndex.x);
		}
		if (Input.IsActionJustPressed("CatIndex UP") | Input.IsActionJustPressed("ToolMatrix UP"))
		{
			Control focus = (Control)Tileviewport.GetParent().GetParent();
			focus.GrabFocus();
			if (Globals.TileCatIndex.y - 1 < 0)
			{
				Globals.TileCatIndex.y = Globals.GTiles[Globals.TileCatIndex.x].tiles.Count - 1;
			}
			else
			{
				Globals.TileCatIndex.y -= 1;
			}
		}
		if (Input.IsActionJustPressed("CatIndex DOWN") | Input.IsActionJustPressed("ToolMatrix DOWN"))
		{
			Control focus = (Control)Tileviewport.GetParent().GetParent();
			focus.GrabFocus();
			if (Globals.TileCatIndex.y + 1 > Globals.GTiles[Globals.TileCatIndex.x].tiles.Count - 1)
			{
				Globals.TileCatIndex.y = 0;
			}
			else
			{
				Globals.TileCatIndex.y += 1;
			}
		}



		roundCoord.x = Math.Clamp(roundCoord.x, 1, (int)Globals.levelSize.x);
		roundCoord.y = Math.Clamp(roundCoord.y, 1, (int)Globals.levelSize.y);

		Globals.CursorPosition = roundCoord;
		if (Input.IsActionJustReleased("TOOL_USE1"))
		{
			TileMatrix.LastPos = new Vector2i(-1, -1);
		}
		//if (Input.IsActionJustReleased("TOOL_USE1"))
		// {
		//Matrix.LastPos = new Vector2i(-1, -1);
		//}

		cursortile.Position = new Vector2i(roundCoord.x * 16, roundCoord.y * 16);
		tilelabel1.Text = roundCoord.ToString();
		if (Globals.tilematrix[Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1, Globals.CurrentLayer - 1].Type == TETileType.Material | Globals.tilematrix[Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1, Globals.CurrentLayer - 1].Type == TETileType.TileHead)
		{
			tilelabel2.Text = Globals.tilematrix[Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1, Globals.CurrentLayer - 1].Data.MaterialOrTileName;
		}
		else if (Globals.tilematrix[Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1, Globals.CurrentLayer - 1].Type == TETileType.TileBody)
		{
			Vector2 a = (Vector2)Globals.tilematrix[Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1, Globals.CurrentLayer - 1].Data.TileIndexOrHeadCoords;
			int b = (int)Globals.tilematrix[Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1, Globals.CurrentLayer - 1].Data.LayerOrDefault;
			//GD.Print(a.x, " ", a.y, " ", b);
			tilelabel2.Text = Globals.tilematrix[(int)a.x - 1, (int)a.y - 1, b -1].Data.MaterialOrTileName;
		}
		else
		{
			tilelabel2.Text = "";
		}
		//Control ctrl = (Control)Geoviewport.GetParent().GetParent();
		Viewport view = GetTree().Root.FindChild("Matrix Display Apprentice Tile", true, false).GetViewport();
		Rect2 rect = view.GetVisibleRect();



		if (Globals.SelectedTile is InternalSpecial t && t.PlaceMethod == SpecialPlaceType.Rect)
		{
			if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Tile & rect.HasPoint(view.GetMousePosition()))
			{
				if (mouseCoord == roundCoord)
				{
					TileMatrix.Action(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
				}
			}
		}
		else
		{
			if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Tile & rect.HasPoint(view.GetMousePosition()))
			{
				if (mouseCoord == roundCoord)
				{
					TileMatrix.Action(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
				}
			}
		}
		if (Input.IsActionPressed("TOOL_USE2") & Globals.State == EditorState.Tile & rect.HasPoint(view.GetMousePosition()))
		{
			if (mouseCoord == roundCoord)
			{
				TileMatrix.Action2(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
			}
		}
		if (Input.IsActionJustPressed("TOOL_USE6"/*"Q"*/) & Globals.State == EditorState.Tile & rect.HasPoint(view.GetMousePosition()))
		{
			if (mouseCoord == roundCoord)
			{
				TileMatrix.SampleTile(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
			}
		}
		if ((Globals.State == EditorState.Tile & rect.HasPoint(view.GetMousePosition())) && (Input.IsActionJustReleased("TOOL_USE1") | Input.IsActionJustReleased("TOOL_USE2")))
		{
			Globals.LoadIntoUndoCache();
		}
	}



	public void GeoCursorCompute()
	{
		//Geoviewport.GetMousePosition();
		Vector2i roundCoord;
		roundCoord = new Vector2i((int)Math.Floor(Geoviewport.GetGlobalMousePosition().x / 16), (int)Math.Floor(Geoviewport.GetGlobalMousePosition().y / 16));

		Vector2i mouseCoord = roundCoord;

		Vector2 realCoord;
		realCoord = Geoviewport.GetViewport().GetMousePosition();
		Globals.MousePosition = realCoord;
		Globals.UnroundedCursorPosition = Geoviewport.GetGlobalMousePosition();
		Globals.ActiveViewportSize = Geoviewport.GetViewport().GetVisibleRect().Size;
		Globals.ActiveViewport = Geoviewport.GetViewport();

		roundCoord.x = Math.Clamp(roundCoord.x, 1, (int)Globals.levelSize.x);
		roundCoord.y = Math.Clamp(roundCoord.y, 1, (int)Globals.levelSize.y);

		Globals.CursorPosition = roundCoord;
		if (Input.IsActionJustReleased("TOOL_USE1"))
		{
			Matrix.LastPos = new Vector2i(-1, -1);
		}

		cursortemp.Position = new Vector2i(roundCoord.x * 16, roundCoord.y * 16);
		cursorlabel.Text = roundCoord.ToString();
		//Control ctrl = (Control)Geoviewport.GetParent().GetParent();
		Viewport view = GetTree().Root.FindChild("Matrix Display Apprentice Geo", true, false).GetViewport();
		Rect2 rect = view.GetVisibleRect();
		//GD.Print(view.GetMousePosition().ToString());
		//GD.Print(rect);
		//if (rect.HasPoint(view.GetMousePosition()))
		//{
		//	GD.Print("True!");
		//}
		//else GD.Print("False!");

		
		if (Input.IsActionJustPressed("CatIndex LEFT") | Input.IsActionJustPressed("ToolMatrix LEFT"))
		{
			if (Globals.ToolMatrixIndex.x - 1 < 1)
			{
				Globals.ToolMatrixIndex.x = 4;
			}
			else
			{
				Globals.ToolMatrixIndex.x -= 1;
			}
			//GD.Print(Globals.TileCatIndex.x);
		}
		if (Input.IsActionJustPressed("CatIndex RIGHT") | Input.IsActionJustPressed("ToolMatrix RIGHT"))
		{
			if (Globals.ToolMatrixIndex.x + 1 > 4)
			{
				Globals.ToolMatrixIndex.x = 1;
			}
			else
			{
				Globals.ToolMatrixIndex.x += 1;
			}
			//GD.Print(Globals.TileCatIndex.x);
		}
		if (Input.IsActionJustPressed("CatIndex UP") | Input.IsActionJustPressed("ToolMatrix UP"))
		{
			if (Globals.ToolMatrixIndex.y - 1 < 1)
			{
				Globals.ToolMatrixIndex.y = 8;
			}
			else
			{
				Globals.ToolMatrixIndex.y -= 1;
			}
		}
		if (Input.IsActionJustPressed("CatIndex DOWN") | Input.IsActionJustPressed("ToolMatrix DOWN"))
		{
			if (Globals.ToolMatrixIndex.y + 1 > 8)
			{
				Globals.ToolMatrixIndex.y = 1;
			}
			else
			{
				Globals.ToolMatrixIndex.y += 1;
			}
		}

		//  if ((Globals.CurrentTool == GeometryTools.Invert || Globals.CurrentTool == GeometryTools.RectWall || Globals.CurrentTool == GeometryTools.RectAir || Globals.CurrentTool == GeometryTools.ChangeLayer || Globals.CurrentTool == GeometryTools.CopyBack || Globals.CurrentTool == GeometryTools.Flip || Globals.CurrentTool == GeometryTools.MirrorToggle) && Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
		// {
		//     if (mouseCoord == roundCoord)
		//     {
		//         Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
		//     }
		//}
		if (rect.HasPoint(view.GetMousePosition()))
		{
			if ( Globals.CurrentTool == GeometryTools.RectWall | Globals.CurrentTool == GeometryTools.RectAir | Globals.CurrentTool == GeometryTools.CopyBack | Globals.CurrentTool == GeometryTools.MirrorToggle | Globals.CurrentTool == GeometryTools.Flip)
			{
				if (Input.IsActionJustPressed("TOOL_USE1"))
				{
					if (mouseCoord == roundCoord)
					{
						Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
					}
				}
			}
			else
			{
				if (Input.IsActionPressed("TOOL_USE1"))
				{
					if (mouseCoord == roundCoord)
					{
						Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
					}
				}
			}
		}
		if ((Globals.State == EditorState.Geometry & rect.HasPoint(view.GetMousePosition())) && (Input.IsActionJustReleased("TOOL_USE1")))
		{
			Globals.LoadIntoUndoCache();
		}

		//else if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
		//{
		//	if (mouseCoord == roundCoord)
		//	{
		//		//LERect rect2 = new LERect(roundCoord.x, roundCoord.y, roundCoord.x, roundCoord.y);
		//		Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
		//	}
		//}

		//GD.Print(roundCoord);

	}


}
