using System;
using System.Runtime.InteropServices;
using LibPDBinding;
using UnityEngine;

public class LibPDTests : MonoBehaviour
{
	private static int SPatch;
	
	void Start()
	{
		loadPatch();
		
		
		
	}
	
	void Update()
	{
		testAudio();	
	}
	
	void OnDestroy()
	{
		closePatch();
	}
    
    public static void loadPatch()
	{
			LibPD.ReInit();
			// this is: 2 inputs channels, 3 outputs channels @ 44100hz
			LibPD.OpenAudio(2, 3, 44100);
			SPatch = LibPD.OpenPatch(Application.dataPath + "/inout.pd");
			LibPD.ComputeAudio(true);
	}
    
    public static void closePatch()
	{
		LibPD.ClosePatch(SPatch);
		LibPD.Release();
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
		Debug.Log(inBuffer[10]);
		Debug.Log(outBuffer[10]);
	}
    
}
