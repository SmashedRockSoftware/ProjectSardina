using UnityEngine;
using System.Collections;

public class SystemPlanet : MonoBehaviour {

	public Transform star;
	public Planet planet; 

	// Use this for initialization
	void Start () {
		planet = null;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(planet != null){
			transform.position = planet.getPos();
		}
	}
}
