using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinStars : MonoBehaviour {

	public float threshold;
	public float roughness;
	public float xRange;
	public float yRange;
	public float spacing;
	public float scaleRange;
	public float offsetRange;
	public GameObject star;
	public List<GameObject> starList = new List<GameObject>();
	public static GameObject[] stars;
	public GameObject[] planets = new GameObject[2];

	// Use this for initialization
	void Start () {
		for(int x = 0; x < xRange; x++){
			for(int y = 0; y < yRange; y++){
				if(Mathf.PerlinNoise(x + roughness, y + roughness) > threshold){
					float xOffset = Random.Range(-offsetRange, offsetRange);
					float yOffset = Random.Range(-offsetRange, offsetRange);
					GameObject starI = Instantiate(star, new Vector3((x + spacing * x) + xOffset, 0, (y + spacing * y) + yOffset), Quaternion.identity) as GameObject;
					starI.transform.Rotate(90, 0, 0);
					float scaleAdd = Random.Range(-scaleRange, scaleRange);
					starI.transform.localScale = new Vector3(starI.transform.localScale.x + scaleAdd, starI.transform.localScale.y + scaleAdd, 0);
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
						Debug.Log("Destroyed star");
					}
				}
			}
		}

		stars = starList.ToArray();
		for(int i = 0; i < stars.Length; i++){
			Star star = stars[i].AddComponent("Star") as Star;
			star.PlanetList = planets;
		}
		for(int i = 0; i < stars.Length; i++){
			Star star = stars[i].GetComponent<Star>() as Star;
			star.ConnectionSharer();
		}
		for(int i = 0; i < stars.Length; i++){
			Star star = stars[i].GetComponent<Star>() as Star;
			star.ConnectionDrawer();
		}
		BasicCamera cam = Camera.main.GetComponent<BasicCamera>() as BasicCamera;
		cam.XLimit = Mathf.RoundToInt(xRange + xRange * spacing);
		cam.ZLimit = Mathf.RoundToInt(yRange + yRange * spacing);
	}
}
