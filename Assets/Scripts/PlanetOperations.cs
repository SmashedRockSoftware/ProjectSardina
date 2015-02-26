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
		float total=0, oxygen=0, nitrogen=0, co2=0, hydrogen=0, methane=0, h2o=0, neon=0, helium=0, other=30;
		List<float> element = new List<float> (){hydrogen, helium, methane, oxygen, nitrogen, co2, h2o, neon, other};	//could use a dictionary later
		string[] elementNames = new string[] {"hydrogen","helium","methane","oxygen","nitrogen","co2","h2o","neon","other"};
		for (int i = 0; i<=element.Count-1; i++) {			
			if (element[i]==0){
				element[i]=RandomGenerator.getFloat (-50,100);		//change this to distribution later maybe
			}
			else if ((i==0|i==1)&&(type==1|type==2)){
				element[i]=RandomGenerator.getFloat (200,300);
			}else{
				float min=element[i]-10f;
				float max=element[i]+10f;
				element[i]=RandomGenerator.getFloat (min,max);
				if (element[i]<=0){
					element[i]=0;
				}
			}
			total+=element[i];
		}
		List<float> atmosphere = new List<float> ();
		for (int i = 0; i<=element.Count-1; i++) {
			if (element[i]>1){
				element[i]=(element[i]/total)*100f;
				atmosphere.Add(element[i]);
			}
		}
		float[] atmComposition = atmosphere.ToArray ();
		planet.atmComposition = atmComposition;
		planet.gasNames = elementNames;
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
