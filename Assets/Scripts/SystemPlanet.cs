using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SystemPlanet : MonoBehaviour, IPointerClickHandler{

	private Planet planet;
	private PlanetSelect ps;

	// Use this for initialization
	void Start () {
		ps = GameObject.FindGameObjectWithTag("SystemText").GetComponent<PlanetSelect>() as PlanetSelect;
	}

	public void SetPlanet(Planet p){
		planet = p;
	}

	public void OnPointerClick (PointerEventData data) {
		ps.SetPlanet(planet);
		ps.ChangeText();
	}
}
