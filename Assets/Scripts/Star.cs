using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star : MonoBehaviour {

	//Planet vars
	public Planet[] planets;

	//Star vars
	public float starMass; //solar masses
	public float starRadius; //solar radii
	public float starLuminosity; //solar luminosity
	public float starTemperature; //kelvin
	public string starClass; //stellar class O through M

	private Camera mainCam;
	private float auMod = 0; //For systems with less planets, expands system out a bit
	private bool notSpaced = true; //Check if extra expansion for small systems has taken place

	public static int[] count = new int[3]; //For stat keeping purposes (generating debug messages)

	//Give a camera reference to this star, called by PerlinStars
	public void setCam (Camera cam){
		mainCam = cam;
	}

	// Use this for initialization
	void Awake () {
		PlanetaryGenerator();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlanetaryGenerator () {

		//Starstuff
		starMass = RandomGenerator.getStarMass(); //Star mass is measured in solar masses

		//Simple mass-radius relationship
		if(starMass > 1.0f){
			starRadius = Mathf.Pow(starMass, 0.57f);
		}else{
			starRadius = Mathf.Pow(starMass, 0.8f);
		}

		//Luminosity-mass relationship for main sequence stars
		if(starMass < 0.43f){
			starLuminosity = 0.23f * Mathf.Pow(starMass, 2.3f);
		}else if(starMass < 2.0f){
			starLuminosity = Mathf.Pow(starMass, 4.0f);
		}else if(starMass < 20.0f){
			starLuminosity = 1.5f * Mathf.Pow(starMass, 3.5f);
		}else{
			starLuminosity = 3200 * starMass;
		}

		//Temerpature from luminosity and radius, using Stephen-Boltzmann Law
		starTemperature = 5780 * Mathf.Pow(starLuminosity/starRadius, 0.25f);

		//Stellar Classification on the Harvard scale. Since most (if not all) stars are main sequence, MK scale is unecessary
		if(starTemperature < 3500){
			starClass = "M";
		}else if(starTemperature < 5000){
			starClass = "K";
		}else if(starTemperature < 6000){
			starClass = "G";
		}else if(starTemperature < 7500){
			starClass = "F";
		}else if(starTemperature < 10000){
			starClass = "A";
		}else if(starTemperature < 30000){
			starClass = "B";
		}else{
			starClass = "O";
		}


		//Planet stuff, see planet generation script below
		int planetNumber = RandomGenerator.getPlanetNumber();
		planets = new Planet[planetNumber];
		for(int i = 0; i < planets.Length; i++){
			GeneratePlanet(i);
		}
	}
	public void LoadSystem () {
		AudioListener listenerMain = mainCam.GetComponent<AudioListener>() as AudioListener;
		mainCam.enabled = false;
		listenerMain.enabled = false;
		SystemStar.planetCam.enabled = true;
		AudioListener listenerPlanet = SystemStar.planetCam.gameObject.GetComponent<AudioListener>() as AudioListener;
		listenerPlanet.enabled = true;
		for(int i = 0; i < planets.Length; i++){
			SystemStar.planets[i].transform.position = planets[i].getPos();
			SpriteRenderer sprite = SystemStar.planets[i].GetComponent<SpriteRenderer>() as SpriteRenderer;
			SpriteRenderer spritePlanet = planets[i].planet.GetComponent<SpriteRenderer>() as SpriteRenderer;
			sprite.sprite = spritePlanet.sprite;
			SystemPlanet planetScript = SystemStar.planets[i].GetComponent<SystemPlanet>() as SystemPlanet;
			planetScript.planet = planets[i];
			SystemStar.ShowPlanet(i);
		}
	}

	public void UnloadSystem () {
		mainCam.enabled = true;
		AudioListener listenerMain = mainCam.GetComponent<AudioListener>() as AudioListener;
		listenerMain.enabled = true;
		AudioListener listenerPlanet = SystemStar.planetCam.gameObject.GetComponent<AudioListener>() as AudioListener;
		listenerPlanet.enabled = false;
		SystemStar.planetCam.enabled = false;
		SystemStar.HidePlanets();
	}

	void GeneratePlanet (int i) {
		Planet planet = GameObject.FindGameObjectWithTag("GameController").AddComponent<Planet>() as Planet; //Add a new planet script
		planets[i] = planet;

		//Type determination
		if(i == 0){
			if(RandomGenerator.getInt(0, 2) == 0){
				//Terrestrial, close to star
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(0.5f, 20.0f)/planets.Length + auMod; //AU
				planets[i].planet = PerlinStars.planetsTerrestrial[RandomGenerator.getInt(0, PerlinStars.planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth

				count[0]++;

			}else{
				//Hot jupiter
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(0.5f, 20.0f)/planets.Length + auMod; //AU
				planets[i].planet = PerlinStars.planetsGas[RandomGenerator.getInt(0, PerlinStars.planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));

				count[1]++;
			}
		}else if(planets[i - 1].orbitRadius + 2 > 15){ //Ice giants introduced, depending on Luminosity of orbiting stars
			int r = RandomGenerator.getInt(0, 5); //Weighted for ice giants
			if(r == 0){
				//Terrestrial, past first orbit
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = PerlinStars.planetsTerrestrial[RandomGenerator.getInt(0, PerlinStars.planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth

				count[0]++;

			}else if(r == 1){
				//Regular jupiter, colder
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = PerlinStars.planetsGas[RandomGenerator.getInt(0, PerlinStars.planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth

				count[1]++;

			}else{
				//Ice giant
				planets[i].planetType = 2;
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = PerlinStars.planetsIce[RandomGenerator.getInt(0, PerlinStars.planetsIce.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.neptuneToEarthMass(planets[i].mass), 2); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.neptuneToEarthMass(planets[i].mass), PlanetOperations.neptuneToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth

				count[2]++;

			}
		}else{
			if(RandomGenerator.getInt(0, 2) == 0){
				//Terrestrial, middle solar system
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 4.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = PerlinStars.planetsTerrestrial[RandomGenerator.getInt(0, PerlinStars.planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth

				count[0]++;
				
			}else{
				//Regular jupiter
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 4.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = PerlinStars.planetsGas[RandomGenerator.getInt(0, PerlinStars.planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth

				count[1]++;
			}
		}

		planets[i].orbitPeriod = 2f * Mathf.PI * Mathf.Sqrt(Mathf.Pow(planets[i].orbitRadius, 3f)/(starMass * 39.42f)) * (360f/365.24f); //Newton's enhancement of Kepler's third law, with conversion from 365 day year to 360 day year

		planets[i].angle = RandomGenerator.getFloat(0f, 360f); //Get angle from horizontal for start of game

		auMod = RandomGenerator.getFloat(2f, 4f)/planets.Length; //Change AU mod for next system

		//Spaces out smaller systems, to get some father out planets in systems of 5 or less planets
		if(planets.Length <= 5 && notSpaced){
			if(RandomGenerator.getInt(0, 4) == 0){
				auMod += RandomGenerator.getFloat(5.0f, 10.0f);
			}
		}

	}
}