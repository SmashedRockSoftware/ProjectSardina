using UnityEngine;
using System.Collections;

public class Pause : MonoBehaviour{

	public GameObject pauseText;

	private static bool isPaused = true;

	void Update(){
		pauseText.SetActive(isPaused);
	}

	public void TogglePause(){
		bool pause = !GetPause();
		SetPause(pause);
	}

	public static bool GetPause(){
		return isPaused;
	}

	public static void SetPause(bool pauseState){
		isPaused = pauseState;
		GameObject.FindWithTag("GameController").GetComponent<NetworkView>().RPC("SetPauseRPC", RPCMode.Others, pauseState);
	}

	[RPC]
	public void SetPauseRPC(bool pauseState){
		isPaused = pauseState;
	}
}
