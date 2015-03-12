using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constellation {


	public string name;
	public float xStart;
	public float xEnd;
	public float yStart;
	public float yEnd;

	public List<Star> starList = new List<Star>();

	public Star[] stars;

	private int count = 1;

	public Constellation (string constellationName, float startX, float endX, float startY, float endY){
		name = constellationName;
		xStart = startX;
		xEnd = endX;
		yStart = startY;
		yEnd = endY;
	}

	public string GetNextName (){
		string next = name + "-" + count;
		count++;
		return next;
	}

	public void FinalizeStars (){
		stars = starList.ToArray();
	}

	public override string ToString ()
	{
		return name + ": (" + xStart + "-" + xEnd + ", " + yStart + "-" + yEnd + ")";
	}
}
