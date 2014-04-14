using UnityEngine;
using System.Collections.Generic;
using LibPDBinding;
using System.Runtime.InteropServices;
using System;
using System.IO;

public class LibPdFilterRead : MonoBehaviour
{
	// C# pointer stuff
	private GCHandle dataHandle;
	private IntPtr dataPtr;

	// Patch handle, create one for each patch
	private int SPatch;

	// Pd related
	private bool islibpdready;
	private int numberOfTicks;

	// Public, patch name
	public string nameOfPatch;
	
	// Pd initialisation and patch open on game awake
	void Awake ()
	{	
		PluginUtils.ResolvePath();

		// Delegate for 'print' 
		LibPD.Print += ReceivePrint;

		// Follow this sequence of initialisation to avoid destruction of the universe
		if (!islibpdready) {
			if(openPd() == 0) {
				SPatch = loadPatch (nameOfPatch);
				LibPD.ComputeAudio (true);
			}
			else Debug.LogError("Error opening libpd");
		}
	}

	// Unity audio callback
	public void OnAudioFilterRead (float[] data, int channels)
	{	
		if(dataPtr == IntPtr.Zero)
		{
			dataHandle = GCHandle.Alloc(data,GCHandleType.Pinned);
			dataPtr = dataHandle.AddrOfPinnedObject();
		}
		
		if (islibpdready) {
			LibPD.Process(numberOfTicks, dataPtr, dataPtr);
		}
	}

	// Close patch and release Pd on quit
	void OnApplicationQuit ()
	{	
		// Unsubscribe Pd print object. 
		LibPD.Print -= ReceivePrint;

		closePatch (SPatch);
		LibPD.Release();
		islibpdready = false;
	}
	
	public void OnDestroy()
	{
		dataHandle.Free();
		dataPtr = IntPtr.Zero;
	}

	/** Pd helper functions from here **/
	
	// Load pd patch, specfied in Unity's inspector, returns patch handle
	public int loadPatch (string patchName)
	{
		string assetsPath = Application.streamingAssetsPath + "/PdAssets/";
		
		string path = assetsPath + patchName;
		
		// Android voodoo to load the patch. TODO: use Android APIs to copy whole folder?
		#if UNITY_ANDROID && !UNITY_EDITOR
		string patchJar = Application.persistentDataPath + "/" + patchName;
		
		if (File.Exists(patchJar))
		{
			Debug.Log("Patch already unpacked");
			File.Delete(patchJar);
			
			if (File.Exists(patchJar))
			{
				Debug.Log("Couldn't delete");				
			}
		}
		
		WWW dataStream = new WWW(path);
		
		
		// Hack to wait till download is done
		while(!dataStream.isDone) 
		{
		}
		
		if (!String.IsNullOrEmpty(dataStream.error))
		{
			Debug.Log("### WWW ERROR IN DATA STREAM:" + dataStream.error);
		}
		
		File.WriteAllBytes(patchJar, dataStream.bytes);
		
		path = patchJar;
		#endif

		Debug.Log("Loading patch:" + path);
		return LibPD.OpenPatch (path);
	}
	
	// Initialise Pd with Unity's sample rate and calculate number of ticks. Returns 0 on success.
	public int openPd ()
	{
		int bufferSize;
		int noOfBuffers;
		AudioSettings.GetDSPBufferSize (out bufferSize, out noOfBuffers);
		
		// Calculate number of ticks for process callback
		numberOfTicks = bufferSize / LibPD.BlockSize;
		
		// Get Unity's sample rate
		int unitySR = AudioSettings.outputSampleRate;
		
		// Initialise Pd with 2 ins and outs and Unity's samplerate. Project dependent.
		int pdOpen = -1;
		pdOpen = LibPD.OpenAudio (2, 2, unitySR);
		if (pdOpen == 0) islibpdready = true;
		
		return pdOpen;
	}
	
	// delegate for [print]
	void ReceivePrint(string msg) 
	{
		Debug.Log("print:" + msg);
	}
	
	
	public bool closePatch (int patchHandle)
	{
		return LibPD.ClosePatch (patchHandle);
	}
	
	/** END Pd helper functions **/
	
}