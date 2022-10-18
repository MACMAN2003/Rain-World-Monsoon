using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel; //what the hell?
using System.Diagnostics; // the fuck is this shit why does visual studio keep adding these
using System.Reflection.Emit;
using System.Threading.Tasks;

namespace RainWorldMonsoon;



public partial class TileController : Node
{
	// Called when the node enters the scene tree for the first time.

	//this will be harder to crack than the geo editor cause there's not a fixed list of tiles
	public Sprite2D UI;

	public Sprite2D Layer1Tiles;
	public Sprite2D Layer1Geo;
	//public ImageTexture Layer1;
	public Sprite2D Layer2Tiles;
	public Sprite2D Layer2Geo;
	//public ImageTexture Layer2;
	public Sprite2D Layer3Tiles;
	public Sprite2D Layer3Geo;

	public Sprite2D Cursor;
	public Image BaseCursorImage;
	public Image SpecialCursorImage;

	public Color col1;
	public Color col2;
	public Color col3;


	public Sprite2D PreviewLayer1;
	public Image Previmg1;
	public ImageTexture Prevtex1;
	public Sprite2D PreviewLayer2;
	public Image Previmg2;
	public ImageTexture Prevtex2;
	public LERect AffectRect;
	public Image TEP0;
	public Image TEP1;
	public Image TEP2;
	public Image TEP3;
	public Image TEP4;
	public Image TEP5;
	public Image TEP6;
	//public Image TEP7;
	//public Image TEP8;
	public Image TEP9;
	public Image TEPCursor;

	public Image TEP20;
	public Image TEP21;
	public Image TEP22;
	public Image TEP23;
	public Image TEP24;
	public Image TEP25;
	public Image TEP26;
	//public Image TEP7;
	//public Image TEP8;
	public Image TEP29;
	public Image TEP2Cursor;

	public Node Apprentice; //soon i will have a new apprentice, one far younger and more powerful

	private TETile[,,] refmatrix;

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
	bool RectDisp = false;



	public Vector2i RectMin;
	public Vector2i RectMax;

	MatrixMaster Master;
	Vector2i LastCursorPosition;
	Image PurpleRect;
	//int fakethreadindex;
	//int fakethreadindexy;
	//bool isfakethreading = false;
	public override void _Ready()
	{
		Master = (MatrixMaster)GetTree().Root.FindChild("Matrix Master", true, false);
		Apprentice = GetTree().Root.FindChild("Matrix Display Apprentice Tile", true, false);
		Layer1Tiles = (Sprite2D)Apprentice.FindChild("TileDisplayImage1", true, false);
		Layer1Geo = (Sprite2D)Apprentice.FindChild("TileEditImage1", true, false);
		Layer2Tiles = (Sprite2D)Apprentice.FindChild("TileDisplayImage2", true, false);
		Layer2Geo = (Sprite2D)Apprentice.FindChild("TileEditImage2", true, false);
		Layer3Tiles = (Sprite2D)Apprentice.FindChild("TileDisplayImage3", true, false);
		Layer3Geo = (Sprite2D)Apprentice.FindChild("TileEditImage3", true, false);
		PreviewLayer1 = (Sprite2D)GetTree().Root.FindChild("TilePrevLayer1", true, false);
		PreviewLayer2 = (Sprite2D)GetTree().Root.FindChild("TilePrevLayer2", true, false);
		Previmg1 = new Image();
		Previmg2 = new Image();
		
		UI = (Sprite2D)Apprentice.FindChild("UIRect", true, false);
		
		Cursor = (Sprite2D)Apprentice.FindChild("DisplayCursor", true, false);

		BaseCursorImage = Utilities.QuickConvert("res://Core/Cursorwh.png");
		SpecialCursorImage = Utilities.QuickConvert("res://Core/Cursorpl.png");



		RedRectV = Utilities.QuickConvert("res://Core/Rect_L.png");
		//WhiteRectV = Utilities.ColorConvert(WhiteRectV, Color.Color8(255, 0, 0, 255), Color.Color8(255, 255, 255, 255));
		RedRectH = Utilities.QuickConvert("res://Core/Rect_T.png");

		Previmg1.Create(320, 320, false, Image.Format.Rgba8);
		Previmg2.Create(320, 320, false, Image.Format.Rgba8);
		//ReloadMatrix();
		col1 = Layer1Geo.SelfModulate;
		col2 = Layer2Geo.SelfModulate;
		col3 = Layer3Geo.SelfModulate;
		TEP0 = Utilities.QuickConvert("res://Core/TEP0.png");
		TEP1 = Utilities.QuickConvert("res://Core/TEP1.png");
		TEP2 = Utilities.QuickConvert("res://Core/TEP2.png");
		TEP3 = Utilities.QuickConvert("res://Core/TEP3.png");
		TEP4 = Utilities.QuickConvert("res://Core/TEP4.png");
		TEP5 = Utilities.QuickConvert("res://Core/TEP5.png");
		TEP6 = Utilities.QuickConvert("res://Core/TEP6.png");
		TEP9 = Utilities.QuickConvert("res://Core/TEP9.png");
		TEPCursor = Utilities.QuickConvert("res://Core/TEPCursor.png");
		TEP20 = Utilities.QuickConvert("res://Core/TEP20.png");
		TEP21 = Utilities.QuickConvert("res://Core/TEP21.png");
		TEP22 = Utilities.QuickConvert("res://Core/TEP22.png");
		TEP23 = Utilities.QuickConvert("res://Core/TEP23.png");
		TEP24 = Utilities.QuickConvert("res://Core/TEP24.png");
		TEP25 = Utilities.QuickConvert("res://Core/TEP25.png");
		TEP26 = Utilities.QuickConvert("res://Core/TEP26.png");
		TEP29 = Utilities.QuickConvert("res://Core/TEP29.png");
		TEP2Cursor = Utilities.QuickConvert("res://Core/TEP2Cursor.png");
	}

	public override void _PhysicsProcess(double delta)
	{
		//base._PhysicsProcess(delta);
		RectMin = new Vector2i(Globals.CursorPosition.x - 1,Globals.CursorPosition.y - 1);
		RectMax = new Vector2i(Globals.CursorPosition.x + 1, Globals.CursorPosition.y + 1);

		if(Input.IsActionPressed("TOOL_USE4"/*"F"*/) & !Input.IsActionPressed("TOOL_USE5"/*"V"*/))
		{
			RectDisp = true;
			RectMin = new Vector2i(Globals.CursorPosition.x - 2, Globals.CursorPosition.y - 2);
			RectMax = new Vector2i(Globals.CursorPosition.x + 2, Globals.CursorPosition.y + 2);
		}
		else if (!Input.IsActionPressed("TOOL_USE5"/*"V"*/))
		{
			RectDisp = false;
		}

		if (Input.IsActionPressed("TOOL_USE5"/*"V"*/))
		{
			RectDisp = true;
			RectMin = new Vector2i(Globals.CursorPosition.x - 3, Globals.CursorPosition.y - 3);
			RectMax = new Vector2i(Globals.CursorPosition.x + 3, Globals.CursorPosition.y + 3);
		}
		else if (!Input.IsActionPressed("TOOL_USE4"/*"F"*/))
		{
			RectDisp = false;
		}

		if (Globals.State == EditorState.Tile)
		{
			LayerDisplay();
			if (Input.IsActionJustPressed("NonGeoChangeLayer"))
			{
				if (Globals.CurrentLayer == 3)
				{
					Globals.CurrentLayer = 1;
					GD.Print("Active layer changed to ", Globals.CurrentLayer);
				}
				else { Globals.CurrentLayer += 1; GD.Print("Active layer changed to ", Globals.CurrentLayer); }
			}
		}
		if (Globals.IsRectOn == true)
		{
			AffectRect = new LERect(new Vector2i(Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1), Globals.RectLockPos);
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
				LastCursorPosition = new Vector2i(Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1);
			}

		}
		else
		{
			AffectRect = new LERect(RectMin, RectMax);
			if (LastCursorPosition != new Vector2i(Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1))
			{
				UpdateToolRect();
				//LastCursorPosition = new Vector2i(Globals.CursorPosition.x - 1, Globals.CursorPosition.y - 1);
			}
		}

	}

	public void LayerDisplay()
	{
		if (Globals.IsEditorEvil == false)
		{
			if (Globals.CurrentLayer == 3)
			{
				Layer1Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.1f);
				Layer1Geo.SelfModulate = new Color(col1.r, col1.g, col1.b, 0.1f);
				Layer2Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.7f);
				Layer2Geo.SelfModulate = new Color(col2.r, col2.g, col2.b, 0.0f);
				Layer3Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.9f);
				Layer3Geo.SelfModulate = new Color(col3.r, col2.g, col3.b, 0.1f);

			}
			if (Globals.CurrentLayer == 2)
			{
				Layer1Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.1f);
				Layer1Geo.SelfModulate = new Color(col1.r, col1.g, col1.b, 0.1f);
				Layer2Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.7f);
				Layer2Geo.SelfModulate = new Color(col2.r, col2.g, col2.b, 1f);
				Layer3Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.6f);
				Layer3Geo.SelfModulate = new Color(col3.r, col2.g, col3.b, 0.1f);
			}
			if (Globals.CurrentLayer == 1)
			{
				Layer1Tiles.SelfModulate = new Color(1f, 1f, 1f, 1f);
				Layer1Geo.SelfModulate = new Color(col1.r, col1.g, col1.b, 1f);
				Layer2Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.6f);
				Layer2Geo.SelfModulate = new Color(col2.r, col2.g, col2.b, 0.1f);
				Layer3Tiles.SelfModulate = new Color(1f, 1f, 1f, 0.6f);
				Layer3Geo.SelfModulate = new Color(col3.r, col2.g, col3.b, 0.1f);
			}
		}
		else
		{
			if (Globals.CurrentLayer == 3)
			{
				Layer1Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.1f);
				Layer1Geo.SelfModulate = new Color(col1.r, col1.g, col1.b, 0.1f);
				Layer2Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.7f);
				Layer2Geo.SelfModulate = new Color(col2.r, col2.g, col2.b, 0.0f);
				Layer3Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.9f);
				Layer3Geo.SelfModulate = new Color(col3.r, col2.g, col3.b, 0.1f);

			}
			if (Globals.CurrentLayer == 2)
			{
				Layer1Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.1f);
				Layer1Geo.SelfModulate = new Color(col1.r, col1.g, col1.b, 0.1f);
				Layer2Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.7f);
				Layer2Geo.SelfModulate = new Color(col2.r, col2.g, col2.b, 1f);
				Layer3Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.6f);
				Layer3Geo.SelfModulate = new Color(col3.r, col2.g, col3.b, 0.1f);
			}
			if (Globals.CurrentLayer == 1)
			{
				Layer1Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 1f);
				Layer1Geo.SelfModulate = new Color(col1.r, col1.g, col1.b, 1f);
				Layer2Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.6f);
				Layer2Geo.SelfModulate = new Color(col2.r, col2.g, col2.b, 0.1f);
				Layer3Tiles.SelfModulate = Color.FromHSV(1f, 1f, 1f, 0.6f);
				Layer3Geo.SelfModulate = new Color(col3.r, col2.g, col3.b, 0.1f);
			}
		}









	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Layer1Tiles.Texture = Globals.TETex1;
		Layer2Tiles.Texture = Globals.TETex2;
		Layer3Tiles.Texture = Globals.TETex3;
		try
		{
			if (Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y] is InternalMaterial m)
			{
				Globals.TEToolType = TETools.Material;
				Globals.TEToolData.MaterialOrTileName = m.Name;
				ImageTexture tex = ImageTexture.CreateFromImage(BaseCursorImage);
				Cursor.Offset = new Vector2i(-1, -1);
				Cursor.Texture = tex;
				Cursor.SelfModulate = new Color(1, 1, 1, 1);
			}
			if (Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y] is InternalSpecial d)
			{
				Globals.TEToolType = TETools.Special;
				Cursor.Offset = new Vector2i(-1, -1);
				//ImageTexture tex = ImageTexture.CreateFromImage(SpecialCursorImage);
				ImageTexture tex = ImageTexture.CreateFromImage(BaseCursorImage);
				Cursor.Texture = tex;
				Cursor.SelfModulate = d.Color;
			}
			if (Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y] is InternalTile t)
			{

				Globals.TEToolType = TETools.Tile;
				Globals.TEToolData.MaterialOrTileName = t.Name;
				Globals.TEToolData.TileIndexOrHeadCoords = new Vector2i(Globals.TileCatIndex.x + 1, Globals.TileCatIndex.y + 1);
				DrawTilePreview();
			}
		}
		catch (Exception)
		{
			// do nothing lol
		}

		//GD.Print(Globals.TEToolType);



		if (Apprentice == null)
		{
			Apprentice = GetTree().Root.FindChild("Matrix Display Apprentice Tile", true, false);
		}
		CheckMatrix();
		//if (isfakethreading == true)
		//{
		//	FakeMultithreadingEcksDee();
		//}
	}

	//this is one of the things that slows it down xd
	/*public void FakeMultithreadingEcksDee()
	{
		if (isfakethreading == false)
		{
			isfakethreading = true;
			fakethreadindex = 0;
			fakethreadindexy = 0;
		}
		if (isfakethreading == true)
		{
			if (fakethreadindex < Globals.levelSize.x)
			{
				if (fakethreadindexy < Globals.levelSize.y)
				{
					UpdateTile(fakethreadindex, fakethreadindexy);
					fakethreadindexy++;
					GD.Print("Y Increased To ", fakethreadindexy);
				}
				else
				{
					fakethreadindexy = 0;
					fakethreadindex++;
					GD.Print("X Increased To ", fakethreadindex);
				}
				//for (int c = 0; c < Globals.levelSize.y; c++)
				//{
				//	UpdateTile(fakethreadindex, c);
				//}
				

			}
			else
			{
				GD.Print("Done!");
				UpdateMatrixDisplay();
				isfakethreading = false;
				fakethreadindex = 0;
				fakethreadindexy = 0;
			}
		}
	}*/
	public void DrawTilePreview()
	{
		InternalTile tl = (InternalTile)Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y];
		Vector2i Midpoint = new Vector2i(Convert.ToInt32((tl.Size.x * 0.5) + 0.4999), Convert.ToInt32((tl.Size.y * 0.5) + 0.4999));
		Vector2i Offset = new Vector2i(12, 12) - Midpoint; //wait isn't 3 * 5 = 15?
		Previmg2.Fill(Colors.Transparent);
		Previmg1.Fill(Colors.Transparent);
		if (tl.Specs2 != null)
		{
			//GD.Print("Sweet Little Bingus The III");
			int n = 0;
			for (int q = 0; q < tl.Size.x; q++)
			{
				for (int c = 0; c < tl.Size.y; c++)
				{
					if (tl.Specs2[n] != -1)
					{
						//reflection ftw
						//GD.Print("TEP" + tl.Specs2[n]);
						Image src = (Image)this.GetType().GetField("TEP2" + tl.Specs2[n]).GetValue(this);
						//src = Utilities.NuclearConvert(src, Colors.Transparent, Color.Color8(150, 150, 150));
						Previmg2.BlitRect(src,src.GetUsedRect(),new Vector2i((q + Offset.x) * 16, (c + Offset.y) * 16));
					}
					n++;
				}
			}
			
			Prevtex2 = new ImageTexture();
			Prevtex2.SetImage(Previmg2);
			PreviewLayer2.Texture = Prevtex2;
		}
		Prevtex2 = new ImageTexture();
		Prevtex2.SetImage(Previmg2);
		PreviewLayer2.Texture = Prevtex2;


		int index = 0;
		for (int q = 0; q < tl.Size.x; q++)
		{
			for (int c = 0; c < tl.Size.y; c++)
			{
				if (tl.Specs[index] != -1)
				{
					//reflection ftw
					//GD.Print("TEP" + tl.Specs2[n]);
					Image src = (Image)this.GetType().GetField("TEP" + tl.Specs[index]).GetValue(this);
					//src = Utilities.NuclearConvert(src, Colors.Transparent, Color.Color8(150, 150, 150));
					Previmg1.BlitRect(src, src.GetUsedRect(), new Vector2i((q + Offset.x) * 16, (c + Offset.y) * 16));
				}
				index++;
			}
		}
		Prevtex1 = new ImageTexture();
		Prevtex1.SetImage(Previmg1);
		PreviewLayer1.Texture = Prevtex1;


		Rect2i rec = new Rect2i(tl.PreviewTilePosition, tl.PreviewTilePositionY, tl.Size.x * 16, tl.Size.y * 16);
		Image prev = Globals.TilePreviewAtlas.GetRect(rec);
		ImageTexture tex = ImageTexture.CreateFromImage(prev);
		Cursor.Texture = tex;
		Vector2 cent = new Vector2();
		if(tl.Size.x % 2 != 0)
		{
			cent.x = -(tl.Size.x / 2) * 16;
		}
		else
		{
			cent.x = (-(tl.Size.x / 2) * 16) + 16;
		}
		if (tl.Size.y % 2 != 0)
		{
			cent.y = -(tl.Size.y / 2) * 16;
		}
		else
		{
			cent.y = (-(tl.Size.y / 2) * 16) + 16;
		}
		Cursor.Offset = cent;
		if (IsTilePositionLegal(new Vector2i(Globals.CursorPosition.x -1, Globals.CursorPosition.y - 1)))
		{
			//GD.Print(Cursor.Material);
			//GD.Print()
			Cursor.Material = null;
            Cursor.SelfModulate = new Color(1,1,1);
        }
		else
		{
			Cursor.Material = (CanvasItemMaterial)ResourceLoader.Load("res://Shaders/ADD_Modulate.material");
			Cursor.SelfModulate = new Color(1,0,0);
		}
		//Cursor.Centered = false;
		//GD.Print("Bingus binted");
	}

	public void CheckMatrix()
	{
		if (refmatrix != Globals.tilematrix || (Input.IsActionJustPressed("RELOAD_MATRICES") && Globals.State == EditorState.Tile))
		{
			ResetMatrix();
			refmatrix = Globals.tilematrix;
		}
	}

	public void Action(int x, int y, int layer)
	{
		x -= 1;
		y -= 1;
		layer -= 1;
		switch (Globals.TEToolType)
		{
			case TETools.Material:
				//LERect l = new LERect(x, y, (uint)1, (uint)1);
				Vector2i[] l = new Vector2i[1];
				l[0] = new Vector2i(x, y);
				//GD.Print("ugh");
				if (Input.IsActionPressed("TOOL_USE4"/*"F"*/) & !Input.IsActionPressed("TOOL_USE5"/*"V"*/))
				{
					l = new Vector2i[9] {new Vector2i(x, y), new Vector2i(x + 1,y), new Vector2i(x - 1, y), new Vector2i(x, y + 1), new Vector2i(x, y - 1), new Vector2i(x - 1, y - 1), new Vector2i(x - 1, y + 1), new Vector2i(x + 1, y + 1), new Vector2i(x + 1, y - 1) };
				}
				if (Input.IsActionPressed("TOOL_USE5"/*"V"*/))
				{
					// oh fuck
					l = new Vector2i[25] { new Vector2i(x, y), new Vector2i(x + 1, y), new Vector2i(x - 1, y), new Vector2i(x, y + 1), new Vector2i(x, y - 1), new Vector2i(x - 1, y - 1), new Vector2i(x - 1, y + 1), new Vector2i(x + 1, y + 1), new Vector2i(x + 1, y - 1), new Vector2i(x, y - 2), new Vector2i(x + 1, y - 2), new Vector2i(x + 2, y - 2), new Vector2i(x - 1, y - 2), new Vector2i(x - 2, y - 2), new Vector2i(x, y + 2), new Vector2i(x + 1, y + 2), new Vector2i(x + 2, y + 2), new Vector2i(x - 1, y + 2), new Vector2i(x - 2, y + 2), new Vector2i(x - 2, y), new Vector2i(x - 2, y + 1), new Vector2i(x - 2, y - 1), new Vector2i(x + 2, y), new Vector2i(x + 2, y + 1), new Vector2i(x + 2, y - 1) };


				}
				foreach (Vector2i q in l)
				{
					if (q.x >= 0 & q.x < Globals.levelSize.x & q.y >= 0 & q.y < Globals.levelSize.y)
					{
						if (Globals.tilematrix[q.x, q.y, layer].Type != TETileType.TileHead & Globals.tilematrix[q.x, q.y, layer].Type != TETileType.TileBody)
						{
							Globals.tilematrix[q.x, q.y, layer].Type = TETileType.Material;
							Globals.tilematrix[q.x, q.y, layer].Data = Globals.TEToolData;
						}
						UpdateTile(q.x, q.y);
					}
				}
				UpdateMatrixDisplay();
				break;
			case TETools.Special:
				break;
			case TETools.Tile:
				if(IsTilePositionLegal(new Vector2i(x,y)) | Input.IsActionPressed("TOOL_USE4"/*"F"*/) | Input.IsActionPressed("TOOL_USE7"/*"G"*/))
				{
					PlaceTile(new Vector2i(x,y), Globals.TileCatIndex);
				}
				UpdateMatrixDisplay();
				break;
			default:
				break;
		}
		//Master.UpdateDisplay();
		UpdateMatrixDisplay();
	}
	public void Action2(int x, int y, int layer)
	{
		x -= 1;
		y -= 1;
		layer -= 1;
		DeleteTile(new Vector2i(x, y));
	}

	public bool IsTilePositionLegal(Vector2i pos)
	{
		bool rtrn = true;
		int n = 0;
		InternalTile tl = new InternalTile();
		if (Globals.TEToolType == TETools.Tile)
		{
			tl = (InternalTile)Globals.GTiles[Globals.TileCatIndex.x].tiles[Globals.TileCatIndex.y];

			Vector2i Midpoint = new Vector2i(Convert.ToInt32((tl.Size.x * 0.5) + 0.4999), Convert.ToInt32((tl.Size.y * 0.5) + 0.4999));

			Vector2i Start = pos - Midpoint + new Vector2i(1, 1);

			if (tl.Specs2 != null & Globals.CurrentLayer - 1 < 2)
			{
				for (int q = Start.x; q < Start.x + tl.Size.x; q++)
				{
					for (int c = Start.y; c < Start.y + tl.Size.y; c++)
					{

						if (q < Start.x + tl.Size.x & c < Start.y + tl.Size.y)
						{
							if (tl.Specs2[n] != -1)
							{
								if (q >= 0 & q < Globals.levelSize.x & c < Globals.levelSize.y & c >= 0)
								{
									if (Utilities.AfaMvLevelEdit(new Vector2i(q, c), Globals.CurrentLayer) != tl.Specs2[n] | Globals.tilematrix[q, c, Globals.CurrentLayer].Type == TETileType.TileHead | Globals.tilematrix[q, c, Globals.CurrentLayer].Type == TETileType.TileBody)
									{
										rtrn = false;
										break;
									}
								}

							}
						}
						n++;
					}
					if (rtrn == false)
					{
						break;
					}
				}
			}

			if (rtrn == true)
			{
				n = 0;
				for (int q = Start.x; q < Start.x + tl.Size.x; q++)
				{
					for (int c = Start.y; c < Start.y + tl.Size.y; c++)
					{
						if (q < Start.x + tl.Size.x & c < Start.y + tl.Size.y)
						{
							if (tl.Specs[n] != -1)
							{
								if (q >= 0 & q < Globals.levelSize.x & c < Globals.levelSize.y & c >= 0)
								{
									if (Utilities.AfaMvLevelEdit(new Vector2i(q, c), Globals.CurrentLayer - 1) != tl.Specs[n] | Globals.tilematrix[q, c, Globals.CurrentLayer - 1].Type == TETileType.TileHead | Globals.tilematrix[q, c, Globals.CurrentLayer - 1].Type == TETileType.TileBody)
									{
										rtrn = false;
										break;
									}
								}

							}
						}
						n++;
					}
					if (rtrn == false)
					{
						break;
					}
				}


			}








		}
		return rtrn;
	}

	public void PlaceTile(Vector2i plctile, Vector2i tmpos)
	{
		if(plctile.x < 0 | plctile.y < 0 | plctile.x > Globals.levelSize.x | plctile.y > Globals.levelSize.y)
		{
			return;
		}
		bool forceadaptterrain = Input.IsActionPressed("TOOL_USE7"/*"G"*/);
		InternalTile tl = (InternalTile)Globals.GTiles[tmpos.x].tiles[tmpos.y];
		Vector2i Midpoint = new Vector2i(Convert.ToInt32((tl.Size.x * 0.5) + 0.4999), Convert.ToInt32((tl.Size.y * 0.5) + 0.4999));

		Vector2i Start = plctile - Midpoint + new Vector2i(1, 1);
		Globals.tilematrix[plctile.x,plctile.y,Globals.CurrentLayer - 1].Type = TETileType.TileHead;
		Globals.tilematrix[plctile.x, plctile.y, Globals.CurrentLayer - 1].Data = new TETileData();
		Globals.tilematrix[plctile.x, plctile.y, Globals.CurrentLayer - 1].Data.TileIndexOrHeadCoords = tmpos + new Vector2i(1,1);
		Globals.tilematrix[plctile.x, plctile.y, Globals.CurrentLayer - 1].Data.MaterialOrTileName = tl.Name;
		//UpdateTile(plctile.x, plctile.y);
		//Globals.CurrentLayer starts at one, so for *important* internal bullshittery subtract it by one
		int n = 0;
		if (tl.Specs2 != null & Globals.CurrentLayer < 3)
		{
			//GD.Print(tl.Specs2.Length);
			//might need to go -1 (or -2) for the tile size
			n = 0;
			for (int q = Start.x; q < Start.x + tl.Size.x; q++)
			{
				for (int c = Start.y; c < Start.y + tl.Size.y; c++)
				{
					if (tl.Specs2[n] != -1 & Utilities.IsInsideLevel(q,c))
					{
						//this is supposed to be for the layer BEHIND the active layer, so will an unmodified globals.currentlayer work?
						//e.g. if active layer is 2, then this adds to 3
						// (internally, if active layer is 1, then this adds to 2)
						// "- 1 + 1" is the same as not subtracting or adding
						Globals.tilematrix[q, c, Globals.CurrentLayer].Type = TETileType.TileBody;
						Globals.tilematrix[q, c, Globals.CurrentLayer].Data = new TETileData();
						Globals.tilematrix[q, c, Globals.CurrentLayer].Data.TileIndexOrHeadCoords = new Vector2i(plctile.x + 1, plctile.y + 1);
						Globals.tilematrix[q, c, Globals.CurrentLayer].Data.LayerOrDefault = Globals.CurrentLayer; //might as well be + 1
						UpdateTile(q, c);
						//above is NOT internal, if it was it'd be - 2
						if (forceadaptterrain == true)
						{
							Globals.matrix[q, c, Globals.CurrentLayer].TileID = (Tiles)tl.Specs2[n];
							Master.UpdateTile(q, c);
						}
					}
					else if(Utilities.IsInsideLevel(q,c)) UpdateTile(q, c);
					n++;
				}
			}
		}

		n = 0;
		for (int q = Start.x; q < Start.x + tl.Size.x; q++)
		{
			for (int c = Start.y; c < Start.y + tl.Size.y; c++)
			{
				if (tl.Specs[n] != -1 & Utilities.IsInsideLevel(q,c))
				{
					if (new Vector2i(q,c) != plctile)
					{
						Globals.tilematrix[q, c, Globals.CurrentLayer - 1].Type = TETileType.TileBody;
						Globals.tilematrix[q, c, Globals.CurrentLayer - 1].Data = new TETileData();
						Globals.tilematrix[q, c, Globals.CurrentLayer - 1].Data.TileIndexOrHeadCoords = new Vector2i(plctile.x + 1, plctile.y + 1);
						Globals.tilematrix[q, c, Globals.CurrentLayer - 1].Data.LayerOrDefault = Globals.CurrentLayer;
						UpdateTile(q, c);
					}
					//this is supposed to be for the layer BEHIND the active layer, so will an unmodified globals.currentlayer work?
					//e.g. if active layer is 2, then this adds to 3
					// (internally, if active layer is 1, then this adds to 2)
					// "- 1 + 1" is the same as not subtracting or adding


					//above is NOT internal, if it was it'd be - 2
					if (forceadaptterrain == true)
					{
						Globals.matrix[q, c, Globals.CurrentLayer - 1].TileID = (Tiles)tl.Specs[n];
						Master.UpdateTile(q, c);
					}
				}
				else if (Utilities.IsInsideLevel(q, c)) UpdateTile(q, c);
				n++;
			}
		}

		// lingo stuff starts at 1
		// c# stuff starts at 0
		//example: the index for "big head" is 14, 3 in the .txt
		//but internally (for this editor), "big head" is at 13, 2
		//another example,
		// "Big head" (14,3 -> 13, 2) is at pos 40, 23, 3 (39, 22, 2)
		//3 layers, lingo is 1, 2, 3
		//C# is 0, 1, 2
		UpdateTile(plctile.x, plctile.y);
		Master.UpdateDisplay();
		UpdateMatrixDisplay();
		//BeginClearTileFragments();
	}
	//for when tilebodies aren't obliterated
	public void BeginClearTileFragments()
	{
		for (int q = 0; q < Globals.levelSize.x; q++)
		{
			for (int c = 0; c < Globals.levelSize.y; c++)
			{
				for (int l = 0; l < 3; l++)
				{
					if (Globals.tilematrix[q,c,l].Type == TETileType.TileBody)
					{
						CheckIfTileFragment(q, c, l);
					}
				}
			}
		}
	}
	public void CheckIfTileFragment(int x, int y, int z)
	{
		//bool t = true;
		TETileData dt = Globals.tilematrix[x, y, z].Data;
		if (Globals.tilematrix[dt.TileIndexOrHeadCoords.x - 1, dt.TileIndexOrHeadCoords.y - 1, dt.LayerOrDefault - 1].Type == TETileType.TileHead)
		{
			//t = false;
			TETileData dt2 = Globals.tilematrix[dt.TileIndexOrHeadCoords.x - 1, dt.TileIndexOrHeadCoords.y - 1, dt.LayerOrDefault - 1].Data;
            InternalTile tl = (InternalTile)Globals.GTiles[dt2.TileIndexOrHeadCoords.x - 1].tiles[dt2.TileIndexOrHeadCoords.y - 1];
			//if ()




        }
		else
		{
			ObliterateTileFragment(x, y, z);
		}
	}
	public void ObliterateTileFragment(int x, int y, int z)
	{
        Globals.tilematrix[x,y,z].Type = TETileType.Default;
        Globals.tilematrix[x,y,z].Data = new TETileData();
    }



	public void DeleteTile(Vector2i pos)
	{
		switch (Globals.tilematrix[pos.x,pos.y,Globals.CurrentLayer - 1].Type)
		{
			case TETileType.Default:
				int x1 = pos.x;
				int y1 = pos.y;
				int layer1 = Globals.CurrentLayer - 1;
				//LERect l = new LERect(x, y, (uint)1, (uint)1);
				Vector2i[] l1 = new Vector2i[1];
				l1[0] = new Vector2i(x1, y1);
				//GD.Print("ugh");
				if (Input.IsActionPressed("TOOL_USE4"/*"F"*/) & !Input.IsActionPressed("TOOL_USE5"/*"V"*/))
				{
					l1 = new Vector2i[9] { new Vector2i(x1, y1), new Vector2i(x1 + 1, y1), new Vector2i(x1 - 1, y1), new Vector2i(x1, y1 + 1), new Vector2i(x1, y1 - 1), new Vector2i(x1 - 1, y1 - 1), new Vector2i(x1 - 1, y1 + 1), new Vector2i(x1 + 1, y1 + 1), new Vector2i(x1 + 1, y1 - 1) };
				}
				if (Input.IsActionPressed("TOOL_USE5"/*"V"*/))
				{
					// oh fuck
					l1 = new Vector2i[25] { new Vector2i(x1, y1), new Vector2i(x1 + 1, y1), new Vector2i(x1 - 1, y1), new Vector2i(x1, y1 + 1), new Vector2i(x1, y1 - 1), new Vector2i(x1 - 1, y1 - 1), new Vector2i(x1 - 1, y1 + 1), new Vector2i(x1 + 1, y1 + 1), new Vector2i(x1 + 1, y1 - 1), new Vector2i(x1, y1 - 2), new Vector2i(x1 + 1, y1 - 2), new Vector2i(x1 + 2, y1 - 2), new Vector2i(x1 - 1, y1 - 2), new Vector2i(x1 - 2, y1 - 2), new Vector2i(x1, y1 + 2), new Vector2i(x1 + 1, y1 + 2), new Vector2i(x1 + 2, y1 + 2), new Vector2i(x1 - 1, y1 + 2), new Vector2i(x1 - 2, y1 + 2), new Vector2i(x1 - 2, y1), new Vector2i(x1 - 2, y1 + 1), new Vector2i(x1 - 2, y1 - 1), new Vector2i(x1 + 2, y1), new Vector2i(x1 + 2, y1 + 1), new Vector2i(x1 + 2, y1 - 1) };


				}
				foreach (Vector2i q in l1)
				{
					if (q.x >= 0 & q.x < Globals.levelSize.x & q.y >= 0 & q.y < Globals.levelSize.y)
					{
						if (Globals.tilematrix[q.x, q.y, layer1].Type != TETileType.TileHead & Globals.tilematrix[q.x, q.y, layer1].Type != TETileType.TileBody)
						{
							Globals.tilematrix[q.x, q.y, layer1].Type = TETileType.Default;
							Globals.tilematrix[q.x, q.y, layer1].Data = new TETileData();
						}
						UpdateTile(q.x, q.y);
						//Master.UpdateDisplay();
					}
				}
				break;
			case TETileType.Material:
				int x = pos.x;
				int y = pos.y;
				int layer = Globals.CurrentLayer - 1;
				//LERect l = new LERect(x, y, (uint)1, (uint)1);
				Vector2i[] l = new Vector2i[1];
				l[0] = new Vector2i(x, y);
				//GD.Print("ugh");
				if (Input.IsActionPressed("TOOL_USE4"/*"F"*/) & !Input.IsActionPressed("TOOL_USE5"/*"V"*/))
				{
					l = new Vector2i[9] { new Vector2i(x, y), new Vector2i(x + 1, y), new Vector2i(x - 1, y), new Vector2i(x, y + 1), new Vector2i(x, y - 1), new Vector2i(x - 1, y - 1), new Vector2i(x - 1, y + 1), new Vector2i(x + 1, y + 1), new Vector2i(x + 1, y - 1) };
				}
				if (Input.IsActionPressed("TOOL_USE5"/*"V"*/))
				{
					// oh fuck
					l = new Vector2i[25] { new Vector2i(x, y), new Vector2i(x + 1, y), new Vector2i(x - 1, y), new Vector2i(x, y + 1), new Vector2i(x, y - 1), new Vector2i(x - 1, y - 1), new Vector2i(x - 1, y + 1), new Vector2i(x + 1, y + 1), new Vector2i(x + 1, y - 1), new Vector2i(x, y - 2), new Vector2i(x + 1, y - 2), new Vector2i(x + 2, y - 2), new Vector2i(x - 1, y - 2), new Vector2i(x - 2, y - 2), new Vector2i(x, y + 2), new Vector2i(x + 1, y + 2), new Vector2i(x + 2, y + 2), new Vector2i(x - 1, y + 2), new Vector2i(x - 2, y + 2), new Vector2i(x - 2, y), new Vector2i(x - 2, y + 1), new Vector2i(x - 2, y - 1), new Vector2i(x + 2, y), new Vector2i(x + 2, y + 1), new Vector2i(x + 2, y - 1) };


				}
				foreach (Vector2i q in l)
				{
					if (q.x >= 0 & q.x < Globals.levelSize.x & q.y >= 0 & q.y < Globals.levelSize.y)
					{
						if (Globals.tilematrix[q.x, q.y, layer].Type != TETileType.TileHead & Globals.tilematrix[q.x, q.y, layer].Type != TETileType.TileBody)
						{
							Globals.tilematrix[q.x, q.y, layer].Type = TETileType.Default;
							Globals.tilematrix[q.x, q.y, layer].Data = new TETileData();
						}
						UpdateTile(q.x, q.y);
						//Master.UpdateDisplay();
					}
				}
				break;
			case TETileType.TileHead:
				DeleteTileTile(pos, Globals.CurrentLayer - 1);
				break;
			case TETileType.TileBody:
				TETileData dt = Globals.tilematrix[pos.x, pos.y, Globals.CurrentLayer - 1].Data;
				if (dt.TileIndexOrHeadCoords.x >= 0 & dt.TileIndexOrHeadCoords.y >= 0 & dt.TileIndexOrHeadCoords.x <= Globals.levelSize.x & dt.TileIndexOrHeadCoords.y <= Globals.levelSize.y)
				{
					if (Globals.tilematrix[dt.TileIndexOrHeadCoords.x - 1, dt.TileIndexOrHeadCoords.y - 1, dt.LayerOrDefault - 1].Type == TETileType.TileHead)
					{
						DeleteTileTile(dt.TileIndexOrHeadCoords - new Vector2i(1,1), dt.LayerOrDefault - 1);
					}
					else
					{
						Globals.tilematrix[pos.x, pos.y, Globals.CurrentLayer - 1].Type = TETileType.Default;
						Globals.tilematrix[pos.x, pos.y, Globals.CurrentLayer - 1].Data = new TETileData();
					}
				}
				else
				{
					Globals.tilematrix[pos.x, pos.y, Globals.CurrentLayer - 1].Type = TETileType.Default;
					Globals.tilematrix[pos.x, pos.y, Globals.CurrentLayer - 1].Data = new TETileData();
				}
				break;
			default:
				break;
		}
		UpdateTile(pos.x, pos.y);
		UpdateMatrixDisplay();
		BeginClearTileFragments();
	}
	
	public void DeleteTileTile(Vector2i pos, Int32 layer) //int32 because i'm feeling VERBOSE today
	{
		TETileData dt = Globals.tilematrix[pos.x, pos.y, layer].Data;
		InternalTile tl = (InternalTile)Globals.GTiles[dt.TileIndexOrHeadCoords.x - 1].tiles[dt.TileIndexOrHeadCoords.y - 1];
		Vector2i Midpoint = new Vector2i(Convert.ToInt32((tl.Size.x * 0.5) + 0.4999), Convert.ToInt32((tl.Size.y * 0.5) + 0.4999));

		Vector2i Start = pos - Midpoint + new Vector2i(1, 1);
		int n = 0;

		if (tl.Specs2 != null & layer < 2)
		{
			n = 0;
			for (int q = Start.x; q < Start.x + tl.Size.x; q++)
			{
				for (int c = Start.y; c < Start.y + tl.Size.y; c++)
				{
					if (Utilities.IsInsideLevel(q,c))
					{
						if (q < Start.x + tl.Size.x & c < Start.y + tl.Size.y)
						{
							if (tl.Specs2[n] != -1)
							{
								Globals.tilematrix[q, c, layer + 1].Type = TETileType.Default;
								//Globals.tilematrix[q, c, Globals.CurrentLayer].Data.TileIndexOrHeadCoords = plctile;
								//Globals.tilematrix[q, c, Globals.CurrentLayer].Data.MaterialOrTileName = null;
								//Globals.tilematrix[q, c, Globals.CurrentLayer].Data.LayerOrDefault = Globals.CurrentLayer - 1;
								Globals.tilematrix[q, c, layer + 1].Data = new TETileData();
							}
						}
						//n++;
						UpdateTile(q, c);
						//Master.UpdateTile(q, c);
					}
					n++;
				}
			}
		}
		n = 0;
		for (int q = Start.x; q < Start.x + tl.Size.x; q++)
		{
			for (int c = Start.y; c < Start.y + tl.Size.y; c++)
			{
				if (Utilities.IsInsideLevel(q,c))
				{
					if (q < Start.x + tl.Size.x & c < Start.y + tl.Size.y)
					{
						if (tl.Specs[n] != -1)
						{
							Globals.tilematrix[q, c, layer].Type = TETileType.Default;
							//Globals.tilematrix[q, c, Globals.CurrentLayer].Data.TileIndexOrHeadCoords = plctile;
							//Globals.tilematrix[q, c, Globals.CurrentLayer].Data.MaterialOrTileName = null;
							//Globals.tilematrix[q, c, Globals.CurrentLayer].Data.LayerOrDefault = Globals.CurrentLayer - 1;
							Globals.tilematrix[q, c, layer].Data = new TETileData();
						}
					}
					//n++;
					UpdateTile(q, c);
					//Master.UpdateTile(q, c);
				}
				n++;
			}
		}
		UpdateMatrixDisplay();
	}
	
	/**/
	public void UpdateTile(int x, int y)
	{
		for (int layer = 0; layer < 3; layer++)
		{
			//GD.Print(layer);
			Rect2i rec = new Rect2i(new Vector2i(x * 16, y * 16), 16, 16);
			LEQuad rct = new LEQuad(new Vector2(x * 16, y * 16), new Vector2((x * 16) + 16, y * 16), new Vector2((x * 16) + 16, (y * 16) + 16), new Vector2(x * 16, (y * 16) + 16));
			//LERect ret = new LERect(new Vector2i(((q - 1) * 16) + 5, ((c - 1) * 16) + 5), new Vector2i((q * 16) - 5, (c * 16) - 5));
			if (Globals.tilematrix[x, y, layer].Type != TETileType.TileBody)
			{
				switch (layer)
				{
					case 0:
						Globals.TEImage1.FillRect(rec, Colors.Transparent);
						break;
					case 1:
						Globals.TEImage2.FillRect(rec, Colors.Transparent);
						break;
					case 2:
						Globals.TEImage3.FillRect(rec, Colors.Transparent);
						break;
					default:
						break;
				}
			}
			switch (Globals.tilematrix[x, y, layer].Type)
			{
				case TETileType.Material:
					LERect ret = new LERect((x * 16) + 5, (y * 16) + 5, (uint)16 - 10, 16 - 10);
					rct.P1 = new Vector2(ret.left, ret.top);
					rct.P2 = new Vector2(ret.right, ret.top);
					rct.P3 = new Vector2(ret.right, ret.bottom);
					rct.P4 = new Vector2(ret.left, ret.bottom);

					switch (Globals.matrix[x, y, layer].TileID)
					{
						case Tiles.Air:
							rct.P1 = new Vector2(-1, -1);
							rct.P2 = new Vector2(-1, -1);
							rct.P3 = new Vector2(-1, -1);
							rct.P4 = new Vector2(-1, -1);
							break;
						case Tiles.Wall:
							break;
						case Tiles.SlopeSE:
							rct.P1 = new Vector2(ret.left, ret.top);
							rct.P2 = new Vector2(ret.left, ret.top);
							rct.P3 = new Vector2(ret.right, ret.bottom);
							rct.P4 = new Vector2(ret.left, ret.bottom);
							break;
						case Tiles.SlopeSW:
							rct.P1 = new Vector2(ret.right, ret.top);
							rct.P2 = new Vector2(ret.right, ret.top + 5);
							rct.P3 = new Vector2(ret.left, ret.bottom);
							rct.P4 = new Vector2(ret.right, ret.bottom);
							break;
						case Tiles.SlopeNE:
							rct.P1 = new Vector2(ret.left, ret.bottom);
							rct.P2 = new Vector2(ret.left, ret.bottom - 5);
							rct.P3 = new Vector2(ret.right, ret.top);
							rct.P4 = new Vector2(ret.left, ret.top);
							break;
						case Tiles.SlopeNW: //i regret naming them something other than numbers
							rct.P1 = new Vector2(ret.right, ret.bottom);
							rct.P2 = new Vector2(ret.right, ret.bottom);
							rct.P3 = new Vector2(ret.left, ret.top);
							rct.P4 = new Vector2(ret.right, ret.top);
							break;
						case Tiles.Floor:
							ret = new LERect((x * 16) + 5, (y * 16) + 5, (uint)16 - 8, 16 - 13);
							rct.P1 = new Vector2(ret.left, ret.top);
							rct.P2 = new Vector2(ret.right, ret.top);
							rct.P3 = new Vector2(ret.right, ret.bottom + 1);
							rct.P4 = new Vector2(ret.left, ret.bottom + 1);
							break;
						case Tiles.ConnectedShortcut:
							break;
						case Tiles.Glass:
							break;
						default:
							break;
					}
					Color cl = Color.Color8(255, 255, 255, 255);



					//for (int i = 0; i < 3; i++)
					//{
					//	for (int j = 0; j < Globals.GTiles[i].tiles.Count; j++)
					//	{
					//		if (Globals.GTiles[i].tiles[j] is InternalMaterial d)
					//		{
					//
					//		}
					//	}
					//}
					Vector2i Value;
					if (Globals.MaterialsDictionary.TryGetValue(Globals.tilematrix[x,y,layer].Data.MaterialOrTileName, out Value))
					{
						if (Globals.GTiles[Value.x].tiles[Value.y] is InternalMaterial d)
						{
							cl = d.Color;
						}
					}



					//gotcolor:

					switch (layer)
					{
						case 0:
							Globals.TEImage1 = Utilities.Qblit(Globals.TEImage1, rct, cl);
							break;
						case 1:
							Globals.TEImage2 = Utilities.Qblit(Globals.TEImage2, rct, cl);
							break;
						case 2:
							Globals.TEImage3 = Utilities.Qblit(Globals.TEImage3, rct, cl);
							break;
						default:
							break;
					}



					break;
				case TETileType.TileHead:
					InternalTile tl = new InternalTile();

					Vector2i Value2;
					for (int i = 0; i < Globals.TileDictionaries.Count; i++)
					{
						if (Globals.TileDictionaries[i].TryGetValue(Globals.tilematrix[x, y, layer].Data.MaterialOrTileName, out Value2))
						{
							//GD.Print(Globals.GTiles.Count, " x ", Value2.x);
							//GD.Print(Globals.GTiles[Value2.x].tiles.Count, " y ", Value2.y);
							try
							{
								if (Globals.GTiles[Value2.x].tiles[Value2.y - 1] is InternalTile d)
								{
									tl = d;
								}
							}
							catch
							{
								GD.Print("Woah!", Value2.x, " ", Value2.y - 1);
								GD.Print("Max size is ", Globals.GTiles[Value2.x].tiles.Count);
							}
						}
					}


					//tl = (InternalTile)Globals.GTiles[6].tiles[4];
					Color clr = Globals.GTiles[Globals.tilematrix[x, y, layer].Data.TileIndexOrHeadCoords.x - 1].CategoryColor;

					Vector2i Midpoint = new Vector2i(Convert.ToInt32((tl.Size.x * 0.5) + 0.4999), Convert.ToInt32((tl.Size.y * 0.5) + 0.4999));

					Vector2i Start = new Vector2i(x, y) - Midpoint + new Vector2i(1, 1); // og leditor code also subracts from the camera position

					if (tl.Specs2 != null & layer < 3)
					{
						//GD.Print(layer);
						for (int g = Start.x; g < Start.x + tl.Size.x; g++)
						{
							for (int h = Start.y; h < Start.y + tl.Size.y; h++)
							{
								bool draw = true;
								if(g < Start.x + tl.Size.x )
								{
									if (h < Start.y + tl.Size.y)
									{
										//int specindex = (h - Start.y) + (g - Start.x) * tl.Size.y + 0;
										//GD.Print(specindex);
										//GD.Print(tl.Specs2.Length, " spec2length");
										//GD.Print(tl.Specs2[specindex], " what");
										//if (tl.Specs2[specindex])
										if (tl.Specs2[(h - Start.y) + (g - Start.x) * tl.Size.y + 0] == -1)
										{
											draw = false;
										}
									}
								}

								//the og leditor code has an else if here that relies on the camera position

								if (draw == true)
								{
									//Rect2i rect2 = new Rect2i();// forced to use godot's BS, this won't end well xd
									//rect2.Position = new Vector2i((g - 1) * 16, (h - 1) * 16);
									//rect2.End = new Vector2i(g * 16, h * 16);
									LERect rect2 = new LERect((g) * 16, (h) * 16, (g) * 16, (h) * 16);
									LERect rect3 = rect2 + new LERect(tl.PreviewTilePosition, tl.PreviewTilePositionY, tl.PreviewTilePosition + 16, tl.PreviewTilePositionY + 16) - new LERect(Start.x * 16, Start.y * 16, Start.x * 16, Start.y * 16);

									Image img = Globals.TilePreviewAtlas.GetRect((Rect2i)rect3.GetRect2());
									Color clr2 = clr;
									clr2.Darkened(0.5f);
									img = Utilities.NuclearConvert(img, Colors.Transparent, clr2);
									Rect2i imgrec = new Rect2i(0, 0, img.GetWidth(), img.GetHeight());
									switch (layer)
									{
										case 0:
											Globals.TEImage2.BlitRect(img, imgrec, new Vector2i(rect2.left, rect2.top));
											break;
										case 1:
											Globals.TEImage3.BlitRect(img, imgrec, new Vector2i(rect2.left, rect2.top));
											break;
										default:
											break;
									}
								}
							}
						}
					}

					for (int g = Start.x; g < Start.x + tl.Size.x; g++)
					{
						for (int h = Start.y; h < Start.y + tl.Size.y; h++)
						{
							bool draw = true;

							//USELESS USELESS USELESS
							//if (tl.Specs[(h - Start.y) + (g - Start.x) * tl.Size.y] == -1)
							//{
							//	draw = false;
							//}
							if (g < Start.x + tl.Size.x)
							{
								if (h < Start.y + tl.Size.y)
								{
									//int specindex = (h - Start.y) + (g - Start.x) * tl.Size.y + 0;
									//GD.Print(specindex);
									//GD.Print(tl.Specs.Length, " speclength");
									//GD.Print(tl.Specs2[specindex], " what");
									//if (tl.Specs2[specindex])
									if (tl.Specs == null)
									{
										GD.Print("THIS ISN'T SUPPOSED TO HAPPEN ", tl.Name);
										//oh my god i just found this fuuuuuuuuuuuuucking hell
									}


									try
									{
                                        if (tl.Specs[(h - Start.y) + (g - Start.x) * tl.Size.y] == -1)
                                        {
                                            draw = false;
                                        }
                                    }
									catch (NullReferenceException ex)
									{
                                        GD.Print(tl.Specs.Length);
                                        GD.Print(g, " gh ", h);
                                        GD.Print((h - Start.y) + (g - Start.x) * tl.Size.y, " <- funny number");
                                        GD.Print(tl.Name);
										throw ex;
                                    }
								}
							}
							if (draw == true)
							{
								//todo FIX THIS SHIT
								LERect rect2 = new LERect((g) * 16, (h) * 16, (g) * 16, (h) * 16);
								LERect rect3 = rect2 + new LERect(tl.PreviewTilePosition, tl.PreviewTilePositionY, tl.PreviewTilePosition + 16, tl.PreviewTilePositionY + 16) - new LERect((Start.x) * 16, (Start.y) * 16, (Start.x) * 16, (Start.y) * 16);

								Image img = Globals.TilePreviewAtlas.GetRect((Rect2i)rect3.GetRect2());
								//img = Utilities.NuclearConvert(img, Colors.Transparent, clr);
								Rect2i imgrec = new Rect2i(0, 0, img.GetWidth(), img.GetHeight());
								//GD.Print(rect2.GetRect2());
								//now let's do this my way // nvm
								//Rect2i rec2 = new Rect2i(tl.PreviewTilePosition, )

								switch (layer)
								{
									case 0:
										Globals.TEImage1.BlitRect(img, imgrec, new Vector2i(rect2.left, rect2.top));
										break;
									case 1:
										Globals.TEImage2.BlitRect(img, imgrec, new Vector2i(rect2.left, rect2.top));
										break;
									case 2:
										Globals.TEImage3.BlitRect(img, imgrec, new Vector2i(rect2.left, rect2.top));
										break;
									default:
										break;
								}
							}




						}
					}







					break;
				default:
					break;
			}


		}






	}


	public void UpdateMatrixDisplay()
	{
		if (Globals.TETex1 == null) // TETex1 is null!!!!!!
		{
			//but for some reason, TETex1 cannot be TEImage1?????
			// but also it can be itself if TETEX2 does the assignment???
			Globals.TETex1 = ImageTexture.CreateFromImage(Globals.TEImage1);
		}
		if (Globals.TETex2 == null)
		{
			Globals.TETex2 = ImageTexture.CreateFromImage(Globals.TEImage2);
		}
		if (Globals.TETex3 == null)
		{
			Globals.TETex3 = ImageTexture.CreateFromImage(Globals.TEImage3);
		}

		Globals.TETex1.SetImage(Globals.TEImage1);
		Globals.TETex2.SetImage(Globals.TEImage2);
		Globals.TETex3.SetImage(Globals.TEImage3);
		Layer1Tiles.Texture = Globals.TETex1;
		Layer2Tiles.Texture = Globals.TETex2;
		Layer3Tiles.Texture = Globals.TETex3;
	}




	public void ResetMatrix()
	{
		//Globals.LevelEditImageShortcuts.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		//Globals.LevelEditImageShortcuts.Fill(Colors.White);
		//Globals.LevelEditImageShortcuts.Resize(16, 16);
		//Globals.LevelEditImageShortcuts.Fill(Colors.White);
		Globals.TEImage1.Fill(Colors.Transparent);
		Globals.TEImage2.Fill(Colors.Transparent);
		Globals.TEImage3.Fill(Colors.Transparent);
		Globals.TEImage1.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.TEImage2.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		Globals.TEImage3.Resize((int)Globals.levelSize.x * 16, (int)Globals.levelSize.y * 16);
		ReloadMatrix();
	}
	//extraordinarily inefficient, replace asap!!!!
	public void ReloadMatrix()
	{
		//Globals.TEImage1.Create((int)Globals.levelSize.x * 16,(int)Globals.levelSize.y * 16, false, Image.Format.Rgba8);
		//GD.Print("Reloading tile display matrix! This will lag!");
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		for(int q = 0; q < Globals.levelSize.x; q++)
		{
			for (int c = 0; c < Globals.levelSize.y; c++)
			{
				UpdateTile(q, c);
			}
		}
		stopwatch.Stop();
		GD.Print("Done!, tile display took ", stopwatch.ElapsedMilliseconds, "ms");
		//FakeMultithreadingEcksDee();



		if (Globals.TETex1 == null) // TETex1 is null!!!!!!
		{
			//but for some reason, TETex1 cannot be TEImage1?????
			// but also it can be itself if TETEX2 does the assignment???
			Globals.TETex1 = ImageTexture.CreateFromImage(Globals.TEImage1);
		}
		if (Globals.TETex2 == null)
		{
			Globals.TETex2 = ImageTexture.CreateFromImage(Globals.TEImage2);
		}
		if (Globals.TETex3 == null)
		{
			Globals.TETex3 = ImageTexture.CreateFromImage(Globals.TEImage3);
		}

		Globals.TETex1.SetImage(Globals.TEImage1);
		Globals.TETex2.SetImage(Globals.TEImage2);
		Globals.TETex3.SetImage(Globals.TEImage3);
		Layer1Tiles.Texture = Globals.TETex1;
		Layer2Tiles.Texture = Globals.TETex2;
		Layer3Tiles.Texture = Globals.TETex3;



		//uncomment when figure out way to do stuff
		/*uint X = 0;
		uint Y = 0;
		uint Z = 0;
		foreach (TETile tile in Globals.tilematrix)
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
		}*/

	}
	public void ClearToolRect()
	{
		RedRect.Fill(Color.FromHSV(0, 0, 0, 0));
		redrect = new ImageTexture();
		redrect.SetImage(RedRect);
		UI.Texture = redrect;
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
		if (RectDisp == true)
		{
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
		}

		redrect = new ImageTexture();
		redrect.SetImage(RedRect);
		UI.Texture = redrect;


	}

	public void DrawToolRect(int x, int y)
	{
		LERect displayToolRect;
		if (Globals.IsRectOn == true)
		{
			displayToolRect = new LERect(AffectRect.left - 1, AffectRect.top - 1, AffectRect.right + 1, AffectRect.bottom + 1);
		}
		else
		{
			displayToolRect = new LERect(AffectRect.left - 1, AffectRect.top - 1, AffectRect.right - 1, AffectRect.bottom - 1);
		}
		
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
}
