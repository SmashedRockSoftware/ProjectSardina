using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlanetOperations {

	public static float getRadiusMass(float massInEarths, int type){
		float earth = 1.196f * Mathf.Pow(massInEarths, 0.412f);
		if(type == 2){
			return earthToNeptuneRadius(earth);
		}else if(type == 1){
			return earthToJupierRadius(earth);
		}else{
			return earth;
		}
	}

	public static float getSurfaceGrav(float massInEarths, float radiusInEarths){
		return 9.807f * (massInEarths/Mathf.Pow(radiusInEarths, 2f));
	}

	public static float jupiterToEarthMass(float jupiterMass){
		return 317.8f * jupiterMass;
	}

	public static float neptuneToEarthMass(float neptuneMass){
		return 17.147f * neptuneMass;
	}

	public static float earthToJupiterMass(float earthMass){
		return earthMass/317.8f;
	}
	
	public static float earthToNeptuneMass(float earthMass){
		return earthMass/17.147f;
	}

	public static float jupiterToEarthRadius(float jupiterRadius){
		return 11.209f * jupiterRadius;
	}
	
	public static float neptuneToEarthRadius(float neptuneRadius){
		return 3.883f * neptuneRadius;
	}
	
	public static float earthToJupierRadius(float earthRadius){
		return earthRadius/11.209f;
	}
	
	public static float earthToNeptuneRadius(float earthRadius){
		return earthRadius/3.883f;
	}

	public void planetAtm(int type, Planet planet, float flux, float mass){
		float oxygen, nitrogen, co2, hydrogen, methane, h2o, neon, helium, other;
		float first, second, third, fourth, fifth, sixth, seventh, eighth, temp;
		List<float> composition = new List<float> (first, second, third, fourth, fifth, sixth, seventh, eighth);
		if (type == 0) {											//atmosphere for terrestrials=type 0
			RandomGenerator.getFloat (20, 82) = first;			//picks a primary element first, an element that will make up (sometimes) most of atm
			RandomGenerator.getFloat (0, 100 - first) = second;	//with every assignement, the numbers naturally get smaller, use that to your advantage
			temp = first + second;
			if (temp != 100) {									//checks if the total number hasn't already reached 100%, if not, it keeps on going
				RandomGenerator.getFloat (0, 100 - temp) = third;
				temp += third;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = fourth;
				temp += fourth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = fifth;
				temp += fifth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = sixth;
				temp += sixth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = seventh;
				temp += seventh;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = eighth;
				temp += eighth;
			}
		}
		if (type == 1) {											//atm for gas giants
			RandomGenerator.getFloat (82, 99) = first;			//higher chance for hydrogen to make up most of the atmosphere (as defined below)
			RandomGenerator.getFloat (0, 100 - first) = second;
			temp = first + second;
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = third;
				temp += third;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = fourth;
				temp += fourth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = fifth;
				temp += fifth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = sixth;
				temp += sixth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = seventh;
				temp += seventh;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = eighth;
				temp += eighth;
			}
		}
		if (type == 2) {											//atm for ice giants
			RandomGenerator.getFloat (72, 92) = first;			//modified from gas giants to allow for higher amounts
			RandomGenerator.getFloat (0, 100 - first) = second;
			temp = first + second;
			if (temp != 100) {
				RandomGenerator.getFloat (0.0, 100 - temp) = third;
				temp += third;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = fourth;
				temp += fourth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = fifth;
				temp += fifth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = sixth;
				temp += sixth;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100f - temp) = seventh;
				temp += seventh;
			}
			if (temp != 100) {
				RandomGenerator.getFloat (0, 100 - temp) = eighth;
				temp += eighth;
			}			
		}
		other=100-temp;									//for any leftovers

		if (type == 0) {										//normal likeliness of hydrogen for terrestrials	
			hydrogen = RandomGenerator.getInt(composition.Count);
			composition.Remove (hydrogen);
		}
		if (type == 1|type == 2) {								//highest amounts of hydrogen in ice giants and gas giants
			hydrogen=first;
		}
		if (type == 2) {										//methane is second most abundant for ice giants
			methane=second;
		}
		if (type == 1 | type == 0) {							//normal amounts of methane for gas giants and terrestrials
			methane = RandomGenerator.getInt(composition.Count);
			composition.Remove (methane);
		}
		oxygen = RandomGenerator.getInt(composition.Count);
		composition.Remove (oxygen);

		nitrogen = RandomGenerator.getInt(composition.Count);
		composition.Remove (nitrogen);

		co2 = RandomGenerator.getInt(composition.Count);
		composition.Remove (co2);

		h2o = RandomGenerator.getInt(composition.Count);
		composition.Remove (h2o);

		neon = RandomGenerator.getInt(composition.Count);
        composition.Remove (neon);

		helium = RandomGenerator.getInt(composition.Count);
		composition.Remove (helium);

		float[] planetComposition = new float[]{hydrogen, methane, oxygen, nitrogen, co2, h2o, neon, helium, other};
		string[] elementNames = new string[] {"hydrogen","methane","oxygen","nitrogen","co2","h2o","neon","helium","other"};

		Planet.atmComposition = planetComposition;
		Planet.gasNames = elementNames;							//End atmospheric composition generator
	}

	public static float planetPressure(int type, float flux, float mass){	//get pressure
		float pressure;
		if (type == 0) {
			if (flux > 6000) {
				pressure = RandomGenerator.getTerrestrialPressure (-.3f, .45f);
			} else if (flux > 7000 && mass > 2.5) {
				pressure = RandomGenerator.getTerrestrialPressure (0, .5f);
			} else if (mass<.2){
				pressure = RandomGenerator.getTerrestrialPressure (0, .5f);
			}else{
				pressure = RandomGenerator.getTerrestrialPressure (0, 1);
			}
		}
		if (type == 1 | type == 2) {
			pressure=1000;
		}
		return pressure;
	}

	public static bool testHabilability(string dependGas, float dependGasAmount, float dependPressure, float dependTemperature, float dependGravity){
	}
}
