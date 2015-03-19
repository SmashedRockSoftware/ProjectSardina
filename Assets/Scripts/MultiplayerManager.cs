using UnityEngine;
using System.Collections;

public class MultiplayerManager : MonoBehaviour {

	public static int seed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		Debug.Log("Loaded");
		GetComponent<PerlinStars>().StartCoroutine("CreateMap");
	}
}
