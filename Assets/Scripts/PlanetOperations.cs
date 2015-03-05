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
	
	public static Gas[] planetAtm(Planet planet, float[] gasArray, string[] elementNames){
		float[] element = new float[gasArray.Length];
		System.Array.Copy(gasArray, element, 9);
		bool primary = true;		//For more realistic figures, this boolean determines a specific element that is (most of the time) majority composition
		float total = 0;			//For the percent calculation later. Stores the total value of elements
		for (int i = 0; i < element.Length; i++) {			
			if (element[i] == 0){	//Calculates values not already set in Star class
				if (((RandomGenerator.getInt(0,6) == 1) && primary && !Mathf.Approximately(element[i], 11f))){		//Random chance that an element is picked as primary element (currently, the first ones in the list are technically more likely)
					element[i] = RandomGenerator.getFloat(500, 1400);	//High value for primary elements
					primary = false;	//So there is only one primary
				}else{
					element[i] = RandomGenerator.getFloat(-700, 300);	//Normal values
				}
			}else{
				element[i] = RandomGenerator.getFloat(element[i]-10, element[i]+10);	//Variance for pre set values in Star
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
				element[i] = (element[i]/total) * 100f;	//Calculates percentages
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
			} else if (flux > 6000 && mass > 2.5) {
				pressure = RandomGenerator.getTerrestrialPressure (0, .5f);
			} else if (mass < .2){
				pressure = RandomGenerator.getTerrestrialPressure (0, .5f);
			}else{
				pressure = RandomGenerator.getTerrestrialPressure (0, 1);
			}
		}
		if (type == 1 || type == 2) {
			pressure = 1000;
		}
		return pressure;
	}

	public static float planetTemperature (Planet planet, Gas gas) {
		float temperature;

		//unknown variables
			//Optical density
		float As=1f;	//Surface albedo

		//variables
		float S = planet.flux;
		float A = planet.albedo;
		float sigma = 0.00000000000332f;

		//Calculations begin

		float Pn = (gas.gasAmount[2]/100f) * planet.atmPressure;	//Methane partial pressure:

		float Pc = (gas.gasAmount[5]/100f) * planet.atmPressure;	//CO2 partial pressure:

		float Ph = (gas.gasAmount[6]/100f) * planet.atmPressure;	//H2O partial pressure:

		float t = 0.025f * Mathf.Pow (Pc, 0.53f) + 0.277 * Mathf.Pow (Ph, 0.3f);	//tau=0.025 Pc^0.53 + 0.277 Ph^0.3

		float tvis = .36f * Mathf.Pow ((t - 0.0723f), 0.411f);	//tvis = 0.36(t - 0.723)^0.411

		float F = (S / 4f) (1 - A);	//F = (S/4)(1-A)

		float Te = Mathf.Pow ((F / sigma), 0.25f);	//Te = (F/σ)^0.25

		float T0 = Te * Mathf.Pow ((1f + 0.75f * t), 0.25f);	//T0 = Te(1 + 0.75t)^0.25

		float F0 = sigma * Mathf.Pow (T0, 4f);	//F0 = σT0^4

		float Labs = F-Mathf.Pow (F, -tvis);	//Labs = F-F^-tvis

		float Fsi = F - Labs;	//Fsi = F - Labs

		float Fabs = (1f - As) * Fsi + (F0 - F);	//Fabs = (1 - As)Fsi + (F0 - F)

		float Fc = 0.369f * Fabs * t / (-0.6f + 2f * t);	//Fc = 0.369Fabs * t/(-0.6 + 2t)

		temperature = Mathf.Pow (((F0 - Labs - Fc) / sigma), 0.25f);//((F0 - Labs - Fc)/σ)^0.25

		return temperature;
	}

	public static float getSurfaceGrav(float massInEarths, float radiusInEarths){
		return 9.807f * (massInEarths/(radiusInEarths * radiusInEarths));
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
