using Godot;
using System;




namespace RainWorldMonsoon;
public static class RNG : object
{
	//Special thanks to PJB and Drizzle for reverse engineering adobe director's RNG algorithm
	//I will remove this if I am asked to, considering it's not my code (it's just heavily modified for me to understand)
	//no seriously i will remove this if i am asked to

	//for reference, lingo's "_system.milliseconds" is Environment.TickCount, which only works when a computer has been on for less than 25 days
	private static uint RandomSeed = 1; // useless because it's set to something else as soon as possible XD


	public static int Seed
	{
		get => (int)RandomSeed;
		set
		{
			RandomSeed = (uint)value;
		}
	}

	//the seed changes all the time and the init doesn't move at all, so i am super hardcoding it
	//internal struct RngState
	//{
	//	public uint Seed;
	//	public uint Init;
	//}


	public static int Random(int Mod = 0)
	{
		//-1560281088 or 2734686208
		//0xA3000000
		uint XORVal = 2734686208;
		if ((RandomSeed & 1) == 0)
		{
			RandomSeed >>= 1;
		}
		else
		{
			RandomSeed = RandomSeed >> 1 ^ XORVal;
		}


		//0x47
		int Returnval = Randomize(RandomSeed * 71);

		if(Mod > 0)
		{
			//0x7FFFFFFF
			Returnval = (int)((long)(ulong)(Returnval & 2147483647) % (long)Mod);
		}
		return Returnval + 1;
	}

	//this is the magic of the rng
	//once more special thanks to PJB and Drizzle for figuring this shit out
	//and if i am asked to i will remove this
	private static int Randomize(uint Value)
	{
		//0xd = 13
		//0x15 = 21
		var var1 = (int)((Value << 13 ^ Value) - ((int)Value >> 21));
		//0x3d73 = 15731
		//0xc0ae5 = 789221
		//0xd208dd0d = 3523796237
		//0x7FFFFFFF = 2147483647
		var var2 = (uint)(((var1 * var1 * 15731 + 789221) * var1 + 3523796237 & 2147483647) + var1);
		//0x2000 = 8192
		//0x15 = 21
		return (int)((var2 * 8192 ^ var2) - ((int)var2 >> 21));
	}

}
