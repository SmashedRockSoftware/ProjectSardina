using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour{

	//On the left is planet definition, on the right is moon definition
	public string planetName;
	public Star star; //Star this planet orbits
	public Planet planet; //If moon, planet this moon orbits.
	public Planet[] moons;

	//Orbit characteristics
	public float orbitRadius; //Radius of orbit, in AU/LU (Lunar units, distance from moon to earth)
	public GameObject sprite; //Sprite for planet
	public float orbitPeriod; //Period of orbit in years/days

	//Planetary characteristics
	public int planetType; //0 = terrestrial, 1 = gas giant, 2 = ice giant. Moons: 0 = no atmosphere, 1 = atmosphered
	public float mass; //In earth/jupiter/neptune mass. In lunar mass.
	public float radius; //In earth/jupiter/neptune radius. In lunar radius.
	public float surfaceGrav; //In m/s^2
	public float flux; //In W/m^2, amount of energy reaching the atmosphere (for earth this is the solar constant) per second
	public float atmPressure; //In bars
	public float albedo; //Percentage of energy reflected
	public float temperature; //In celsius
	public Gas[] atmosphericComposition; //Array to hold gases of the atmosphere
	public Resource[] resources; //Resources and amounts on this planet
	
	public static GameObject[] moonSprites; //Sprites assigned in star controller

	//For generation of moons
	private string[] moonNames = new string[]{"I","II","III","IV"};
	private int[] moonNumbersT = new int[]{0, 0, 1, 1, 1, 2};
	private int[] moonNumbersG = new int[]{1, 2, 2, 3, 3, 3, 3, 4, 4};
	private int[] moonNumbersI = new int[]{0, 1, 1, 2, 2, 2, 2, 3, 3};

	//Planet atmosphere vars
	//Assign an element a specific number if you want it to be more or less abundant
	//If you want an element to not be spawned, then set it to -10
	//If an element is 0, then the generator will pick a number between -700 and 300, or it will make it a primary element
	//Elements assigned to 11 will never be a primary element
	private float[] gasElements = new float[]	{11,          11,      0,         0,         0,         0,     11,    11,      11};//Atmospheric composition for atmosphere'd moons
	private string[] elementNames = new string[]	{"hydrogen", "helium", "methane", "oxygen", "nitrogen", "co2", "h2o", "argon", "other"};//Must be in respective order as above

	//Planet resources vars
	//Planet resources are completely random provided values
	//This number represents the "richness" of a planet in said resource.
	//Give the names of the resources to be generated, and the range you want generated
	private string[] moonResources = new string[]{"Metals", "Test2", "Test3", "Test4"};
	private int[,] moonRange = new int[,]{{-100, 100},{-100, 100},{-100,100},{-100,100}};

	public static int[] count = new int[2]; //For debug messages

	void FixedUpdate(){

	}

	public void MoonGenerator(){
		moons = new Planet[GetMoonNumber()];

		float pRadius = PlanetOperations.AUtoLU(PlanetOperations.EarthRadiusToAU(radius));
		if(planetType == 1){
			pRadius  = PlanetOperations.AUtoLU(PlanetOperations.EarthRadiusToAU(PlanetOperations.JupiterToEarthRadius(radius)));
		}else if(planetType == 2){
			pRadius  = PlanetOperations.AUtoLU(PlanetOperations.EarthRadiusToAU(PlanetOperations.NeptuneToEarthRadius(radius)));
		}

		for(int i = 0; i < moons.Length; i++){
			Planet moon = star.gameObject.AddComponent<Planet>() as Planet;

			moon.planet = this;

			moon.mass = RandomGenerator.GetTerrestrialMass(); //Set mass
			moon.radius = PlanetOperations.GetRadiusMass(moon.mass, 0); //Set radius 
			moon.surfaceGrav = PlanetOperations.GetSurfaceGrav(PlanetOperations.MoonToEarthMass(moon.mass), PlanetOperations.MoonToEarthRadius(moon.radius));//Surface gravity by proportion to earth

			moon.resources = PlanetOperations.PlanetResources(moonResources, moonRange);

			moon.flux = flux; //This should be changed sometime

			if(i == 0){
				
				moon.orbitRadius = RandomGenerator.GetFloat(0.5f, 1.0f) + pRadius; //LU

				if(RandomGenerator.GetInt(0, 10) != 0){
					moon.planetType = 0; //Set planet type
					moon.sprite = moonSprites[RandomGenerator.GetInt(0, moonSprites.Length)]; //Get planet sprite
					moon.atmosphericComposition = null;	//No atmosphere on airless moons
					
					count[0]++;
				}else{
					moon.planetType = 1; //Set planet type
					moon.sprite = moonSprites[RandomGenerator.GetInt(0, moonSprites.Length)]; //Get planet sprite
					moon.atmosphericComposition = PlanetOperations.PlanetAtm(moon, gasElements, elementNames); //Atmosphere on air'd moons

					count[1]++;
				}
			}else{
				
				moon.orbitRadius = RandomGenerator.GetFloat(0.5f, 1.0f) + moons[i - 1].orbitRadius; //LU

				if(RandomGenerator.GetInt(0, 10) != 0){
					moon.planetType = 0; //Set planet type
					moon.sprite = moonSprites[RandomGenerator.GetInt(0, moonSprites.Length)]; //Get planet sprite
					moon.atmosphericComposition = null;	//No atmosphere on airless moons
					
					count[0]++;
				}else{
					moon.planetType = 1; //Set planet type
					moon.sprite = moonSprites[RandomGenerator.GetInt(0, moonSprites.Length)]; //Get planet sprite
					moon.atmosphericComposition = PlanetOperations.PlanetAtm(moon, gasElements, elementNames); //Atmosphere on air'd moons

					count[1]++;
				}
				
			}

			moon.planetName = planetName + " " + moonNames[i];
			
			//Set planet atm pressure. Done here because planet flux doesn't exist before now. Not done for 0 moons
			if(moon.planetType == 1){
				moon.atmPressure = PlanetOperations.PlanetPressure(moon);
			}
			
			//Albedo
			moon.albedo = RandomGenerator.GetAlbedo(moon);
			
			//Temperature
			moon.temperature = PlanetOperations.PlanetTemperature(moon);

			float pMass = mass;
			if(planetType == 1){
				pMass = PlanetOperations.JupiterToEarthMass(mass);
			}else if(planetType == 2){
				pMass = PlanetOperations.NeptuneToEarthMass(mass);
			}
			
			//Newton's enhancement of Kepler's third law, with conversion from 365 day year to 360 day year. This time using G = 0.05236 LU^3/(Earth Mass * days^2)
			moon.orbitPeriod = 2f * Mathf.PI * Mathf.Sqrt(Mathf.Pow(moon.orbitRadius, 3f)/((pMass + PlanetOperations.MoonToEarthMass(moon.mass)) * 0.05236f));

			moons[i] = moon;
		}
	}

	public override string ToString (){
		return planetName;
	}

	private int GetMoonNumber(){
		if(planetType == 0){
			return moonNumbersT[RandomGenerator.GetInt(0, moonNumbersT.Length)];
		}else if(planetType == 1){
			return moonNumbersG[RandomGenerator.GetInt(0, moonNumbersG.Length)];
		}else{
			return moonNumbersI[RandomGenerator.GetInt(0, moonNumbersI.Length)];
		}
	}
}
