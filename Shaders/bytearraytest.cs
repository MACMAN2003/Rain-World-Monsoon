using Godot;
using System;
using System.Linq;

namespace RainWorldMonsoon;


public partial class bytearraytest : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        int[] sizearr = new int[2];
        sizearr[0] = 200;
        sizearr[1] = 693;
		foreach (int i in sizearr)
		{
			GD.Print(i + " C# BASE");
		}
		GD.Print(sizearr.Length + " LENGTH ");
        byte[] SizeData = new byte[sizearr.Length * sizeof(int)];
        Buffer.BlockCopy(sizearr, 0, SizeData, 0, SizeData.Length);


        //int[] bytesAsInts = Array.ConvertAll(SizeData, c => (int)c);

        foreach (int b in SizeData)
		{
			GD.Print(b + " C# "); // it outputs "200, 0, 0, 0, 181, 2, 0, 0 same as the gdscript code
		}
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
