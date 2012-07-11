using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using LibPDBinding;

public class libpd4unity
{
	private static int SPatch;
	private static int SDllHandle;
		
    [DllImport("kernel32")]
	static extern int LoadLibrary(string lpLibFileName);
		
	[DllImport("kernel32")]
	static extern bool FreeLibrary(int hModule);
		
	static void Main()
    {
        Console.WriteLine("libpd4unity");
        loadPatch();
        testAudio();
        closePatch();
        Console.ReadLine();
    }
    
    public static void loadPatch()
	{
			SDllHandle = LoadLibrary("libpdcsharp.dll");
			LibPD.ReInit();

			LibPD.OpenAudio(2, 3, 44100);
			SPatch = LibPD.OpenPatch(@".\test_csharp.pd");
			LibPD.ComputeAudio(true);
	}
    
    public static void closePatch()
	{
		LibPD.Release();
		
		while(FreeLibrary(SDllHandle)) 
		{}
	}
    
    public static void testAudio()
	{
		float[] inBuffer = new float[256];
		float[] outBuffer = new float[256];
		for (int i = 0; i < 256; i++)
		{
			inBuffer[i] = i;
		}
		int err = LibPD.Process(2, inBuffer, outBuffer);
		Console.WriteLine(inBuffer[10]);
		Console.WriteLine(outBuffer[10]);
	}
    
}
