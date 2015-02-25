using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinStars : MonoBehaviour {

	public float threshold;
	public double frequency;
	public double persistence;
	public int octaves;
	public int seed;
	public int constellationSize;
	public float xRange;
	public float yRange;
	public float spacing;
	public float offsetRange;
	public GameObject star;
	public List<GameObject> starList = new List<GameObject>();

	public SpriteRenderer[] planetsTempGas;
	public SpriteRenderer[] planetsTempTerrestrial;
	public SpriteRenderer[] planetsTempIce;
	public SpriteRenderer[] starsTemp;
	public Animator[] animatorsTemp;

	public static GameObject[] stars;
	public static Constellation[,] constellations;

	private string[] constellationNames = new string[]{"Alpha","Beta","Gamma","Delta","Epsilon","Zeta","Eta","Theta","Iota","Kappa","Lambda","Mu"};

	// Use this for initialization
	void Start () {
		Star.planetsIce = planetsTempIce;
		Star.planetsGas = planetsTempGas;
		Star.planetsTerrestrial = planetsTempTerrestrial;
		Star.starSprites = starsTemp;
		constellations = new Constellation[Mathf.RoundToInt((xRange + xRange * spacing)/constellationSize), Mathf.RoundToInt((yRange + yRange * spacing)/constellationSize)];

		RandomGenerator.setSeed(seed);
		RandomGenerator.setPerlin(octaves, frequency, persistence);

		//Generate star gameobjects
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

		//Destroying stars on top of each other
		for(int i = 0; i < starList.Count; i++){
			for(int x = 0; x < starList.Count; x++){
				if(starList[i] != starList[x]){
					if(Vector3.Distance(starList[i].transform.position, starList[x].transform.position) < 1f){
						GameObject star = starList[i];
						starList.Remove(star);
						Destroy(star);
					}
				}
			}
		}

		//Generationg constellations
		int count = 0;
		for(int x = 0; x < constellations.GetLength(0); x++){
			for(int y = 0; y < constellations.GetLength(1); y++){
				constellations[x,y] = new Constellation(constellationNames[count], x * constellationSize, (x + 1) * constellationSize, y * constellationSize, (y + 1) * constellationSize);
				count++;
			}
		}

		//Adding star object to the stars
		float lumin = 0;
		int luminSmall = 0;
		float mass = 0;
		int massSmall = 0;
		stars = starList.ToArray();
		for(int i = 0; i < stars.Length; i++){
			Star star = stars[i].AddComponent("Star") as Star;
			star.setCam(Camera.main);
			star.starAnimators = animatorsTemp;

			//Finding constellation of the star
			int row = 0;
			int column = 0;
			for(int j = 0; j < constellations.GetLength(0); j++){
				if(star.transform.position.x > constellations[j,0].xStart && star.transform.position.x < constellations[j,0].xEnd){
					column = j;
				}
			}

			for(int j = 0; j < constellations.GetLength(1); j++){
				if(star.transform.position.z > constellations[0,j].yStart && star.transform.position.z < constellations[0,j].yEnd){
					row = j;
				}
			}

			constellations[row,column].starList.Add(star);
			star.starConstellation = constellations[row,column];
			
			//Debug stats
			lumin += star.starLuminosity;
			if(star.starLuminosity < 2.0f){
				luminSmall++;
			}
			mass += star.starMass;
			if(star.starMass < 2.0f){
				massSmall++;
			}
		}

		for(int x = 0; x < constellations.GetLength(0); x++){
			for(int y = 0; y < constellations.GetLength(1); y++){
				constellations[x,y].finalizeStars();
			}
		}

		//Outputting debug stats
		Debug.Log(mass/stars.Length + " Avg Mass, " + massSmall + " small mass, " + (stars.Length - massSmall) + " large mass.");
		Debug.Log(lumin/stars.Length + " Avg Luminosity, " + luminSmall + " small lumin, " + (stars.Length - luminSmall) + " large lumin.");
		float total = Star.count[0] + Star.count[1] + Star.count[2];
		Debug.Log(Star.count[0] + "T " + Star.count[1] + "G " + Star.count[2] + "I, " + Star.count[0]/total + "T " + Star.count[1]/total + "G " + Star.count[2]/total + "I");

		//Setting up the camera
		BasicCamera cam = Camera.main.GetComponent<BasicCamera>() as BasicCamera;
		cam.XLimit = Mathf.RoundToInt(xRange + xRange * spacing);
		cam.ZLimit = Mathf.RoundToInt(yRange + yRange * spacing);
	}
}
