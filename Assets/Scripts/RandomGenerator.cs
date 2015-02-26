using UnityEngine;
using System.Collections;

using LibNoise.Unity.Generator;

public static class RandomGenerator {
	
	private static Perlin perlinGen = new Perlin();
	private static int[] planetNumber = new int[]{0, 1, 2, 3, 3, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 7, 7, 8, 8, 9, 10};

	public static void setSeed(int seed){
		Random.seed = seed;
		perlinGen.Seed = seed;
	}

	public static void setPerlin(int octaveCount, double frequency, double persistence){
		perlinGen.OctaveCount = octaveCount;
		perlinGen.Frequency = frequency;
		perlinGen.Persistence = persistence;
	}

	public static int getInt(int min, int max){
		return Random.Range(min, max);
	}

	public static float getFloat(float min, float max){
		return Random.Range(min, max);
	}

	public static double get2DNoise(double x, double y){
		return perlinGen.GetValue(x, y, 1);
	}


	public static float getStarMass(){
		float x = getFloat(0, 1);
		//return 1.53846f * Mathf.Log(1/(1-x)) + 0.1f; //Exponential Distribution
		return Mathf.Log(1/(1-x)) + 0.1f; //Gamma distribution, alpha = 1, beta = 1
	}

	public static float getGasMass(){
		float x = getFloat(0, 1);
		return 2.85714f * Mathf.Log(1/(1-x)) + 0.1f;
	}

	public static float getTerrestrialMass(){
		float x = getFloat(0,1);
		return 1.66667f * Mathf.Log(1/(1-x)) + 0.1f;
	}

	public static int getPlanetNumber(){
		return planetNumber[getInt(0, planetNumber.Length)];
	}
	public static float getTerrestrialPressure(float min, float max){
		float x = getFloat (min, max);
		return Mathf.Pow (10f, 12f * (x - .75f));
	}
}
