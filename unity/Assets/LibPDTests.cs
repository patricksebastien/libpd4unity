using System;
using System.Runtime.InteropServices;
using LibPDBinding;
using UnityEngine;

public class LibPDTests : MonoBehaviour
{
	public float volume = 1.0F;
	public Transform sphere;
	
	private static int SPatch;
	
	
	void Start()
	{
		loadPatch();
		
		// receive [s osc] float
		LibPD.Subscribe("osc");
		LibPD.Float += Receive;
	}
	
	
	// delegate for [s osc] float
	void Receive(string recv, float x) 
	{
		this.sphere.transform.position = new Vector3(0, x, 0);
	}
	
	
	void Update()
	{
		testAudio();
		
	}
	
	
	void OnApplicationQuit()
	{
		closePatch();
	}
    
	
    public static void loadPatch()
	{
			LibPD.ReInit();
			// this is: 2 inputs channels, 2 outputs channels @ 44100hz
			LibPD.OpenAudio(2, 2, 44100);
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
		LibPD.Process(2, inBuffer, outBuffer);
		
		Debug.Log(inBuffer[10]);
		Debug.Log(outBuffer[10]);
	}
	

    void OnGUI()
	{
		volume = GUILayout.VerticalSlider(volume, 10.0F, 0.0F);

		// send float to [r volume]
		LibPD.SendFloat("volume", volume);
    }
    
}
