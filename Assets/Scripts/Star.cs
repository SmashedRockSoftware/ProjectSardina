using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star : MonoBehaviour {

	//Planet vars
	public Planet[] planets;

	//~Planet atmosphere vars~
	//Assign an element a specific number if you want it to be more or less abundant
	//If you want an element to not be spawned, then set it to -10
	//If an element is 0, then the generator will pick a number between -700 and 300, or it will make it a primary element
	//Elements assigned to 11 will never be a primary element
	static float hydrogen, helium, methane, oxygen, nitrogen, co2, h2o, neon, other;
	public float[] gasElements = new float[]{hydrogen=5000, helium=400, methane=200, oxygen=0, nitrogen=0, co2=0, h2o=0, neon=0, other=11};//Atmospheric composition for gas giants
	public float[] terrestrialElements = new float[]{hydrogen=0, helium=0, methane=0, oxygen=0, nitrogen=0, co2=0, h2o=0, neon=0, other=11};//Atmospheric composition for terrestrials
	public float[] iceElements = new float[]{hydrogen=5000, helium=400, methane=700, oxygen=0, nitrogen=0, co2=0, h2o=0, neon=0, other=11};//Atmospheric composition for ice giants
	public string[] elementNames = new string[]{"hydrogen", "helium", "methane", "oxygen", "nitrogen", "co2", "h2o", "neon", "other"};//Must be in respective order as above

	//Star vars
	public float starMass; //solar masses
	public float starRadius; //solar radii
	public float starLuminosity; //solar luminosity
	public float starTemperature; //kelvin
	public string starClass; //stellar class O through M
	public string starName; //Name of the star
	public Constellation starConstellation; //constellation this star is a part of

	private Camera mainCam;
	private float auMod = 0; //For systems with less planets, expands system out a bit
	private bool notSpaced = true; //Check if extra expansion for small systems has taken place

	//Planet sprites, assigned on the star controller
	public static SpriteRenderer[] planetsGas;
	public static SpriteRenderer[] planetsTerrestrial;
	public static SpriteRenderer[] planetsIce;

	//Star sprites, assigned in star controller
	public static SpriteRenderer[] starSprites;
	public Animator[] starAnimators;

	public static int[] count = new int[3]; //For stat keeping purposes (generating debug messages)

	//Give a camera reference to this star, called by PerlinStars
	public void setCam (Camera cam){
		mainCam = cam;
	}

	// Use this for initialization
	void Awake () {

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void PlanetaryGenerator () {

		//Star generation has been moved to the StarGenerator

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
		SystemStar.planetCam.enabled = true;					//This line sometimes returns a NRE "Object reference not set to an instance of an object"
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
		SystemStar.planetCam.transform.position = new Vector3(1000, 10, 0);
		SystemStar.planetCam.orthographicSize = 20;
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
				planets[i].planet = planetsTerrestrial[RandomGenerator.getInt(0, planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth

				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], terrestrialElements, elementNames);		//set atmospheric composition
				planets[i].atmPressure = PlanetOperations.planetPressure (planets[i].planetType,planets[i].flux,planets[i].mass);	//set atmospheric pressure

				count[0]++;

			}else{
				//Hot jupiter
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(0.5f, 20.0f)/planets.Length + auMod; //AU
				planets[i].planet = planetsGas[RandomGenerator.getInt(0, planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], gasElements, elementNames);				//set atmospheric composition
				planets[i].atmPressure = PlanetOperations.planetPressure (planets[i].planetType,planets[i].flux,planets[i].mass);	//set atmospheric pressure

				count[1]++;
			}
		}else if((27430000000 * Mathf.Pow(starTemperature, 4) * Mathf.Pow(starRadius, 2))/Mathf.Pow((planets[i - 1].orbitRadius + 2) * 149597876600, 2) < 4){ //Ice giants introduced, depending on flux of orbit
			int r = RandomGenerator.getInt(0, 5); //Weighted for ice giants
			if(r == 0){
				//Terrestrial, past first orbit
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = planetsTerrestrial[RandomGenerator.getInt(0, planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], terrestrialElements, elementNames);		//set atmospheric composition
				planets[i].atmPressure = PlanetOperations.planetPressure (planets[i].planetType,planets[i].flux,planets[i].mass);	//set atmospheric pressure

				count[0]++;

			}else if(r == 1){
				//Regular jupiter, colder
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = planetsGas[RandomGenerator.getInt(0, planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], gasElements, elementNames);				//set atmospheric composition
				planets[i].atmPressure = PlanetOperations.planetPressure (planets[i].planetType,planets[i].flux,planets[i].mass);	//set atmospheric pressure

				count[1]++;

			}else{
				//Ice giant
				planets[i].planetType = 2;
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = planetsIce[RandomGenerator.getInt(0, planetsIce.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.neptuneToEarthMass(planets[i].mass), 2); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.neptuneToEarthMass(planets[i].mass), PlanetOperations.neptuneToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], iceElements, elementNames);				//set atmospheric composition
				planets[i].atmPressure = PlanetOperations.planetPressure (planets[i].planetType,planets[i].flux,planets[i].mass);	//set atmospheric pressure

				count[2]++;

			}
		}else{
			if(RandomGenerator.getInt(0, 2) == 0){
				//Terrestrial, middle solar system
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 4.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = planetsTerrestrial[RandomGenerator.getInt(0, planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], terrestrialElements, elementNames);		//set atmospheric composition
				planets[i].atmPressure = PlanetOperations.planetPressure (planets[i].planetType,planets[i].flux,planets[i].mass);	//set atmospheric pressure

				count[0]++;
				
			}else{
				//Regular jupiter
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 4.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].planet = planetsGas[RandomGenerator.getInt(0, planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], gasElements, elementNames);				//set atmospheric composition
				planets[i].atmPressure = PlanetOperations.planetPressure (planets[i].planetType,planets[i].flux,planets[i].mass);	//set atmospheric pressure

				count[1]++;
			}
		}

		//Using Stefan-Boltzmann Law, we can determine the amount of energy per second that reaches the surface. Uses Stefan-Boltzmann coef of 2.743* 10^10 W/(Solar radius^2 * Kelvin^4).
		planets[i].flux = (27430000000 * Mathf.Pow(starTemperature, 4) * Mathf.Pow(starRadius, 2))/Mathf.Pow((planets[i].orbitRadius) * 149597876600, 2);

		//Newton's enhancement of Kepler's third law, with conversion from 365 day year to 360 day year
		planets[i].orbitPeriod = 2f * Mathf.PI * Mathf.Sqrt(Mathf.Pow(planets[i].orbitRadius, 3f)/(starMass * 39.42f)) * (360f/365.24f);

		planets[i].angle = RandomGenerator.getFloat(0f, 360f); //Get angle from horizontal for start of game

		auMod = RandomGenerator.getFloat(2f, 4f)/planets.Length; //Change AU mod for next system

		//Spaces out smaller systems, to get some father out planets in systems of 5 or less planets
		if(planets.Length <= 5 && notSpaced){
			if(RandomGenerator.getInt(0, 4) == 0){
				auMod += RandomGenerator.getFloat(5.0f, 10.0f);
			}
		}

		for(int j = 0; j < planets[i].atmosphericComposition.Length; j++){
			Debug.Log(planets[i].planetType + " " + i + ": " + planets[i].atmosphericComposition[j].ToString());
		}

	}
}
