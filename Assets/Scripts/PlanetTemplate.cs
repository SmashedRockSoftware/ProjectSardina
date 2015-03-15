using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetTemplate {

	public GameObject[] sprite;

	public float pressureMin; //In bars
	public float pressureMax;
	public float albedoMin; //Percentage of energy reflected
	public float albedoMax;
	public float[,] gasRanges;
	public string[] gasNames;

	public PlanetTemplate (float[,] gasRanges, string[] gasNames, float pressureMin, float pressureMax, float albedoMin, float albedoMax){
		this.gasRanges = gasRanges;
		this.gasNames = gasNames;
		this.pressureMin = pressureMin;
		this.pressureMax = pressureMax;
		this.albedoMin = albedoMin;
		this.albedoMax = albedoMax;
	}

	public void FillPlanet(Planet planet){
		planet.atmPressure = RandomGenerator.GetFloat(pressureMin, pressureMax);
		planet.albedo = RandomGenerator.GetFloat(albedoMin, albedoMax);
		float total = 0;

		List<Gas> gases = new List<Gas>();
		for(int i = 0; i < gasNames.Length; i++){
			Gas g = new Gas(RandomGenerator.GetFloat(gasRanges[i,0], gasRanges[i,1]), gasNames[i]);
			total += g.gasAmount;
			gases.Add(g);
		}
		gases = RemoveZeros(gases);

		Gas[] result = new Gas[gases.Count];
		for(int i = 0; i < result.Length; i++){
			Gas g = new Gas(gases[i].gasAmount/total, gases[i].gasName);
			result[i] = g;
		}
		planet.atmosphericComposition = result;
	}

	private List<Gas> RemoveZeros(List<Gas> gases){
		List<Gas> result = new List<Gas>();
		for(int i = 0; i < gases.Count; i++){
			if(gases[i].gasAmount > 0){
				result.Add(gases[i]);
			}
		}
		return result;
	}

}
