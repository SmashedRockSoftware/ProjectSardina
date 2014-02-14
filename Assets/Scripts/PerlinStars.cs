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
					GameObject starI = Instantiate(star, new Vector3(x + 0.5f * x, 0, y + 0.5f * y), Quaternion.identity) as GameObject;
					starI.transform.Rotate(90, 0, 0);
					float scaleAdd = Random.Range(-0.5f, 0.5f);
					starI.transform.localScale = new Vector3(starI.transform.localScale.x + scaleAdd, starI.transform.localScale.y + scaleAdd, 0);
				}
			}
		}
	}
}
