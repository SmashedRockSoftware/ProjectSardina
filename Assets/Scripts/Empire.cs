using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Empire : MonoBehaviour {

	public List<Fleet> fleets = new List<Fleet>();
	public Fleet temp;

	// Use this for initialization
	void Start () {
		fleets.Add(temp);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OrderFleets(GameObject g){
		for(int i = 0; i < fleets.Count; i++){
			fleets[i].SetTarget(g);
		}
	}
}
