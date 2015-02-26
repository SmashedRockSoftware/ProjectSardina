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

	public static void planetAtm(int type, Planet planet){
		float oxygen=0, nitrogen=0, co2=0, hydrogen=0, methane=0, h2o=0, neon=0, helium=0, other=0;
		float first=0, second=0, third=0, fourth=0, fifth=0, sixth=0, seventh=0, eighth=0, temp=0;
		List<float> composition = new List<float> (){first, second, third, fourth, fifth, sixth, seventh, eighth};
		if (type == 0) {											//atmosphere for terrestrials=type 0
			first=RandomGenerator.getFloat (20f, 82f);			//picks a primary element first, an element that will make up (sometimes) most of atm
			second=RandomGenerator.getFloat (0f, 100f - first);	//with every assignement, the numbers naturally get smaller, use that to your advantage
			temp = first + second;
			if (temp != 100f) {									//checks if the total number hasn't already reached 100%, if not, it keeps on going
				third=RandomGenerator.getFloat (0f, 100f - temp);
				temp += third;
			}
			if (temp != 100f) {
				fourth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += fourth;
			}
			if (temp != 100f) {
				fifth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += fifth;
			}
			if (temp != 100f) {
				sixth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += sixth;
			}
			if (temp != 100f) {
				seventh=RandomGenerator.getFloat (0f, 100f - temp);
				temp += seventh;
			}
			if (temp != 100f) {
				eighth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += eighth;
			}
		}
		if (type == 1) {											//atm for gas giants
			first=RandomGenerator.getFloat (82f, 99f);		//higher chance for hydrogen to make up most of the atmosphere (as defined below)
			second=RandomGenerator.getFloat (0f, 100f - first);
			temp = first + second;
			if (temp != 100f) {									//checks if the total number hasn't already reached 100%, if not, it keeps on going
				third=RandomGenerator.getFloat (0f, 100f - temp);
				temp += third;
			}
			if (temp != 100f) {
				fourth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += fourth;
			}
			if (temp != 100f) {
				fifth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += fifth;
			}
			if (temp != 100f) {
				sixth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += sixth;
			}
			if (temp != 100f) {
				seventh=RandomGenerator.getFloat (0f, 100f - temp);
				temp += seventh;
			}
			if (temp != 100f) {
				eighth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += eighth;
			}
		}
		if (type == 2) {											//atm for ice giants
			first=RandomGenerator.getFloat (72f, 92f);			//modified from gas giants to allow for higher amounts
			second=RandomGenerator.getFloat (0f, 100f - first);
			temp = first + second;
			if (temp != 100f) {									//checks if the total number hasn't already reached 100%, if not, it keeps on going
				third=RandomGenerator.getFloat (0f, 100f - temp);
				temp += third;
			}
			if (temp != 100f) {
				fourth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += fourth;
			}
			if (temp != 100f) {
				fifth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += fifth;
			}
			if (temp != 100f) {
				sixth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += sixth;
			}
			if (temp != 100f) {
				seventh=RandomGenerator.getFloat (0f, 100f - temp);
				temp += seventh;
			}
			if (temp != 100f) {
				eighth=RandomGenerator.getFloat (0f, 100f - temp);
				temp += eighth;
			}			
		}
		other=100-temp;									//for any leftovers

		if (type == 0) {										//normal likeliness of hydrogen for terrestrials	
			hydrogen = composition[RandomGenerator.getInt(0,composition.Count)];
			composition.Remove (hydrogen);
		}
		if (type == 1|type == 2) {								//highest amounts of hydrogen in ice giants and gas giants
			hydrogen=first;
		}
		if (type == 2) {										//methane is second most abundant for ice giants
			methane=second;
		}
		if (type == 1 | type == 0) {							//normal amounts of methane for gas giants and terrestrials
			methane = composition[RandomGenerator.getInt(0,composition.Count)];
			composition.Remove (methane);
		}


		oxygen = composition[RandomGenerator.getInt(0,composition.Count)];
		composition.Remove (oxygen);

		nitrogen = composition[RandomGenerator.getInt(0,composition.Count)];
		composition.Remove (nitrogen);

		co2 = composition[RandomGenerator.getInt(0,composition.Count)];
		composition.Remove (co2);

		h2o = composition[RandomGenerator.getInt(0,composition.Count)];
		composition.Remove (h2o);

		neon = composition[RandomGenerator.getInt(0,composition.Count)];
        composition.Remove (neon);

		helium = composition[RandomGenerator.getInt(0,composition.Count)];
		composition.Remove (helium);

		float[] planetComposition = new float[]{hydrogen, methane, oxygen, nitrogen, co2, h2o, neon, helium, other};
		string[] elementNames = new string[] {"hydrogen","methane","oxygen","nitrogen","co2","h2o","neon","helium","other"};



		planet.atmComposition = planetComposition;
		planet.gasNames = elementNames;							//End atmospheric composition generator
	}

	public static float planetPressure(int type, float flux, float mass){	//get pressure
		float pressure=0;
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
}
