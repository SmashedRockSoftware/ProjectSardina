using UnityEngine;
using System.Collections;

public class StarGenerator {

	public float starMass; //solar masses
	public float starRadius; //solar radii
	public float starLuminosity; //solar luminosity
	public float starTemperature; //kelvin
	public string starClass; //stellar class O through M

	public void GenerateStar () {
		//Starstuff
		starMass = RandomGenerator.getStarMass(); //Star mass is measured in solar masses
		
		//Simple mass-radius relationship
		if(starMass > 1.0f){
			starRadius = Mathf.Pow(starMass, 0.57f);
		}else{
			starRadius = Mathf.Pow(starMass, 0.8f);
		}
		
		//Luminosity-mass relationship for main sequence stars
		if(starMass < 0.43f){
			starLuminosity = 0.23f * Mathf.Pow(starMass, 2.3f);
		}else if(starMass < 2.0f){
			starLuminosity = Mathf.Pow(starMass, 4.0f);
		}else if(starMass < 20.0f){
			starLuminosity = 1.5f * Mathf.Pow(starMass, 3.5f);
		}else{
			starLuminosity = 3200 * starMass;
		}
		
		//Temerpature from luminosity and radius, using Stefan-Boltzmann Law
		starTemperature = 5780 * Mathf.Pow(starLuminosity/(starRadius * starRadius), 0.25f);
		
		//Stellar Classification on the Harvard scale. Since most (if not all) stars are main sequence, MK scale is unecessary. Sprite is also assigned here.
		if(starTemperature < 3500){
			starClass = "M";
		}else if(starTemperature < 5000){
			starClass = "K";
		}else if(starTemperature < 6000){
			starClass = "G";
		}else if(starTemperature < 7500){
			starClass = "F";
		}else if(starTemperature < 10000){
			starClass = "A";
		}else if(starTemperature < 30000){
			starClass = "B";
		}else{
			starClass = "O";
		}
	}

	public void FillStar(Star star){
		star.starMass = starMass;
		star.starRadius = starRadius;
		star.starLuminosity = starLuminosity;
		star.starTemperature = starTemperature;
		star.starClass = starClass;
	}
}
