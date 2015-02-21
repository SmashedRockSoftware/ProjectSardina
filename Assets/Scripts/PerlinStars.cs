using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinStars : MonoBehaviour {

	public float threshold;
	public double frequency;
	public double persistence;
	public int octaves;
	public int seed;
	public float xRange;
	public float yRange;
	public float spacing;
	public float offsetRange;
	public GameObject star;
	public List<GameObject> starList = new List<GameObject>();

	public SpriteRenderer[] planetsTempGas;
	public SpriteRenderer[] planetsTempTerrestrial;
	public SpriteRenderer[] planetsTempIce;

	public static GameObject[] stars;
	public static SpriteRenderer[] planetsGas;
	public static SpriteRenderer[] planetsTerrestrial;
	public static SpriteRenderer[] planetsIce;

	// Use this for initialization
	void Start () {
		planetsIce = planetsTempIce;
		planetsGas = planetsTempGas;
		planetsTerrestrial = planetsTempTerrestrial;

		RandomGenerator.setSeed(seed);
		RandomGenerator.setPerlin(octaves, frequency, persistence);

		for(int x = 0; x < xRange; x++){
			for(int y = 0; y < yRange; y++){
				if(RandomGenerator.get2DNoise(x, y) > threshold){
					GameObject starI = Instantiate(star, new Vector3((x + spacing * x), 0, (y + spacing * y)), Quaternion.identity) as GameObject;
					starI.transform.Rotate(90, 0, 0);
					starI.transform.position = new Vector3(starI.transform.position.x + RandomGenerator.getFloat(0, offsetRange), 0, starI.transform.position.z + RandomGenerator.getFloat(0, offsetRange));
					starList.Add(starI);
				}
			}
		}

		for(int i = 0; i < starList.Count; i++){
			for(int x = 0; x < starList.Count; x++){
				if(starList[i] != starList[x]){
					if(Vector3.Distance(starList[i].transform.position, starList[x].transform.position) < 1f){
						GameObject star = starList[i];
						starList.Remove(star);
						Destroy(star);
						//Debug.Log("Destroyed star");
					}
				}
			}
		}

		float lumin = 0;
		int luminSmall = 0;
		float mass = 0;
		int massSmall = 0;
		stars = starList.ToArray();
		for(int i = 0; i < stars.Length; i++){
			Star star = stars[i].AddComponent("Star") as Star;
			star.setCam(Camera.main);
			lumin += star.starLuminosity;
			if(star.starLuminosity < 2.0f){
				luminSmall++;
			}
			mass += star.starMass;
			if(star.starMass < 2.0f){
				massSmall++;
			}
		}

		Debug.Log(mass/stars.Length + " Avg Mass, " + massSmall + " small mass, " + (stars.Length - massSmall) + " large mass.");
		Debug.Log(lumin/stars.Length + " Avg Luminosity, " + luminSmall + " small lumin, " + (stars.Length - luminSmall) + " large lumin.");
		float total = Star.count[0] + Star.count[1] + Star.count[2];
		Debug.Log(Star.count[0] + "T " + Star.count[1] + "G " + Star.count[2] + "I, " + Star.count[0]/total + "T " + Star.count[1]/total + "G " + Star.count[2]/total + "I");

		BasicCamera cam = Camera.main.GetComponent<BasicCamera>() as BasicCamera;
		cam.XLimit = Mathf.RoundToInt(xRange + xRange * spacing);
		cam.ZLimit = Mathf.RoundToInt(yRange + yRange * spacing);
	}
}
