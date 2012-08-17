using UnityEngine;
using System.Collections.Generic;
using LibPDBinding;
using System.Runtime.InteropServices;
using System;

public class LibPdFilter : MonoBehaviour
{
	private GCHandle dataHandle;
	private IntPtr dataPtr;
	
	public string patch;
	private int SPatch;
	private bool islibpdready;
	private bool isready;
	public double time;
	public float value;
	private string path;
	
	void Awake ()
	{
		path = Application.dataPath + "/" + patch;
		loadPatch ();
		isready = true;
	}
	
	public void loadPatch ()
	{
		Debug.Log("Load patch");
		if(!islibpdready)
		{
			LibPD.OpenAudio (1, 1, 48000);
		}
		SPatch = LibPD.OpenPatch (path);
		LibPD.ComputeAudio (true);
		islibpdready = true;
		
		LibPD.Print += Receive;
	}

	// delegate for [s osc] float
	void Receive(string msg) 
	{
		Debug.Log("print:" + msg);
	}
	
	
	public void closePatch ()
	{
		//LibPD.Print -= Receive;
		LibPD.ClosePatch (SPatch);
		LibPD.Release ();
	}
	
	public void OnAudioFilterRead (float[] data, int channels)
	{	
		if(dataPtr == IntPtr.Zero)
		{
			dataHandle = GCHandle.Alloc(data,GCHandleType.Pinned);
			dataPtr = dataHandle.AddrOfPinnedObject();
		}
		
		if (islibpdready && isready) {	
			LibPD.SendFloat(SPatch + "freq", value);
			LibPD.SendFloat(SPatch + "turn", 1f);
			LibPD.Process(32, dataPtr, dataPtr);	
			LibPD.SendFloat(SPatch + "turn", 0f);
		}
		
	}
	

	void OnApplicationQuit ()
	{
		closePatch ();
	}
	
	public void OnDestroy()
	{
		dataHandle.Free();
		dataPtr = IntPtr.Zero;
	}
}