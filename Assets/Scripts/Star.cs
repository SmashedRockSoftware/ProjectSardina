using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Star : MonoBehaviour {

	//Planet vars
	public Planet[] planets;

	//Planet atmosphere vars
	//Assign an element a specific number if you want it to be more or less abundant
	//If you want an element to not be spawned, then set it to -10
	//If an element is 0, then the generator will pick a number between -700 and 300, or it will make it a primary element
	//Elements assigned to 11 will never be a primary elemen
	public float[] gasElements = new float[]		{5000,       400,      100,       11,       11,         11,    11,    11,      11};//Atmospheric composition for gas giants
	public float[] terrestrialElements = new float[]{11,         11,       0,         0,        0,          0,     11,    11,      11};//Atmospheric composition for terrestrials
	public float[] iceElements = new float[]		{5000,       400,      500,       11,       11,         11,    11,    11,      11};//Atmospheric composition for ice giants
	public string[] elementNames = new string[]		{"hydrogen", "helium", "methane", "oxygen", "nitrogen", "co2", "h2o", "argon", "other"};//Must be in respective order as above

	//Star vars, units listed
	public float starMass; //solar masses
	public float starRadius; //solar radii
	public float starLuminosity; //solar luminosity
	public float starTemperature; //kelvin
	public string starClass; //stellar class O through M
	public string starName; //Name of the star
	public Constellation starConstellation; //constellation this star is a part of
	public GameObject sprite;
	public Animator anim;

	private Camera mainCam;
	private float auMod = 0; //For systems with less planets, expands system out a bit
	private bool notSpaced = true; //Check if extra expansion for small systems has taken place
	private string[] planetNames = new string[]{"a","b","c","d","e","f","g","h","i","j"}; //For getting letter of planet

	//Planet sprites, assigned on the star controller
	public static GameObject[] planetsGas;
	public static GameObject[] planetsTerrestrial;
	public static GameObject[] planetsIce;

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

		//Star generation has been moved to the StarGenerator, name is done here
		starName = starConstellation.getNextName();
		gameObject.name = starName;

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
		SystemStar.LoadSystem(planets, this);
	}

	public void UnloadSystem () {
		mainCam.enabled = true;
		AudioListener listenerMain = mainCam.GetComponent<AudioListener>() as AudioListener;
		listenerMain.enabled = true;
		AudioListener listenerPlanet = SystemStar.planetCam.gameObject.GetComponent<AudioListener>() as AudioListener;
		listenerPlanet.enabled = false;
		SystemStar.planetCam.enabled = false;
		SystemStar.HidePlanets();
		SystemStar.nextPlanetLoc = 4;
	}

	void GeneratePlanet (int i) {
		Planet planet = gameObject.AddComponent<Planet>(); //Add a new planet script
		planets[i] = planet;
		planets[i].star = this;

		//Type determination
		if(i == 0){
			if(RandomGenerator.getInt(0, 2) == 0){
				//Terrestrial, close to star
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(0.1f, 1.0f) + auMod; //AU
				planets[i].sprite = planetsTerrestrial[RandomGenerator.getInt(0, planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth

				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], terrestrialElements, elementNames);		//set atmospheric composition

				count[0]++;

			}else{
				//Hot jupiter
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(0.1f, 1.0f) + auMod; //AU
				planets[i].sprite = planetsGas[RandomGenerator.getInt(0, planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], gasElements, elementNames);				//set atmospheric composition

				count[1]++;
			}
		}else if((27430000000 * Mathf.Pow(starTemperature, 4) * Mathf.Pow(starRadius, 2))/Mathf.Pow((planets[i - 1].orbitRadius + 2) * 149597876600, 2) < 4){ //Ice giants introduced, depending on flux of orbit
			int r = RandomGenerator.getInt(0, 5); //Weighted for ice giants
			if(r == 0){
				//Terrestrial, past first orbit
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].sprite = planetsTerrestrial[RandomGenerator.getInt(0, planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], terrestrialElements, elementNames);		//set atmospheric composition

				count[0]++;

			}else if(r == 1){
				//Regular jupiter, colder
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].sprite = planetsGas[RandomGenerator.getInt(0, planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius 
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], gasElements, elementNames);				//set atmospheric composition

				count[1]++;

			}else{
				//Ice giant
				planets[i].planetType = 2;
				planets[i].orbitRadius = RandomGenerator.getFloat(2f, 8.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].sprite = planetsIce[RandomGenerator.getInt(0, planetsIce.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.neptuneToEarthMass(planets[i].mass), 2); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.neptuneToEarthMass(planets[i].mass), PlanetOperations.neptuneToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], iceElements, elementNames);				//set atmospheric composition

				count[2]++;

			}
		}else{
			if(RandomGenerator.getInt(0, 2) == 0){
				//Terrestrial, middle solar system
				planets[i].planetType = 0; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(0.25f, 1.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].sprite = planetsTerrestrial[RandomGenerator.getInt(0, planetsTerrestrial.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getTerrestrialMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(planets[i].mass, 0); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(planets[i].mass, planets[i].radius);//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], terrestrialElements, elementNames);		//set atmospheric composition

				count[0]++;
				
			}else{
				//Regular jupiter
				planets[i].planetType = 1; //Set planet type
				planets[i].orbitRadius = RandomGenerator.getFloat(0.25f, 1.0f) + planets[i - 1].orbitRadius + auMod; //AU
				planets[i].sprite = planetsGas[RandomGenerator.getInt(0, planetsGas.Length)]; //Get planet sprite
				planets[i].mass = RandomGenerator.getGasMass(); //Set mass
				planets[i].radius = PlanetOperations.getRadiusMass(PlanetOperations.jupiterToEarthMass(planets[i].mass), 1); //Set radius
				planets[i].surfaceGrav = PlanetOperations.getSurfaceGrav(PlanetOperations.jupiterToEarthMass(planets[i].mass), PlanetOperations.jupiterToEarthRadius(planets[i].radius));//Surface gravity by proportion to earth
				
				planets[i].atmosphericComposition = PlanetOperations.planetAtm(planets[i], gasElements, elementNames);				//set atmospheric composition

				count[1]++;
			}
		}

		planets[i].planetName = starName + planetNames[i];

		//Using Stefan-Boltzmann Law, we can determine the amount of energy per second that reaches the surface. Uses Stefan-Boltzmann coef of 2.743* 10^10 W/(Solar radius^2 * Kelvin^4).
		planets[i].flux = (27430000000 * Mathf.Pow(starTemperature, 4) * Mathf.Pow(starRadius, 2))/Mathf.Pow((planets[i].orbitRadius) * 149597876600, 2);

		//Set planet atm pressure. Done here because planet flux doesn't exist before now.
		planets[i].atmPressure = PlanetOperations.planetPressure(planets[i].planetType, planets[i].flux, planets[i].mass);

		//Albedo
		planets[i].albedo = RandomGenerator.getAlbedo(planets[i]);

		//Temperature
		planets[i].temperature = PlanetOperations.planetTemperature(planets[i]) - 273.2f;

		//Newton's enhancement of Kepler's third law, with conversion from 365 day year to 360 day year
		planets[i].orbitPeriod = 2f * Mathf.PI * Mathf.Sqrt(Mathf.Pow(planets[i].orbitRadius, 3f)/(starMass * 39.42f)) * (360f/365.24f);

		if(i > 4){
			auMod = RandomGenerator.getFloat(2f, 4f); //Change AU mod for next system
		}

		//Spaces out smaller systems, to get some father out planets in systems of 5 or less planets
		if(planets.Length <= 5 && notSpaced){
			if(RandomGenerator.getInt(0, 4) == 0){
				auMod += RandomGenerator.getFloat(5.0f, 10.0f);
			}
		}

		planets[i].MoonGenerator();

		/*for(int j = 0; j < planets[i].atmosphericComposition.Length; j++){
			Debug.Log(planets[i].planetName + " " + planets[i].planetType + ": " + planets[i].atmosphericComposition[j].ToString());
		}*/
	}
}
