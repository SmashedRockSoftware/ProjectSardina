using UnityEngine;
using System.Collections;

public class Calender : MonoBehaviour {

	public static readonly double dayLength = 2; //Kept in seconds, on time 1
	public static readonly int monthLength = 30; //In days
	public static readonly int monthsInYear = 12; //In months
	public static readonly int startYear = 2025; //From 0 AD

	private float lastTime;
	private bool wasPaused;

	public static int[] date = new int[3]{1, 1, startYear};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Pause.GetPause()){
			wasPaused = true;
		}else if (wasPaused == true && Pause.GetPause() == false){
			lastTime = Time.time - lastTime;
			wasPaused = false;
		}
		if(Time.time - lastTime >= dayLength && Pause.GetPause() == false){
			if(date[0] > monthLength - 1){
				if(date[1] > monthsInYear - 1){
					date[2] = date[2] + 1;
					date[1] = 0;
					date[0] = 0;
					lastTime = Time.time;
				}
				date[1] = date[1] + 1;
				date[0] = 0;
				lastTime = Time.time;
			}
			date[0] = date[0] + 1;
			lastTime = Time.time;
		}
	}
}
