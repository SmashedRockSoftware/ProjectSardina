using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlanetSelect : MonoBehaviour {

	private Text gText;
	private Text title;
	private Planet planet;
	private int window = 2;

	// Use this for initialization
	void Start () {
		gText = GetComponent<Text>() as Text;
		title = GameObject.FindGameObjectWithTag("NameText").GetComponent<Text>() as Text;
	}

	public void SetPlanet(Planet planet){
		this.planet = planet;
	}

	public void ChangeWindow(int window){
		this.window = window;
	}
	
	public void ChangeText(){
		if(planet != null){
			title.text = planet.planetName;
			if(window == 0){
				string text = "TO DO";
				gText.text = text;
			}else if(window == 1){
				if(planet.planet == null){
					string text = "Semimajor Axis: " + planet.orbitRadius.ToString("F2") + " AU\n";
					text += "Period: " + planet.orbitPeriod.ToString("F2") + " Years\n";
					if(planet.planetType == 0){
						text += "Mass: " + planet.mass.ToString("F2") + " Earth Masses\n";
						text += "Radius: " + planet.radius.ToString("F2") + " Earth Radii\n";
					}else if(planet.planetType == 1){
						text += "Mass: " + planet.mass.ToString("F2") + " Jupiter Masses\n";
						text += "Radius: " + planet.radius.ToString("F2") + " Jupiter Radii\n";
					}else{
						text += "Mass: " + planet.mass.ToString("F2") + " Neptune Masses\n";
						text += "Radius: " + planet.radius.ToString("F2") + " Neptune Radii\n";
					}
					text += "Surface Gravity: " + planet.surfaceGrav.ToString("F2") + " m/s²\n";
					text += "Albedo: " + planet.albedo.ToString("F2") + "\n";
					text += "Avg. Temperature: " + planet.temperature.ToString("F2") + " °C";
					gText.text = text;
				}else{
					string text = "Semimajor Axis: " + planet.orbitRadius.ToString("F2") + " LU\n";
					text += "Period: " + planet.orbitPeriod.ToString("F2") + " Days\n";
					text += "Mass: " + planet.mass.ToString("F2") + " Lunar Masses\n";
					text += "Radius: " + planet.radius.ToString("F2") + " Lunar Radii\n";
					text += "Surface Gravity: " + planet.surfaceGrav.ToString("F2") + " m/s²\n";
					text += "Albedo: " + planet.albedo.ToString("F2") + "\n";
					text += "Avg. Temperature: " + planet.temperature.ToString("F2") + " °C";
					gText.text = text;
				}
			}else{
				string text = "";
				if(planet.planet == null){
					if(planet.planetType == 0){
						text = "Atmospheric Pressure: " + planet.atmPressure.ToString("F2") + " Bar\n";
						text += "Atmospheric Compostion: \n";
					}else{
						text = "Atmospheric Composition at 1 Bar: \n";
					}
				}else{
					if(planet.planetType == 0){
						text = "No Atmosphere\n";
					}else{
						text = "Atmospheric Pressure: " + planet.atmPressure.ToString("F2") + " Bar\n";
					}
				}

				if(planet.atmosphericComposition != null){
					Gas[] sorted = SortGases(planet.atmosphericComposition);
					Gas g;
					for(int i = 0; i < sorted.Length; i++){
						g = sorted[i];
						text += UppercaseFirst(g.gasName) + ": " + (g.gasAmount * 100f).ToString("F2") + "%\n";
					}
				}
				text += "\nPlanetary Resources: \n";
				Resource r;
				for(int i = 0; i < planet.resources.Length; i++){
					r = planet.resources[i];
					text += UppercaseFirst(r.name) + ": " + r.amount + "\n";
				}
				gText.text = text;
			}
		}
	}

	public void ResetWindow(){
		planet = null;
		title.text = "No Planet Selected";
		gText.text = "";
	}

	private Gas[] SortGases(Gas[] start){
		List<Gas> gas = new List<Gas>();
		float other = 0;

		Gas g;
		for(int i = 0; i < start.Length; i++){
			g = start[i];
			Gas h = new Gas(0, "Temp");
			if(g.gasAmount > 0.01 && !g.gasName.Equals("other")){
				h.gasAmount = g.gasAmount;
				if(g.gasName.Equals("co2")){
					h.gasName = "Carbon Dioxide";
				}else if(g.gasName.Equals("h2o")){
					h.gasName = "Water Vapor";
				}else{
					h.gasName = g.gasName;
				}
				gas.Add(h);
			}else{
				other += g.gasAmount;
			}
		}


		gas.Sort();
		gas.Add(new Gas(other, "Other"));
		return gas.ToArray();
	}

	private string UppercaseFirst(string s)
	{
		// Check for empty string.
		if (string.IsNullOrEmpty(s))
		{
			return string.Empty;
		}
		// Return char and concat substring.
		return char.ToUpper(s[0]) + s.Substring(1);
	}
}
