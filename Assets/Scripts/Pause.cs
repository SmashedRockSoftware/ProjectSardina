using UnityEngine;
using System.Collections;

public static class Pause {

	private static bool isPaused;

	public static bool GetPause(){
		return isPaused;
	}

	public static void SetPause(bool pauseState){
		isPaused = pauseState;
	}
}
