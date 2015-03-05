using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour{

	public string planetName;
	public Star star; //Star this planet orbits

	//Orbit characteristics
	public float orbitRadius; //orbitRadius of orbit, in AU. Planets go in perfect circles currently
	public SpriteRenderer planet; //Sprite for planet
	public float orbitPeriod; //Period of orbit in years
	public float angle; //Angle of the planet from the horizontal (pos x axis)

	//Planetary characteristics
	public int planetType; //0 = terrestrial, 1 = gas giant, 2 = ice giant
	public float mass; //In earth/jupiter/neptune mass
	public float radius; //In earth/jupiter/neptune radius
	public float surfaceGrav; //In m/s^2
	public float flux; //In W/m^2, amount of energy reaching the surface (for earth this is the solar constant) per second
	public Gas[] atmosphericComposition;
	public float atmPressure; //In bars
	public float albedo; //Percentage of energy reflected
	public float temperature; //In celsius

	void FixedUpdate(){

	}

	public override string ToString (){
		return planetName;
	}
}
