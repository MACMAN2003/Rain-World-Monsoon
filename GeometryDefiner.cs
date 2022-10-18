using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace RainWorldMonsoon;



public struct GeometryTile
{
	public Tiles TileID; // air is 0, wall is 1, slopes are 2 3 4 5, droppable floor is 6, shortcut is 7, glass is 9 (community editor only?)

	public List<Tiles2> Tile2IDS; // list is empty if there's nothing in the geometry tile
    /*
		geo editor tile 2 IDS: (null if nothing)
		1 = horizontal beam (works on all 3 layers)
		2 = vertical beam (works on all 3 layers)
		3 = batfly nest
		4 = shortcut
		5 = shortcut connector dot
		6 = entrance
		7 = dragon den (enemy entrance)
		9 = rock
		10 = spear
		11 = carve / crack (works on all 3 layers)
		12 = forbid batfly chain
		13 = garbage worm hole
		14 = ?? (deprecated?)
		15 = ?? (deprecated?)
		16 = ?? (deprecated?)
		17 = ?? (deprecated?)
		18 = waterfall
		19 = whack a mole hole
		20 = worm grass
		21 = scavenger hole
	*/
}




//this won't work
//public struct Square 
//{
	//public GeometryTile[ , , ] Layers; //is this the right way to use this? // no it's not lol


	/*
	public GeometryTile Layer1Tile;
	public GeometryTile Layer2Tile;
	public GeometryTile layer3Tile;
	*/
//}


//lol i need a 3d array not a nested one :P
//public struct Column
//	//public Square[] Row;
//}



public static partial class GeometryDefiner : object
{
	
	
	
	
	
	
	
	
}
