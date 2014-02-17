using UnityEngine;
using System.Collections;

public class SelectSystem : MonoBehaviour {

	Star star;

	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1") && Camera.main != null){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				if(hit.transform.gameObject.GetComponent<Star>() != null){
					star = hit.transform.gameObject.GetComponent<Star>() as Star;
					star.LoadSystem();
				}
			}
		}
		if(Input.GetButton("Fire2") && Camera.main == null){
			star.UnloadSystem();
		}
	}
}
