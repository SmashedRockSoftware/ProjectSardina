﻿using UnityEngine;
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
	public List<Vector3> starPositionsList = new List<Vector3>();

	public SpriteRenderer[] planetsTempGas;
	public SpriteRenderer[] planetsTempTerrestrial;
	public SpriteRenderer[] planetsTempIce;
	public SpriteRenderer[] moonsTemp;
	public GameObject[] starObjects;

	public static GameObject[] stars;
	public static Constellation[,] constellations;

	private string[] constellationNames = new string[]{"Alpha","Beta","Gamma","Delta","Epsilon","Zeta","Eta","Theta","Iota","Kappa","Lambda","Mu"};
	private Vector3[] starPositions;
	private List<GameObject> starList = new List<GameObject>();

	// Use this for initialization
	void Start () {
		Planet.moonSprites = moonsTemp;
		Star.planetsIce = planetsTempIce;
		Star.planetsGas = planetsTempGas;
		Star.planetsTerrestrial = planetsTempTerrestrial;
		
		constellations = new Constellation[Mathf.RoundToInt((xRange + xRange * spacing)/constellationSize), Mathf.RoundToInt((yRange + yRange * spacing)/constellationSize)];

		RandomGenerator.setSeed(seed);
		RandomGenerator.setPerlin(octaves, frequency, persistence);

		//Generate star gameobjects
		for(int x = 0; x < xRange; x++){
			for(int y = 0; y < yRange; y++){
				if(RandomGenerator.get2DNoise(x, y) > threshold){
					starPositionsList.Add(new Vector3((x + spacing * x) + RandomGenerator.getFloat(0, offsetRange), 0, (y + spacing * y) + RandomGenerator.getFloat(0, offsetRange)));
				}
			}
		}

		//Destroying stars on top of each other
		for(int i = 0; i < starPositionsList.Count; i++){
			for(int x = 0; x < starPositionsList.Count; x++){
				if(starPositionsList[i] != starPositionsList[x]){
					if(Vector3.Distance(starPositionsList[i], starPositionsList[x]) < 1f){
						Vector3 star = starPositionsList[i];
						starPositionsList.Remove(star);
					}
				}
			}
		}

		//Generationg constellations
		int count = 0;
		for(int x = 0; x < constellations.GetLength(0); x++){
			for(int y = 0; y < constellations.GetLength(1); y++){
				constellations[x,y] = new Constellation(constellationNames[count], x * constellationSize, (x + 1) * (constellationSize + spacing), y * constellationSize, (y + 1) * constellationSize);
				count++;
				Debug.Log(constellations[x,y]);
			}
		}

		//Adding star object to the stars
		float lumin = 0;
		int luminSmall = 0;
		float mass = 0;
		int massSmall = 0;
		starPositions = starPositionsList.ToArray();
		StarGenerator gen = new StarGenerator();
		GameObject temp;
		for(int i = 0; i < starPositions.Length; i++){
			gen.GenerateStar();

			if(gen.starClass.Equals("M")){
				temp = starObjects[6];
			}else if(gen.starClass.Equals("K")){
				temp = starObjects[5];
			}else if(gen.starClass.Equals("G")){
				temp = starObjects[4];
			}else if(gen.starClass.Equals("F")){
				temp = starObjects[3];
			}else if(gen.starClass.Equals("A")){
				temp = starObjects[2];
			}else if(gen.starClass.Equals("B")){
				temp = starObjects[1];
			}else{
				temp = starObjects[0];
			}

			GameObject go = Instantiate(temp, starPositions[i], Quaternion.identity) as GameObject;
			go.transform.eulerAngles = new Vector3(90, 0, 0);

			Star star = go.AddComponent<Star>() as Star;
			gen.FillStar(star);
			star.setCam(Camera.main);
			starList.Add(go);

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
			star.PlanetaryGenerator(); //Done now so planets can be named
			
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

		//Finalizing arrays from lists
		stars = starList.ToArray();
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
