using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using static Godot.RenderingDevice;

namespace RainWorldMonsoon;

public partial class MatrixMaster : Node
{
	public bool LayerIncompatStackables(Tiles2 tile)
	{
		if (tile == (Tiles2)1 | tile == (Tiles2)2 | tile == (Tiles2)11)
		{
			return true;
		}
		else return false;
	}


	public bool RectLock = false;
	public Vector2i BeginRect;
	public Vector2i LastPos;
	public LERect AffectRect;
	public LERect MirrorAffectRect;
	public Vector2i LastCursorPosition;


	// so many sprite2Ds...
	private GeometryTile[,,] ReferenceMatrix;
	private Node GeoMatrixMaster;
	private Node GeoEdMatrixApprentice;
	private Node TileEdMatrixApprentice;
	private Node CameraEdMatrixApprentice;
	private Node LightEdMatrixApprentice;
	private Node EffectsEdMatrixApprentice;
	private Node PropEdMatrixApprentice;
	private Node EnviroEdMatrixApprentice;
	private LECamera2D Camera;
	private Sprite2D levedshtext;
	private Sprite2D leved1text;
	private Sprite2D leved2text;
	private Sprite2D leved3text;

	private Sprite2D geoedshtext;
	private Sprite2D geoed1text;
	private Sprite2D geoed2text;
	private Sprite2D geoed3text;
	private Sprite2D geoedrecttext;
	private Sprite2D geoeduitext;
	private Sprite2D geoedmirrortext;

	private Sprite2D tileedshtext;
	private Sprite2D tileed1text;
	private Sprite2D tileed2text;
	private Sprite2D tileed3text;

	private Sprite2D cameraedshtext;
	private Sprite2D cameraed1text;
	private Sprite2D cameraed2text;
	private Sprite2D cameraed3text;

	private Sprite2D lightedshtext;
	private Sprite2D lighted1text;
	private Sprite2D lighted2text;
	private Sprite2D lighted3text;

	private Sprite2D fxedshtext;
	private Sprite2D fxed1text;
	private Sprite2D fxed2text;
	private Sprite2D fxed3text;

	private Sprite2D propedshtext;
	private Sprite2D proped1text;
	private Sprite2D proped2text;
	private Sprite2D proped3text;

	private Sprite2D envedshtext;
	private Sprite2D enved1text;
	private Sprite2D enved2text;
	private Sprite2D enved3text;

	//deprecated stupid stuff that isn't even used anymore
	//it made so many instances
	//slow machine inator 90000
	//PackedScene Air = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/Air.tscn");
	//PackedScene Wall = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/Wall.tscn");
	//PackedScene SlopeSW = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/SlopeSW.tscn");
	//PackedScene SlopeSE = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/SlopeSE.tscn");
	// PackedScene SlopeNW = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/SlopeNW.tscn");
	// PackedScene SlopeNE = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/SlopeNE.tscn");
	// PackedScene Floor = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/Floor.tscn");

	//PackedScene HorizBar = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/HorizBar.tscn");
	//PackedScene VertiBar = GD.Load<PackedScene>("res://godotScenes/LECore/Geo/VertiBar.tscn");
	ImageTexture whiterect;
	Image WhiteRectV;
	Image WhiteRectH;
   // Image WhiteRectR;
	//Image WhiteRectB;
	Image WhiteRect;
	ImageTexture redrect;
	Image RedRectV;
	Image RedRectH;
	//Image RedRectR;
	//Image RedRectB;
	Image RedRect;
	ImageTexture purplerect;
	Image PurpleRectV;
	Image PurpleRectH;
	//Image PurpleRectR;
	//Image PurpleRectB;
	Image PurpleRect;


	Image Wall;
	Image Slope1;
	Image Slope2;
	Image Slope3;
	Image Slope4;
	Image Floor;

	Image HBar;
	Image VBar;
	Image BatflyNest;
	Image FakeTransparent;
	Image ShortcutUnconnected;
	Image ShortcutArrowLeft;
	Image ShortcutArrowUp;
	Image ShortcutArrowRight;
	Image ShortcutArrowDown;
	Image ShortcutDot;
	Image RoomEntrance;
	Image EnemyDen;
	Image Rock;
	Image Spear;
	Image Cracks;
	Image VertiCracks;
	Image HorizCracks;
	Image NullCracks;
	Image ForbidBatflyChains;
	Image GarbageWormDen;
	Image Waterfall;
	Image WhackAMoleHole;
	Image WormGrass;
	Image ScavengerTeleporterHole;

	public override void _Ready()
	{
		//ReferenceMatrix = Globals.matrix;
		GeoMatrixMaster = GetTree().Root.FindChild("Matrix Display Master 2D", true, false);
		GeoEdMatrixApprentice = GetTree().Root.FindChild("Matrix Display Apprentice Geo", true, false);
        TileEdMatrixApprentice = GetTree().Root.FindChild("Matrix Display Apprentice Tile", true, false);
        Camera = (LECamera2D)GetTree().Root.FindChild("Camera2D", true, false);
		//GD.Print(GeoMatrixMaster);
		//Texture2D walltex = (Texture2D)GD.Load("res://Core/EasyWallID1.png");

		/* base copy paste
		edshtext = (Sprite2D)EdMatrixApprentice.FindChild("EditImageShortcuts", true);
		ed1text = (Sprite2D)EdMatrixApprentice.FindChild("EditImage1", true);
		ed2text = (Sprite2D)EdMatrixApprentice.FindChild("EditImage2", true);
		ed3text = (Sprite2D)EdMatrixApprentice.FindChild("EditImage3", true);
		*/
		levedshtext = (Sprite2D)GeoMatrixMaster.FindChild("LevelEditImageShortcuts", true);
		leved1text = (Sprite2D)GeoMatrixMaster.FindChild("LevelEditImage1", true);
		leved2text = (Sprite2D)GeoMatrixMaster.FindChild("LevelEditImage2", true);
		leved3text = (Sprite2D)GeoMatrixMaster.FindChild("LevelEditImage3", true);

		geoedshtext = (Sprite2D)GeoEdMatrixApprentice.FindChild("GeoEditImageShortcuts", true);
		geoed1text = (Sprite2D)GeoEdMatrixApprentice.FindChild("GeoEditImage1", true);
		geoed2text = (Sprite2D)GeoEdMatrixApprentice.FindChild("GeoEditImage2", true);
		geoed3text = (Sprite2D)GeoEdMatrixApprentice.FindChild("GeoEditImage3", true);
		geoedrecttext = (Sprite2D)GeoEdMatrixApprentice.FindChild("BoundsRect", true);
		geoeduitext = (Sprite2D)GeoEdMatrixApprentice.FindChild("UIRect", true);
        geoedmirrortext = (Sprite2D)GeoEdMatrixApprentice.FindChild("MirrorRect", true);

        tileedshtext = (Sprite2D)TileEdMatrixApprentice.FindChild("TileEditImageShortcuts", true);
        tileed1text = (Sprite2D)TileEdMatrixApprentice.FindChild("TileEditImage1", true);
        tileed2text = (Sprite2D)TileEdMatrixApprentice.FindChild("TileEditImage2", true);
        tileed3text = (Sprite2D)TileEdMatrixApprentice.FindChild("TileEditImage3", true);






        WhiteRectV = Utilities.QuickConvert("res://Core/Rect_L.png");
		WhiteRectV = Utilities.ColorConvert(WhiteRectV, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
		WhiteRectH = Utilities.QuickConvert("res://Core/Rect_T.png");
		WhiteRectH = Utilities.ColorConvert(WhiteRectH, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
	   // WhiteRectR = Utilities.QuickConvert("res://Core/Rect_R.png");
		//WhiteRectR = Utilities.ColorConvert(WhiteRectT, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
		//WhiteRectB = Utilities.QuickConvert("res://Core/Rect_B.png");
	   // WhiteRectB = Utilities.ColorConvert(WhiteRectB, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		RedRectV = Utilities.QuickConvert("res://Core/Rect_L.png");
		//WhiteRectV = Utilities.ColorConvert(WhiteRectV, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
		RedRectH = Utilities.QuickConvert("res://Core/Rect_T.png");
		//WhiteRectH = Utilities.ColorConvert(WhiteRectH, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
		//RedRectR = Utilities.QuickConvert("res://Core/Rect_R.png");
		//RedRectB = Utilities.QuickConvert("res://Core/Rect_B.png");

		PurpleRectV = Utilities.QuickConvert("res://Core/Rect_L.png");
		PurpleRectV = Utilities.ColorConvert(PurpleRectV, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
		PurpleRectH = Utilities.QuickConvert("res://Core/Rect_T.png");
		PurpleRectH = Utilities.ColorConvert(PurpleRectH, Color.Color8(255, 0, 0, 255), Color.Color8(255, 0, 255, 255));
		//PurpleRectR = Utilities.QuickConvert("res://Core/Rect_L.png");
		//PurpleRectR = Utilities.ColorConvert(PurpleRectR, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
		//PurpleRectB = Utilities.QuickConvert("res://Core/Rect_T.png");
		//PurpleRectB = Utilities.ColorConvert(PurpleRectB, Color.Color8(255, 0, 0, 255), Color.Color8(255, 0, 255, 255));

		//primary tile textures
		Texture2D walltex = (Texture2D)GD.Load("res://Core/EasyWallID1.png");
		Texture2D slope1tex = (Texture2D)GD.Load("res://Core/EasySlopeID2.png");
		Texture2D slope2tex = (Texture2D)GD.Load("res://Core/EasySlopeID3.png");
		Texture2D slope3tex = (Texture2D)GD.Load("res://Core/EasySlopeID4.png");
		Texture2D slope4tex = (Texture2D)GD.Load("res://Core/EasySlopeID5.png");
		Texture2D floortex = (Texture2D)GD.Load("res://Core/EasyFloorID5.png");
		Wall = walltex.GetImage();
		Wall.Convert(Image.Format.Rgba8);
		//GD.Print("Wall format: ", Wall.GetFormat());
		Slope1 = slope1tex.GetImage();
		Slope1.Convert(Image.Format.Rgba8);
		// GD.Print("Slope1 format: ", Wall.GetFormat());
		Slope2 = slope2tex.GetImage();
		Slope2.Convert(Image.Format.Rgba8);
		// GD.Print("Slope2 format: ", Wall.GetFormat());
		Slope3 = slope3tex.GetImage();
		Slope3.Convert(Image.Format.Rgba8);
		// GD.Print("Slope3 format: ", Wall.GetFormat());
		Slope4 = slope4tex.GetImage();
		Slope4.Convert(Image.Format.Rgba8);
		// GD.Print("Slope4 format: ", Wall.GetFormat());
		Floor = floortex.GetImage();
		Floor.Convert(Image.Format.Rgba8);
		//GD.Print("Floor format: ", Wall.GetFormat());

		//secondary stackable tile textures
		Texture2D hbartex = (Texture2D)GD.Load("res://Core/EasyHorizontalBarID1.png");
		Texture2D vbartex = (Texture2D)GD.Load("res://Core/EasyVerticalBarID2.png");
		//Texture2D walltex = (Texture2D)GD.Load("res://Core/EasyWallID1.png");
		//Texture2D walltex = (Texture2D)GD.Load("res://Core/EasyWallID1.png");
		//Texture2D walltex = (Texture2D)GD.Load("res://Core/EasyWallID1.png");

		HBar = hbartex.GetImage();
		HBar.Convert(Image.Format.Rgba8);

		VBar = vbartex.GetImage();
		VBar.Convert(Image.Format.Rgba8);

		// easy copy paste res://Assets/QuickImport/png_from_bmp_files/ 

		BatflyNest = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_062_hiveGrass.png");
		BatflyNest = Utilities.ColorConvert(BatflyNest, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		BatflyNest = Utilities.ColorConvert(BatflyNest, Color.Color8(0, 0, 0, 255), Color.Color8(100, 100, 100, 255));

		FakeTransparent = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_219_semiTransperent.png");
		FakeTransparent = Utilities.ColorConvert(FakeTransparent, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));

		ShortcutUnconnected = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_141_shortCutArrow0.0.png");
		ShortcutUnconnected = Utilities.ColorConvert(ShortcutUnconnected, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		ShortcutUnconnected = Utilities.ColorConvert(ShortcutUnconnected, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		//11 x 6 -> 11 x 8/9 gonna go with 9 cause it's pointy-er
		// -1,0 LEFT
		// 0, -1 UP
		// 1, 0 RIGHT
		//0, 1 DOWN
		ShortcutArrowLeft = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_137_shortCutArrow-1.0.png");
		ShortcutArrowLeft.Resize(9, 11, Image.Interpolation.Nearest);
		ShortcutArrowLeft = Utilities.ColorConvert(ShortcutArrowLeft, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		ShortcutArrowLeft = Utilities.ColorConvert(ShortcutArrowLeft, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		ShortcutArrowUp = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_138_shortCutArrow0.-1.png");
		ShortcutArrowUp.Resize(11, 9, Image.Interpolation.Nearest);
		ShortcutArrowUp = Utilities.ColorConvert(ShortcutArrowUp, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		ShortcutArrowUp = Utilities.ColorConvert(ShortcutArrowUp, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		ShortcutArrowRight = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_139_shortCutArrow1.0.png");
		ShortcutArrowRight.Resize(9, 11, Image.Interpolation.Nearest);
		ShortcutArrowRight = Utilities.ColorConvert(ShortcutArrowRight, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		ShortcutArrowRight = Utilities.ColorConvert(ShortcutArrowRight, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		ShortcutArrowDown = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_140_shortCutArrow0.1.png");
		ShortcutArrowDown.Resize(11, 9, Image.Interpolation.Nearest);
		ShortcutArrowDown = Utilities.ColorConvert(ShortcutArrowDown, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		ShortcutArrowDown = Utilities.ColorConvert(ShortcutArrowDown, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		ShortcutDot = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_026_pxl.png");
		ShortcutDot.Resize(3, 3, Image.Interpolation.Nearest);
		ShortcutDot = Utilities.ColorConvert(ShortcutDot, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		RoomEntrance = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_195_p.png");
		RoomEntrance.Resize(8, 10, Image.Interpolation.Nearest);
		RoomEntrance = Utilities.ColorConvert(RoomEntrance, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		RoomEntrance = Utilities.ColorConvert(RoomEntrance, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		EnemyDen = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_196_e.png");
		EnemyDen.Resize(6, 10, Image.Interpolation.Nearest);
		EnemyDen = Utilities.ColorConvert(EnemyDen, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		EnemyDen = Utilities.ColorConvert(EnemyDen, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		Rock = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_099_rockIcon.png");
		Rock.Resize(12, 8, Image.Interpolation.Nearest);
		Rock = Utilities.ColorConvert(Rock, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		Rock = Utilities.ColorConvert(Rock, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		Spear = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_098_spearIcon.png");
		//Spear.Resize(12, 8, Image.Interpolation.Nearest);
		Spear = Utilities.ColorConvert(Spear, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		Spear = Utilities.ColorConvert(Spear, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		ForbidBatflyChains = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_659_iconforbidbats.png");
		ForbidBatflyChains.Resize(16, 16, Image.Interpolation.Nearest);
		ForbidBatflyChains = Utilities.ColorConvert(ForbidBatflyChains, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		ForbidBatflyChains = Utilities.ColorConvert(ForbidBatflyChains, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		Cracks = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_219_semiTransperent.png");
		Cracks = Utilities.ColorConvert(Cracks, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		Cracks = Utilities.ColorConvert(Cracks, Color.Color8(0, 0, 0, 255), Color.Color8(164, 164, 164, 255));

		VertiCracks = Utilities.QuickConvert("res://Core/EasyCracksVertical.png");
		VertiCracks = Utilities.ColorConvert(VertiCracks, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		VertiCracks = Utilities.ColorConvert(VertiCracks, Color.Color8(0, 0, 0, 255), Color.Color8(164, 164, 164, 255));

		HorizCracks = Utilities.QuickConvert("res://Core/EasyCracksHorizontal.png");
		HorizCracks = Utilities.ColorConvert(HorizCracks, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		HorizCracks = Utilities.ColorConvert(HorizCracks, Color.Color8(0, 0, 0, 255), Color.Color8(164, 164, 164, 255));


		NullCracks = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_098_spearIcon.png");
		NullCracks = Utilities.ColorConvert(NullCracks, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		NullCracks = Utilities.ColorConvert(NullCracks, Color.Color8(0, 0, 0, 255), Color.Color8(56, 0, 0, 255));

		GarbageWormDen = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_197_g.png");
		GarbageWormDen.Resize(8, 10, Image.Interpolation.Nearest);
		GarbageWormDen = Utilities.ColorConvert(GarbageWormDen, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		GarbageWormDen = Utilities.ColorConvert(GarbageWormDen, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		Waterfall = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_590_waterfallW.png");
		Waterfall.Resize(11, 9, Image.Interpolation.Nearest);
		Waterfall = Utilities.ColorConvert(Waterfall, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		Waterfall = Utilities.ColorConvert(Waterfall, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		WhackAMoleHole = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_194_w.png");
		WhackAMoleHole.Resize(10, 10, Image.Interpolation.Nearest);
		WhackAMoleHole = Utilities.ColorConvert(WhackAMoleHole, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		WhackAMoleHole = Utilities.ColorConvert(WhackAMoleHole, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		WormGrass = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_680_iconWormGrass.png");
		WormGrass.Resize(16, 16, Image.Interpolation.Nearest);
		WormGrass = Utilities.ColorConvert(WormGrass, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		WormGrass = Utilities.ColorConvert(WormGrass, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));

		ScavengerTeleporterHole = Utilities.QuickConvert("res://Assets/QuickImport/png_from_bmp_files/Internal_198_s.png");
		ScavengerTeleporterHole.Resize(8, 10, Image.Interpolation.Nearest);
		ScavengerTeleporterHole = Utilities.ColorConvert(ScavengerTeleporterHole, Color.Color8(255, 255, 255, 255), Color.Color8(0, 0, 0, 0));
		ScavengerTeleporterHole = Utilities.ColorConvert(ScavengerTeleporterHole, Color.Color8(0, 0, 0, 255), Color.Color8(255, 255, 255, 255));


		// Image  = (Image)GD.Load("res://Core/EasyWallID1.png");
		// Image Wall = (Image)GD.Load("res://Core/EasyWallID1.png");
		// Image Wall = (Image)GD.Load("res://Core/EasyWallID1.png");

	}

	//public void DisplayGeoMatrix() 
	//{
	//	if (IsInGeometryEditor == true)
	//	{
	//		
	//		
	//	}
	//
	//
	//
	//
	//}
	public override void _PhysicsProcess(double delta)
	{
		//Vector2i lastCursorPos = new Vector2i(Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1);
		if(GeoMatrixMaster == null)
		{
			GeoMatrixMaster = GetTree().Root.FindChild("Matrix Display Master 2D", true, false);
			GD.Print(GeoMatrixMaster);
		}
		if (Globals.IsRectOn == true)
		{
			AffectRect = new LERect(new Vector2i (Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1), Globals.RectLockPos);
			if (AffectRect.top > AffectRect.bottom)
			{
				int sav = AffectRect.bottom;
				AffectRect.bottom = AffectRect.top;
				AffectRect.top = sav;
			}
			if (AffectRect.left > AffectRect.right)
			{
				int sav = AffectRect.right;
				AffectRect.right = AffectRect.left;
				AffectRect.left = sav;
			}
			if (LastCursorPosition != new Vector2i(Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1))
			{
                UpdateToolRect();
            }
			
		}
		if (Globals.IsMirrorOn == true)
		{
			UpdateMirrorRect();
		}

		LastCursorPosition = new Vector2i(Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1);
		CheckMatrix(); // replace with something faster


	}
	public void CheckMatrix()
	{
		if(ReferenceMatrix != Globals.matrix || Input.IsActionPressed("RELOAD_MATRICES"))
		{
			ReloadMatrix();
		}
	}
	public void UpdateDisplay()
	{
		if (Globals.LevelEditorShortcutLayer == null)
		{
			Globals.LevelEditorShortcutLayer = ImageTexture.CreateFromImage(Globals.LevelEditImageShortcuts);
		}
		if (Globals.LevelEditorLayer1 == null)
		{
			Globals.LevelEditorLayer1 = ImageTexture.CreateFromImage(Globals.LevelEditImage1);
		}
		if (Globals.LevelEditorLayer2 == null)
		{
			Globals.LevelEditorLayer2 = ImageTexture.CreateFromImage(Globals.LevelEditImage2);
		}
		if (Globals.LevelEditorLayer3 == null)
		{
			Globals.LevelEditorLayer3 = ImageTexture.CreateFromImage(Globals.LevelEditImage3);
		}
		Globals.LevelEditorShortcutLayer.SetImage(Globals.LevelEditImageShortcuts);
		Globals.LevelEditorLayer1.SetImage(Globals.LevelEditImage1);
		Globals.LevelEditorLayer2.SetImage(Globals.LevelEditImage2);
		Globals.LevelEditorLayer3.SetImage(Globals.LevelEditImage3);


		levedshtext.Texture = Globals.LevelEditorShortcutLayer;
		leved1text.Texture = Globals.LevelEditorLayer1;
		leved2text.Texture = Globals.LevelEditorLayer2;
		leved3text.Texture = Globals.LevelEditorLayer3;

		geoedshtext.Texture = Globals.LevelEditorShortcutLayer;
		geoed1text.Texture = Globals.LevelEditorLayer1;
		geoed2text.Texture = Globals.LevelEditorLayer2;
		geoed3text.Texture = Globals.LevelEditorLayer3;

		tileedshtext.Texture = Globals.LevelEditorShortcutLayer;
		tileed1text.Texture = Globals.LevelEditorLayer1;
		tileed2text.Texture = Globals.LevelEditorLayer2;
		tileed3text.Texture = Globals.LevelEditorLayer3;

		/*cameraedshtext.Texture = Globals.LevelEditorShortcutLayer;
		cameraed1text.Texture = Globals.LevelEditorLayer1;
		cameraed2text.Texture = Globals.LevelEditorLayer2;
		cameraed3text.Texture = Globals.LevelEditorLayer3;

		lightedshtext.Texture = Globals.LevelEditorShortcutLayer;
		lighted1text.Texture = Globals.LevelEditorLayer1;
		lighted2text.Texture = Globals.LevelEditorLayer2;
		lighted3text.Texture = Globals.LevelEditorLayer3;

		fxedshtext.Texture = Globals.LevelEditorShortcutLayer;
		fxed1text.Texture = Globals.LevelEditorLayer1;
		fxed2text.Texture = Globals.LevelEditorLayer2;
		fxed3text.Texture = Globals.LevelEditorLayer3;

		propedshtext.Texture = Globals.LevelEditorShortcutLayer;
		proped1text.Texture = Globals.LevelEditorLayer1;
		proped2text.Texture = Globals.LevelEditorLayer2;
		proped3text.Texture = Globals.LevelEditorLayer3;

		envedshtext.Texture = Globals.LevelEditorShortcutLayer;
		enved1text.Texture = Globals.LevelEditorLayer1;
		enved2text.Texture = Globals.LevelEditorLayer2;
		enved3text.Texture = Globals.LevelEditorLayer3;

		//levedshtext.Texture = Globals.LevelEditorShortcutLayer;
		//leved1text.Texture = Globals.LevelEditorLayer1;
		//leved2text.Texture = Globals.LevelEditorLayer2;
		//leved3text.Texture = Globals.LevelEditorLayer3;*/




		ReferenceMatrix = Globals.matrix;
		UpdateBoundsRect();
	}

	public void UpdateToolRect()
	{
        if (RedRect == null)
        {
            RedRect = new Image();
            RedRect.Create((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16, false, Image.Format.Rgba8);
        }
        RedRect.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
        RedRect.Fill(Color.Color8(0, 0, 0, 0));
        int X = 0;
        int Y = 0;
        int Z = 0;
        foreach (GeometryTile tile in Globals.matrix)
        {
            if ((Z + 1) == 1)
            {
                DrawToolRect(X, Y);
            }


            //lol copy and pasted
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
        redrect = new ImageTexture();
        redrect.SetImage(RedRect);
        geoeduitext.Texture = redrect;


    }

    public void UpdateMirrorRect()
    {
        if (PurpleRect == null)
        {
            PurpleRect = new Image();
            PurpleRect.Create((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16, false, Image.Format.Rgba8);
        }
        PurpleRect.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
        PurpleRect.Fill(Color.Color8(0, 0, 0, 0));
        int X = 0;
        int Y = 0;
        int Z = 0;
        foreach (GeometryTile tile in Globals.matrix)
        {
            if ((Z + 1) == 1)
            {
                DrawMirrorRect(X, Y);
            }


            //lol copy and pasted
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
        purplerect = new ImageTexture();
        purplerect.SetImage(PurpleRect);
        geoedmirrortext.Texture = purplerect;


    }

    public void DrawMirrorRect(int x, int y)
    {
        LERect displayMirrorRect = new LERect(Globals.MirrorXPos, -1, Globals.MirrorXPos, (int)Globals.levelSize.y);
        if (displayMirrorRect.IsOnEdge(new Vector2i(x, y)) == true)
        {
            LERectEdge edge = displayMirrorRect.WhichEdge(new Vector2i(x, y));
            switch (edge)
            {
                case LERectEdge.Null:
                    break;
                case LERectEdge.Left:
                    PurpleRect.BlitRect(PurpleRectV, PurpleRectV.GetUsedRect(), new Vector2i((x * 16) + 7, (y * 16)));
                    break;
                case LERectEdge.Top:
                    PurpleRect.BlitRect(PurpleRectH, PurpleRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16) + 7));
                    break;
                case LERectEdge.Right:
                    PurpleRect.BlitRect(PurpleRectV, PurpleRectV.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                case LERectEdge.Bottom:
                    PurpleRect.BlitRect(PurpleRectH, PurpleRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                case LERectEdge.TopLeft:
                    PurpleRect.BlitRect(PurpleRectH, PurpleRectH.GetUsedRect(), new Vector2i((x * 16) + 7, (y * 16) + 7));
                    break;
                case LERectEdge.TopRight:
                    //WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16) + 14));
                    break;
                case LERectEdge.BottomRight:
                    //WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                case LERectEdge.BottomLeft:
                    //WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                default:
                    break;
            }
        }
        else PurpleRect.FillRect(new Rect2i((int)x * 16, (int)y * 16, 16, 16), Color.FromHSV(0, 0, 0, 0));


    }
    public void ClearMirrorRect()
    {
        PurpleRect.Fill(Color.FromHSV(0, 0, 0, 0));
        purplerect = new ImageTexture();
        purplerect.SetImage(PurpleRect);
        geoedmirrortext.Texture = purplerect;
    }

    public void ClearToolRect()
	{
        RedRect.Fill(Color.FromHSV(0, 0, 0, 0));
        redrect = new ImageTexture();
        redrect.SetImage(RedRect);
        geoeduitext.Texture = redrect;
    }


	public void DrawToolRect(int x, int y)
	{
		LERect displayToolRect = new LERect(AffectRect.left - 1, AffectRect.top - 1, AffectRect.right + 1, AffectRect.bottom + 1);
        if (displayToolRect.IsOnEdge(new Vector2i(x, y)) == true)
        {
            LERectEdge edge = displayToolRect.WhichEdge(new Vector2i(x, y));
            switch (edge)
            {
                case LERectEdge.Null:
                    break;
                case LERectEdge.Left:
                    RedRect.BlitRect(RedRectV, RedRectV.GetUsedRect(), new Vector2i((x * 16) + 14, (y * 16)));
                    break;
                case LERectEdge.Top:
                    RedRect.BlitRect(RedRectH, RedRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16) + 14));
                    break;
                case LERectEdge.Right:
                    RedRect.BlitRect(RedRectV, RedRectV.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                case LERectEdge.Bottom:
                    RedRect.BlitRect(RedRectH, RedRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                case LERectEdge.TopLeft:
                    RedRect.BlitRect(RedRectH, RedRectH.GetUsedRect(), new Vector2i((x * 16) + 14, (y * 16) + 14));
                    break;
                case LERectEdge.TopRight:
                    //WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16) + 14));
                    break;
                case LERectEdge.BottomRight:
                    //WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                case LERectEdge.BottomLeft:
                    //WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
                    break;
                default:
                    break;
            }
        }
        else RedRect.FillRect(new Rect2i((int)x * 16, (int)y * 16, 16, 16), Color.FromHSV(0, 0, 0, 0));


    }







	public void UpdateTile(int x, int y)
	{
		Globals.LevelEditImageShortcuts.FillRect(new Rect2i(x * 16, y * 16, 16, 16), Colors.Transparent);
		Globals.LevelEditImage1.FillRect(new Rect2i(x * 16, y * 16, 16, 16), Colors.Transparent);
		Globals.LevelEditImage2.FillRect(new Rect2i(x * 16, y * 16, 16, 16), Colors.Transparent);
		Globals.LevelEditImage3.FillRect(new Rect2i(x * 16, y * 16, 16, 16), Colors.Transparent);


		int X = x;
		int Y = y;
		//int Z = 0;

		//GeometryTile tile = Globals.matrix[X, Y];

		for (int Z = 0; Z < 3; Z++)
		{
			GeometryTile tile = Globals.matrix[X, Y, Z];
			switch (tile.TileID)
			{
				case Tiles.Air:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.FillRect(new Rect2i((int)X * 16, (int)Y * 16, 16, 16), Color.FromHSV(0, 0, 0, 0));
							break;
						case 2:
							Globals.LevelEditImage2.FillRect(new Rect2i((int)X * 16, (int)Y * 16, 16, 16), Color.FromHSV(0, 0, 0, 0));
							break;
						case 3:
							Globals.LevelEditImage3.FillRect(new Rect2i((int)X * 16, (int)Y * 16, 16, 16), Color.FromHSV(0, 0, 0, 0));
							break;
						default:
							break;
					}
					break;
				case Tiles.Wall:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.BlitRect(Wall, Wall.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 2:
							Globals.LevelEditImage2.BlitRect(Wall, Wall.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 3:
							Globals.LevelEditImage3.BlitRect(Wall, Wall.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						default:
							break;
					}
					break;
				case Tiles.SlopeSE:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.BlitRect(Slope1, Slope1.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 2:
							Globals.LevelEditImage2.BlitRect(Slope1, Slope1.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 3:
							Globals.LevelEditImage3.BlitRect(Slope1, Slope1.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						default:
							break;
					}
					break;
				case Tiles.SlopeSW:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.BlitRect(Slope2, Slope2.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 2:
							Globals.LevelEditImage2.BlitRect(Slope2, Slope2.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 3:
							Globals.LevelEditImage3.BlitRect(Slope2, Slope2.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						default:
							break;
					}
					break;
				case Tiles.SlopeNE:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.BlitRect(Slope3, Slope3.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 2:
							Globals.LevelEditImage2.BlitRect(Slope3, Slope3.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 3:
							Globals.LevelEditImage3.BlitRect(Slope3, Slope3.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						default:
							break;
					}
					break;
				case Tiles.SlopeNW:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.BlitRect(Slope4, Slope4.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 2:
							Globals.LevelEditImage2.BlitRect(Slope4, Slope4.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 3:
							Globals.LevelEditImage3.BlitRect(Slope4, Slope4.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						default:
							break;
					}
					break;
				case Tiles.Floor:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.BlitRect(Floor, Floor.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 2:
							Globals.LevelEditImage2.BlitRect(Floor, Floor.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 3:
							Globals.LevelEditImage3.BlitRect(Floor, Floor.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						default:
							break;
					}
					break;
				case Tiles.ConnectedShortcut:
					switch (Z + 1)
					{
						case 1:
							Globals.LevelEditImage1.BlitRect(FakeTransparent, FakeTransparent.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 2:
							Globals.LevelEditImage2.BlitRect(FakeTransparent, FakeTransparent.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case 3:
							Globals.LevelEditImage3.BlitRect(FakeTransparent, FakeTransparent.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
					}
					break;
				case Tiles.Glass:
					break;
				default:
					break;
			}
			if (tile.Tile2IDS != null)
			{
				//Globals.LevelEditorShortcutLayer.Update(Globals.LevelEditImageShortcuts);
				//GD.Pint(Globals.LevelEditImageShortcuts.Ind);
				foreach (Tiles2 stackable in tile.Tile2IDS)
				{
					switch (stackable)
					{
						case Tiles2.Horizonpole:
							switch (Z + 1)
							{
								case 1:
									Globals.LevelEditImage1.BlitRect(HBar, HBar.GetUsedRect(), new Vector2i((int)X * 16, (int)(Y * 16) + 6));
									break;
								case 2:
									Globals.LevelEditImage2.BlitRect(HBar, HBar.GetUsedRect(), new Vector2i((int)X * 16, (int)(Y * 16) + 6));
									break;
								case 3:
									Globals.LevelEditImage3.BlitRect(HBar, HBar.GetUsedRect(), new Vector2i((int)X * 16, (int)(Y * 16) + 6));
									break;
								default:
									break;
							}
							break;
						case Tiles2.Vertipole:
							switch (Z + 1)
							{
								case 1:
									Globals.LevelEditImage1.BlitRect(VBar, VBar.GetUsedRect(), new Vector2i((int)(X * 16) + 6, (int)Y * 16));
									break;
								case 2:
									Globals.LevelEditImage2.BlitRect(VBar, VBar.GetUsedRect(), new Vector2i((int)(X * 16) + 6, (int)Y * 16));
									break;
								case 3:
									Globals.LevelEditImage3.BlitRect(VBar, VBar.GetUsedRect(), new Vector2i((int)(X * 16) + 6, (int)Y * 16));
									break;
								default:
									break;
							}
							break;
						case Tiles2.BatflyHive:
							Globals.LevelEditImageShortcuts.BlitRect(BatflyNest, BatflyNest.GetUsedRect(), new Vector2i((int)X * 16, (int)Y * 16));
							break;
						case Tiles2.ShortcutEntrance:
							bool shortcut;
							List<Vector2i> l = new List<Vector2i>();
							List<Vector2i> l2 = new List<Vector2i>();
							Vector2i[] dir = { new Vector2i(-1, 0), new Vector2i(0, -1), new Vector2i(1, 0), new Vector2i(0, 1) };
							Vector2i[] dir2 = { new Vector2i(-1, -1), new Vector2i(1, -1), new Vector2i(1, 1), new Vector2i(-1, 1) };
							for (int i = 0; i < dir.Length; i++)
							{
								//GD.Print(X," ", Y," ", Z, "dir ", dir[i]);
								//GD.Print("tile checked ", X + dir[i].x, " ", Y + dir[i].y);
								//looooooooooooooooooooong
								if (Globals.matrix[X + dir[i].x,Y + dir[i].y, 0].Tile2IDS != null && (Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].Tile2IDS.Contains(Tiles2.ShortcutConnector) || Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].Tile2IDS.Contains(Tiles2.RoomEntrance) || Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].Tile2IDS.Contains(Tiles2.EnemyDen) || Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].Tile2IDS.Contains(Tiles2.GarbageWormEntrance) || Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].Tile2IDS.Contains(Tiles2.WhackAMoleHole) || Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].Tile2IDS.Contains(Tiles2.ScavengerEntrance)))
								{

									l.Add(dir[i]);
									// GD.Print(l[0]);
								}
								if (Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].TileID == Tiles.Air || Globals.matrix[X + dir[i].x, Y + dir[i].y, 0].TileID == Tiles.Floor)
								{
									l2.Add(dir[i]);
									// GD.Print("2 ",l2[0]);
								}

							}
							shortcut = false;
							if (l.Count == 1 & l2.Count == 1)
							{
								if (l[0] == -l2[0])
								{
									//GD.Print("Shortcut at ", X, ", ", Y, " Is pointing ", l[0]);
									shortcut = true;
								}
							}
							for (int i = 0; i < dir2.Length; i++)
							{
								if (Globals.matrix[X + dir2[i].x, Y + dir2[i].y, 0].TileID != Tiles.Wall)
								{
									shortcut = false;
									break;
								}
							}
							if (shortcut == true)
							{
								// -1,0 LEFT
								// 0, -1 UP
								// 1, 0 RIGHT
								//0, 1 DOWN
								Globals.matrix[X, Y, 0].TileID = Tiles.ConnectedShortcut;
								switch (l[0])
								{
									case (-1, 0):
										Globals.LevelEditImageShortcuts.BlitRect(ShortcutArrowLeft, ShortcutArrowLeft.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
										break;
									case (0, -1):
										Globals.LevelEditImageShortcuts.BlitRect(ShortcutArrowUp, ShortcutArrowUp.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
										break;
									case (1, 0):
										Globals.LevelEditImageShortcuts.BlitRect(ShortcutArrowRight, ShortcutArrowRight.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
										break;
									case (0, 1):
										Globals.LevelEditImageShortcuts.BlitRect(ShortcutArrowDown, ShortcutArrowDown.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
										break;
									default:
										break;
								}
							}
							else
							{
								Globals.LevelEditImageShortcuts.BlitRect(ShortcutUnconnected, ShortcutUnconnected.GetUsedRect(), new Vector2i((int)(X * 16) + 1, (int)(Y * 16) + 1));
								Globals.matrix[X, Y, 0].TileID = Tiles.Air;
							}
							break;
						case Tiles2.ShortcutConnector:
							Globals.LevelEditImageShortcuts.BlitRect(ShortcutDot, ShortcutDot.GetUsedRect(), new Vector2i((int)(X * 16) + 6, (int)(Y * 16) + 6));
							break;
						case Tiles2.RoomEntrance:
							Globals.LevelEditImageShortcuts.BlitRect(RoomEntrance, RoomEntrance.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
							break;
						case Tiles2.EnemyDen:
							Globals.LevelEditImageShortcuts.BlitRect(EnemyDen, EnemyDen.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
							break;
						case Tiles2.Rock:
							Globals.LevelEditImageShortcuts.BlitRect(Rock, Rock.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
							break;
						case Tiles2.Spear:
							Globals.LevelEditImageShortcuts.BlitRect(Spear, Spear.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
							break;
						case Tiles2.Cracks:
							// idk how but the horizontal and vertical cracks are swapped
							bool any = false;
							bool vertic = false;
							bool horiz = false;
							if ((CheckTileIfCracksOpen(X + -1, Y, Z) == true) || (CheckTileIfCracksOpen(X + 1, Y, Z) == true))
							{
								any = true;
								horiz = true;
							}
							if ((CheckTileIfCracksOpen(X, Y + -1, Z) == true) || (CheckTileIfCracksOpen(X, Y + 1, Z) == true))
							{
								any = true;
								vertic = true;
							}

							if (any == true)
							{
								if (vertic == true)
								{
									switch (Z + 1)
									{
										case 1:
											Globals.LevelEditImage1.BlitRect(VertiCracks, VertiCracks.GetUsedRect(), new Vector2i((int)(X * 16) + 4, (int)Y * 16));
											break;
										case 2:
											Globals.LevelEditImage2.BlitRect(VertiCracks, VertiCracks.GetUsedRect(), new Vector2i((int)(X * 16) + 4, (int)Y * 16));
											break;
										case 3:
											Globals.LevelEditImage3.BlitRect(VertiCracks, VertiCracks.GetUsedRect(), new Vector2i((int)(X * 16) + 4, (int)Y * 16));
											break;
										default:
											break;
									}
								}
								if (horiz == true)
								{
									switch (Z + 1)
									{
										case 1:
											Globals.LevelEditImage1.BlitRect(HorizCracks, HorizCracks.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16) + 4));
											break;
										case 2:
											Globals.LevelEditImage2.BlitRect(HorizCracks, HorizCracks.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16) + 4));
											break;
										case 3:
											Globals.LevelEditImage3.BlitRect(HorizCracks, HorizCracks.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16) + 4));
											break;
										default:
											break;
									}
								}
							}
							else
							{
								switch (Z + 1)
								{
									case 1:
										Globals.LevelEditImage1.BlitRect(NullCracks, NullCracks.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
										break;
									case 2:
										Globals.LevelEditImage2.BlitRect(NullCracks, NullCracks.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
										break;
									case 3:
										Globals.LevelEditImage3.BlitRect(NullCracks, NullCracks.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
										break;
									default:
										break;
								}
							}

							break;
						case Tiles2.ForbidBatflyChain:
							Globals.LevelEditImageShortcuts.BlitRect(ForbidBatflyChains, ForbidBatflyChains.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
							break;
						case Tiles2.GarbageWormEntrance:
							Globals.LevelEditImageShortcuts.BlitRect(GarbageWormDen, GarbageWormDen.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
							break;
						case Tiles2.Waterfall:
							Globals.LevelEditImageShortcuts.BlitRect(Waterfall, Waterfall.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
							break;
						case Tiles2.WhackAMoleHole:
							Globals.LevelEditImageShortcuts.BlitRect(WhackAMoleHole, WhackAMoleHole.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
							break;
						case Tiles2.WormGrass:
							Globals.LevelEditImageShortcuts.BlitRect(WormGrass, WormGrass.GetUsedRect(), new Vector2i((int)(X * 16), (int)(Y * 16)));
							break;
						case Tiles2.ScavengerEntrance:
							Globals.LevelEditImageShortcuts.BlitRect(ScavengerTeleporterHole, ScavengerTeleporterHole.GetUsedRect(), new Vector2i((int)(X * 16) + 3, (int)(Y * 16) + 3));
							break;
						default:
							break;
					}


				}

			}
			else
			{
				Globals.LevelEditImageShortcuts.FillRect(new Rect2i((int)X * 16, (int)Y * 16, 16, 16), Color.FromHSV(0, 0, 0, 0));
			}


		}

		



	}





	public bool CheckTileIfCracksOpen(int x, int y, int z)
	{
		if (Globals.matrix[x, y, z].TileID != Tiles.Wall || (Globals.matrix[x,y,z].Tile2IDS != null && Globals.matrix[x, y, z].Tile2IDS.Contains(Tiles2.Cracks)))
		{
			//if (Globals.matrix[x, y, z].Tile2IDS.Contains(Tiles2.Cracks))
			//{
			//	GD.Print(x, " ", y, " ", z, " cracks!");
			//}
			//GD.Print(Globals.matrix[x, y, z].TileID,"  ",x, " ",y, " ",z," true!");
			return true; // true = 1
		}
		else return false; // false = 0
	}

	public void UpdateBoundsRect()
	{
		if (WhiteRect == null)
		{
			WhiteRect = new Image();
			WhiteRect.Create((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16, false, Image.Format.Rgba8);
		}
		WhiteRect.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		WhiteRect.Fill(Color.Color8(0, 0, 0, 0));
		int X = 0;
		int Y = 0;
		int Z = 0;
		foreach (GeometryTile tile in Globals.matrix)
		{
			if ((Z + 1) == 1)
			{
				UpdateBoundsRectSize(X, Y);
			}


			//lol copy and pasted
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
		whiterect = new ImageTexture();
		whiterect.SetImage(WhiteRect);
		geoedrecttext.Texture = whiterect;
	}

	public void UpdateBoundsRectSize(int x, int y)
	{
		LERect rect = new LERect(Globals.extraTiles.left - 1, Globals.extraTiles.top - 1, (int)Globals.levelSize.x - (Globals.extraTiles.right), (int)Globals.levelSize.y - (Globals.extraTiles.bottom));
		if (rect.IsOnEdge(new Vector2i(x, y)) == true)
		{
			LERectEdge edge = rect.WhichEdge(new Vector2i(x, y));
			switch (edge)
			{
				case LERectEdge.Null:
					break;
				case LERectEdge.Left:
					WhiteRect.BlitRect(WhiteRectV, WhiteRectV.GetUsedRect(), new Vector2i((x * 16) + 14, (y * 16)));
					break;
				case LERectEdge.Top:
					WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16) + 14));
					break;
				case LERectEdge.Right:
					WhiteRect.BlitRect(WhiteRectV, WhiteRectV.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
					break;
				case LERectEdge.Bottom:
					WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
					break;
				case LERectEdge.TopLeft:
					WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16) + 14, (y * 16) + 14));
					break;
				case LERectEdge.TopRight:
					//WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16) + 14));
					break;
				case LERectEdge.BottomRight:
					//WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
					break;
				case LERectEdge.BottomLeft:
					//WhiteRect.BlitRect(WhiteRectH, WhiteRectH.GetUsedRect(), new Vector2i((x * 16), (y * 16)));
					break;
				default:
					break;
			}
		}
		else WhiteRect.FillRect(new Rect2i((int)x * 16, (int)y * 16, 16, 16), Color.FromHSV(0, 0, 0, 0));
	}



	public void UseTool(int X, int Y, int Layer)
	{
		X -= 1;
		Y -= 1;
		Layer -= 1;
		bool hasChangedAlreadyThisFrame = false;
		switch (Globals.CurrentTool)
		{
			case GeometryTools.Invert:
				if (Globals.matrix[X,Y,Layer].TileID == (Tiles)0 && hasChangedAlreadyThisFrame == false)
				{
					//GD.Print("Walled! ", X + 1, " ", Y + 1);
					LERect rect = new LERect(X, Y, X, Y);
					ChangeTo(rect, Layer, 1);
					//Globals.matrix[X,Y,Layer].TileID = (Tiles)1;
					hasChangedAlreadyThisFrame = true;
				}
				if (Globals.matrix[X, Y, Layer].TileID == (Tiles)1 && hasChangedAlreadyThisFrame == false)
				{
					//GD.Print("Aired! ", X + 1, " ", Y + 1);
					LERect rect = new LERect(X, Y, X, Y);
					ChangeTo(rect, Layer, 0);
					//Globals.matrix[X, Y, Layer].TileID = (Tiles)0;
					hasChangedAlreadyThisFrame=true;
				}
			   //UpdateTile(X, Y);
				break;
			case GeometryTools.PaintWall:
				LERect rect2 = new LERect(X, Y, X, Y);
				ChangeTo(rect2, Layer, 1);
				break;
			case GeometryTools.PaintAir:
				LERect rect3 = new LERect(X, Y, X, Y);
				ChangeTo(rect3, Layer, 0);
				break;
			case GeometryTools.Floor:
				LERect rect4 = new LERect(X, Y, X, Y);
				ChangeTo(rect4, Layer, 6);
				break;
			case GeometryTools.Slope:
				int slope = SlopeTile(new Vector2i(X, Y));
				if (slope != 0)
				{
                    LERect rect6 = new LERect(X, Y, X, Y);
                    ChangeTo(rect6, Layer, slope);
                }
				break;
			case GeometryTools.EnemyDen:
				if (Layer == 0 && LastPos != new Vector2i(X,Y))
				{
					AddRemoveFeature(X, Y, Layer, 7);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.Entrance:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 6);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.RectWall:
				if (LastPos != new Vector2i(X, Y))
				{
					bool thisframerect = false;
					if (Globals.IsRectOn == false)
					{
						Globals.RectLockPos = new Vector2i(X, Y);
						Globals.IsRectOn = true;
						thisframerect = true;
					}
					else if (thisframerect == false)
					{
						//GD.Print(AffectRect.ToString());
						ChangeTo(AffectRect, Layer, 1);
						ClearToolRect();
						Globals.IsRectOn = false;
					}
				}
				break;
			case GeometryTools.RectAir:
                if (LastPos != new Vector2i(X, Y))
                {
                    bool thisframerect = false;
                    if (Globals.IsRectOn == false)
                    {
                        Globals.RectLockPos = new Vector2i(X, Y);
                        Globals.IsRectOn = true;
                        thisframerect = true;
                    }
                    else if (thisframerect == false)
                    {
                        //GD.Print(AffectRect.ToString());
                        ChangeTo(AffectRect, Layer, 0);
                        ClearToolRect();
                        Globals.IsRectOn = false;
                    }
                }
                break;
			case GeometryTools.CopyBack:
                if (LastPos != new Vector2i(X, Y))
                {
                    bool thisframerect = false;
                    if (Globals.IsRectOn == false)
                    {
                        Globals.RectLockPos = new Vector2i(X, Y);
                        Globals.IsRectOn = true;
                        thisframerect = true;
                    }
                    else if (thisframerect == false)
                    {
						if (Globals.CurrentLayer != 3)
						{
							for(int q = 0; q < Globals.levelSize.x; q++)
							{
								for (int c = 0; c < Globals.levelSize.y; c++)
								{
									if (AffectRect.IsInsideRect(new Vector2i(q, c)))
									{
                                        Tiles tileid = Globals.matrix[q, c, Layer].TileID;
										//List<Tiles2> tiles3 = Globals.matrix[q, c, Layer].Tile2IDS;
										LERect rect7 = new LERect(q, c, q, c);
										ChangeTo(rect7, Layer + 1, (int)tileid);
                                    }
									//Tiles tile = Globals.matrix[q, c, Layer].TileID;
									//LERect rect7 = new LERect(q, c, q,)

								}
							}
						}
                        ClearToolRect();
                        Globals.IsRectOn = false;
                    }
                }
                break;
			case GeometryTools.Flip:
                if (LastPos != new Vector2i(X, Y))
                {
                    bool thisframerect = false;
                    if (Globals.IsRectOn == false)
                    {
                        Globals.RectLockPos = new Vector2i(X, Y);
                        Globals.IsRectOn = true;
                        thisframerect = true;
                    }
                    else if (thisframerect == false)
                    {
						for(int i = 0; i < 3; i++)
						{
                            ChangeTo(AffectRect, i, 0);
                            for (int q = 0; q < Globals.levelSize.x; q++)
                            {
                                for (int c = 0; c < Globals.levelSize.y; c++)
                                {
                                    if (AffectRect.IsInsideRect(new Vector2i(q, c)))
                                    {
                                        Tiles tile = Globals.matrix[q, c, Layer].TileID;
                                        //if (Globals.matrix[q,c,Layer].Tile2IDS != null)
                                        //{
                                        //    List<Tiles2> tiles2 = Globals.matrix[q, c, Layer].Tile2IDS;
                                        // }
                                        //List<Tiles2> tiles2 = Globals.matrix[q, c, Layer].Tile2IDS;
                                        LERect rect7 = new LERect(q, c, q, c);
										ClearFeatures(q, c, i);
                                        //ChangeTo(rect7, Layer + 1, (int)tile);
                                    }
                                    //Tiles tile = Globals.matrix[q, c, Layer].TileID;
                                    //LERect rect7 = new LERect(q, c, q,)

                                }
                            }
                        }
                        //GD.Print(AffectRect.ToString());
                        //ChangeTo(AffectRect, Layer, 1);
                        ClearToolRect();
                        Globals.IsRectOn = false;
                    }
                }
                break;
			case GeometryTools.HorizBeam:
				if (LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 1);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.VertiBeam:
				if (LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 2);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.Glass:
				LERect rect5 = new LERect(X, Y, X, Y);
				ChangeTo(rect5, Layer, 9);
				break;
			case GeometryTools.Shortcut:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 4);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.ShortcutDot:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 5);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.BatflyHive:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 3);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.ChangeLayer:
				if (Globals.CurrentLayer == 3)
				{
					Globals.CurrentLayer = 1;
					GD.Print("Active layer changed to ", Globals.CurrentLayer);
				}
				else {Globals.CurrentLayer += 1; GD.Print("Active layer changed to ", Globals.CurrentLayer);}
				break;
			case GeometryTools.MirrorToggle:
				if(Globals.IsMirrorOn == true)
				{
					ClearMirrorRect();
				}
				Globals.IsMirrorOn = !Globals.IsMirrorOn;
				break;
			case GeometryTools.MirrorMove:
				Globals.MirrorXPos = X;
				break;
			case GeometryTools.Rock:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 9);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.Spear:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 10);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.Crack:
				if (LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 11);				
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.ForbidBatflyChain:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 12);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.GarbageWormHole:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 13);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.Waterfall:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 18);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.WhackAMoleHole:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 19);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.Wormgrass:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 20);
					LastPos = new Vector2i(X, Y);
				}
				break;
			case GeometryTools.ScavengerHole:
				if (Layer == 0 && LastPos != new Vector2i(X, Y))
				{
					AddRemoveFeature(X, Y, Layer, 21);
					LastPos = new Vector2i(X, Y);
				}
				break;
			default:
				break;
		}
		//UpdateTile(X,Y);
		UpdateDisplay();
		hasChangedAlreadyThisFrame = false;


	}

	public int SlopeTile(Vector2i pos)
	{
		string nbs = "";
		int result;
        Vector2i[] dir = { new Vector2i(-1, 0), new Vector2i(0, -1), new Vector2i(1, 0), new Vector2i(0, 1) };
        for (int i = 0; i < dir.Length; i++)
		{
			nbs = nbs + Utilities.AfaMvLevelEdit(pos + dir[i], Globals.CurrentLayer - 1).ToString();
		}
		//GD.Print(nbs);
		switch (nbs)
		{
			case "1001":
				result = 2;
				break;
			case "0011":
				result = 3;
				break;
			case "1100":
				result = 4;
				break;
			case "0110":
				result = 5;
				break;
			default:
				result = 0;
				break;
		}
		return result;
	}


	public void ClearFeatures(int x, int y, int z)
	{
        if (Globals.matrix[x, y, z].Tile2IDS != null)
		{
            Globals.matrix[x, y, z].Tile2IDS.Clear();
        }
        LERect rect = new LERect(x - 1, y - 1, x + 2, y + 2);
        for (int q = rect.left; q < rect.right; q++)
        {
            for (int c = rect.top; c < rect.bottom; c++)
            {
                if (q > 0 & q < Globals.levelSize.x)
                {
                    if (c > 0 & c < Globals.levelSize.y)
                    {
                        //GD.Print(q + 1, " 2 ", c + 1);
                        UpdateTile(q, c);
                    }
                }
                //GD.Print("2y ", c);
                // GD.Print(q + 1, " 2 ", c + 1);
                // UpdateTile(q, c);
            }
        }
    }
	public void AddRemoveFeature(int x, int y, int z, int featuretype)
	{
		if (Globals.matrix[x,y,z].Tile2IDS != null && Globals.matrix[x, y, z].Tile2IDS.Contains((Tiles2)featuretype))
		{
			Globals.matrix[x, y, z].Tile2IDS.Remove((Tiles2)featuretype);
		}
		else
		{
			if (Globals.matrix[x,y,z].Tile2IDS == null)
			{
				Globals.matrix[x,y,z].Tile2IDS = new List<Tiles2>();
			}
			Globals.matrix[x,y,z].Tile2IDS.Add((Tiles2)featuretype);
		}
		LERect rect = new LERect(x - 1, y - 1, x + 2, y + 2);
		for (int q = rect.left; q < rect.right; q++)
		{
			for (int c = rect.top; c < rect.bottom; c++)
			{
				if (q >= 0 & q < Globals.levelSize.x)
				{
					if (c >= 0 & c < Globals.levelSize.y)
					{
						//GD.Print(q + 1, " 2 ", c + 1);
						UpdateTile(q, c);
					}
				}
				//GD.Print("2y ", c);
				// GD.Print(q + 1, " 2 ", c + 1);
				// UpdateTile(q, c);
			}
		}
	}

	public void ChangeTo(LERect rect, int Z, int tiletype = 0)
	{
		for (int q = rect.left; q <= rect.right; q++)
		{
			for (int c = rect.top; c <= rect.bottom; c++) // lol funny coding language
			{
				//GD.Print(q + 1, " ", c + 1);
				Globals.matrix[q, c, Z].TileID = (Tiles)tiletype;
				UpdateTile(q, c);
			}
		}
		LERect rect2 = new LERect(rect.left - 1, rect.top -1, rect.right + 2, rect.bottom + 2);
		for (int q = rect2.left; q < rect2.right; q++)
		{
			for (int c = rect2.top; c < rect2.bottom; c++)
			{
				if (q > 0 & q < Globals.levelSize.x)
				{
					if (c > 0 & c < Globals.levelSize.y)
					{
						//GD.Print(q + 1, " 2 ", c + 1);
						UpdateTile(q, c);
					}
				}
				//GD.Print("2y ", c);
			   // GD.Print(q + 1, " 2 ", c + 1);
			   // UpdateTile(q, c);
			}
		}
		if (Globals.IsMirrorOn == true)
		{
			LERect rect3 = MirrorRect(rect);
            for (int q = rect3.left; q <= rect3.right; q++)
            {
                for (int c = rect3.top; c <= rect3.bottom; c++) // lol funny coding language
                {
                    //GD.Print(q + 1, " ", c + 1);
					if ((q > 0 & q < Globals.levelSize.x) & (c > 0 & c < Globals.levelSize.y))
					{
						int tile2type = tiletype;
						if (tiletype == 4)
						{
							tile2type = 5;
						}
						if (tiletype == 5)
						{
							tile2type = 4;
						}
						if (tiletype == 2)
						{
							tile2type = 3;
						}
						if (tiletype == 3)
						{
							tile2type = 2;
						}
                        Globals.matrix[q, c, Z].TileID = (Tiles)tile2type;
                        UpdateTile(q, c);
                    }
                    //Globals.matrix[q, c, Z].TileID = (Tiles)tiletype;
                    //UpdateTile(q, c);
                }
            }
            LERect rect4 = new LERect(rect3.left - 1, rect3.top - 1, rect3.right + 2, rect3.bottom + 2);
            for (int q = rect4.left; q < rect4.right; q++)
            {
                for (int c = rect4.top; c < rect4.bottom; c++)
                {
                    if (q > 0 & q < Globals.levelSize.x)
                    {
                        if (c > 0 & c < Globals.levelSize.y)
                        {
                            //GD.Print(q + 1, " 2 ", c + 1);
                            UpdateTile(q, c);
                        }
                    }
                    //GD.Print("2y ", c);
                    // GD.Print(q + 1, " 2 ", c + 1);
                    // UpdateTile(q, c);
                }
            }
        }
	}

	public LERect MirrorRect(LERect rect1)
	{
		LERect rect2;
		int left = Globals.MirrorXPos - (rect1.left - Globals.MirrorXPos);
		int right = Globals.MirrorXPos - (rect1.right - Globals.MirrorXPos);

		if (left < right)
		{
			rect2 = new LERect(left, rect1.top, right, rect1.bottom);
		}
		else
		{
			rect2 = new LERect(right, rect1.top, left, rect1.bottom);
		}
		return rect2;
	}
	public void ResetMatrix()
	{
        Globals.LevelEditImageShortcuts.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.LevelEditImageShortcuts.Fill(Colors.White);
		//Globals.LevelEditImageShortcuts.Resize(16, 16);
        //Globals.LevelEditImageShortcuts.Fill(Colors.White);
        Globals.LevelEditImage1.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
        Globals.LevelEditImage2.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
        Globals.LevelEditImage3.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		ReloadMatrix();
    }

	public void ReloadMatrix()
	{
		Globals.LevelEditImageShortcuts.Fill(Color.FromHSV(0, 0, 0, 0)); //temporary artifact obliterator;
		uint X = 0; // remember that arrays start at 0 in c#, but start at 1 in lingo (do they?) they do.
		uint Y = 0;
		uint Z = 0;
		foreach (GeometryTile tile in Globals.matrix)
		{
			UpdateTile((int)X, (int)Y);

			//lol copy and pasted
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





		//GD.Print("shlayer ", Globals.LevelEditorShortcutLayer);
		//GD.Print("shtex ", Globals.LevelEditImageShortcuts);


		if (Globals.LevelEditorShortcutLayer == null)
		{
			Globals.LevelEditorShortcutLayer = ImageTexture.CreateFromImage(Globals.LevelEditImageShortcuts);
		}
		if (Globals.LevelEditorLayer1 == null)
		{
			Globals.LevelEditorLayer1 = ImageTexture.CreateFromImage(Globals.LevelEditImage1);
		}
		if (Globals.LevelEditorLayer2 == null)
		{
			Globals.LevelEditorLayer2 = ImageTexture.CreateFromImage(Globals.LevelEditImage2);
		}
		if (Globals.LevelEditorLayer3 == null)
		{
			Globals.LevelEditorLayer3 = ImageTexture.CreateFromImage(Globals.LevelEditImage3);
		}
		Globals.LevelEditorShortcutLayer.SetImage(Globals.LevelEditImageShortcuts);
		Globals.LevelEditorLayer1.SetImage(Globals.LevelEditImage1);
		Globals.LevelEditorLayer2.SetImage(Globals.LevelEditImage2);
		Globals.LevelEditorLayer3.SetImage(Globals.LevelEditImage3);


		levedshtext.Texture = Globals.LevelEditorShortcutLayer;
		leved1text.Texture = Globals.LevelEditorLayer1;
		leved2text.Texture = Globals.LevelEditorLayer2;
		leved3text.Texture = Globals.LevelEditorLayer3;

		geoedshtext.Texture = Globals.LevelEditorShortcutLayer;
		geoed1text.Texture = Globals.LevelEditorLayer1;
		geoed2text.Texture = Globals.LevelEditorLayer2;
		geoed3text.Texture = Globals.LevelEditorLayer3;

		//tileedshtext.Texture = Globals.LevelEditorShortcutLayer;
		//tileed1text.Texture = Globals.LevelEditorLayer1;
		//tileed2text.Texture = Globals.LevelEditorLayer2;
		//tileed3text.Texture = Globals.LevelEditorLayer3;

		/*cameraedshtext.Texture = Globals.LevelEditorShortcutLayer;
		cameraed1text.Texture = Globals.LevelEditorLayer1;
		cameraed2text.Texture = Globals.LevelEditorLayer2;
		cameraed3text.Texture = Globals.LevelEditorLayer3;

		lightedshtext.Texture = Globals.LevelEditorShortcutLayer;
		lighted1text.Texture = Globals.LevelEditorLayer1;
		lighted2text.Texture = Globals.LevelEditorLayer2;
		lighted3text.Texture = Globals.LevelEditorLayer3;

		fxedshtext.Texture = Globals.LevelEditorShortcutLayer;
		fxed1text.Texture = Globals.LevelEditorLayer1;
		fxed2text.Texture = Globals.LevelEditorLayer2;
		fxed3text.Texture = Globals.LevelEditorLayer3;

		propedshtext.Texture = Globals.LevelEditorShortcutLayer;
		proped1text.Texture = Globals.LevelEditorLayer1;
		proped2text.Texture = Globals.LevelEditorLayer2;
		proped3text.Texture = Globals.LevelEditorLayer3;

		envedshtext.Texture = Globals.LevelEditorShortcutLayer;
		enved1text.Texture = Globals.LevelEditorLayer1;
		enved2text.Texture = Globals.LevelEditorLayer2;
		enved3text.Texture = Globals.LevelEditorLayer3;

		//levedshtext.Texture = Globals.LevelEditorShortcutLayer;
		//leved1text.Texture = Globals.LevelEditorLayer1;
		//leved2text.Texture = Globals.LevelEditorLayer2;
		//leved3text.Texture = Globals.LevelEditorLayer3;*/




		ReferenceMatrix = Globals.matrix;
		UpdateBoundsRect();
		Camera.UpdateCamera();
	}



}
