#[compute]
// i literally copy + pasted the og but now it's INVERSE HAHAHAHHAA
#version 460
#extension GL_KHR_vulkan_glsl: enable

// i don't know what the hell i'm doing lmao
layout(local_size_x = 1, local_size_y = 1) in;


// i'm just following a yt tutorial lol
//shoutout to simpleflips
// and also not simpleflips who made the tutorial: https://www.youtube.com/watch?v=5CKvGYqagyI
layout(set = 0, binding = 0, std430) buffer BufferIn {int[] data;}
buffer_in;

layout(set = 0, binding = 1, std430) buffer BufferOut {int[] data;} //hardcoded for rgba8 or r8b8g8a8
buffer_out;

layout(set = 0, binding = 2, std430) buffer SizeDataBuffer
{
	int width;
	int height;
	//uvec3 stcol; // starting color, or "target" color to change to the desired color
	//uvec3 encol; // desired color
}
size_data;

layout(set = 0, binding = 3, std430) buffer StartColorBuffer {int[] col;}
target_color;
// 0 1 2 3 r g b a
layout(set = 0, binding = 4, std430) buffer DesiredColorBuffer {int[] col;}
desired_color;

void main() 
{
	
	//what i will need this to do
	//this is for EACH PIXEL i think? 
	//global size i guess?
	// check r g b a ints if they are = to target color ints
	// if all are yes then set the color to the desired color
	//then set the final channels in the "out" buffer (at the coordinates) to whatever the channels are in the current thingy
	// should this be done like where it is or like ugh i can't think right now
	//four channels [1 2 3 4] [5 6 7 8]
	// the x and y should be mapped with what * number to get channels
	// there is no print function fuck this shit
	// i had this same ISSUE WITH THE FUCKING GEOMETRY DISPLAY WHY CAN'T I THINK OF ONE NOW???????
	// in reverse?????????? 1 * 4 = 4 - 3 = 1 but is it 1 though?????
	//sleep time
	//ok so now i'm awake
	//there is a total of width * height * 4 integers.
	//e.g. 200 * 639 * 4 = 511200
	// top left GloID would be 1 x 1 right?
	// row major order, which means that it goes 1x1y to 2x1y
	// BUUUUT glsl is column major order!
	// it should iterate between the r g b and a numbers at the x and y
	// the x and y is WIDTH * X + Y // will this work? IDK LET'S FIND OUT!!!!
	//uint dex = ((size_data.width * 4) * gl_GlobalInvocationID.x + gl_GlobalInvocationID.y);
	uint dex = gl_GlobalInvocationID.x;
	//for (int i = 0; i < 4; i++)
	//{
	//lol nested if statement xd
	//int idex = int(dex);
	bool bruh = true;
	if(dex % 4 == 0)
	{
		int similar = 0;
		//atomicExchange(buffer_out.data[dex], buffer_in.data[dex]);
		if (buffer_in.data[dex + 0] != target_color.col[0]) {similar += 1;}
		if (buffer_in.data[dex + 1] != target_color.col[1]) {similar += 1;}
		if (buffer_in.data[dex + 2] != target_color.col[2]) {similar += 1;}
		if (buffer_in.data[dex + 3] != target_color.col[3]) {similar += 1;}
		if (similar == 4) 
		{
			// 0 1 2 3 =  r g b a
			// maybe don't think about this in a 2D way?
			// maybe just do all the stuff in 1D?
			//do it tomorrow (today you dumbass)
			// it works like above but find way to convert rgba into a signed integer
			//atomicExchange(buffer_out.data[(dex + 0) / 4], desired_color.col[0]);
			//atomicExchange(buffer_out.data[(dex + 1) / 4], desired_color.col[1]);
			//atomicExchange(buffer_out.data[(dex + 2) / 4], desired_color.col[2]);
			//atomicExchange(buffer_out.data[(dex + 3) / 4], desired_color.col[3]);

			int r = desired_color.col[0];
			int g = desired_color.col[1];
			int b = desired_color.col[2];
			int a = desired_color.col[3];

			int color = (r << 0) + (g << 8) + (b << 16) + (a << 24);
			atomicExchange(buffer_out.data[(dex) / 4], color);
			//atomicExchange(buffer_out.data[(dex) / 4], desired_color.col[1]);
			//atomicExchange(buffer_out.data[(dex) / 4], desired_color.col[2]);
			//atomicExchange(buffer_out.data[(dex) / 4], desired_color.col[3]);
		}
		else // figure out why it's not changing colors to the left or right of stuff?
		{
			//atomicExchange(buffer_out.data[dex - 3], buffer_in.data[dex - 3]);
			//atomicExchange(buffer_out.data[dex - 2], buffer_in.data[dex - 2]);
			//atomicExchange(buffer_out.data[dex - 1], buffer_in.data[dex - 1]);
			//atomicExchange(buffer_out.data[dex - 0], buffer_in.data[dex - 0]);
			// this is big if true
			// IF TRUE

			int r = buffer_in.data[dex + 0];
			int g = buffer_in.data[dex + 1];
			int b = buffer_in.data[dex + 2];
			int a = buffer_in.data[dex + 3];

			int color = (r << 0) + (g << 8) + (b << 16) + (a << 24);
			atomicExchange(buffer_out.data[(dex) / 4], color);
			//atomicExchange(buffer_out.data[(dex) / 4], buffer_in.data[dex + 1]);
			//atomicExchange(buffer_out.data[(dex) / 4], buffer_in.data[dex + 2]);
			//atomicExchange(buffer_out.data[(dex) / 4], buffer_in.data[dex + 3]);
			//atomicExchange(buffer_out.data[dex >> 0], buffer_in.data[dex + 0]);
			//atomicExchange(buffer_out.data[dex >> 4], buffer_in.data[dex + 1]);
			//atomicExchange(buffer_out.data[dex >> 8], buffer_in.data[dex + 2]);
			//atomicExchange(buffer_out.data[dex >> 12], buffer_in.data[dex + 3]);
		}
	}
	//else { atomicExchange(buffer_out.data[dex], int(dex)); }
	// here's hoping it works!
	//atomicAdd(buffer_out.data[gl_GlobalInvocationID.y], int(gl_GlobalInvocationID.y));
	//atomicExchange(buffer_out.data[dex], int(dex));
	
	//modulo
	//abrg modulo
	//yes, to avoid
	//}
}






//layout(set = 0, binding = 2) int[] targcolor;
// idk what the fuck i'm doing
//layout(set = 0, binding = 1)uniform sampler2D img;
//looking at the book of shaders, learning a little bit more about how FUCKED UP glsl is
//functions have to be ABOVE main() ????????
//https://thebookofshaders.com <- MVP
//mfw i cannot spaghetti :(
//no glsl bolognese with python sprinkles and a side of lua bread

// uniforms are just things that the language syncs with all the little gpu gnomes in the gpu
// the gnomes have no power over the uniforms


//this is where the "fun" begins

