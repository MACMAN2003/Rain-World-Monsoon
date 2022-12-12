using Godot;
using System;


namespace RainWorldMonsoon;
public partial class DroughtTagChecker : RichTextLabel
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
        // will reinstate on request
        /*
		try
		{
            if (Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y] != null && Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y] is InternalMaterial d)
            {
                //InternalMaterial d;
                //d = (InternalMaterial)Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y];
                if (d.Tags.Contains("droughtReserve"))
                {
                    Visible = true;
                }
                else
                {
                    Visible = false;
                }
            }
            if (Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y] != null && Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y] is InternalTile e)
            {
                if (e.Tags.Contains("droughtReserve"))
                {
                    Visible = true;
                }
                else
                {
                    Visible = false;
                }
            }
        }
		catch (Exception)
		{

			// uhhhh do nothing lol, the exception is handled already ecks dee
		}
        */
	}
}
