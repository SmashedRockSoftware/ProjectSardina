using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeWindow(int windowID){
		Camera.main.transform.position = new Vector3(200*windowID, 0, -10);
	}
}
