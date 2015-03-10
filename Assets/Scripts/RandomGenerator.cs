using UnityEngine;
using System.Collections;

using LibNoise.Unity.Generator;

public static class RandomGenerator {
	
	private static Perlin perlinGen = new Perlin();
	private static int[] planetNumber = new int[]{0, 1, 2, 3, 3, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 9, 10};

	public static void SetSeed(int seed){
		Random.seed = seed;
		perlinGen.Seed = seed;
	}

	public static void SetPerlin(int octaveCount, double frequency, double persistence){
		perlinGen.OctaveCount = octaveCount;
		perlinGen.Frequency = frequency;
		perlinGen.Persistence = persistence;
	}

	public static int GetInt(int min, int max){
		return Random.Range(min, max);
	}

	public static float GetFloat(float min, float max){
		return Random.Range(min, max);
	}

	public static double Get2DNoise(double x, double y){
		return perlinGen.GetValue(x, y, 1);
	}

	public static float GetStarMass(){
		float x = GetFloat(0, 1);
		//return 1.53846f * Mathf.Log(1/(1-x)) + 0.1f; //Exponential Distribution
		return Mathf.Log(1/(1-x)) + 0.1f; //Gamma distribution, alpha = 1, beta = 1
	}

	public static float GetGasMass(){
		float x = GetFloat(0, 1);
		return 2.85714f * Mathf.Log(1/(1-x)) + 0.1f;
	}

	public static float GetTerrestrialMass(){
		float x = GetFloat(0,1);
		return 1.66667f * Mathf.Log(1/(1-x)) + 0.1f;
	}

	public static int GetPlanetNumber(){
		return planetNumber[GetInt(0, planetNumber.Length)];
	}

	public static float GetTerrestrialPressure(float min, float max){
		float x = GetFloat(min, max);
		return Mathf.Pow (10f, 12f * (x - .75f));
	}

	public static float GetAlbedo(Planet planet){
		float albedo = 0;
		if(planet.planetType == 1){
			albedo = GetFloat(0.3f, 0.4f);
		}else if(planet.planetType == 2){
			albedo = GetFloat(0.25f, 0.35f);
		}else{
			if(planet.flux < 5f){
				albedo = GetFloat(0.4f, 0.99f);
			}else if(planet.flux > 6000f){
				albedo = GetFloat(0.0f, 0.1f);
			}else if(planet.atmPressure > 50f){
				albedo = GetFloat(0.85f, 0.95f);
			}else{
				albedo = GetFloat(0.2f, 0.4f);
			}
		}
		return albedo;
	}
}
