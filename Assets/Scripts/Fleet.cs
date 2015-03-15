using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fleet : MonoBehaviour {

	public List<Ship> ships = new List<Ship>();

	private bool isSelected = false;
	private SpriteRenderer select;

	// Use this for initialization
	void Start () {
		SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>() as SpriteRenderer[];
		select = sr[1];
	}
	
	// Update is called once per frame
	void Update () {
	
		if(isSelected){}

	}

	void OnMouseDown(){
		if(isSelected){
			select.enabled = false;
		}else{
			select.enabled = true;
		}
		isSelected = !isSelected;
	}
}
