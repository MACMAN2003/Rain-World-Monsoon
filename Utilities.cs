using Godot;
using Godot.NativeInterop;
using System;
using System.IO;
using System.Linq;

namespace RainWorldMonsoon;
public static class Utilities : object
{
	//static Color DefaultSourceColor = new Color("#FFFFFFFF");
	//static Color DefaultPreferredColor = new Color("#00000000");
	public static Image QuickConvert(string path, Image.Format format = Image.Format.Rgba8)
	{
		if (ResourceLoader.Exists(path))
		{
			Texture2D texture = (Texture2D)GD.Load(path);
			Image image = texture.GetImage();
			image.Convert(format);
			return image;
		}
		else
		{
			throw new DirectoryNotFoundException(String.Format("{0} does not exist!", path));
		}






	}
	public static Image QuickConvert(Image image, Image.Format format = Image.Format.Rgba8)
	{
		image.Convert(format);
		return image;
	}

	public static RenderingDevice RD;
	//public static RDShaderFile shaderf;
	public static RDShaderSPIRV spirv;
	public static RID SHDR;
	//public static RDUniform ReadUniform;
	//public static RDUniform WriteUniform;
	//public static RDUniform SizeUniform; //lol
	//public static RDUniform TargUniform;
	//public static RDUniform DesUniform;

	//for super fast gpu color conversion
	//not very fast >:(
	//what it does now: convert image to 0-255 "byte" (wink, wink) array, send it to the gpu, get "byte" array back from then convert the "byte" array to a real byte array and make a new image from that
	//what it's supposed to do: above but real snazzy like (and fast) (read in a super mutant voice)
	public static Image SuperConvert(Image Img, Color TargetColor, Color DesiredColor)
	{
		//ok now fix this cause it doesn't free the rids
		if (RD == null)
		{
			RD = RenderingServer.CreateLocalRenderingDevice();
		}
		//RenderingDevice 
		RDShaderFile shaderf = GD.Load<RDShaderFile>("res://Shaders/TileColorConvertTest2.glsl");
		RDShaderSPIRV spirv = shaderf.GetSpirv();
		//GD.Print(SHDR.Id);
		RID SHDR = RD.ShaderCreateFromSpirv(spirv);

		byte[] ImgData = Img.GetData();
		int[] IntData = new int[ImgData.Length];
		Array.Copy(ImgData, IntData, IntData.Length);

		byte[] ReadData = new byte[IntData.Length * sizeof(int)]; // r g b a 0 - 255 for each pixel, which is height * width
		//disregard above line's comment, it is now r 0 0 0 g 0 0 0 b 0 0 0 a 0 0 0 and the rgba is in 0-255 the other bytes are just there for ease of access
		//yeah this ain't working out the way i want it to xd, time to go crazy mode

		Buffer.BlockCopy(IntData, 0, ReadData, 0, ReadData.Length);
		/*
		GD.Print(ReadData.Length, " inlen");
		for(int i = 0; i < 500; i++)
		{
			GD.Print(ReadData[i], " in");
		}
		*/
		RID ReadBuffer = RD.StorageBufferCreate((uint)ReadData.Length, ReadData);

		RDUniform ReadUniform = new RDUniform();
		ReadUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		ReadUniform.Binding = 0;
		ReadUniform.AddId(ReadBuffer);


		byte[] WriteData;

		WriteData = new byte[ReadData.Length / 4];

		RID WriteBuffer = RD.StorageBufferCreate((uint)WriteData.Length, WriteData);
		RDUniform WriteUniform = new RDUniform();
		WriteUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		WriteUniform.Binding = 1;
		WriteUniform.AddId(WriteBuffer);


		int[] sizearr = new int[2];
		sizearr[0] = Img.GetWidth();
		sizearr[1] = Img.GetHeight();
		byte[] SizeData = new byte[sizearr.Length * sizeof(int)];
		Buffer.BlockCopy(sizearr, 0, SizeData, 0, SizeData.Length);
		//hold on brb gotta test this with arbitrary numbers in gdscript as well as c# to see if i get the same number
		//arbnum 1 is 200 arbnum 2 is 693
		//it's the same
		RID SizeBuffer = RD.StorageBufferCreate(8, SizeData);
		RDUniform SizeUniform = new RDUniform();
		SizeUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		SizeUniform.Binding = 2;
		SizeUniform.AddId(SizeBuffer);

		int[] col1arr = new int[4];
		col1arr[0] = TargetColor.r8;
		col1arr[1] = TargetColor.g8;
		col1arr[2] = TargetColor.b8;
		col1arr[3] = TargetColor.a8;

		byte[] TargetColorData = new byte[col1arr.Length * sizeof(int)];
		Buffer.BlockCopy(col1arr, 0, TargetColorData, 0, TargetColorData.Length);

		RID TargBuffer = RD.StorageBufferCreate((uint)TargetColorData.Length, TargetColorData);
		RDUniform TargUniform = new RDUniform();
		TargUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		TargUniform.Binding = 3;
		TargUniform.AddId(TargBuffer);

		int[] col2arr = new int[4];
		col2arr[0] = DesiredColor.r8;
		col2arr[1] = DesiredColor.g8;
		col2arr[2] = DesiredColor.b8;
		col2arr[3] = DesiredColor.a8;

		byte[] DesiredColorData = new byte[col2arr.Length * sizeof(int)];
		Buffer.BlockCopy(col2arr, 0, DesiredColorData, 0, DesiredColorData.Length);

		RID DesBuffer = RD.StorageBufferCreate((uint)DesiredColorData.Length, DesiredColorData);
		RDUniform DesUniform = new RDUniform();
		DesUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		DesUniform.Binding = 4;
		DesUniform.AddId(DesBuffer);


		Godot.Collections.Array<RDUniform> uniformset = new Godot.Collections.Array<RDUniform>();
		//uniformset.Resize(5);
		uniformset.Insert(0, ReadUniform);
		uniformset.Insert(1, WriteUniform);
		uniformset.Insert(2, SizeUniform);
		uniformset.Insert(3, TargUniform);
		uniformset.Insert(4, DesUniform);
		//foreach(RDUniform uniform in uniformset)
		//{
		//	GD.Print(uniform, " aeiou uuuuuuu");
		//}

		RID UniformSet = RD.UniformSetCreate(uniformset, SHDR, 0);
		//GD.Print(RD.UniformSetIsValid(UniformSet));

		RID Pipeline = RD.ComputePipelineCreate(SHDR);

		long Complist = RD.ComputeListBegin(); // 64 bits fucking hell, 32 bits, 16 bits, 8 bits, 4 bits, 2 bits, 1 bit, half bit, quarter bit, THE WRIST GAAAAAAME
		RD.ComputeListBindComputePipeline(Complist, Pipeline);
		RD.ComputeListBindUniformSet(Complist, UniformSet, 0);
		RD.ComputeListDispatch(Complist, (uint)Img.GetWidth() * (uint)Img.GetHeight() * 4,1 , 1);// fuck it

		RD.ComputeListEnd();
		//GD.Print("Submitting!");
		RD.Submit();

		RD.Sync();

		byte[] outdata = RD.BufferGetData(WriteBuffer);
		//it's packed FROM INT32's!!!! // THERE'S NO BYTE IN GLSL FUCK THIS LOOL
		//i've got to unfuck the bytes!!!!!!!
		//little endian for moi, idk about other people's though
		//int[] bruh1 = new int[(Img.GetWidth() * Img.GetHeight()) * 4];

		//byte[] bruh2 = new byte[(Img.GetHeight() * Img.GetWidth()) * 4];


		//bruh1 = Array.ConvertAll(outdata, Convert.ToInt32);
		//Buffer.BlockCopy(outdata, 0, bruh1, 0, (Img.GetHeight() * Img.GetWidth()) * 4);
		//bruh1.CopyTo(bruh2, 0);




		
		//GD.Print(outdata.Length, " outlen");
		//for (int i = 0; i < 500; i++)
		//{
		//	GD.Print(outdata[i]);
		//}

		//byte[] bruh = new byte[4];
		//bruh[0] = (0);
		// bruh[1] = (255);
		//bruh[2] = (0);
		//bruh[3] = (0);
		//Array.Reverse(bruh);

		//byte breakdown (little endian)
		//each byte goes from 0 to 255, then it increments the next one
		//e.g (b0 = 255 b1 = 0) b0 + 1 = (b0 = 0 b1 = 1)
		//with the second byte the total becomes 2^(2*8) (0 - 65535, aka 65536) possible numbers
		//oh god
		//with the third byte the total is 2^(3*8) (0 - 16777215, aka 16777216) possible numbers
		//oh fuck
		// and finally, with the fourth byte the total becomes 2(4*8) (0 - 4294967295, aka 4294967296) possible numbers
		//that's almost a morbillion possible numbers
		//aka four billion two hundred ninety-four million nine hundred sixty-seven thousand two hundred ninety-six
		//but you see, we're dealing with ints not uints.

		//gotta take the 255 255 255 255 and convert it to a big ass number
		// wait just go r * b * g * a?
		// BITWISE OPERATIONS!!!!!!!
		// HAHAHAHHAHAHAHHAHAHAHHAHAAAAAAAAAAAAAAAAA
		// SPEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEED

		//int uhhh = BitConverter.ToInt32(bruh, 0);
		//tred (255 0 0 0) is 255 in int32 (little endian)
		//white (255 255 255 255) is -1 in int32 (little endian)
		//black (0 0 0 255) is -16777216 in int32 (little endian)
		//tyellow (255 255 0 0) is 65535 in int32 (little endian)
		//GD.Print(uhhh, " uhhh");

		//byte[] outdata2 = new byte[outdata.Length];
		//for (int i = 0; i < outdata.Length; i++)
		//{
		//	if (i % 4 == 0)
		//	{
		//		outdata2[i / 4] = outdata[i];
		//		//idk what i'm doing lmao
		//		//it works!!!!!!!
		//		//IT WORKS HAHAHAHAHHAA!!!!!!
		//	}
		//}
		//Array.Copy(outdata, outdata2, outdata.Length);
		RD.FreeRid(Pipeline);
		RD.FreeRid(UniformSet);
		RD.FreeRid(DesBuffer);
		RD.FreeRid(TargBuffer);
		RD.FreeRid(ReadBuffer);
		RD.FreeRid(WriteBuffer);
		RD.FreeRid(SizeBuffer);
		RD.FreeRid(SHDR);
		Image outimg = new Image();
		outimg.CreateFromData(Img.GetWidth(), Img.GetHeight(), false, Image.Format.Rgba8, outdata);
		return outimg; // hopefully it's as shrimple as that.
	}

	public static Image NuclearConvert(Image Img, Color TargetColor, Color DesiredColor)
	{
		// this is the same as SuperConvert, but NUCLEAR!!!!!
		//oh and it also nukes any color that's not the target color :)
		// (that's why it's called nuclear convert)
		// the leftover ashes of the image are the target color and the desired color.
		if (RD == null)
		{
			RD = RenderingServer.CreateLocalRenderingDevice();
		}
		//RenderingDevice 
		RDShaderFile shaderf = GD.Load<RDShaderFile>("res://Shaders/TileColorNuke.glsl");
		RDShaderSPIRV spirv = shaderf.GetSpirv();
		//GD.Print(SHDR.Id);
		RID SHDR = RD.ShaderCreateFromSpirv(spirv);

		byte[] ImgData = Img.GetData();
		int[] IntData = new int[ImgData.Length];
		Array.Copy(ImgData, IntData, IntData.Length);

		byte[] ReadData = new byte[IntData.Length * sizeof(int)]; // r g b a 0 - 255 for each pixel, which is height * width
																  //disregard above line's comment, it is now r 0 0 0 g 0 0 0 b 0 0 0 a 0 0 0 and the rgba is in 0-255 the other bytes are just there for ease of access
																  //yeah this ain't working out the way i want it to xd, time to go crazy mode

		Buffer.BlockCopy(IntData, 0, ReadData, 0, ReadData.Length);
		/*
		GD.Print(ReadData.Length, " inlen");
		for(int i = 0; i < 500; i++)
		{
			GD.Print(ReadData[i], " in");
		}
		*/
		RID ReadBuffer = RD.StorageBufferCreate((uint)ReadData.Length, ReadData);

		RDUniform ReadUniform = new RDUniform();
		ReadUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		ReadUniform.Binding = 0;
		ReadUniform.AddId(ReadBuffer);


		byte[] WriteData;

		WriteData = new byte[ReadData.Length / 4];

		RID WriteBuffer = RD.StorageBufferCreate((uint)WriteData.Length, WriteData);
		RDUniform WriteUniform = new RDUniform();
		WriteUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		WriteUniform.Binding = 1;
		WriteUniform.AddId(WriteBuffer);


		int[] sizearr = new int[2];
		sizearr[0] = Img.GetWidth();
		sizearr[1] = Img.GetHeight();
		byte[] SizeData = new byte[sizearr.Length * sizeof(int)];
		Buffer.BlockCopy(sizearr, 0, SizeData, 0, SizeData.Length);
		//hold on brb gotta test this with arbitrary numbers in gdscript as well as c# to see if i get the same number
		//arbnum 1 is 200 arbnum 2 is 693
		//it's the same
		RID SizeBuffer = RD.StorageBufferCreate(8, SizeData);
		RDUniform SizeUniform = new RDUniform();
		SizeUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		SizeUniform.Binding = 2;
		SizeUniform.AddId(SizeBuffer);

		int[] col1arr = new int[4];
		col1arr[0] = TargetColor.r8;
		col1arr[1] = TargetColor.g8;
		col1arr[2] = TargetColor.b8;
		col1arr[3] = TargetColor.a8;

		byte[] TargetColorData = new byte[col1arr.Length * sizeof(int)];
		Buffer.BlockCopy(col1arr, 0, TargetColorData, 0, TargetColorData.Length);

		RID TargBuffer = RD.StorageBufferCreate((uint)TargetColorData.Length, TargetColorData);
		RDUniform TargUniform = new RDUniform();
		TargUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		TargUniform.Binding = 3;
		TargUniform.AddId(TargBuffer);

		int[] col2arr = new int[4];
		col2arr[0] = DesiredColor.r8;
		col2arr[1] = DesiredColor.g8;
		col2arr[2] = DesiredColor.b8;
		col2arr[3] = DesiredColor.a8;

		byte[] DesiredColorData = new byte[col2arr.Length * sizeof(int)];
		Buffer.BlockCopy(col2arr, 0, DesiredColorData, 0, DesiredColorData.Length);

		RID DesBuffer = RD.StorageBufferCreate((uint)DesiredColorData.Length, DesiredColorData);
		RDUniform DesUniform = new RDUniform();
		DesUniform.UniformType = RenderingDevice.UniformType.StorageBuffer;
		DesUniform.Binding = 4;
		DesUniform.AddId(DesBuffer);


		Godot.Collections.Array<RDUniform> uniformset = new Godot.Collections.Array<RDUniform>();
		//uniformset.Resize(5);
		uniformset.Insert(0, ReadUniform);
		uniformset.Insert(1, WriteUniform);
		uniformset.Insert(2, SizeUniform);
		uniformset.Insert(3, TargUniform);
		uniformset.Insert(4, DesUniform);
		//foreach(RDUniform uniform in uniformset)
		//{
		//	GD.Print(uniform, " aeiou uuuuuuu");
		//}

		RID UniformSet = RD.UniformSetCreate(uniformset, SHDR, 0);
		//GD.Print(RD.UniformSetIsValid(UniformSet));

		RID Pipeline = RD.ComputePipelineCreate(SHDR);

		long Complist = RD.ComputeListBegin(); // 64 bits fucking hell, 32 bits, 16 bits, 8 bits, 4 bits, 2 bits, 1 bit, half bit, quarter bit, THE WRIST GAAAAAAME
		RD.ComputeListBindComputePipeline(Complist, Pipeline);
		RD.ComputeListBindUniformSet(Complist, UniformSet, 0);
		RD.ComputeListDispatch(Complist, (uint)Img.GetWidth() * (uint)Img.GetHeight() * 4, 1, 1);// fuck it

		RD.ComputeListEnd();
		//GD.Print("Submitting!");
		RD.Submit();

		RD.Sync();

		byte[] outdata = RD.BufferGetData(WriteBuffer);
		RD.FreeRid(Pipeline);
		RD.FreeRid(UniformSet);
		RD.FreeRid(DesBuffer);
		RD.FreeRid(TargBuffer);
		RD.FreeRid(ReadBuffer);
		RD.FreeRid(WriteBuffer);
		RD.FreeRid(SizeBuffer);
		RD.FreeRid(SHDR);
		Image outimg = new Image();
		outimg.CreateFromData(Img.GetWidth(), Img.GetHeight(), false, Image.Format.Rgba8, outdata);
		return outimg;
	}
	public static Image Qblit(Image image, LEQuad quad, Color color)
	{
		//add a check to find the smallest bounding box that contains ALL the points
		//this shit slow
		//should be from top left to top right in quad coords, not global image coords xd
		for (int x = (int)quad.GetBoundsMin().x; x < (int)quad.GetBoundsMax().x; x++)
		{
			for (int y = (int)quad.GetBoundsMin().y; y < (int)quad.GetBoundsMax().y; y++)
			{
				if(quad.IsInsideQuad(new Vector2i(x, y)))
				{
					image.SetPixel(x, y, color);
				}
			}
		}
		// ok idk if i fixed that but it SHOULD be much faster now, it doesn't have to check every fucking pixel in the image xd
		return image;
	}



	public static float Deg2Rad(float input) // because godot is STUPID! STUPID STUPID STUPID!!!!!!!!
	{
		return ((float)Math.PI / 2) - input;
	}

	public static int AfaMvLevelEdit(Vector2i pos, int layer)
	{
		if (pos.x >= 0 & pos.x < Globals.levelSize.x & pos.y >= 0 & pos.y < Globals.levelSize.y)
		{
			return (int)Globals.matrix[pos.x, pos.y, layer].TileID;
		}
		else return 1;
	}

	public static bool IsInsideLevel(Vector2i point)
	{
		if (point.x < 0 | point.x > Globals.levelSize.x - 1 | point.y < 0 | point.y > Globals.levelSize.y - 1)
		{
			return false;
		}
		else return true;
	}

    public static bool IsInsideLevel(int x, int y)
    {
		Vector2i point = new Vector2i(x, y);
        if (point.x < 0 | point.x > Globals.levelSize.x - 1 | point.y < 0 | point.y > Globals.levelSize.y - 1)
        {
            return false;
        }
        else return true;
    }



    public static Image ColorConvert(Image Img, Color SourceColor, Color PreferredColor)
	{
		for(int x = 0; x < Img.GetWidth(); x++)
		{
			//GD.Print(x);
			for (int y = 0; y < Img.GetHeight(); y++)
			{
				//GD.Print("y ", y);
				if (Img.GetPixel(x,y) == SourceColor)
				{
					//GD.Print("Setting the color ", SourceColor, " to ", PreferredColor);
					Img.SetPixel(x,y,PreferredColor);
				}
			}
		}
		return Img;
	}
	//shity CPU version of NuclearConvert
	public static Image SpillConvert(Image Img, Color SourceColor, Color PreferredColor)
	{
        for (int x = 0; x < Img.GetWidth(); x++)
        {
            //GD.Print(x);
            for (int y = 0; y < Img.GetHeight(); y++)
            {
                //GD.Print("y ", y);
                if (Img.GetPixel(x, y) != SourceColor)
                {
                    //GD.Print("Setting the color ", SourceColor, " to ", PreferredColor);
                    Img.SetPixel(x, y, PreferredColor);
                }
            }
        }
        return Img;
    }


}
