using UnityEngine;
using System.Collections;

public class SystemPlanet : MonoBehaviour {

	private Planet planet;
	private PlanetSelect ps;

	// Use this for initialization
	void Start () {
		ps = GameObject.FindGameObjectWithTag("SystemText").GetComponent<PlanetSelect>() as PlanetSelect;
	}

	public void SetPlanet(Planet p){
		planet = p;
	}

	void OnMouseDown () {
		ps.SetPlanet(planet);
		ps.ChangeText();
	}
}
