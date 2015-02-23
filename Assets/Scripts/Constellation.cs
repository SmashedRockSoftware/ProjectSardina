using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Constellation {


	public string name;
	public int xStart;
	public int xEnd;
	public int yStart;
	public int yEnd;

	public List<Star> starList = new List<Star>();

	public Star[] stars;

	private int count = 1;

	public Constellation (string constellationName, int startX, int endX, int startY, int endY){
		name = constellationName;
		xStart = startX;
		xEnd = endX;
		yStart = startY;
		yEnd = endY;
	}

	public string getNextName (){
		string next = name + " " + count;
		count++;
		return next;
	}

	public void finalizeStars (){
		stars = starList.ToArray();

		//Name the stars. This is done here because references to the constellation do not exist at star creation.
		foreach(Star star in stars){
			star.starName = getNextName();
		}
	}
}
