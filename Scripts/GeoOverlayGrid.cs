using Godot;
using System;


namespace RainWorldMonsoon;

public partial class GeoOverlayGrid : Sprite2D
{
	private ImageTexture grid;
	private Image gridsq;
	private Image grids;
	private LevelSize internallevelsize;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		grid = new ImageTexture();
		grids = Utilities.QuickConvert("res://Core/quickGrid.png");
		gridsq = Utilities.QuickConvert("res://Core/quickGridCell.png");
		//gridsq = Utilities.ColorCOnv
		internallevelsize = new LevelSize(1, 1);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Vector2i globsize = new Vector2i((int)Globals.levelSize.x, (int)Globals.levelSize.y);
		Vector2i intsize = new Vector2i((int)internallevelsize.x, (int)internallevelsize.y);
		if (intsize != globsize)
		{
			ResetSize();
		}

	}

	public void ResetSize()
	{

        grids.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		int X = 0;
		int Y = 0;
		int Z = 0;
		foreach(GeometryTile tile in Globals.matrix)
		{



            grids.BlitRect(gridsq, gridsq.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
            Z++;
            if (Z + 1 > 3)
            {
                Z = 0;
                Y++;
            }
            if (Y + 1 > Globals.levelSize.y)
            {
                Y = 0;
                X++;
            }




        }

		grid.SetImage(grids);
        Texture = grid;
		internallevelsize = Globals.levelSize;
	}
}
