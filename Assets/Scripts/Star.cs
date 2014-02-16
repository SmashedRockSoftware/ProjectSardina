﻿using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public GameObject[] PlanetList = new GameObject[2];
	public Planet[] planets;
	public int planetLimit;
	public float orbitalVariation;
	public float orbitalMult;
	public float orbitalSpeedVariation;
	Camera main;

	// Use this for initialization
	void Start () {
		PlanetaryGenerator();
		main = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlanetaryGenerator () {
		int planetNumber = Random.Range(0, planetLimit);
		planets = new Planet[planetNumber];
		for(int i = 0; i < planetNumber; i++){
			planets[i] = new Planet();
			planets[i].radius = 100/(i + 1) * Random.Range(-orbitalVariation, orbitalVariation) + 2;
			Debug.Log(planets[i].radius.ToString());
			planets[i].planet = PlanetList[Random.Range(0, PlanetList.Length)];

			//Commented for later, in case we can get it to not throw 1000 errors every second
			planets[i].mass = Random.Range(5973600000000f, 1899604800000000f); // Remeber, these numbers are 12 digits smaller than they should be
			planets[i].orbitVelocity = Mathf.Sqrt((0.0000000000667f*(planets[i].mass*1000000000000))/planets[i].radius);
			planets[i].orbitPeriod = ((2f*3.1415f*(planets[i].radius * 1))/planets[i].orbitVelocity)/31536000;
			Debug.Log(planets[i].orbitPeriod.ToString());
			planets[i].orbitSpeed= 0.0036f/planets[i].orbitPeriod;
		}
	}
	public void LoadSystem () {
		SystemStar.ShowPlanets();
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
		}
		SystemStar.HidePlanets(planets.Length);
	}

	public void UnloadSystem () {
		main.enabled = true;
		AudioListener listenerMain = Camera.main.GetComponent<AudioListener>() as AudioListener;
		listenerMain.enabled = true;
		AudioListener listenerPlanet = SystemStar.planetCam.gameObject.GetComponent<AudioListener>() as AudioListener;
		listenerPlanet.enabled = false;
		SystemStar.planetCam.enabled = false;
	}
}
