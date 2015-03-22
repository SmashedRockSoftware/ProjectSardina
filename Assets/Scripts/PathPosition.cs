using UnityEngine;
using System.Collections;

public class PathPosition {

	public GameObject go;
	public PathPosition parent;
	public float G;
	public float H;
	public float F;

	public PathPosition(GameObject g, PathPosition p, float G, float H, float F){
		go = g;
		parent = p;
		this.G = G;
		this.H = H;
		this.F = F;
	}

	public bool IsEqual(PathPosition p){
		if(go.Equals(p.go)){
			return true;
		}else{
			return false;
		}
	}

	public bool IsEqual(GameObject p){
		if(go.Equals(p)){
			return true;
		}else{
			return false;
		}
	}
}
