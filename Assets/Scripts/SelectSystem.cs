using UnityEngine;
using System.Collections;

public class SelectSystem : MonoBehaviour {

	Star star;
	MoveShip ship;
	public Camera planetCam;
	

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
				if(hit.transform.gameObject.GetComponent<MoveShip>() != null){
					ship = hit.transform.gameObject.GetComponent<MoveShip>() as MoveShip;
				}
			}
		}
		if(Input.GetButton("Fire2") && Camera.main == null){
			star.UnloadSystem();
		}

		if(Input.GetButton("Fire2") && Camera.main != null){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)){
				if(ship != null){
					if(hit.transform.gameObject.GetComponent<Star>() != null){
						ship.ShipMover(hit.transform.gameObject.transform.position + new Vector3(0, 1, 0.5f), 0.25f);
					}
				}
			}
		}
	}
}
