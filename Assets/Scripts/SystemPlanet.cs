using UnityEngine;
using System.Collections;

public class SystemPlanet : MonoBehaviour {

	public Transform star;
	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(star.position, Vector3.up, speed);
	}
}
