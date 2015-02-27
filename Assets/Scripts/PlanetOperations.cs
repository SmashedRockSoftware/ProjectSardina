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
	
	public static Gas[] planetAtm(Planet planet, float[] element, string[] elementNames){
		float total = 0;
		for (int i = 0; i < element.Length; i++) {			
			if (element[i] == 0){
				element[i] = RandomGenerator.getFloat (-50, 100);		//change this to distribution later maybe
			}else{
				element[i] = RandomGenerator.getFloat (element[i]-10, element[i]+10);
			}
			if (element[i] <= 0){
				element[i] = 0;
			}
			total += element[i];
		}
		List<Gas> atmosphere = new List<Gas>();
		for (int i = 0; i < element.Length; i++) {
			if (element[i] >= 1){
				element[i] = (element[i]/total) * 100f;
				Gas gas = new Gas(element[i], elementNames[i]);
				atmosphere.Add (gas);

			}
		}
		return atmosphere.ToArray(); 
	}
	
	public static float planetPressure(int type, float flux, float mass){	//get pressure
		float pressure=0;
		if (type == 0) {
			if (flux > 6000) {
				pressure = RandomGenerator.getTerrestrialPressure (-.3f, .45f);
			} else if (flux > 7000 && mass > 2.5) {
				pressure = RandomGenerator.getTerrestrialPressure (0, .5f);
			} else if (mass < .2){
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

}
