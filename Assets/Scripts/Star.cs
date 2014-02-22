using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star : MonoBehaviour {

	public GameObject[] PlanetList = new GameObject[2];
	public Planet[] planets;
	public int planetLimit = 10;
	public float orbitalVariation = 1;
	public float orbitalMult = 2;
	public float orbitalSpeedVariation = 5;
	public float StarMass;
	public List<GameObject> connectionPossibleList = new List<GameObject>();
	private GameObject[] connectionsTemp;
	public GameObject[] connectionSelf = new GameObject[2];
	public List<GameObject> connectionList = new List<GameObject>();
	public GameObject[] connections;
	Camera main;

	// Use this for initialization
	void Awake () {
		PlanetaryGenerator();
		ConnectionGenerator();
		main = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void ConnectionGenerator () {
		for(int i = 0; i < PerlinStars.stars.Length; i++){
			if(Vector3.Distance(transform.position, PerlinStars.stars[i].transform.position) < 15.0f){
				connectionPossibleList.Add(PerlinStars.stars[i]);
			}
		}
		connectionsTemp = connectionPossibleList.ToArray();

		GameObject closest1 = null;
		GameObject closest2 = null;
		for(int i = 0; i < connectionsTemp.Length; i++){
			if(connectionsTemp[i] != gameObject){
				if(closest1 == null){
					closest1 = connectionsTemp[i];
				}else if(closest1 != null && closest2 == null){
					closest2 = connectionsTemp[i];
				}else if(Vector3.Distance(transform.position, connectionsTemp[i].transform.position) < Vector3.Distance(transform.position, closest1.transform.position)){
					closest2 = closest1;
					closest1 = connectionsTemp[i];
				}
			}
		}

		connectionList.Add(closest1);
		connectionList.Add(closest2);
		connectionSelf[0] = closest1;
		connectionSelf[1] = closest2;
	}

	void PlanetaryGenerator () {
		StarMass = Random.Range (1.989f, 527.085f);
		int planetNumber = Random.Range(0, planetLimit);
		planets = new Planet[planetNumber];
		for(int i = 0; i < planetNumber; i++){
			GeneratePlanet(i);
			while(float.IsNaN(planets[i].orbitSpeed) || planets[i].radius < 0.5f){
				GeneratePlanet(i);
			}
		}
	}
	public void LoadSystem () {
		AudioListener listenerMain = Camera.main.GetComponent<AudioListener>() as AudioListener;
		Camera.main.enabled = false;
		listenerMain.enabled = false;
		SystemStar.planetCam.enabled = true;
		AudioListener listenerPlanet = SystemStar.planetCam.gameObject.GetComponent<AudioListener>() as AudioListener;
		listenerPlanet.enabled = true;
		for(int i = 0; i < planets.Length; i++){
			SystemStar.planets[i].transform.position = new Vector3(1000, 0, 0);
			SystemStar.planets[i].transform.position += new Vector3(Random.Range(-100.0f, 100.0f), 0, Random.Range(-100.0f, 100.0f)).normalized * planets[i].radius;
			SpriteRenderer sprite = SystemStar.planets[i].GetComponent<SpriteRenderer>() as SpriteRenderer;
			SpriteRenderer spritePlanet = planets[i].planet.GetComponent<SpriteRenderer>() as SpriteRenderer;
			sprite.sprite = spritePlanet.sprite;
			SystemPlanet planetScript = SystemStar.planets[i].GetComponent<SystemPlanet>() as SystemPlanet;
			planetScript.speed = planets[i].orbitSpeed;
			SystemStar.ShowPlanet(i);
		}
	}

	public void UnloadSystem () {
		main.enabled = true;
		AudioListener listenerMain = Camera.main.GetComponent<AudioListener>() as AudioListener;
		listenerMain.enabled = true;
		AudioListener listenerPlanet = SystemStar.planetCam.gameObject.GetComponent<AudioListener>() as AudioListener;
		listenerPlanet.enabled = false;
		SystemStar.planetCam.enabled = false;
		SystemStar.HidePlanets();
	}

	void GeneratePlanet (int i) {
		planets[i] = new Planet();
		planets[i].radius = 100/(i + 1) * Random.Range(-orbitalVariation, orbitalVariation) + 2;
		planets[i].planet = PlanetList[Random.Range(0, PlanetList.Length)];
		//Commented for later, in case we can get it to not throw 1000 errors every second
		planets[i].mass = Random.Range(5973600000000f, 1899604800000000f); // Remeber, these numbers are 12 digits smaller than they should be
		planets[i].orbitVelocity = Mathf.Sqrt((0.0000000000667f*(StarMass*1000000000000000000000000000000f))/(planets[i].radius*149597870700f));
		planets[i].orbitPeriod = ((2f*3.1415f*((planets[i].radius*149597870700f) * 1))/planets[i].orbitVelocity)/31536000;
		planets[i].orbitSpeed= 0.036f/planets[i].orbitPeriod;
	}

	public void ConnectionDrawer () {
		connections = connectionList.ToArray();
		for(int i = 0; i < connections.Length; i++){
			MeshLine.DrawLine(gameObject.transform.position, connections[i].transform.position, 0.2f);
		}
	}

	public void ConnectionSharer () {
		for(int i = 0; i < connectionSelf.Length; i++){
			Star starI = connectionSelf[i].GetComponent<Star>() as Star;
			if(starI.connectionSelf[0] != gameObject && starI.connectionSelf[1]){
				starI.connectionList.Add(gameObject);
			}
		}
	}

	public void ConnectionPruner () {
		for(int i = 0; i < connectionList.Count; i++){
			for(int x = 0; x < connectionList.Count; x++){
				if(connectionList[i] != connectionList[x]){
					float AB = Vector3.Distance(transform.position, connectionList[i].transform.position);
					float BC = Vector3.Distance(transform.position, connectionList[x].transform.position);
					float CA = Vector3.Distance(connectionList[i].transform.position, connectionList[x].transform.position);
					float c = Mathf.Max(AB, Mathf.Max(BC,CA));
					float a;
					float b;
					if(c == AB){
						a = BC;
						b = CA;
					}else if(c == BC){
						a = AB;
						b = CA;
					}else{
						a = AB;
						b = BC;
					}
					if(Mathf.Acos(((a*a) + (b*b) - (c*c))/(2*a*b)) * Mathf.Rad2Deg < 15.0f){
						GameObject connRemove;
						if(AB > BC){
							connRemove = connectionList[i];
						}else{
							connRemove = connectionList[x];
						}
						Star star = connRemove.GetComponent<Star>() as Star;
						star.connectionList.Remove(connRemove);
						connectionList.Remove(connRemove);
						Debug.Log("Removed connection " + (Mathf.Acos(((a*a) + (b*b) - (c*c))/(2*a*b)) * Mathf.Rad2Deg).ToString());
					}
				}
			}
		}
	}
}
