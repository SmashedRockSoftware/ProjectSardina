using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MultiplayerManager : MonoBehaviour {

	public static int seed;
	public static string[] names;
	public static string userName;

	public Text startText;
	public GameObject startGameButton;
	public GameObject startGame;


	private int connectedPlayers = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnEnable(){
		Pause.SetPause(true);
		GetComponent<PerlinStars>().StartCoroutine("CreateMap");
		if(Network.isClient){
			GetComponent<NetworkView>().RPC("AddConnectedPlayer", RPCMode.All, userName);
			startGameButton.SetActive(false);
		}
	}

	public void StartGame(){
		GetComponent<NetworkView>().RPC("StartGameRPC", RPCMode.All);
		Camera.main.GetComponent<BasicCamera>().SetCamera(true);
	}

	[RPC]
	public void StartGameRPC(){
		startGame.SetActive(false);
	}

	[RPC]
	public void AddConnectedPlayer(string name){
		startText.text += name + " has connected.\n";
		if(Network.isServer){
			connectedPlayers += 1;
			if(Network.connections.Length <= connectedPlayers){
				GetComponent<NetworkView>().RPC("AddMessage", RPCMode.All, "All players have connected.");
			}
		}
	}

	[RPC]
	public void AddMessage(string message){
		startText.text += message + "\n";
	}
}
