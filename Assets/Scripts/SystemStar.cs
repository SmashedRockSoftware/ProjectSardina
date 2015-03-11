using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SystemStar : MonoBehaviour {
	
	public static Camera planetCam;
	public Camera planetCamTemp;

	public static float nextPlanetLoc = 4f; //For setting up system

	private static List<GameObject> objects = new List<GameObject>();

	// Use this for initialization
	void Start () {
		planetCam = planetCamTemp;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void HidePlanets () {
		GameObject g;
		for(int i = 0; i < objects.Count; ){
			g = objects[i];
			objects.Remove(g);
			Destroy(g);
		}
	}

	public static void LoadSystem (Planet[] planets, Star star) {
		GameObject s = Instantiate(star.sprite, new Vector3(1000, 0, 0), Quaternion.identity) as GameObject;
		s.transform.eulerAngles  = new Vector3(90, 0, 0);
		s.transform.localScale = new Vector3(8, 8, 8);
		objects.Add(s);

		for(int i = 0; i < planets.Length; i++){
			GameObject g = Instantiate(planets[i].sprite, new Vector3(1000 - SystemStar.nextPlanetLoc, 0, 0), Quaternion.identity) as GameObject;
			g.transform.eulerAngles = new Vector3(90, 0, 0);

			float moonLoc = 1f;
			if(planets[i].planetType == 1){
				moonLoc = 1.5f;
			}else if(planets[i].planetType == 2){
				moonLoc = 1.25f;
			}

			for(int k = 0; k < planets[i].moons.Length; k++){
				GameObject m = Instantiate(planets[i].moons[k].sprite, new Vector3(1000 - SystemStar.nextPlanetLoc, 0, 0 + moonLoc), Quaternion.identity) as GameObject;
				m.transform.eulerAngles = new Vector3(90, 0, 0);
				moonLoc += 1.0f;
				m.GetComponent<SystemPlanet>().SetPlanet(planets[i].moons[k]);
				objects.Add(m);
			}

			SystemStar.nextPlanetLoc += 2.5f;
			g.GetComponent<SystemPlanet>().SetPlanet(planets[i]);
			objects.Add(g);
		}
	}
}
