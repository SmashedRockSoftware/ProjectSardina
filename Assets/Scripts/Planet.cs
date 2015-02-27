using UnityEngine;
using System.Collections;

public class Planet : MonoBehaviour{

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
	public float flux; //In W/m^2, amount of energy reaching the surface (for earth this is the solar constant)
	public Gas[] atmosphericComposition;
	public float atmPressure; //In bars

	private Vector3 position; //Position of planet

	void FixedUpdate(){

		//Movement
		angle += (0.5f/orbitPeriod)/60f;
		position = new Vector3(1000 + Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius, 0, Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius);

		//Reset angle when planet goes a rotation
		if(angle > 360){
			angle -= 360;
		}

	}

	public Vector3 getPos(){
		return position;
	}

	public override string ToString (){
		return "Planet: " + position;
	}
}
