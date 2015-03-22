using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour {

	public int timeSpeed = 1;
	public Text speedText;
	public Text dateText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		speedText.text = "" + timeSpeed;
		dateText.text = Calender.date[0] + "/" + Calender.date[1] + "/" + Calender.date[2];
	}

	public void IncrementSpeed (int increment){
		if(Calender.speed + increment <= 5 && Calender.speed + increment >= 1){
			GetComponent<NetworkView>().RPC("IncrementSpeedRPC", RPCMode.All, increment);
		}
	}

	[RPC]
	public void IncrementSpeedRPC(int increment){
		timeSpeed += increment;
		Calender.speed += increment;
	}
}
