using UnityEngine;
using System.Collections;

public static class Pause {

	private static bool isPaused;

	public static bool getPause(){
		return isPaused;
	}

	public static void setPause(bool pauseState){
		isPaused = pauseState;
	}
}
