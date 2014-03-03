using UnityEngine;
using System.Collections;
using LibPDBinding;

public class GUIToggleScript : MonoBehaviour {

	bool toggleCheck = true;

	void OnGUI() {
		
		toggleCheck = GUI.Toggle(new Rect(10, 10, 200, 200), toggleCheck, " Toggle Random in Pd");

		if(toggleCheck) 
			LibPD.SendFloat("oscControl", 1);
		else
			LibPD.SendFloat("oscControl", 0);
	}
}
