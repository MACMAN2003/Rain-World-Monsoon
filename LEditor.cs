using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates; // no idea what the fuck this is or does, vscode just auto generated it
using System.Text.RegularExpressions; // lol regex

namespace RainWorldMonsoon;
public enum Tiles
{
	// for the slopes, the right angle points towards a direction on a compass
	Air = 0,
	Wall = 1,
	SlopeSE = 2,
	SlopeSW = 3,
	SlopeNE = 4,
	SlopeNW = 5,
	Floor = 6,
	ConnectedShortcut = 7,
	//something = 8,
	Glass = 9
}
public enum GeometryTools
{
	Invert = 0,
	PaintWall = 1,
	PaintAir = 2,
	Floor = 3,
	Slope = 4,
	EnemyDen = 5,
	Entrance = 6,
	RectWall = 7,
	RectAir = 8,
	Move = 9,
	CopyBack = 10,
	Flip = 11,
	HorizBeam = 12,
	VertiBeam = 13,
	Glass = 14,
	Shortcut = 15,
	ShortcutDot = 16,
	BatflyHive = 17,
	ChangeLayer = 18,
	MirrorToggle = 19,
	MirrorMove = 20,
	Rock = 21,
	Spear = 22,
	Crack = 23,
	ForbidBatflyChain = 24,
	GarbageWormHole = 25,
	Waterfall = 26,
	WhackAMoleHole = 27,
	Wormgrass = 28,
	ScavengerHole = 29
}

public enum EditorState
{
	LoadSaveIO = 0,
	Main = 1,
	Geomery = 2,
	Tile = 3,
	Camera = 4,
	Light = 5,
	Properties = 6,
	Effects = 7,
	Props = 8,
	Environment = 9
}


public struct LevelFileBuffer
{
	public string Line1;
	public string Line2;
	public string Line3;
	public string Line4;
	public string Line5;
	public string Line6;
	public string Line7;
	public string Line8;
	public string Line9;
}


public struct EditorSettings
{
	public EditorSettings()
	{
		IsFreecam = false;
		ShowCompass = true;
	}
	public bool IsFreecam;
	public bool ShowCompass;
}



public enum TETools
{
	Material,
	Special,
	Tile
}

public enum Tiles2
{
	Horizonpole = 1,
	Vertipole = 2,
	BatflyHive = 3,
	ShortcutEntrance = 4,
	ShortcutConnector = 5,
	RoomEntrance = 6,
	EnemyDen = 7,
	Rock = 9,
	Spear = 10,
	Cracks = 11,
	ForbidBatflyChain = 12,
	GarbageWormEntrance = 13,
	Waterfall = 18,
	WhackAMoleHole = 19,
	WormGrass = 20,
	ScavengerEntrance = 21
}
/*
public struct LEProps 
{
	public Column[] matrix;
	// other stuff might be here.
}*/


public struct LevelSize
{
	public LevelSize(uint X, uint Y)
	{

		x = X;
		y = Y;
	}

	//unsigned ints because going below zero just won't do.
	public uint x;
	public uint y;
}


public enum LERectEdge
{
	Null = 0,
	Left = 1,
	Top = 2,
	Right = 3,
	Bottom = 4,
	TopLeft = 5,
	TopRight = 6,
	BottomRight = 7,
	BottomLeft = 8,
}

public struct LERect
{
	public LERect(int Left, int Top, int Right, int Bottom)
	{
		left = Left;
		top = Top;
		right = Right;
		bottom = Bottom;
	}
	public LERect(int X, int Y, uint Width, uint Height)
	{
		left = X;
		top = Y;
		right = X + (int)Width - 1;
		bottom = Y + (int)Height - 1;
	}
	public LERect(Vector2i TopLeft, Vector2i BottomRight)
	{
		left = TopLeft.x;
		top = TopLeft.y;
		right = BottomRight.x;
		bottom = BottomRight.y;
	}
	public bool IsInsideRect(Vector2i Point)
	{
		if ((Point.x < left || Point.x > right) || (Point.y < top || Point.y > bottom))
		{
			return false;
		}
		else return true;
	}
	public bool IsOnEdge(Vector2i Point)
	{
		if ((Point.x != left & Point.x != right) & (Point.y != top & Point.y != bottom))
		{
			return false;
		}
		else return true;
	}
	public Rect2 GetRect2()
	{
		Rect2 rect = new Rect2();
		rect.Position = new Vector2i(left, top);
		rect.End = new Vector2i(right, bottom);
		return rect;
	}
	public LERectEdge WhichEdge(Vector2i Point)
	{
		if (IsOnEdge(Point) == true)
		{
			//LERect intrect = new LERect(left - 1, top - 1, right + 1, bottom + 1);
			if (IsInsideRect(Point))
			{
				LERectEdge edge;
				// fucking do something that's not an IF/ELSE shit
				if (Point.x == left)
				{
					if (Point.y == top)
					{
						edge = LERectEdge.TopLeft;
					}
					else if (Point.y == bottom)
					{
						edge = LERectEdge.BottomLeft;
					}
					else
					{
						edge = LERectEdge.Left;
					}
				}
				else if (Point.x == right)
				{
					if (Point.y == top)
					{
						edge = LERectEdge.TopRight;
					}
					else if (Point.y == bottom)
					{
						edge = LERectEdge.BottomRight;
					}
					else
					{
						edge = LERectEdge.Right;
					}
				}
				else if (Point.y == top)
				{
					if (Point.x == left)
					{
						edge = LERectEdge.TopLeft;
					}
					else if (Point.x == right)
					{
						edge = LERectEdge.TopRight;
					}
					else
					{
						edge = LERectEdge.Top;
					}
				}
				else
				{
					if (Point.x == left)
					{
						edge = LERectEdge.BottomLeft;
					}
					else if (Point.x == right)
					{
						edge = LERectEdge.BottomRight;
					}
					else
					{
						edge = LERectEdge.Bottom;
					}
				}





				return edge;
			}
			else return LERectEdge.Null;

		}
		else
		{
			return LERectEdge.Null;
		}

	}

	public int left;
	public int top;
	public int right;
	public int bottom;

	//hastily slapped together because fuck it
	public static LERect operator +(LERect a, LERect b) => new LERect(a.left + b.left, a.top + b.top, a.right + b.right, a.bottom + b.bottom);
	public static LERect operator -(LERect a, LERect b) => new LERect(a.left - b.left, a.top - b.top, a.right - b.right, a.bottom - b.bottom);
	public static LERect operator *(LERect a, LERect b) => new LERect(a.left * b.left, a.top * b.top, a.right * b.right, a.bottom * b.bottom);
	public static LERect operator /(LERect a, LERect b) => new LERect(a.left / b.left, a.top / b.top, a.right / b.right, a.bottom / b.bottom);



}

public struct LEQuad // yes i am crazy how could you tell?
{
	public LEQuad(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
	{
		P1 = p1;
		P2 = p2;
		P3 = p3;
		P4 = p4;
	}
	public Vector2 P1;
	public Vector2 P2;
	public Vector2 P3;
	public Vector2 P4;


	public bool IsInsideBounds(Vector2 point)
	{
		float minx;
		float miny;
		float maxx;
		float maxy;

		float[] xs = new float[]{P1.x, P2.x, P3.x, P4.x};
		float[] ys = new float[]{P1.y, P2.y, P3.y, P4.y };
		minx = xs.Min();
		miny = ys.Min();
		maxx = xs.Max();
		maxy = ys.Max();

		if (point.x < minx | point.x > maxx | point.y < miny | point.y > maxy)
		{
			return false;
		}
		else return true;

	}
	public Vector2 GetBoundsMin()
	{
		float minx;
		float miny;
		float maxx;
		float maxy;

		float[] xs = new float[] { P1.x, P2.x, P3.x, P4.x };
		float[] ys = new float[] { P1.y, P2.y, P3.y, P4.y };
		minx = xs.Min();
		miny = ys.Min();
		maxx = xs.Max();
		maxy = ys.Max();
		return new Vector2(minx, miny);
	}
	public Vector2 GetBoundsMax()
	{
		float minx;
		float miny;
		float maxx;
		float maxy;

		float[] xs = new float[] { P1.x, P2.x, P3.x, P4.x };
		float[] ys = new float[] { P1.y, P2.y, P3.y, P4.y };
		minx = xs.Min();
		miny = ys.Min();
		maxx = xs.Max();
		maxy = ys.Max();
		return new Vector2 (maxx, maxy);
	}
	public bool IsInsideQuad(Vector2 point)
	{
		if (IsInsideBounds(point))
		{
			if (IsInsideTri(point, P1, P2, P3))
			{
				return true;
			}
			else if (IsInsideTri(point, P1, P3, P4))
			{
				return true;
			}
			else return false;
		}
		else return false;






	}

	private static bool IsInsideTri(Vector2 point, Vector2 Vert0, Vector2 Vert1, Vector2 Vert2)
	{
		/*
		var s = (Vert1.x - Vert3.x) * (point.y - Vert3.y) - (Vert1.y - Vert3.y) * (point.x - Vert3.x);
		var t = (Vert2.x - Vert1.x) * (point.y - Vert1.y) - (Vert2.y - Vert1.y) * (point.x - Vert1.x);

		if ((s < 0) != (t < 0) && s != 0 && t != 0)
		{
			return false;
		}

		var d = (Vert3.x - Vert2.x) * (point.y - Vert2.y) - (Vert3.y - Vert2.y) * (point.x - Vert2.x);

		return d == 0 || (d < 0) == (s + t <= 0);
		*/
		//CLOCK WISE CLOCK WISE
		float e01 = (point.x - Vert0.x) * (Vert1.y - Vert0.y) - (point.y - Vert0.y) * (Vert1.x - Vert0.x);
		float e12 = (point.x - Vert1.x) * (Vert2.y - Vert1.y) - (point.y - Vert1.y) * (Vert2.x - Vert1.x);
		float e20 = (point.x - Vert2.x) * (Vert0.y - Vert2.y) - (point.y - Vert2.y) * (Vert0.x - Vert2.x);

		if (e01 <= 0 && e12 <= 0 && e20 <= 0)
		{
			return true;
		}
		else return false;

	}




}




public struct ExtraTiles
{

	public ExtraTiles(int Left, int Top, int Right, int Bottom)
	{
		left = Left;
		top = Top;
		right = Right;
		bottom = Bottom;
	}
	public int left;
	public int top;
	public int right;
	public int bottom;
}

//namespace RainWorldMonsoon;
public static class Globals 
{
	public static bool IsEditorEvil = false; //let me show you the power of the dark side
	//store the level's project txt file as a 9 stringed buffer;
	public static string gLoadedName = "New Level";
	public static string gLOADPATH = null;
	public static EditorState State = EditorState.Main;
	public static EditorState PrevState = EditorState.Main;

	public static EditorSettings Settings = new EditorSettings();
	//Settings
	public static Vector2i DefaultMaterial;
	public static string DefaultMaterialName;
	public static GeometryTools CurrentTool = GeometryTools.Invert;
	public static int CurrentLayer = 1;
	public static Vector2i CursorPosition;
	public static Vector2i RectLockPos;
	public static int MirrorXPos = 0;

	public static TETools TEToolType;
	public static TETileData TEToolData;

	public static Vector2i LastTile;

	public static bool IsLightOn = true;
	public static bool IsBorderSolid = true;
	public static int TileSeed;


	//why didn't i think of this earlier XD
	public static Dictionary<string, Vector2i> MaterialsDictionary = new Dictionary<string, Vector2i>();
	public static List<Dictionary<string, Vector2i>> TileDictionaries = new List<Dictionary<string, Vector2i>>();

	public static List<int> TMSavePositionLookup = new List<int>();

	public static bool IsRectOn = false;
	public static Boolean IsMirrorOn = false; //boolean > bool because it sounds cool

	public static LevelFileBuffer LevelFile;
	//example: the geometry stuff is line 1

	public static Vector2 CameraPosition;
	public static Vector2 CameraZoom;
	//----------------------------------------------------------------------------------------------------------------------------------------------------
	//todo: change if needed
	public static bool QuickLoadSpeedX = false;
	public static bool CPUConversion = false;
	//please todo: change if needed
	//----------------------------------------------------------------------------------------------------------------------------------------------------
	/*

	afsddddddddddddddddddddddddddddddddddddddddddddddddddddddddd
	afsddddddddddddddddddddddddddddddddddddddddddddddddddddddd

	asfddddddddasfasdfglkjfadg;lkjagfds;lkjgf;dlkjgfda;lkagsj;lkagfsd

	asgdlkj;adlsfgkjadf;glkjgafd;lkgjf;lkdfsgj;alsdfkgja;sdlgkjgas;dlksdgj



	this will help find the quick load speed x

	ajkhdsflkjhafsdlkjhadfslkajsdfhlakjdsfhlaskdjfhalskdfjha
	afdskjhadsfkjahsdfkjasdhfkjasdfhkasjdfhaskdjfhasdkjfhasdkjfhasdkfjahsd




	*/
	public static Image LevelEditImageShortcuts;
	public static Image LevelEditImage1;
	public static Image LevelEditImage2;
	public static Image LevelEditImage3;

	public static Image TEImage1;
	public static Image TEImage2;
	public static Image TEImage3;

	public static ImageTexture TETex1;
	public static ImageTexture TETex2;
	public static ImageTexture TETex3;

	public static ImageTexture LevelEditorShortcutLayer;
	public static ImageTexture LevelEditorLayer1;
	public static ImageTexture LevelEditorLayer2;
	public static ImageTexture LevelEditorLayer3;

	public static Image TilePreviewAtlas;
	public static Image TilePreviewAtlas2;
	public static Image TilePreviewAtlas3;

	public static ImageTexture TilePreviewTexture;
	public static ImageTexture TilePreviewTexture2;
	public static ImageTexture TilePreviewTexture3;

	public static LevelFileBuffer NewLevelFile; // warning, LOOOOOONG!!!!!!

	public static List<TileCategory> GTiles;
	public static Vector2i TileCatIndex = new Vector2i(0, 0); // X is category Y is index

	// the official editor can only show 52x40 grids in the geo editor
	public static LevelSize levelSize = new LevelSize(72, 43); // camelCase ftw
	//extra tiles, in order of left top right bottom, default is 12 3 12 5
	public static ExtraTiles extraTiles = new ExtraTiles(12, 3, 12, 5);
	public static GeometryTile[,,] matrix = new GeometryTile[levelSize.x, levelSize.y, 3];// haha hardcoding lol lmao;
	public static TETile[,,] tilematrix = new TETile[levelSize.x, levelSize.y, 3]; // once more haha lol lmao;

	//^ all that just to hold the data of //ONE LINE ;-;// TWO LINES :D

	//should never be 0 or less!
	//public static Vector2i LevelSize;
	//f it i'm making another struct lol


	// for reading line 1
	//public static LEProps gLeProps; //now for the hard part :(
	//for reading line 2

	//for reading line 3

	//for reading line 4

	//for reading line 5

	//for reading line 6

	//for reading line 7

	//for reading line 8

	//for reading line 9

	//for reading line 10 lol jk line 10 is unused
	public static void Init()
	{
		GD.Print("Beginning Initialization");
		Stopwatch timer = new Stopwatch();
		timer.Start();
		string path1 = OS.GetExecutablePath().GetBaseDir();
		string configpath = path1 + "/Monsoon_Config.txt";
		if (System.IO.File.Exists(configpath))
		{
			GD.Print("Loading Configuration");
			Regex QuickStartRegex = new Regex("\\[Fast\\sStartup:\\s([^\\]]+)\\]", RegexOptions.IgnoreCase);
			Regex CPURegex = new Regex("\\[CPU\\sColor\\sConversion:\\s([^\\]]+)\\]", RegexOptions.IgnoreCase);
			foreach(string s in System.IO.File.ReadLines(configpath))
			{
				Match QuickMatch = QuickStartRegex.Match(s);
				if (QuickMatch.Groups[1].Success)
				{
					if (QuickMatch.Groups[1].Value == "true" | QuickMatch.Groups[1].Value == "True")
					{
						QuickLoadSpeedX = true;
					}
					else
					{
						QuickLoadSpeedX = false;
					}
				}
				Match CPUMatch = CPURegex.Match(s);
				{
					if (CPUMatch.Groups[1].Success)
					{
						if (QuickMatch.Groups[1].Value == "true" | QuickMatch.Groups[1].Value == "True")
						{
							CPUConversion = true;
						}
						else
						{
							CPUConversion = false;
						}
					}
				}
			}
		}
		else
		{
			GD.Print("Config file at", configpath, " was expected, but it's not there!");
		}





		// basic hardcoded init part xd (just like the simulatio- lingo code)
		GTiles = new List<TileCategory>();
		GTiles.Add(new TileCategory("Materials", 0)); // zero cause zero based lists ;-;
		// damn i didn't expect lists to look EXACTLY the same as arrays lol
		GTiles[0].tiles.Add(new InternalMaterial("Standard",1,1,0,"Unified", Color.Color8(150,150,150)));
		GTiles[0].tiles.Add(new InternalMaterial("Concrete", 1, 1, 0, "Unified", Color.Color8(150, 255, 255)));
		GTiles[0].tiles.Add(new InternalMaterial("RainStone", 1, 1, 0, "Unified", Color.Color8(0, 0, 255)));
		GTiles[0].tiles.Add(new InternalMaterial("Steel", 1, 1, 0, "Unified", Color.Color8(220, 170, 195)));
		GTiles[0].tiles.Add(new InternalMaterial("Bricks", 1, 1, 0, "Unified", Color.Color8(200, 150, 100)));
		GTiles[0].tiles.Add(new InternalMaterial("BigMetal", 1, 1, 0, "Unified", Color.Color8(255, 0, 0)));
		GTiles[0].tiles.Add(new InternalMaterial("Tiny Signs", 1, 1, 0, "Unified", Color.Color8(255, 200, 255)));
		GTiles[0].tiles.Add(new InternalMaterial("Scaffolding", 1, 1, 0, "Unified", Color.Color8(60, 60, 40)));
		GTiles[0].tiles.Add(new InternalMaterial("Dense Pipes", 1, 1, 0, "DensePipeType", Color.Color8(0, 0, 150)));
		GTiles[0].tiles.Add(new InternalMaterial("SuperStructure", 1, 1, 0, "Unified", Color.Color8(160, 180, 255)));
		GTiles[0].tiles.Add(new InternalMaterial("SuperStructure2", 1, 1, 0, "Unified", Color.Color8(190, 160, 0)));
		GTiles[0].tiles.Add(new InternalMaterial("Tiled Stone", 1, 1, 0, "Tiles", Color.Color8(100, 0, 255)));
		GTiles[0].tiles.Add(new InternalMaterial("Chaotic Stone", 1, 1, 0, "Tiles", Color.Color8(255, 0, 255)));
		GTiles[0].tiles.Add(new InternalMaterial("Small Pipes", 1, 1, 0, "PipeType", Color.Color8(255, 255, 0)));
		GTiles[0].tiles.Add(new InternalMaterial("Trash", 1, 1, 0, "PipeType", Color.Color8(90, 255, 0)));
		GTiles[0].tiles.Add(new InternalMaterial("Invisible", 1, 1, 0, "AddVoidLmao", Color.Color8(200, 200, 200)));
		GTiles[0].tiles.Add(new InternalMaterial("LargeTrash", 1, 1, 0, "LargeTrashType", Color.Color8(175, 30, 255)));
		GTiles[0].tiles.Add(new InternalMaterial("3DBricks", 1, 1, 0, "Tiles", Color.Color8(255, 150, 0)));
		GTiles[0].tiles.Add(new InternalMaterial("Random Machines", 1, 1, 0, "Tiles", Color.Color8(72, 116, 80))); // <- this motherfucker is the GOAT, the MVP
		GTiles[0].tiles.Add(new InternalMaterial("Dirt", 1, 1, 0, "DirtType", Color.Color8(124, 72, 52)));
		GTiles[0].tiles.Add(new InternalMaterial("Ceramic Tile", 1, 1, 0, "CeramicType", Color.Color8(60, 60, 100)));
		GTiles[0].tiles.Add(new InternalMaterial("Temple Stone", 1, 1, 0, "Tiles", Color.Color8(0, 120, 180)));
		GTiles[0].tiles.Add(new InternalMaterial("Circuits", 1, 1, 0, "DensePipeType", Color.Color8(0, 150, 0)));

		DefaultMaterial = new Vector2i(0,1);
		if (GTiles[0].tiles[1] is InternalMaterial d)
		{
			DefaultMaterialName = d.Name;
		}
		// ===============
		// hand copied from the lingo code (the september 1st branch of the community editor's)
		// on request i will comment the below out.
		GTiles.Add(new TileCategory("Drought Materials", 1));
		GTiles[1].tiles.Add(new InternalMaterial("4Mosaic", 1, 1, 0, "Tiles", Color.Color8(227, 76, 13), "droughtReserve"));
		GTiles[1].tiles.Add(new InternalMaterial("Color A Ceramic", 1, 1, 0, "CeramicAType", Color.Color8(120, 0, 90)));
		GTiles[1].tiles.Add(new InternalMaterial("Color B Ceramic", 1, 1, 0, "CeramicBType", Color.Color8(0, 175, 175)));
		GTiles[1].tiles.Add(new InternalMaterial("Random Pipes", 1, 1, 0, "RandomPipesType", Color.Color8(80, 0, 140)));
		GTiles[1].tiles.Add(new InternalMaterial("Rocks", 1, 1, 0, "RockType", Color.Color8(185, 200, 0), "droughtReserve"));
		GTiles[1].tiles.Add(new InternalMaterial("Rough Rock", 1, 1, 0, "RoughRock", Color.Color8(155, 170, 0), "droughtReserve"));
		GTiles[1].tiles.Add(new InternalMaterial("Random Metal", 1, 1, 0, "Tiles", Color.Color8(180, 10, 10)));
		GTiles[1].tiles.Add(new InternalMaterial("Cliff", 1, 1, 0, "Unified", Color.Color8(75, 75, 75), "droughtReserve"));
		GTiles[1].tiles.Add(new InternalMaterial("Non-Slip Metal", 1, 1, 0, "Unified", Color.Color8(180, 80, 80), "droughtReserve"));
		GTiles[1].tiles.Add(new InternalMaterial("Stained Glass", 1, 1, 0, "Unified", Color.Color8(180, 80, 180)));
		GTiles[1].tiles.Add(new InternalMaterial("Sandy Dirt", 1, 1, 0, "RoughRock", Color.Color8(180, 180, 80)));
		GTiles[1].tiles.Add(new InternalMaterial("MegaTrash", 1, 1, 0, "MegaTrashType", Color.Color8(135, 10, 255)));
		GTiles[1].tiles.Add(new InternalMaterial("Shallow Dense Pipes", 1, 1, 0, "DensePipeType", Color.Color8(13, 23, 110)));
		// that's that stuff
		// now for the community materials
		GTiles.Add(new TileCategory("Community Materials", 2));
		GTiles[2].tiles.Add(new InternalMaterial("Shallow Circuits", 1, 1, 0, "DensePipeType", Color.Color8(15, 200, 155)));
		GTiles[2].tiles.Add(new InternalMaterial("Random Machines 2", 1, 1, 0, "Tiles", Color.Color8(116, 116, 80)));
		GTiles[2].tiles.Add(new InternalMaterial("Small Machines", 1, 1, 0, "Tiles", Color.Color8(80, 116, 116)));
		GTiles[2].tiles.Add(new InternalMaterial("Random Metals", 1, 1, 0, "Tiles", Color.Color8(255, 0, 80)));
		GTiles[2].tiles.Add(new InternalMaterial("ElectricMetal", 1, 1, 0, "Unified", Color.Color8(255, 0, 100)));
		GTiles[2].tiles.Add(new InternalMaterial("Grate", 1, 1, 0, "Unified", Color.Color8(190, 50, 190)));
		GTiles[2].tiles.Add(new InternalMaterial("CageGrate", 1, 1, 0, "Unified", Color.Color8(50, 190, 190)));
		// special rect stuff below done: added the special rect stuff
		GTiles.Add(new TileCategory("Special", 3));
		GTiles[3].tiles.Add(new InternalSpecial("Rect Clear", 1, 1, 0, "Rect", Color.Color8(255, 0, 0)));
		GTiles[3].tiles.Add(new InternalSpecial("SH Pattern Box", 1, 1, 0, "Rect", Color.Color8(210, 0, 255)));
		GTiles[3].tiles.Add(new InternalSpecial("SH Grate Box", 1, 1, 0, "Rect", Color.Color8(160, 0, 255)));
		//leditor code says this one is drought
		GTiles[3].tiles.Add(new InternalSpecial("Alt Grate Box", 1, 1, 0, "Rect", Color.Color8(75, 75, 240)));
		//my eyes and fingerses hurtses, precious
		// the painses oh the painses, it is too much for me
		// gollum, gollum

		for (int i = 0; i < GTiles.Count - 1; i++)
		{
			for (int j = 0; j < GTiles[i].tiles.Count; j++)
			{
				if (GTiles[i].tiles[j] is InternalMaterial m)
				{
					MaterialsDictionary.Add(m.Name, new Vector2i(i, j));
				}
				//MaterialsDictionary.Add(GTiles[i].)
			}
		}


		Regex CategoryRegex = new Regex("-\\[\"([^\"]+)\",\\scolor\\(([0-9]+),\\s([0-9]+),\\s([0-9]+)\\)\\]", RegexOptions.IgnoreCase);
		Regex InternalTileRegex = new Regex("#nm:\"([^\"]+)\",\\s#sz:point\\(([0-9]+),(\\s?[0-9]+)\\),\\s#specs:\\[([^\\]]*)\\],\\s#specs2:(\\[[^\\]]*\\]|[0-9]*|void),\\s?#tp:\"([^\"]+)\",\\s?(\\s#repeatL:\\[([^\\]]+)\\],)?\\s#bfTiles:([0-9]+),\\s#rnd:([0-9]+),\\s#ptPos:[0-9]*,\\s#tags:\\[(\"([^\\]]*)\")?\\]\\]", RegexOptions.IgnoreCase);
		// jesus fucking christ that's a long ass regex
		Regex Numbers = new Regex("-*[0-9]+");
		Regex Words = new Regex("[^\",\\s]+");
		string GraphicsDir = OS.GetExecutablePath().GetBaseDir();
		GraphicsDir += "/Graphics";
		string GraphicsFile = GraphicsDir + "/Init.txt";
		//GD.Print(GraphicsFile);
		if (System.IO.File.Exists(GraphicsFile))
		{
			int index = 4;
			//GD.Print("!!!!");
			GD.Print("Initializing Graphics/Init.txt!");
			foreach (string line in System.IO.File.ReadLines(GraphicsFile))
			{
				//GD.Print("New Line!", line);
				Match CatMatch = CategoryRegex.Match(line);
				if (CatMatch.Success == true)
				{
					//GD.Print(index);
					//GD.Print(CatMatch.Groups[1].Value);
					//GD.Print(CatMatch.Groups[2].Value);
					//GD.Print(CatMatch.Groups[3].Value);
					//GD.Print(CatMatch.Groups[4].Value);
					byte r = byte.Parse(CatMatch.Groups[2].Value);
					byte g = byte.Parse(CatMatch.Groups[3].Value);
					byte b = byte.Parse(CatMatch.Groups[4].Value);
					GTiles.Add(new TileCategory(CatMatch.Groups[1].Value, index, Color.Color8(r, g, b)));
					TileDictionaries.Add(new Dictionary<string, Vector2i>());
					index++;
				}
				Match indMatch = InternalTileRegex.Match(line);
				if (indMatch.Success == true)
				{
					List<int> specs;
					//if (line[indMatch.Groups[4].Index - 1] == char.Parse("["))
					//{
					//	GD.Print("yes 763");
					//}
					//if (indMatch.Groups[4].Value != "0")
					if (line[indMatch.Groups[4].Index - 1] == char.Parse("["))
					{
						MatchCollection matches = Numbers.Matches(indMatch.Groups[4].Value);
						//int matchindx = 0;
						specs = new List<int>();
						foreach (Match match in matches)
						{
							specs.Add(int.Parse(match.Value));
						}
					}
					else
					{
						specs = null;
						GD.Print(indMatch.Groups[1].Value + "'s SPECS IS NULL! THIS WILL BREAK STUFF!");
					}
					//TODO: find way to differentiate between Specs: 0 and Specs: [0]
					List<int> specs2;
					//if (indMatch.Groups[5].Value != "0" & indMatch.Groups[5].Value != "void") // for the edge cases "Crane House Platform" and "Crane House"
					if (line[indMatch.Groups[5].Index - 1] == char.Parse("["))
					{
						MatchCollection matches = Numbers.Matches(indMatch.Groups[5].Value);
						//GD.Print(indMatch.Groups[5].Value);
						//int matchindx = 0;
						specs2 = new List<int>();
						foreach (Match match in matches)
						{
							specs2.Add(int.Parse(match.Value));
							//GD.Print(match.Value);
							//GD.Print()
						}
						//GD.Print(specs2.Count(), " count");
					}
					else
					{
						specs2 = null;
					}
					List<int> repeatl;
					if (indMatch.Groups[7].Success)
					{
						MatchCollection matches = Numbers.Matches(indMatch.Groups[7].Value);
						//int matchindx = 0;
						repeatl = new List<int>();
						foreach (Match match in matches)
						{
							repeatl.Add(int.Parse(match.Value));
						}
					}
					else
					{
						repeatl = null; // shouldn't EVER go null // nvm it should be nullable
						//GD.Print("repeatL for \"", indMatch.Groups[1].Value, "\" is null, but it shouldn't be!");
					}
					MatchCollection matches2 = Words.Matches(indMatch.Groups[11].Value);
					List<string> tags = new List<string>();
					if (matches2.Count > 0)
					{
						foreach(Match match2 in matches2)
						{
							tags.Add(match2.Value);
						}
					}
					string[] stags = new string[tags.Count];
					for (int i = 0; i < tags.Count; i++)
					{
						stags[i] = tags[i];
					}
					//TileRenderType tp = Enum.Parse<TileRenderType>(indMatch.Groups[6].Value);
					string tp = indMatch.Groups[6].Value;
					tp = char.ToUpper(tp[0]) + tp.Substring(1);
					InternalTile t;
					if (repeatl == null)
					{
						t = new InternalTile(indMatch.Groups[1].Value, int.Parse(indMatch.Groups[2].Value), int.Parse(indMatch.Groups[3].Value), specs, specs2, tp, new List<int>(), int.Parse(indMatch.Groups[9].Value), int.Parse(indMatch.Groups[10].Value), 0, stags);
					}
					else
					{
						t = new InternalTile(indMatch.Groups[1].Value, int.Parse(indMatch.Groups[2].Value), int.Parse(indMatch.Groups[3].Value), specs, specs2, tp, repeatl, int.Parse(indMatch.Groups[9].Value), int.Parse(indMatch.Groups[10].Value), 0, stags);
					}
					//GD.Print(GTiles[index - 1].CategoryName);
					GTiles[index - 1].tiles.Add(t);
					//GD.Print(TileDictionaries.Count, " ", index - 4);
					TileDictionaries[index - 5].TryAdd(t.Name, new Vector2i(index - 1, GTiles[index - 1].tiles.Count));






				}



			}
			GD.Print("Graphics/init.txt read Successfuly!");
		}
		else GD.Print(string.Format("There is no Init.txt found at {0}!", GraphicsDir));
		for (int q = 0; q < GTiles.Count; q++)
		{
			TMSavePositionLookup.Add(1);
		}
		timer.Stop();
		GD.Print("Tile Data init took ", timer.ElapsedMilliseconds, " ms");
		int SixK = 16384; //<- SIXK UHD 260 FPS NVIDIA AMD RTX 
		TilePreviewAtlas = new Image();
		TilePreviewAtlas.Create(SixK, SixK,false,Image.Format.Rgba8);
		//TilePreviewAtlas.Resize(SixK,SixK);

		timer.Restart();
		GD.Print("Creating Graphics Atlas...");
		int ptposx = 1;
		int ptposy = 1;
		int tempptposy = 0;
		if (GTiles.Count > 4)// return the > when done testing
		{
			for (int q = 0; q < GTiles.Count(); q++)
			{
				for (int c = 0; c < GTiles[q].tiles.Count; c++)
				{
					//Image sav2 = Image.LoadFromFile(GraphicsDir + GTiles[q].tiles[c].Name);
					if (GTiles[q].tiles[c] is InternalTile ad)// 2000 ad robocop reference?!?!?!?!?!?! // judge dredd, not robocop, get some sleep
					{
						// find a way to disable the warning that pops up
						Image sav2 = Image.LoadFromFile(GraphicsDir + "/" + ad.Name + ".png"); // im going to go moron mode

						sav2 = Utilities.QuickConvert(sav2);
						/*if (ad.Name == "Big Chain Horizontal")
						{
							Image sav3 = Image.LoadFromFile(GraphicsDir + "/Big Chain Horizontal - Copy(modified).png");
							sav3 = Utilities.QuickConvert(sav3);
							byte[] bruh = sav3.GetData();
							int p = 0;
							//int c = 0;
							for (int i = 0; i < 604 * 4; i++)
							{
								if (i % 4 == 0)
								{
									p++;
									 //i think i'm on to something....
									 // it's row major order...
								}
								GD.Print(bruh[i], " ", p);
							}
							GD.Print(bruh);
						}*/
						//Image sav2 = null;
						//sav2 = Utilities.ColorConvert(sav2, Colors.Black, GTiles[q].CategoryColor);
						//sav2 = Utilities.ColorConvert(sav2, Colors.White, Colors.Transparent);
						//if (QuickLoadSpeedX == false)
						//{
						//	if(CPUConversion == true)
						//	{
						//		
						//	}
						//	sav2 = Utilities.SuperConvert(sav2, Colors.White, Colors.Transparent);
						//	sav2 = Utilities.NuclearConvert(sav2, Colors.Transparent, GTiles[q].CategoryColor);
						//}
						//if (ad.Name == "Big Head")
					   // {
						//    sav2 = Utilities.SuperConvert(sav2, Colors.White, Colors.Green);
						//	//sav2 = Utilities.SuperConvert(sav2, Colors.Black, GTiles[q].CategoryColor);
						//}
						int CalculatedHeight = sav2.GetHeight();
						if (ad.Type == TileRenderType.VoxelStruct)
						{
							CalculatedHeight = 1 + (16 * ad.Size.y) + (20 * (ad.Size.y + (ad.BFTiles * 2)) * ad.RepeatL.Count());
							//GD.Print(CalculatedHeight);
						}

						if (tempptposy <= (16 * ad.Size.y) + 1)
						{
							tempptposy += (16 * ad.Size.y) + 1;
							//GD.Print("MORE Y!");
						}
						//GD.Print(ad.Name + " " + ad.Type);
						//GD.Print(CalculatedHeight);
						//LERect rect = new LERect(0, CalculatedHeight - (16 * ad.Size.y), (16 * ad.Size.x), CalculatedHeight);
						Rect2i rct = new Rect2i(0, CalculatedHeight - (16 * ad.Size.y), (16 * ad.Size.x), (16 * ad.Size.y));
						//GD.Print(rct.ToString());
						//GD.Print("newrect " + rect.top + " " + rect.bottom);
						
						if (QuickLoadSpeedX == false)
						{
							Image sav3 = sav2.GetRect(rct);
							if (CPUConversion == true)
							{
								sav3 = Utilities.ColorConvert(sav3, Colors.White, Colors.Transparent);
								sav3 = Utilities.SpillConvert(sav3, Colors.Transparent, GTiles[q].CategoryColor);
							}
							else
							{
								sav3 = Utilities.SuperConvert(sav3, Colors.White, Colors.Transparent);
								sav3 = Utilities.NuclearConvert(sav3, Colors.Transparent, GTiles[q].CategoryColor);
							}
							sav2.BlitRect(sav3,new Rect2i(0,0,sav3.GetWidth(),sav3.GetHeight()),new Vector2i(0, CalculatedHeight - (16 * ad.Size.y)));
						}



						if (ptposx + (16*ad.Size.x) + 1 > TilePreviewAtlas.GetWidth())
						{
							GD.Print("MORE X!");
							ptposx = 1;
							ptposy += tempptposy;
							if (ptposy + (16 * ad.Size.y) + 1 > TilePreviewAtlas.GetHeight())
							{
								GD.Print("OH GOD!");
								GD.PrintErr("OH FUCK!!");
								throw new IndexOutOfRangeException("Whoops! too many tiles for Y workaround!");
								//Crash(); //lol, lmao
							}
							//GD.Print(ptposy + " MORE Y!");
							TilePreviewAtlas.BlitRect(sav2, rct, new Vector2i(ptposx, ptposy));
							ad.PreviewTilePosition = ptposx;
							ad.PreviewTilePositionY = ptposy;
							GTiles[q].tiles[c] = ad;
							//InternalTile xd = (InternalTile)GTiles[q].tiles[c];
						   // GD.Print(xd.PreviewTilePosition);
							ptposx += (16 * ad.Size.x) + 1;
						}
						else
						{
							//ptposx += (16 * ad.Size.x) + 1;
							//if (ad.Name == "MushroomTree" | ad.Name == "TriangularFridge")
							//{
							//	GD.Print(ad.Name + " " + ptposx + " " + ad.Size.x);
							//}
							//if (ptposx = )
							TilePreviewAtlas.BlitRect(sav2, rct, new Vector2i(ptposx, ptposy));
							ad.PreviewTilePosition = ptposx;
							ad.PreviewTilePositionY = ptposy;
							GTiles[q].tiles[c] = ad;
							//InternalTile xd = (InternalTile)GTiles[q].tiles[c];
							//GD.Print(xd.PreviewTilePosition);
							ptposx += (16 * ad.Size.x) + 1;
						}



					}
				}
			}





		}
		GD.Print("Graphics initialization took ", timer.ElapsedMilliseconds, " ms!");
	}
	public static void Import(string levelname)
	{
		gLOADPATH = levelname;
		string temp = levelname.Split(@"/").Last();
		int tempindex = temp.IndexOf(".txt");
		string temp2 = temp.Remove(tempindex);
		gLoadedName = temp2;

		Regex SizeRegex1 = new Regex("#size:\\spoint\\([0-9]+,\\s[0-9]+\\)", RegexOptions.IgnoreCase);
		Regex SizeRegex2 = new Regex("[0-9]+");
		Match SizeMatch = SizeRegex1.Match(LevelFile.Line6);


		int sizexy = 0;
		foreach(Match SizeMatch2 in SizeRegex2.Matches(SizeMatch.Value))
		{
			if(sizexy == 0)
			{
				string match2 = SizeMatch2.Value;
				uint xint = uint.Parse(match2);
				levelSize.x = xint;
			}
			else
			{
				string match2 = SizeMatch2.Value;
				uint yint = uint.Parse(match2);
				levelSize.y = yint;
			}
			sizexy++;
		}
		Regex BufferRegex1 = new Regex("#extraTiles:\\s\\[[0-9]+,\\s[0-9]+,\\s[0-9]+,\\s[0-9]+\\]", RegexOptions.IgnoreCase);
		Regex BufferRegex2 = new Regex("[0-9]+");
		Match BufferMatch = BufferRegex1.Match(LevelFile.Line6);
		int sizebuffer = 0;
		foreach(Match buffermatch in BufferRegex2.Matches(BufferMatch.Value))
		{
			switch (sizebuffer)
			{
				case 0:
					string buffermatch2 = buffermatch.Value;
					int lint = int.Parse(buffermatch2);
					extraTiles.left = lint;
					break;
				case 1:
					string buffermatch3 = buffermatch.Value;
					int tint = int.Parse(buffermatch3);
					extraTiles.top = tint;
					break;
				case 2:
					string buffermatch4 = buffermatch.Value;
					int rint = int.Parse(buffermatch4);
					extraTiles.right = rint;
					break;
				case 3:
					string buffermatch5 = buffermatch.Value;
					int bint = int.Parse(buffermatch5);
					extraTiles.bottom = bint;
					//bogos binted
					break;
				default:
					break;
			}
			sizebuffer++;
		}
		bool _seedInFile = false;
		Regex SeedRegex = new Regex("#tileSeed:\\s([0-9]+)", RegexOptions.IgnoreCase);
		Match SeedMatch = SeedRegex.Match(LevelFile.Line6);
		if (SeedMatch.Groups[1].Success)
		{
			_seedInFile = true;
			TileSeed = int.Parse(SeedMatch.Groups[1].Value);
		}

		bool _lightingInFile = false;
		Regex LightTypeRegex = new Regex("#light:\\s([0-9]+)", RegexOptions.IgnoreCase);
		Match LightMatch = LightTypeRegex.Match(LevelFile.Line6);
		if (LightMatch.Groups[1].Success)
		{
			if (LightMatch.Groups[1].Value == "1")
			{
				_lightingInFile = true;
				//GD.Print("Sunlight is: ");
				IsLightOn = true;
			}
			else
			{
				_lightingInFile = true;
				IsLightOn = false;
			}
		}

		Regex TerrainRegex = new Regex("#defaultTerrain:\\s([0-9]+)", RegexOptions.IgnoreCase);
		Match TerrainMatch = TerrainRegex.Match(LevelFile.Line5);
		bool _terrainInFile = false;
		if (TerrainMatch.Groups[1].Success)
		{
			if (TerrainMatch.Groups[1].Value == "1")
			{
				_terrainInFile = true;
				IsBorderSolid = true;
			}
			else
			{
				_terrainInFile = true;
				IsBorderSolid = false;
			}
		}

		if (_terrainInFile)
		{
			GD.Print("Level Border Solidity: ", IsBorderSolid);
		}
		else
		{
			GD.Print("Level file has no \"#defaultTerrain\" parameter, defaulting border solidity to True");
		}

		if (_seedInFile)
		{
			GD.Print("Level Tile Seed: ", TileSeed);
		}
		else
		{
			TileSeed = (int)GD.Randi() % 400;
			GD.Print("Level file has no \"#tileSeed\" parameter, defaulting to ", TileSeed, "!");
			//TileSeed = (int)GD.Randi() % 400;
		}

		//GD.Print(SizeMatch.Value);
		GD.Print("Level Size: ", levelSize.x, ", ", levelSize.y);
		GD.Print("Level Buffer Tiles: ", extraTiles.left, ", ", extraTiles.top, ", ", extraTiles.right, ", ", extraTiles.bottom);
		if (_lightingInFile)
		{
			GD.Print("Sunlight: ", IsLightOn);
		}
		else
		{
			GD.Print("Level file has no \"#light\" parameter, defaulting light to True");
		}
		matrix = new GeometryTile[levelSize.x, levelSize.y, 3];
		tilematrix = new TETile[levelSize.x, levelSize.y, 3];
		Globals.LevelEditImageShortcuts.Resize((int)levelSize.x * 16, (int)levelSize.y * 16);
		Globals.LevelEditImage1.Resize((int)levelSize.x * 16, (int)levelSize.y * 16);
		Globals.LevelEditImage2.Resize((int)levelSize.x * 16, (int)levelSize.y * 16);
		Globals.LevelEditImage3.Resize((int)levelSize.x * 16, (int)levelSize.y * 16);
		//Regex rx = new Regex(@"^");
		// regex is a miss lol
		// gotta find a way to parse the string lol
		// reference string [[[[1, []], [6, []], [0, [11]]], [[0, []], [0, []], [0, [11]]], [[0, [1]], [6, []], [0, [11]]]], [[[0, []], [0, []], [0, [11]]], [[0, []], [0, []], [0, [11]]], [[0, []], [0, []], [0, [11]]]], [[[0, [2]], [6, []], [0, [11]]], [[0, []], [0, []], [0, [11]]], [[0, [1, 2]], [6, []], [0, [11]]]]]
		// hand parsed reference string: [[[[int, [<int array>]], [int, [<int array>]], [int, [<int array>]]], [[int, [<int array>]], [int, [<int array>]], [int, [<int array>]]], [[int, [<int array>]], [int, [<int array>]], [int, [<int array>]]]], [[[int, [<int array>]], [int, [<int array>]], [int, [<int array>]]], [[int, [<int array>]], [int, [<int array>]], [int, [<int array>]]], [[int, [<int array>]], [0, []], [0, [11]]]], [[[0, [2]], [6, []], [0, [11]]], [[0, []], [0, []], [0, [11]]], [[0, [1, 2]], [6, []], [0, [11]]]]]
		// not so shrimple now
		// it's actually quite clamplicated....
		//JSON JSON JSON JSON JSON IT'S IN JSON HOLY SHIT
		// I DON't EVEN NEED TO JSON HAAAAHAHAHHAHAHHAA
		// I've ALREADY GOT THE OTHERS I JUST NEED TO SPLIT HTE SUBRSTRICNGS!!!!!!!!
		// MAD WITH POEWRER!!!!!!!!!!!!!!!!!
		// GeometryTile[,,] ugh = Json.Decode;
		//LOOKS LIKE REGEX WILL HAVE A USE HAHAHAHHAA
		//SPECIAL THANKS TO regexr.com FOR HELPING ME UNDERSTAND THE CRUEL NATURE OF REGEX
		Regex L1Regex = new Regex("\\[[0-9]+,\\s\\[([0-9]*,?\\s*[0-9]*)*\\]{1,2}", RegexOptions.IgnoreCase); //the holy grail, at last! used for the [int, [<int array>]]
		Regex Tile1Regex = new Regex("\\[[0-9]+,\\s\\[", RegexOptions.IgnoreCase); // used for the first digit(s) in the [1, [1, 2, 3]]
		Regex Tile2Regex = new Regex("\\[([0-9]*,*\\s?)*\\]", RegexOptions.IgnoreCase); // used for the [1, 2, 3] in the [1, [1, 2, 3]]
		Regex Tiles2Regex = new Regex("[0-9]+", RegexOptions.IgnoreCase); // used for the numbers in the [1, 2, 3]
		Regex Tiles1IDRegex = new Regex("[0-9]+", RegexOptions.IgnoreCase); // used for the numbers before the [1, 2, 3]
		//wait...
		//List<string> Line1List;
		/*for (int Length = 0; Length < LevelFile.Line1.Length; Length++)
		{

			//split the string into substrings and get the thingies from them


		}*/
		int Z = 1;
		int X = 1;
		int Y = 1;
		int i = 0;
		MatchCollection L1Matches = L1Regex.Matches(LevelFile.Line1);
		GeometryTile[] _matrixLine = new GeometryTile[L1Matches.Count];
		GD.Print(L1Matches.Count, " Total cells in matrix!");
		foreach(Match match in L1Matches) // each -> [1, [1, 2, 3]] in the input string
		{
			Match T1Match = Tile1Regex.Match(match.Value);
			Match T1IDMatch = Tiles1IDRegex.Match(T1Match.Value);
			GeometryTile _tile = new GeometryTile();
			string _t1string = T1IDMatch.Value;
			int _t1int = int.Parse(_t1string); //int.Parse only likes it when there's ONLY an int?? what the fuck??
			_tile.TileID = (Tiles)_t1int;
			Match T2Match = Tile2Regex.Match(match.Value);
			List<Tiles2> tiles2s = new List<Tiles2>();
			MatchCollection T2Matches = Tiles2Regex.Matches(T2Match.Value);
			foreach(Match match2 in T2Matches)
			{
				string _t2string = match2.Value;
				int _t2int = int.Parse(_t2string);
				Tiles2 tile2 = (Tiles2)_t2int;
				tiles2s.Add(tile2);
			}
			_tile.Tile2IDS = tiles2s;

			//_matrixLine = new GeometryTile[match.Index + 1];
			//GD.Print(match.Index); //ok now i figured out why the array doesn't like it :P
			//GD.Print(i + 1); //i + 1 because it's confusing seeing something that's 72 x 43 x 3 show up as 9287 instead of 9288
			//_matrixLine[i] = _tile;
			i++;
			matrix[X - 1, Y - 1, Z - 1] = _tile; // will combust into flames if x, y, or z is larger than the array's internal xyz!


			Z++;
			if (Z > 3)
			{
				Z = 1;
				Y++;
			}
			if (Y > levelSize.y) // temporary hardcode ecks dee // dehardcoded(?)
			{
				Y = 1;
				X++;
			}
		}
		//GD.Print(matrix.Length);

		//Regex TETileMatrixRegex = new Regex("#tlmatrix:\\s\\[(\\[.*\\])*\\]", RegexOptions.IgnoreCase);
		//Regex TETileCellsRegex = new Regex("\\[#tp:\\s\"[a-z]*\",\\s#Data:\\s([0-9]|\"([^\"]*)\"|\\[[^\\]]*\\])", RegexOptions.IgnoreCase);
		//Regex TETileTypeRegex = new Regex("#tp:\\s\"([^\"]+)\"", RegexOptions.IgnoreCase);
		//Regex TETileDataRegexMain = new Regex("#Data:\\s[^\\]]*", RegexOptions.IgnoreCase);
		//Regex TETileDataRegexTileBody = new Regex("point\\(([0-9]+),\\s([0-9]+)\\),\\s([0-9]+)", RegexOptions.IgnoreCase); // here is where i realized the importance of regex groups xd
		//Regex TETileDataRegexMaterial = new Regex("#Data:\\s\"([^\"]+)\"", RegexOptions.IgnoreCase);
		//Regex TETileDataRegexTileHead = new Regex("point\\([0-9]+,\\s[0-9]+\\),\\s\"([^\"]*)\"", RegexOptions.IgnoreCase);
		Regex TEMatrixOmniRegex = new Regex("\\[#tp:\\s\"([^\"]*)\",\\s#Data:\\s(0|\"([^\"]*)\"|\\[point\\(([0-9]+),\\s([0-9]+)\\),\\s(\"([^\"]*)\"|([0-9]+)))\\]", RegexOptions.IgnoreCase);
		//wooh that's a spicy re-a-gex a!
		int TZ = 1;
		int TX = 1;
		int TY = 1;
		//int Ti = 0;
		MatchCollection TEMatches = TEMatrixOmniRegex.Matches(LevelFile.Line2);
		//GD.Print(LevelFile.Line2);
		foreach(Match tematch in TEMatches)
		{
			TETile _tile = new TETile();
			//groups are zero based, so the first is 0 second is 1
			string tp = tematch.Groups[1].Value;
			tp = char.ToUpper(tp[0]) + tp.Substring(1);
			_tile.Type = Enum.Parse<TETileType>(tp);
			if (tematch.Groups[2].Value == "0")
			{
				_tile.Data.LayerOrDefault = 0;
			}
			else
			{
				if (tematch.Groups[3].Success && !tematch.Groups[4].Success)
				{
					_tile.Data.MaterialOrTileName = tematch.Groups[3].Value;
				}
				else
				{
					if (tematch.Groups[8].Success)
					{
						int i1 = int.Parse(tematch.Groups[4].Value);
						int i2 = int.Parse(tematch.Groups[5].Value);
						int i3 = int.Parse(tematch.Groups[8].Value);
						_tile.Data.TileIndexOrHeadCoords = new Vector2i(i1, i2);
						_tile.Data.LayerOrDefault = i3;
					}
					if (tematch.Groups[7].Success)
					{
						int i1 = int.Parse(tematch.Groups[4].Value);
						int i2 = int.Parse(tematch.Groups[5].Value);
						_tile.Data.TileIndexOrHeadCoords = new Vector2i(i1, i2);
						_tile.Data.MaterialOrTileName = tematch.Groups[7].Value;
					}



				}



			}


			//GD.Print(TX, "< X ",TY, "< Y ", TZ, "< Z ",_tile.Type, "< Type ", _tile.Data.MaterialOrTileName, "< matort ", _tile.Data.TileIndexOrHeadCoords.ToString(), "< indcords ", _tile.Data.LayerOrDefault, "< laydef");

			tilematrix[TX - 1, TY - 1, TZ - 1] = _tile;

			TZ++;
			if (TZ > 3)
			{
				TZ = 1;
				TY++;
			}
			if (TY > levelSize.y) // temporary hardcode ecks dee // dehardcoded(?)
			{
				TY = 1;
				TX++;
			}
		}
		//todo: do the stuff
		//todo: remove todo: do the stuff, because the stuff has been do





	}



	public static void Export(string Filepath)
	{
		LevelFileBuffer NewLevel = new LevelFileBuffer();
		string l1;
		List<string> columns = new List<string>();
		for (int q = 0; q < levelSize.x; q++)
		{
			string col;
			List<string> rows = new List<string>();
			for (int c = 0; c < levelSize.y; c++)
			{
				string row;
				List<string> cells = new List<string>();
				for (int l = 0; l < 3; l++)
				{
					string cell;
					string stackables;
					if (matrix[q, c, l].Tile2IDS.Count != 0)
					{
						stackables = string.Join(", ", matrix[q, c, l].Tile2IDS.Cast<int>().ToArray());
					}
					else stackables = null;
					cell = string.Format("[{0}, [{1}]]", Convert.ToInt32(matrix[q, c, l].TileID), stackables);
					cells.Add(cell);
				}
				row = "[" + string.Join(", ", cells) + "]";
				rows.Add(row);
			}
			col = "[" + string.Join(", ", rows) + "]";
			columns.Add(col);
		}
		l1 = "[" + string.Join(", ", columns) + "]";
		NewLevel.Line1 = l1;
		string l2m;
		List<string> tcols = new List<string>();
		for (int q = 0; q < levelSize.x; q++)
		{
			string col;
			List<string> rows = new List<string>();
			for (int c = 0; c < levelSize.y; c++)
			{
				string row;
				List<string> cells = new List<string>();
				for (int l = 0; l < 3; l++)
				{
					string cell;
					string type = Convert.ToString(tilematrix[q,c,l].Type);
					type = char.ToLower(type[0]) + type.Substring(1);
					string name;
					string pos;
					string layer;
					string data;
					//string datasubname;
					if (tilematrix[q, c, l].Type == TETileType.Default)
					{
						data = "0";
						cell = String.Format("[#tp: \"{0}\", #Data: {1}]", type, data);
						cells.Add(cell);
					}
					else
					{
						switch (tilematrix[q, c, l].Type)
						{
							case TETileType.Material:
								name = tilematrix[q, c, l].Data.MaterialOrTileName;
								cell = String.Format("[#tp: \"{0}\", #Data: \"{1}\"]", type, name);
								cells.Add(cell);
								break;
							case TETileType.TileHead:
								int cat = tilematrix[q, c, l].Data.TileIndexOrHeadCoords.x;
								int ind = tilematrix[q, c, l].Data.TileIndexOrHeadCoords.y;
								pos = String.Format("{0}, {1}", cat, ind);
								name = tilematrix[q, c, l].Data.MaterialOrTileName;
								cell = String.Format("[#tp: \"{0}\", #Data: [point({1}), \"{2}\"]]", type, pos, name);
								cells.Add(cell);
								break;
							case TETileType.TileBody:
								int x = tilematrix[q, c, l].Data.TileIndexOrHeadCoords.x;
								int y = tilematrix[q, c, l].Data.TileIndexOrHeadCoords.y;
								int z = tilematrix[q, c, l].Data.LayerOrDefault;
								pos = String.Format("{0}, {1}", x, y);
								layer = z.ToString();
								cell = String.Format("[#tp: \"{0}\", #Data: [point({1}), {2}]]", type, pos, layer);
								cells.Add(cell);
								break;
							default:
								break;
						}
					}
				}
				row = "[" + string.Join(", ", cells) + "]";
				rows.Add(row);
				//GD.Print(row);
			}
			col = "[" + string.Join(", ", rows) + "]";
			tcols.Add(col);
			//GD.Print(col);
		}
		l2m = "#tlMatrix: [" + String.Join(", ", tcols) + "]";
		string l2tmsav = String.Join(", ", TMSavePositionLookup);
		string l2 = String.Format("[#lastKeys: [], #Keys: [], #workLayer: 1, #lstMsPs: point(0, 0), {0}, #defaultMaterial: \"Concrete\", #toolType: \"material\", #toolData: \"Big Metal\", #tmPos: point(1, 1), #tmSavPosL: [{1}], #specialEdit: 0]", l2m, l2tmsav);
		NewLevel.Line2 = l2;
		NewLevel.Line3 = LevelFile.Line3;
		NewLevel.Line4 = LevelFile.Line4;
		NewLevel.Line5 = LevelFile.Line5;
		NewLevel.Line6 = LevelFile.Line6;
		NewLevel.Line7 = LevelFile.Line7;
		NewLevel.Line8 = LevelFile.Line8;
		NewLevel.Line9 = LevelFile.Line9;

		List<string> lines = new List<string>();
		lines.Add(NewLevel.Line1);
		lines.Add(NewLevel.Line2);
		lines.Add(NewLevel.Line3);
		lines.Add(NewLevel.Line4);
		lines.Add(NewLevel.Line5);
		lines.Add(NewLevel.Line6);
		lines.Add(NewLevel.Line7);
		lines.Add(NewLevel.Line8);
		lines.Add(NewLevel.Line9);
		//lines.Add(new string(" "));
		//GD.Print(lines.Count);
		//if (!System.IO.File.Exists(Filepath))
		//{
		//	System.IO.File.WriteAllLines(Filepath, lines);
		//}
		System.IO.File.WriteAllLines(Filepath, lines);
	}

}


public partial class LEditor : Node
{
	// Called when the node enters the scene tree for the first time.
	public FileDialog filediag;
	public MenuButton settings;


	public override void _Ready()
	{
		//GetViewport().GetChild<Window>(1).Theme = GD.Load<Theme>("res://godotScenes/LECore/UI/LETheme.tres");
		//GetNode<Window>("/root").Theme = GD.Load<Theme>("res://godotScenes/LECore/UI/LETheme.tres");
		settings = (MenuButton)GetTree().Root.FindChild("SettingsMenu", true, false);

		Callable noclipToggle = new Callable(this, nameof(ToggleSetting));
		PopupMenu pop = settings.GetPopup();
		pop.Connect("index_pressed", noclipToggle);


		Texture2D levelshortex = (Texture2D)GD.Load("res://Assets/QuickImport/png_from_bmp_files/Internal_039_levelEditImageShortCuts.png");
		Texture2D leveled1tex = (Texture2D)GD.Load("res://Assets/QuickImport/png_from_bmp_files/Internal_040_levelEditImage1.png");
		Texture2D leveled2Tex = (Texture2D)GD.Load("res://Assets/QuickImport/png_from_bmp_files/Internal_041_levelEditImage2.png");
		Texture2D leveled3Tex = (Texture2D)GD.Load("res://Assets/QuickImport/png_from_bmp_files/Internal_042_levelEditImage3.png");

		//File
		//Globals.NewLevelFile.Line1 = ResourceLoader.Load







		Globals.LevelEditImageShortcuts = levelshortex.GetImage();
		//GD.Print("Format sh: ", Globals.LevelEditImageShortcuts.GetFormat());
		Globals.LevelEditImage1 = leveled1tex.GetImage();
	   // GD.Print("Format 1: ", Globals.LevelEditImage1.GetFormat());
		Globals.LevelEditImage2 = leveled2Tex.GetImage();
	   // GD.Print("Format 2: ", Globals.LevelEditImage2.GetFormat());
		Globals.LevelEditImage3 = leveled3Tex.GetImage();
	   // GD.Print("Format 3: ", Globals.LevelEditImage3.GetFormat());
		Globals.levelSize = new LevelSize(72, 43);
		Globals.matrix = new GeometryTile[Globals.levelSize.x, Globals.levelSize.y, 3];
		GD.Print("Level Size: ", Globals.levelSize.x, ", ", Globals.levelSize.y);
		GD.Print("Level Buffer Tiles: ", Globals.extraTiles.left, ", ", Globals.extraTiles.top, ", ", Globals.extraTiles.right, ", ", Globals.extraTiles.bottom);
	   // Globals.matrix = new GeometryTile[Globals.levelSize.x, Globals.levelSize.y, 3];
		Globals.LevelEditImageShortcuts.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.LevelEditImage1.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.LevelEditImage2.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.LevelEditImage3.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);

		Texture2D teimg1 = (Texture2D)GD.Load("res://Assets/QuickImport/png_from_bmp_files/levelEditor_262484_TEimg1.png");
		Texture2D teimg2 = (Texture2D)GD.Load("res://Assets/QuickImport/png_from_bmp_files/levelEditor_262485_TEimg2.png");
		Texture2D teimg3 = (Texture2D)GD.Load("res://Assets/QuickImport/png_from_bmp_files/levelEditor_262486_TEimg3.png");

		Globals.TEImage1 = teimg1.GetImage();
		Globals.TEImage2 = teimg2.GetImage();
		Globals.TEImage3 = teimg3.GetImage();

		Globals.TEImage1.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.TEImage2.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.TEImage3.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);


		//MatrixMaster matmas = (MatrixMaster)GetNode("Matrix Master");
		//matmas.ReloadMatrix();
		//matmas.UpdateDisplay();
		//GD.Print(Globals.matrix.Length);
		//GD.Print(Globals.levelSize.x," ", Globals.levelSize.y);
		int x = 0;
		int y = 0;
		int z = 0;
		foreach (GeometryTile tile in Globals.matrix)
		{
			if (z < 2)
			{
				Globals.matrix[x, y, z].TileID = (Tiles)1;
			}
			z++;
			if (z + 1 > 3)
			{
				z = 0;
				y++;
			}
			if (y + 1 > Globals.levelSize.y)
			{
				y = 0;
				x++;
			}
		}
		//matmas.ReloadMatrix();
		//matmas.UpdateDisplay();
		//matmas.ResetMatrix();
		Globals.Init();
		string path1 = OS.GetExecutablePath().GetBaseDir();
		string FilePath = path1 + "/LevelEditorProjects/Level_Editor_Core/New Project.txt";
		LevelFileSelected(FilePath);
	}

	public void OpenLoadFileDialog()
	{
		Globals.PrevState = Globals.State;
		Globals.State = EditorState.LoadSaveIO;
		GD.Print("Opening load file dialog!");
		if (filediag == null)
		{
			filediag = new FileDialog();
			AddChild(filediag);
		}
		//Vector2i windowsize = GetViewport().GetChild<Window>(1).Size;
		string path1 = OS.GetExecutablePath().GetBaseDir();
		//path1 = Path.GetDirectoryName(path1);
		//GD.Print("Path obtained!");
		string FilePath = path1 + "/LevelEditorProjects";
		GD.Print(FilePath);
		filediag.FileMode = FileDialog.FileModeEnum.OpenFile;
		filediag.Mode = FileDialog.ModeEnum.Maximized;
		filediag.ContentScaleMode = FileDialog.ContentScaleModeEnum.CanvasItems;
		filediag.ContentScaleAspect = FileDialog.ContentScaleAspectEnum.Expand;
		filediag.Unresizable = false;
		filediag.Access = FileDialog.AccessEnum.Filesystem;
		filediag.CurrentScreen = 0;
		filediag.Size = new Vector2i(1920, 1080);
		filediag.AlwaysOnTop = true;
		filediag.Transient = true;
		filediag.AddFilter("*.txt");
		filediag.Exclusive = true;
		filediag.Theme = GD.Load<Theme>("res://godotScenes/LECore/UI/LETheme.tres");
		filediag.Invalidate();
		//filediag.RootSubfolder = "";
		filediag.CurrentDir = path1 + "/LevelEditorProjects/";
		//GD.Print(filediag.CurrentPath);
		//filediag.GetNode<Button>("@@44")._Pressed();
		filediag.Show();
		filediag.Invalidate();
		var LevelFile = filediag;
		Callable InSignal = new Callable(this, nameof(LevelFileSelected));
		Callable Killbox = new Callable(this, nameof(LevelFileSelectQuit));
		LevelFile.Connect("file_selected", InSignal);
		LevelFile.Connect("cancelled", Killbox);

		//GD.Print("can draw = ", filediag.CanDraw());
	}
	public void LevelFileSelectQuit()
	{
		Globals.State = Globals.PrevState;
		filediag.QueueFree();
		filediag = null;
	}
	public void LevelFileSelected(string Filepath)
	{
		//I'd use System.IO, but Godot hates it for some reason?
		//Like, please? Let me use System.IO? It's much better!
		//HAHA UPDATE FIXED IT SYSTEM IO HERE I COME!!
		GD.Print(Filepath);

		int counter = 0;
		foreach (string line in System.IO.File.ReadLines(Filepath))
		{
			if (counter >= 10)
			{
				GD.Print("More than 10 Lines, ignoring!");
				break;
			}
			//GD.Print(counter + 1, " <- line number");
			counter++;
		}
		GD.Print(counter, " <- total lines");
		string[] Lines = System.IO.File.ReadLines(Filepath).Take(10).ToArray();
		//int i = 0;
		//foreach (string line in Lines)
		//{
		//	GD.Print(i + 1," <-line_number ", line);
		//	i++;
		//}

		Globals.LevelFile.Line1 = Lines[0];
		Globals.LevelFile.Line2 = Lines[1];
		Globals.LevelFile.Line3 = Lines[2];
		Globals.LevelFile.Line4 = Lines[3];
		Globals.LevelFile.Line5 = Lines[4];
		Globals.LevelFile.Line6 = Lines[5];
		Globals.LevelFile.Line7 = Lines[6];
		Globals.LevelFile.Line8 = Lines[7];
		Globals.LevelFile.Line9 = Lines[8];
		GD.Print("Level file loaded!");
		
		Globals.Import(Filepath);

		Globals.State = EditorState.Main;
		if (filediag != null)
		{
			filediag.QueueFree();
			filediag = null;
		}

		

		/*var file = new Godot.File();
		file.Open(Filepath, Godot.File.ModeFlags.Read);
		if (file.IsOpen())
		{
			GD.Print(file);
		}
		do
		{
			//placeholder stuff to make sure it's working
			string line = file.GetLine();
			GD.Print(line);
		} while (file.GetPosition() > file.GetLength());
		file.Close();
		*/
		
	}
	public void SaveToFile()
	{
		string path1 = OS.GetExecutablePath().GetBaseDir();
		string InitPath = path1 + "/LevelEditorProjects/Level_Editor_Core/New Project.txt";
		if (Globals.gLOADPATH != InitPath)
		{
			string Filepath = Globals.gLOADPATH;
			Globals.Export(Filepath);
		}
		else
		{
			OpenSaveFileDialog();
		}
	}
	public void OpenSaveFileDialog()
	{
		Globals.PrevState = Globals.State;
		Globals.State = EditorState.LoadSaveIO;
		GD.Print("Opening save file dialog!");
		if (filediag == null)
		{
			filediag = new FileDialog();
			AddChild(filediag);
		}
		//Vector2i windowsize = GetViewport().GetChild<Window>(1).Size;
		string path1 = OS.GetExecutablePath().GetBaseDir();
		//path1 = Path.GetDirectoryName(path1);
		//GD.Print("Path obtained!");
		string FilePath = path1 + "/LevelEditorProjects";
		GD.Print(FilePath);
		filediag.FileMode = FileDialog.FileModeEnum.SaveFile;
		filediag.Mode = FileDialog.ModeEnum.Maximized;
		filediag.ContentScaleMode = FileDialog.ContentScaleModeEnum.CanvasItems;
		filediag.ContentScaleAspect = FileDialog.ContentScaleAspectEnum.Expand;
		filediag.Unresizable = false;
		filediag.Access = FileDialog.AccessEnum.Filesystem;
		filediag.CurrentScreen = 0;
		filediag.Size = new Vector2i(1920, 1080);
		filediag.AlwaysOnTop = true;
		filediag.Transient = true;
		filediag.AddFilter("*.txt");
		filediag.Exclusive = true;
		filediag.Theme = GD.Load<Theme>("res://godotScenes/LECore/UI/LETheme.tres");
		filediag.Invalidate();
		//filediag.RootSubfolder = "";
		filediag.CurrentDir = path1 + "/LevelEditorProjects/";
		//GD.Print(filediag.CurrentPath);
		//filediag.GetNode<Button>("@@44")._Pressed();
		filediag.Show();
		filediag.Invalidate();
		var LevelFile = filediag;
		Callable OutSignal = new Callable(this, nameof(SaveFileSelected));
		Callable Killbox = new Callable(this, nameof(LevelFileSelectQuit));
		LevelFile.Connect("file_selected", OutSignal);
		LevelFile.Connect("cancelled", Killbox);

		//GD.Print("can draw = ", filediag.CanDraw());
	}

	public void SaveFileSelected(string Filepath)
	{
		string path1 = OS.GetExecutablePath().GetBaseDir();
		string InitPath = path1 + "/LevelEditorProjects/Level_Editor_Core/New Project.txt";
		if (Filepath == InitPath)
		{
			GD.Print("Saving over that will break stuff!, Better for you to swap it manually! (Make backups!)");
		}
		else
		{
			//GD.Print(Filepath, " ok now do the saving");
			Globals.Export(Filepath);
		}
		//GD.Print(Filepath, " ok now do the saving");
		Globals.State = Globals.PrevState;
		filediag.QueueFree();
		filediag = null;
	}




	public void ToggleSetting(int input)
	{
		//sv_cheats 1
		//noclip
		if (input == 0)
		{
			GD.Print("Noclip set to " + !Globals.Settings.IsFreecam + "!");
			Globals.Settings.IsFreecam = !Globals.Settings.IsFreecam;
		}
		if (input == 1)
		{
			GD.Print("Compass Visibility = " + !Globals.Settings.ShowCompass);
			Globals.Settings.ShowCompass = !Globals.Settings.ShowCompass;
		}
		if (input == 2)
		{
			GD.Print("Evil Mode set to " + !Globals.IsEditorEvil);
			Globals.IsEditorEvil = !Globals.IsEditorEvil;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

}

//GENERAL THINGIES -==============================================================================================-
/*
	!!!!!! important!!!!!!!!!



		adobe director ink colors
			(the #Ink:<int> variables)

				(i have no idea what these do)
							0Copy—					32Blend—
							1Transparent—			33Add pin—
							2Reverse—				34Add—
							3Ghost—					35Subtract pin—
							4Not copy—				36Background transparent —
							5Not transparent—		37Lightest—
							6Not reverse—			38Subtract—
							7Not ghost—				39Darkest—
							8Matte—					40Lighten—
							9Mask—					41Darken—
			(36 appears to use a background color for transparency)
			(that won't be too hard to implement xd)
			(39 appears to compare two pixels then pick the darkest one(?))
			(oh god, i'm going to have to implement these)
			(they shouldn't be TOO hard to implement, every image editor worth it's salt has them xd)
			4 channels, comparing each pixel, sometimes the pixels in the MILLIONS, oh fuck
			lol i am waayyyy over my head in this

			the lingo code appears to mostly use 36 and 39 (mostly 36)
			(one instantse of "10" appears in RenderEffects at line 3351) <- what does 10 do??? the list is missing 10 - 31
*/
