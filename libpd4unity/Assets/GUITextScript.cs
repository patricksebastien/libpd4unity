using UnityEngine;
using System.Collections;
using LibPDBinding;
using System;

public class GUITextScript : MonoBehaviour {
	
	float fromPd;

	void Start() {

#if UNITY_ANDROID && !UNITY_EDITOR
		Screen.orientation = ScreenOrientation.Landscape;
#endif

		// subscribing to receive
		LibPD.Subscribe("note");
		LibPD.Float += receiveFloat;
	}

	void OnGUI() {
		GUI.Label(new Rect(10, 50, 300, 300), "Note from Pd: " + fromPd);
	}

	void receiveFloat(string nameofSend, float value) {
		if (String.Compare (nameofSend, "note") == 0) {
			fromPd = value;
		}
	}
}
