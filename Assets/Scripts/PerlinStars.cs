using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerlinStars : MonoBehaviour {

	public float threshold;
	public float roughness;
	public float xRange;
	public float yRange;
	public GameObject star;
	public List<GameObject> starList = new List<GameObject>();
	public GameObject[] stars;

	// Use this for initialization
	void Start () {
		for(int x = 0; x < xRange; x++){
			for(int y = 0; y < yRange; y++){
				if(Mathf.PerlinNoise(x + roughness, y + roughness) > threshold){
					Instantiate(star, new Vector3(x, Random.Range(-1.0f, 1.0f), y), Quaternion.identity);
				}
			}
		}
	}
}
