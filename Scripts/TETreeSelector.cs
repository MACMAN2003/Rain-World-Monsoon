using Godot;
using System;
using System.Text.RegularExpressions;

namespace RainWorldMonsoon;
public partial class TETreeSelector : Tree
{
	//haha im cheating the system
	bool firstframe = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (firstframe == true)
		{
			//GD.Print("Bruh");
			TreeItem Root = CreateItem();
			Root.SetText(0, "Root");
			HideRoot = true;
			for (int i = 0; i < Globals.GTiles.Count; i++)
			{
				//GD.Print("bruh");
				TreeItem test = CreateItem(Root);
				
				for(int j = 0; j < Globals.GTiles[i].tiles.Count; j++)
				{
					TreeItem subtest = test.CreateChild();
					if (Globals.GTiles[i].tiles[j] is InternalMaterial d)
					{
						subtest.SetText(0, "[" + (j + 1) + "] " + d.Name);
						//subtest.SetCustomColor(0, d.Color);
					}
					else if (Globals.GTiles[i].tiles[j] is InternalTile t)
					{
						subtest.SetText(0, "[" + (j + 1) + "] " + t.Name);
					}
					else
					{
						InternalSpecial s = (InternalSpecial)Globals.GTiles[i].tiles[j];
						subtest.SetText(0, "[" + (j + 1) + "] " + s.Name);
					}
					//subtest.SetText(0, Globals.GTiles[i].tiles[j]);
				}
				test.SetText(0, "[" + (i + 1) + "] " + Globals.GTiles[i].CategoryName);
				test.Collapsed = true;
			}
			Callable InSignal = new Callable(this, nameof(ItemMouseSelect));
			Connect("item_mouse_selected", InSignal);
		}

	}
	Regex numreg = new Regex("\\[([0-9]+)\\]", RegexOptions.IgnoreCase);

	public void ItemMouseSelect(Vector2 pos, int dex)
	{
		//GD.Print("bruh" + dex);
		TreeItem sel = GetSelected();
		//GD.Print(sel.GetText(0));
		if (sel.GetParent().GetText(0) != "Root")
		{
			//Match
			int x = int.Parse(numreg.Match(sel.GetParent().GetText(0)).Groups[1].Value);
			int y = int.Parse(numreg.Match(sel.GetText(0)).Groups[1].Value);
			//GD.Print(x);
			//GD.Print(sel.GetText(0));
			Globals.TileCatIndex = new Vector2i(x - 1, y - 1);
		}
		else Globals.TileCatIndex.x = int.Parse(numreg.Match(sel.GetText(0)).Groups[1].Value) - 1;
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//leet hax0r moment right here
		if (firstframe == false)
		{
			firstframe = true;
			_Ready();
		}
		//TreeItem bruh = GetItemAtPosition(new Vector2(5, 6));
		//GD.Print(GetItemAtPosition(new Vector2(5,4)).GetIndex());

	}
}
