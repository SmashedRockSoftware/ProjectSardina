using UnityEngine;
using System.Collections;

public class SystemPlanet : MonoBehaviour {

	public SpriteRenderer sr;

	public void setUpPlanet(Planet planet){
		sr.sprite = planet.sprite.sprite;
		if(planet.planetType == 0){
			transform.localScale = new Vector3(1f, 1f, 1f);
		}else if(planet.planetType == 1){
			transform.localScale = new Vector3(4f, 4f, 4f);
		}else{
			transform.localScale = new Vector3(2f, 2f, 2f);
		}
		transform.position = new Vector3(1000 - SystemStar.nextPlanetLoc, 0, 0);
		SystemStar.nextPlanetLoc += 2.5f;
	}
}
