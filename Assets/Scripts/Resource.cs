using UnityEngine;
using System.Collections;

public class Resource {

	public int amount; //1-100, richness of this resource on a planet
	public string name; //Name of this resource

	public Resource(int amount, string name){
		this.amount = amount;
		this.name = name;
	}

	public override string ToString(){
		return name + ": " + amount;
	}
}
