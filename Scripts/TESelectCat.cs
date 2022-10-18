using Godot;
using System;



namespace RainWorldMonsoon;
public partial class TESelectCat : RichTextLabel
{
	[Export]RichTextLabel list;
	[Export] bool IsIndexShower = false;
	//[Export] RichTextLabel Indexer;
	Vector2i lastvec;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		lastvec.x = -1;
		if (list == null && IsIndexShower == false)
		{
			list = GetParent().GetNode<RichTextLabel>("VFlowContainer/TILE LIST");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (list == null && IsIndexShower == false)
		{
			list = (RichTextLabel)GetTree().Root.FindChild("TILE LIST", true, false);
		}

		if (Globals.State == EditorState.Tile)
		{
			if (lastvec != Globals.TileCatIndex)
			{

				if (IsIndexShower == false)
				{
					list.Clear();
					Compute();
				}
				else Compute2();
			}

		}
	}
	public void Compute2()
	{
		Text = "[b] { " + (Globals.TileCatIndex.x + 1) + ", " + (Globals.TileCatIndex.y + 1) + " }";
	}

	public void Compute()
	{
		//Clear();
		Text = "[b] [ " + Globals.GTiles[Globals.TileCatIndex.x].CategoryName + " ]";
		//list.Clear();
		list.PushBold();
		for (int i = 0; i < Globals.GTiles[Globals.TileCatIndex.x].tiles.Count; i++)
		{
			string index;
			if (Globals.GTiles[Globals.TileCatIndex.x].tiles[i] is InternalMaterial d)
			{
				//InternalMaterial d;
				//d = (InternalMaterial)Globals.GTiles[Globals.TileCatIndex.x].tiles[i];
				index = d.Name;
			}
			else if (Globals.GTiles[Globals.TileCatIndex.x].tiles[i] is InternalSpecial s)
			{
				//Internal d;
				//s = (InternalTile)Globals.GTiles[Globals.TileCatIndex.x].tiles[i];
				index = s.Name;
			}
			else
			{
				InternalTile t;
				t = (InternalTile)Globals.GTiles[Globals.TileCatIndex.x].tiles[i];
				index = t.Name;
			}
			//list.Newline();
			if (i == Globals.TileCatIndex.y)
			{
				list.AddText("- " + index + " -");
			}
			else
			{
				list.AddText(index);
			}
			list.Newline();
		}
		lastvec = Globals.TileCatIndex;
		if (Globals.TileCatIndex.y < 22)
		{
			list.ScrollToLine(0);
		}
		else
		{
			list.ScrollToLine(Globals.TileCatIndex.y);
		}
	}




}
