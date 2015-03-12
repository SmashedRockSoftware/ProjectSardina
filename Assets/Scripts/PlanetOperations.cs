using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlanetOperations {

	public static float GetRadiusMass(float massInEarths, int type){
		float earth = 1.196f * Mathf.Pow(massInEarths, 0.412f);
		if(type == 2){
			return EarthToNeptuneRadius(earth);
		}else if(type == 1){
			return EarthToJupierRadius(earth);
		}else{
			return earth;
		}
	}

	public static float GetRadiusMassMoon(float massInMoons){
		float moon = 0.671f * Mathf.Exp(0.4241f * massInMoons);
		return moon;
	}
	
	public static Gas[] PlanetAtm(Planet planet, float[] gasArray, string[] elementNames){
		float[] element = new float[gasArray.Length];
		System.Array.Copy(gasArray, element, 9);
		bool primary = true;		//For more realistic figures, this boolean determines a specific element that is (most of the time) majority composition
		float total = 0;			//For the percent calculation later. Stores the total value of elements
		for (int i = 0; i < element.Length; i++) {			
			if (element[i] == 0){	//Calculates values not already set in Star class
				if (((RandomGenerator.GetInt(0,6) == 1) && primary && !Mathf.Approximately(element[i], 11f))){		//Random chance that an element is picked as primary element (currently, the first ones in the list are technically more likely)
					element[i] = RandomGenerator.GetFloat(500, 1400);	//High value for primary elements
					primary = false;	//So there is only one primary
				}else{
					element[i] = RandomGenerator.GetFloat(-700, 300);	//Normal values
				}
			}else{
				element[i] = RandomGenerator.GetFloat(element[i]-10, element[i]+10);	//Variance for pre set values in Star
				if (element[i] > 1500){	//Makes assigning values in Star worthwhile, or else there's a big chance your assigned value never gets used
					primary = false;
				}
			}
			if (element[i] <= 0){	//Remove some elements
				element[i] = 0;
			}
			total += element[i];
		}
		List<Gas> atmosphere = new List<Gas>();
		for (int i = 0; i < element.Length; i++) {
			if (element[i] >= 1){
				Gas gas = new Gas(element[i]/total, elementNames[i]);
				atmosphere.Add(gas);
			}
		}
		return atmosphere.ToArray(); 
	}
	
	public static float PlanetPressure(Planet planet){	//get pressure
		float pressure = 0;
		float mass = planet.mass;
		if(planet.planet != null){
			mass = MoonToEarthMass(planet.mass);
		}

		if (planet.planetType == 0 || planet.planet != null) {
			if (planet.flux > 6000) {
				pressure = RandomGenerator.GetTerrestrialPressure (-.3f, .45f);
			} else if (planet.flux > 6000 && mass > 2.5) {
				pressure = RandomGenerator.GetTerrestrialPressure (0, .5f);
			} else if (mass < .2){
				pressure = RandomGenerator.GetTerrestrialPressure (0, .5f);
			}else{
				pressure = RandomGenerator.GetTerrestrialPressure (0, 1);
			}
		}else if (planet.planetType == 1 || planet.planetType == 2) {
			pressure = 1000;
		}
		return pressure;
	}

	public static float PlanetTemperature (Planet planet) {
		float temperature;

		float As = 0.2f;	//Surface albedo, the effect of this is very small

		//variables
		float S = planet.flux; 
		float A = planet.albedo; 
		float pressure = planet.atmPressure * 100000; //From bars to pascals
		float sigma = 0.000000056704f; //In W/m^2K^4

		//Calculations begin
		if(planet.planetType == 0 || planet.planet != null){ //Terrestrial or moon
			float Pm = 0, Pc = 0, Ph = 0; //Methane, CO2, and H2O declared

			if(planet.atmosphericComposition != null){
				for(int i = 0; i < planet.atmosphericComposition.Length; i++){
					if(planet.atmosphericComposition[i].gasName.Equals("methane")){
						Pm = planet.atmosphericComposition[i].gasAmount * pressure;
					}else if(planet.atmosphericComposition[i].gasName.Equals("co2")){
						Pc = planet.atmosphericComposition[i].gasAmount * pressure;
					}else if(planet.atmosphericComposition[i].gasName.Equals("h2o")){
						Ph = planet.atmosphericComposition[i].gasAmount * pressure;
					}
				}
			}

			float t = 0.0290f * Mathf.Pow(Pc, 0.5f) + 0.083f * Mathf.Pow(Ph, 0.5f) + 0.225f * Mathf.Pow(Pm, 0.5f);	//tau=0.025 Pc^0.53 + 0.277 Ph^0.3

			float tvis = 0;
			if(t > 0.723){
				tvis = 0.36f * Mathf.Pow ((t - 0.723f), 0.411f);	//tvis = 0.36(t - 0.723)^0.411
			}

			float F = (S / 4f) * (1f - A);	//F = (S/4)(1-A)

			if(planet.planet != null){ //Reduce flux for moons based on % time blocked from the sun by primary
				float d = AUtoLU(EarthRadiusToAU(planet.planet.radius));
				if(planet.planet.planetType == 1){
					d = AUtoLU(EarthRadiusToAU(JupiterToEarthRadius(planet.planet.radius)));
				}else if(planet.planet.planetType == 2){
					d = AUtoLU(EarthRadiusToAU(NeptuneToEarthRadius(planet.planet.radius)));
				}
				
				F *= (1 - d/(2 * Mathf.PI * planet.orbitRadius));
			}

			float Te = Mathf.Pow ((F / sigma), 0.25f);	//Te = (F/σ)^0.25

			float T0 = Te * Mathf.Pow ((1f + 0.75f * t), 0.25f);	//T0 = Te(1 + 0.75t)^0.25

			float F0 = sigma * 0.95f * Mathf.Pow (T0, 4f);	//F0 = σT0^4

			float Labs = F - F*Mathf.Exp(-tvis);	//Labs = F-Fe^-tvis

			float Fsi = F - Labs;	//Fsi = F - Labs

			float Fabs = (1f - As) * Fsi + 0.95f * (F0 - F);	//Fabs = (1 - As)Fsi + (F0 - F)

			float Fc = 0.369f * Fabs * t/(-0.6f + 2f * t);	//Fc = 0.369Fabs * t/(-0.6 + 2t)

			temperature = Mathf.Pow(((F0 - Labs - Fc)/sigma), 0.25f);//((F0 - Labs - Fc)/σ)^0.25

			//Debug.Log(planet.orbitRadius + "AU " + planet.atmPressure + "P " + S + "S " + A + "A " + Pm + "m " + Pc + "c " + Ph + "h " + t + "t " + tvis + "tv " + F + "F " + Te + "Te " + T0 + "T0 " + F0 + "F0 " + Labs + "Labs " + Fsi + "Fsi " + Fabs + "Fabs " + Fc + "Fc " + temperature + "K " + planet.planetName);

			if(float.IsNaN(temperature)){
				//In case of NaN temps, use effective temp
				if(planet.planet == null){
					temperature = Mathf.Pow((planet.star.starLuminosity * (1 - A))/(16 * Mathf.PI * 0.00000000000332f * planet.orbitRadius * planet.orbitRadius), 0.25f);
				}else{
					temperature = Mathf.Pow((planet.planet.star.starLuminosity * (1 - A))/(16 * Mathf.PI * 0.00000000000332f * planet.planet.orbitRadius * planet.planet.orbitRadius), 0.25f);
				}
			}
		}else if(planet.planetType == 1){
			//Effective Temp + Simulated Core heating. At 1 bar. Using Solar Luminosities/AU^2K^4 S-B Constant
			temperature = Mathf.Pow((planet.star.starLuminosity * (1 - A))/(16 * Mathf.PI * 0.00000000000332f * planet.orbitRadius * planet.orbitRadius), 0.25f) + RandomGenerator.GetFloat(50f, 60f);
		}else{
			//Effective Temp + Simulated Core heating. At 1 bar. Using Solar Luminosities/AU^2K^4 S-B Constant
			temperature = Mathf.Pow((planet.star.starLuminosity * (1 - A))/(16 * Mathf.PI * 0.00000000000332f * planet.orbitRadius * planet.orbitRadius), 0.25f) + RandomGenerator.GetFloat(15f, 30f);
		}

		return temperature;
	}

	public static Resource[] PlanetResources(string[] planetResources, int[,] amount){
		Resource[] rArray = new Resource[planetResources.Length];

		for(int i = 0; i < planetResources.Length; i++){
			rArray[i] = new Resource(RandomGenerator.GetInt(amount[i,0], amount[i,1]), planetResources[i]);
		}

		List<Resource> resourceList = new List<Resource>();
		for(int i = 0; i < rArray.Length; i++){
			if(rArray[i].amount > 0){
				resourceList.Add(rArray[i]);
			}
		}

		return resourceList.ToArray();
	}

	public static float GetSurfaceGrav(float massInEarths, float radiusInEarths){
		return 9.807f * (massInEarths/(radiusInEarths * radiusInEarths));
	}

	public static float JupiterToEarthMass(float jupiterMass){
		return 317.8f * jupiterMass;
	}

	public static float NeptuneToEarthMass(float neptuneMass){
		return 17.147f * neptuneMass;
	}

	public static float MoonToEarthMass(float moonMass){
		return 0.012300f * moonMass;
	}

	public static float EarthToJupiterMass(float earthMass){
		return earthMass/317.8f;
	}
	
	public static float EarthToNeptuneMass(float earthMass){
		return earthMass/17.147f;
	}

	public static float EarthToMoonMass(float earthMass){
		return earthMass/0.012300f; 
	}

	public static float JupiterToEarthRadius(float jupiterRadius){
		return 11.209f * jupiterRadius;
	}
	
	public static float NeptuneToEarthRadius(float neptuneRadius){
		return 3.883f * neptuneRadius;
	}

	public static float MoonToEarthRadius(float moonRadius){
		return 0.273f * moonRadius;
	}
	
	public static float EarthToJupierRadius(float earthRadius){
		return earthRadius/11.209f;
	}
	
	public static float EarthToNeptuneRadius(float earthRadius){
		return earthRadius/3.883f;
	}

	public static float EarthToMoonRadius(float earthRadius){
		return earthRadius/0.273f;
	}

	public static float LUtoAU(float LU){
		return 0.00257f * LU;
	}

	public static float AUtoLU(float AU){
		return AU/0.00257f;
	}

	public static float EarthRadiusToAU(float earthRadius){
		return earthRadius * 0.000042563739f;
	}

}
