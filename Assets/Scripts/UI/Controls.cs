using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

	void Update(){
		if(Camera.main == null && Input.GetButtonDown("Cancel")){
			Star.UnloadSystem();
		}
	}

}
