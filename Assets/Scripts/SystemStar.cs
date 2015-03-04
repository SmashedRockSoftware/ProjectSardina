using UnityEngine;
using System.Collections;

public class SystemStar : MonoBehaviour {

	public static GameObject[] planets = new GameObject[10];
	public GameObject[] planetTemp = new GameObject[10];
	public static Camera planetCam;
	public Camera planetCamTemp;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < planetTemp.Length; i++){
		planets[i] = planetTemp[i];
		}
		planetCam = planetCamTemp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void HidePlanets () {
		for(int i = 0; i < planets.Length; i++){
			planets[i].GetComponent<Renderer>().enabled = false;
		}
	}

	public static void ShowPlanet (int i) {
		planets[i].GetComponent<Renderer>().enabled = true;
	}
}
