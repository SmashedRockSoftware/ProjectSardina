using UnityEngine;
using System.Collections;

public class Calender : MonoBehaviour{

	public static readonly double dayLength = 2; //Kept in seconds, on time 1
	public static readonly int monthLength = 30; //In days
	public static readonly int monthsInYear = 12; //In months
	public static readonly int startYear = 2050; //From 0 AD
	public static readonly int[] speedMods = {1, 3, 5, 7, 9};
	public static int speed = 1;

	private float lastTime;
	private bool wasPaused;

	public static int[] date = new int[3]{1, 1, startYear};

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Network.isServer){
			if(Pause.GetPause()){
				wasPaused = true;
			}else if(wasPaused == true && Pause.GetPause() == false){
				lastTime = Time.time - lastTime;
				wasPaused = false;
			}
			if(Time.time - lastTime >= dayLength/speedMods[speed - 1] && Pause.GetPause() == false){
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
				GetComponent<NetworkView>().RPC("UpdateDate", RPCMode.Others, date);
			}
		}
	}

	public static int[] GetFutureDate(int days){
		int[] futureDate = {date[0], date[1], date[2]};
		futureDate[0] += days;
		while(futureDate[0] > 30){
			futureDate[0] -= 30;
			futureDate[1] += 1;
		}
		while(futureDate[1] > 12){
			futureDate[1] -= 12;
			futureDate[2] += 1;
		}
		return futureDate;
	}

	public static bool IsDate(int[] checkDate){
		if(checkDate[0] == date[0] && checkDate[1] == date[1] && checkDate[2] == date[2]){
			return true;
		}else{
			return false;
		}
	}

	[RPC]
	public void UpdateDate(int[] newDate){
		date = newDate;
	}
}
