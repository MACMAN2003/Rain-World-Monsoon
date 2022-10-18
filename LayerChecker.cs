using Godot;
using System;

namespace RainWorldMonsoon;
public partial class LayerChecker : Label
{
	enum LayerCheckerQuickEnum
	{
		Unassigned,
		CheckLayer,
		CheckMirror,
		CheckDefaultMaterial,
		CheckCursorTileType
	}
	[Export] bool checkMirror = false;
	[Export] LayerCheckerQuickEnum QuickChecker;
	[Export] string PrefixString;
	[Export] string SuffixString;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (checkMirror == true & QuickChecker == 0)
		{
			QuickChecker = LayerCheckerQuickEnum.CheckMirror;
		}
		if (checkMirror == false & QuickChecker == 0)
		{
			QuickChecker = LayerCheckerQuickEnum.CheckLayer;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		switch (QuickChecker)
		{
			case LayerCheckerQuickEnum.Unassigned:
				break;
			case LayerCheckerQuickEnum.CheckLayer:
                Text = PrefixString + Globals.CurrentLayer.ToString() + SuffixString;
                break;
			case LayerCheckerQuickEnum.CheckMirror:
                Text = PrefixString + Globals.IsMirrorOn.ToString() + SuffixString;
                break;
			case LayerCheckerQuickEnum.CheckDefaultMaterial:
                if (Globals.GTiles[Globals.DefaultMaterial.x].tiles[Globals.DefaultMaterial.y] is InternalMaterial d)
                {
                    Text = PrefixString + d.Name + SuffixString;
                }
                break;
			//Text = PrefixString + Globals.GTiles[Globals.DefaultMaterial.x].tiles[Globals.TileCatIndex.y].Name;
			case LayerCheckerQuickEnum.CheckCursorTileType:



				break;
			default:
				break;
		}
		//if (checkMirror == false)
		//{
		//	Text = PrefixString + Globals.CurrentLayer.ToString();
		//}
		//else 
		//{
		//	Text = PrefixString + Globals.IsMirrorOn.ToString();
		//}
	}
}
