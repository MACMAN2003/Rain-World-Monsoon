using Godot;
using System;

namespace RainWorldMonsoon;


public partial class CursorController : Node
{
	// Called when the node enters the scene tree for the first time.

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
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Globals.State == EditorState.Geomery)
        {
            GeoCursorCompute();
            //GeoToolCursorCompute();
        }
        if (Globals.State == EditorState.Tile)
        {
            TileCursorCompute();
        }
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



        Vector2i mouseCoord = roundCoord;
        if (Input.IsActionJustPressed("CatIndex LEFT"))
        {
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
        if (Input.IsActionJustPressed("CatIndex RIGHT"))
        {
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
        if (Input.IsActionJustPressed("CatIndex UP"))
        {
            if (Globals.TileCatIndex.y - 1 < 0)
            {
                Globals.TileCatIndex.y = Globals.GTiles[Globals.TileCatIndex.x].tiles.Count - 1;
            }
            else
            {
                Globals.TileCatIndex.y -= 1;
            }
        }
        if (Input.IsActionJustPressed("CatIndex DOWN"))
        {
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


        if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Tile & rect.HasPoint(view.GetMousePosition()))
        {
            if (mouseCoord == roundCoord)
            {
                TileMatrix.Action(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
            }
        }
        if (Input.IsActionPressed("TOOL_USE2") & Globals.State == EditorState.Tile & rect.HasPoint(view.GetMousePosition()))
        {
            if (mouseCoord == roundCoord)
            {
                TileMatrix.Action2(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
            }
        }


    }



	public void GeoCursorCompute()
	{
        //Geoviewport.GetMousePosition();
        Vector2i roundCoord;
        roundCoord = new Vector2i((int)Math.Floor(Geoviewport.GetGlobalMousePosition().x / 16), (int)Math.Floor(Geoviewport.GetGlobalMousePosition().y / 16));

        Vector2i mouseCoord = roundCoord;


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

      //  if ((Globals.CurrentTool == GeometryTools.Invert || Globals.CurrentTool == GeometryTools.RectWall || Globals.CurrentTool == GeometryTools.RectAir || Globals.CurrentTool == GeometryTools.ChangeLayer || Globals.CurrentTool == GeometryTools.CopyBack || Globals.CurrentTool == GeometryTools.Flip || Globals.CurrentTool == GeometryTools.MirrorToggle) && Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
       // {
       //     if (mouseCoord == roundCoord)
       //     {
       //         Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
       //     }
        //}
        switch (Globals.CurrentTool)
        {
            case GeometryTools.Invert:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                    break;
            case GeometryTools.PaintWall:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.PaintAir:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Floor:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Slope:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.EnemyDen:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Entrance:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.RectWall:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.RectAir:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.CopyBack:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Flip:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.HorizBeam:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.VertiBeam:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Glass:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Shortcut:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.ShortcutDot:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.BatflyHive:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.ChangeLayer:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.MirrorToggle:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.MirrorMove:
                if (Input.IsActionJustPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Rock:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Spear:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Crack:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.ForbidBatflyChain:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.GarbageWormHole:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Waterfall:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.WhackAMoleHole:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.Wormgrass:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            case GeometryTools.ScavengerHole:
                if (Input.IsActionPressed("TOOL_USE1") & Globals.State == EditorState.Geomery & rect.HasPoint(view.GetMousePosition()))
                {
                    if (mouseCoord == roundCoord)
                    {
                        Matrix.UseTool(roundCoord.x, roundCoord.y, Globals.CurrentLayer);
                    }
                }
                break;
            default:
                break;
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
