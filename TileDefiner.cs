using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RainWorldMonsoon;


// so far the only thing that is not internal, it's put in the level output .txts on saving (not rendering)
public enum TETileType
{
	Default,
	Material,
	TileHead,
	TileBody
}

public struct TETileData // data's vector2 is for if the tileHead name fails to get a match, it just gets the tile at category X index Y
{
	public TETileData(int l = 0, string s = null, int px = 0, int py = 0)
	{
		LayerOrDefault = l;
		MaterialOrTileName = s;
		TileIndexOrHeadCoords = new Vector2i(px, py);
	}
	public override string ToString()
	{
		
		string s = "Layer: " + LayerOrDefault + " Tile/Mat Name: " + MaterialOrTileName + " Tile Index / Head Coordinates: " + TileIndexOrHeadCoords;
		return s;
	}
	// all nullable because some won't be used by all
	public int LayerOrDefault; // only used by TileBody and Default
	public string MaterialOrTileName; // only used by TileHead and Material
	public Vector2i TileIndexOrHeadCoords; // only used by TileHead and TileBody, TileBody points to the grid coord of its corresponding head, and TileHead points to the category and index of the tile in the list
}


public struct TETile
{
	public TETileType Type;
	public TETileData Data;
}



public enum MaterialRenderType
{
	Unified,
	Tiles,
	PipeType,
	RockType,
	LargeTrashType,
	RoughRock,
	MegaTrashType,
	DirtType,
	SandyDirtType,
	WoodType,
	DensePipeType,
	RandomPipesType,
	CeramicType,
	CeramicAType,
	CeramicBType,
	AddVoidLmao
}
// they're separate because i cannot tell the difference between the lingo code and the init file xd
public enum TileRenderType
{
	VoxelStruct,
	VoxelStructRandomDisplaceHorizontal,
	VoxelStructRandomDisplaceVertical,
	VoxelStructRockType,
	Box //But what kind of box????
}
public enum SpecialPlaceType
{
	Rect
}



public struct TileCategory
{
	public TileCategory(string catname, int catid, Color? catcolor = null)
	{
		CategoryName = catname;
		CategoryID = catid;
		tiles = new List<object>();
		if (catcolor != null)
		{
			CategoryColor = (Color)catcolor;
		}
		else CategoryColor = Color.Color8(0, 0, 0, 0);
	}
	public string CategoryName;
	public int CategoryID;
	public List<object> tiles;
	public Color CategoryColor;
}

public struct InternalSpecial // IDK WHAT THESE DO HELP
{
	public InternalSpecial(string nm, int pointx, int pointy, int specs, string placetype, Color? color = null, params string[] tags)
	{
		Name = nm;
		Size = new Vector2i(pointx, pointy);
		Specs = new int[specs];
		PlaceMethod = Enum.Parse<SpecialPlaceType>(placetype);
		if (color != null)
		{
			Color = (Color)color;
		}
		else Color = new Color(0, 0, 0, 0);
		Tags = new List<string>();
		foreach (string tag in tags)
		{
			Tags.Add(tag);
		}
	}
	public string Name;
	public Vector2i Size;
	public int[] Specs;
	public SpecialPlaceType PlaceMethod;
	public Color Color;
	public List<string> Tags;
}




public struct InternalMaterial
{
	//                                                  v ouch, PointY
	public InternalMaterial(string nm, int PointX, int PointY, int specs, string renderType = "Unified", Color? color = null, params string[] tags)
	{
		Name = nm;
		Size = new Vector2i(PointX, PointY);
		Specs = new int[specs];
		RenderType = Enum.Parse<MaterialRenderType>(renderType); // can't believe this works lol
		if (color != null)
		{
			Color = (Color)color;
		}
		else Color = new Color(0, 0, 0, 0);
		Tags = new List<string>();
		foreach (string tag in tags)
		{
			Tags.Add(tag);
		}
	}
	// example gTiles[1].tls.add( [#nm:"SuperStructure", #sz:point(1,1), #specs:[0], #renderType:"unified", #color:color(160,180,255), #tags:[]]    )
	public string Name;
	public Vector2i Size; // doesn't usually go above 1,1
	public int[] Specs; // doesn't go above 0, ever, in the lingo code
	public MaterialRenderType RenderType;
	// RGBA without the A, in the lingo code 255,255,255 is "transparent"
	public Color Color;
	public List<string> Tags;
}

public struct InternalTile
{
	//todo: make thingy does new when call new InternalTile()
	//don't code when you're tired ^^
	public InternalTile(string nm, int pointX, int pointY, List<int> specs, List<int> specs2, string tp, List<int> repeatl, int bftiles, int rnd, int ptPos, params string[] tags)
	{
		Name = nm;
		Size = new Vector2i(pointX, pointY);
		if (specs != null)
		{
			Specs = new int[specs.Count];
			for (int i = 0; i < specs.Count; i++)
			{
				Specs[i] = specs[i]; // thank you vscode autocomplete
			}
		}
		else Specs = null;
		if (specs2 != null)
		{
			//GD.Print(specs2.Count);
			Specs2 = new int[specs2.Count]; // JLKHAFSDKJ OH MY GOD I FORGOT THE () IN THE .Count() 
			for (int i = 0; i < specs2.Count; i++)
			{
				Specs2[i] = specs2[i];
			}
			//GD.Print(Specs2.Length);
		}
		else Specs2 = null;
		//if (Specs2 != null)
		//{
		//	for (int i = 0; i < Specs2.Length; i++)
		//	{
		//		GD.Print(Specs2[i]);
		//	}
		//	//GD.Print("------===xxXX_ULTRAxSUPERx", Specs2.Length, "_XXxx===------"); //edgier than a line between two vertices
		//}

		//GD.Print(Specs2.Length, " length");
		//GD.Print(Specs2.Count(), " count");
		Type = Enum.Parse<TileRenderType>(tp);
		if (repeatl != null)
		{
			RepeatL = new int[repeatl.Count()];
			for (int i = 0; i < repeatl.Count(); i++)
			{
				RepeatL[i] = repeatl[i];
			}
		}
		else RepeatL = null;

		BFTiles = bftiles;
		Random = rnd;
		PreviewTilePosition = ptPos;
		PreviewTilePositionY = 0;
		Tags = new List<string>();
		foreach (string tag in tags)
		{
			Tags.Add(tag);
			//GD.Print(tag);
		}
		unassigned = false;
	}


	// example [#nm:"Big Head", #sz:point(4,4), #specs:[1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1], #specs2:0, #tp:"voxelStruct", #repeatL:[1,1,1,1,6], #bfTiles:1, #rnd:1, #ptPos:0, #tags:["nonSolid"]]
	public string Name;
	public Vector2i Size;
	public int[] Specs; // geo matrix, goes down until it reaches SIZE.Y then it resumes at SIZE.X + 1
	public int[] Specs2; // geo matrix used for "background" tiles
	public TileRenderType Type;
	public int[] RepeatL; // no idea what this does// NEVERMIND IT'S THE IMAGE SIZE FOR THE LAYERS??
	public int BFTiles; // best friend tiles :) (no idea what this does) (buffer?)
	public int Random; //used for random variations?
	public int PreviewTilePosition; // <- lingo code uses SIXTY THOUSAND (60,000!!!!!) as the image size for the preview tiles!!!! VERY INCOMPATIBLE WITH GODOT!!!!!!! Godot's max width and height for images is 16384 x 16384 (16k)
	// the previewTiles image is 60,000 x 500 with a bit depth of 1, that seems like a waste of potential space?
	// 30 million pixels, for 60k x 500
	// 16k is 16384
	// meanwhile 16k x 16k is 268435456 or 268 million (close enough!!!!) i know how to fix this [INSERT_EVIL_FACE_EMOTICON_HERE]
	public int PreviewTilePositionY; // hahahha i am evil and insane!!!!!! <- planned to be used internally, no human interaction required!
	// format la8?? (that will still take up a metric shit-ton of space but at least it'll have alpha support!);
	public List<string> Tags;
	public bool unassigned = true;
}






public partial class TileDefiner : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
