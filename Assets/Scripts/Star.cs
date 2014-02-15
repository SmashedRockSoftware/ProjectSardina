﻿using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {

	public GameObject Planet;
	public Planet[] planets;
	public int planetLimit;
	public float orbitalVariation;
	public float orbitalMult;
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
			planets[i].radius = i * orbitalMult + Random.Range(-orbitalVariation, orbitalVariation);
			planets[i].planet = Planet;
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
			SystemStar.planets[i].transform.position = new Vector3(Random.Range(-100.0f, 100.0f), 0, Random.Range(-100.0f, 100.0f)).normalized * planets[i].radius;
			SystemStar.planets[i].transform.position += new Vector3(998 - 2 * i, 0, 0);
			SpriteRenderer sprite = SystemStar.planets[i].GetComponent<SpriteRenderer>() as SpriteRenderer;
			SpriteRenderer spritePlanet = planets[i].planet.GetComponent<SpriteRenderer>() as SpriteRenderer;
			sprite.sprite = spritePlanet.sprite;
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
