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
        testSend();
        testAudio();
        testReceive();
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
    
    public static void testSend()
	{
    	// send to [r baz]
		LibPD.SendFloat("baz", 10);
    }
    
    public static void testReceive()
    {
    		var receiver = "spam";
			var listArgs = new object[]{"hund", 1, "katze", 2.5, "maus", 3.1f};
			var msgName = "testing";
			var msgArgs = new object[]{"one", 1, "two", 2};

			LibPD.Subscribe(receiver);

			var n = 0;

			LibPDBang delBang = delegate(string recv)
			{
				Console.WriteLine(recv);
				n++;
			};

			LibPD.Bang += delBang;

			LibPDFloat delFloat = delegate(string recv, float x) 
			{
				Console.WriteLine(x);
				n++;
			};

			LibPD.Float += delFloat;

			LibPDSymbol delSymbol = delegate(string recv, string sym) 
			{
	
				n++;
			};

			LibPD.Symbol += delSymbol;

			LibPDList delList = delegate(string recv, object[] args) 
			{  


				for (int i = 0; i < args.Length; i++) 
				{
		
				}
				n++;
			};

			LibPD.List += delList;

			LibPDMessage delMessage = delegate(string recv, string msg, object[] args) 
			{  



				for (int i = 0; i < args.Length; i++) 
				{

				}
				n++;
			};

			LibPD.Message += delMessage;

			LibPD.SendBang(receiver);
			LibPD.SendFloat("spami", 42);
			LibPD.SendSymbol(receiver, "hund katze maus");
			LibPD.SendList(receiver, listArgs);
			LibPD.SendMessage(receiver, msgName, msgArgs);

			LibPD.Bang -= delBang;
			LibPD.Float -= delFloat;
			LibPD.Symbol -= delSymbol;
			LibPD.List -= delList;
			LibPD.Message -= delMessage;
    	
    }
    
}
