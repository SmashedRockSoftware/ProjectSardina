using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public GameObject Planet;
	public Planet[] planets;
	public int planetLimit;
	public float orbitalVariation;
	public float orbitalMult;

	// Use this for initialization
	void Start () {
		PlanetaryGenerator();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlanetaryGenerator () {
		int planetNumber = Random.Range(0, planetLimit);
		planets = new Planet[planetNumber];
		for(int i = 0; i < planetNumber; i++){
			planets[i].radius = i * orbitalMult + Random.Range(-orbitalVariation, orbitalVariation);
			planets[i].planet = Planet;
		}
	}
}
