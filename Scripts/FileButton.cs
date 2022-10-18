using Godot;
using System;
using System.Runtime.CompilerServices;
using System.IO;

namespace RainWorldMonsoon;
public partial class FileButton : MenuButton
{
	//private PopupMenu _menu;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//_menu = GetPopup();
		var _menu = GetPopup();
		Callable InSignal = new Callable(this, nameof(LoadPressed));
		_menu.Connect("id_pressed", InSignal);
	}


	public void LoadPressed(int id)
	{
		if (id == 1)
		{
			LEditor leditor = GetNode<LEditor>("/root/Level Editor");
			leditor.OpenLoadFileDialog();
			//GD.Print("Opened load file dialog!");

			//string printstring = OS.GetExecutablePath();
			//GD.Print(printstring);
		}
		if (id == 2)
		{
            LEditor leditor = GetNode<LEditor>("/root/Level Editor");
            leditor.SaveToFile();
        }
		if (id == 3)
		{
            LEditor leditor = GetNode<LEditor>("/root/Level Editor");
            leditor.OpenSaveFileDialog();
        }
	}




}
