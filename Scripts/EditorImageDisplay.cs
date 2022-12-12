using Godot;
using System;


//why didn't i think of this before xd


namespace RainWorldMonsoon;
public partial class EditorImageDisplay : Sprite2D
{

	public enum WhichTexture
	{
		Layer1 = 0,
		Layer2 = 1,
		Layer3 = 2,
		Shortcuts = 4,
		TilePreview = 5//,
		//TilePreviewAtlas = 6
	}
	[Export] public WhichTexture whichtext;
	bool set = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta) // unfortunately this has to update every frame ;-;
	{
		if (whichtext == WhichTexture.Layer1)
		{
			Texture = Globals.LevelEditorLayer1;
		}
		if (whichtext == WhichTexture.Layer2)
		{
			Texture = Globals.LevelEditorLayer2;
		}
		if (whichtext == WhichTexture.Layer3)
		{
			Texture = Globals.LevelEditorLayer3;
		}
		if (whichtext == WhichTexture.Shortcuts)
		{
			Texture = Globals.LevelEditorShortcutLayer;
		}
		if (whichtext == WhichTexture.TilePreview)
		{
			Visible = Globals.Settings.ShowTileAtlas;
			Texture = Globals.TilePreviewTexture;
		}
		/*if (whichtext == WhichTexture.TilePreviewAtlas)
		{
			if (set == false)
			{
				Texture = Globals.TilePreviewTexture;
				set = true;
			}
		}*/ //lol
	}
}
