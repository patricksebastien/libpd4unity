using UnityEngine;
using System.Collections;
using LibPDBinding;

public class getsetdata : MonoBehaviour {
	
	private static int SPatch;
	private int blocksize;
	private int tick;
	
		
	void Awake()
	{
		blocksize = 64;
		tick = 1;
		loadPatch();
		//AudioClip.Create( "test"
	}
	
	
    void Start()
	{
		
		//testAudio();
    }
	
	
	void OnApplicationQuit()
	{
		closePatch();
	}
    
	
    public void loadPatch()
	{
			// this is: 2 inputs channels, 2 outputs channels @ 44100hz
			LibPD.OpenAudio(audio.clip.channels, audio.clip.channels, audio.clip.frequency);
			SPatch = LibPD.OpenPatch(Application.dataPath + "/inout.pd");
			LibPD.ComputeAudio(true);
	}
    
	
    public static void closePatch()
	{
		LibPD.ClosePatch(SPatch);
		LibPD.Release();
	}
	
	
	public void testAudio()
	{
		 /*
		for (i = 0; i < NUMBERSECOND * SAMPLE_RATE / (BLOCKSIZE * TICK); i++) {
		// pulseaudio input
		if (pa_simple_read(r, inbuf, sizeof(t_float) * BLOCKSIZE * NBCHANNEL * TICK, &error) < 0) {
			fprintf(stderr, __FILE__": pa_simple_read() failed: %s\n", pa_strerror(error));
			goto problem;
		}
		*/
		
		
		float[] inBuffer = new float[audio.clip.samples * audio.clip.channels];
		float[] outBuffer = new float[audio.clip.samples * audio.clip.channels];
        audio.clip.GetData(inBuffer, 0);
		
		Debug.Log("unity samplerate: "+AudioSettings.outputSampleRate);
		Debug.Log("channels: "+audio.clip.channels);
		Debug.Log("frequency: "+audio.clip.frequency);
		Debug.Log("length: "+audio.clip.length);
		Debug.Log("samples: "+audio.clip.samples);
		
		/*
		int i = 0;
		while(i <= inBuffer.Length) {
			audio.clip.GetData(inBuffer, i);
			LibPD.Process(1024, inBuffer, outBuffer);
			audio.clip.SetData(outBuffer, i);
			i = i * 1024;
		}
		*/
		/*
		float[] samples = new float[audio.clip.samples * audio.clip.channels];
        audio.clip.GetData(samples, 0);
        int i = 0;
        while (i < samples.Length) {
            samples[i] = samples[i] * 0.5F;
            ++i;
        }
        audio.clip.SetData(samples, 0);
        */
        
	}
	

	public void OnAudioFilterRead(float[] data, int channels) {
		Debug.Log(data.Length);
		float[] returnArray = new float[data.Length];
		LibPD.Process(1, data, returnArray);
		data = returnArray;
	}
}