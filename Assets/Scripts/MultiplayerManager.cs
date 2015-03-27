using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour {

	public static int seed;
	public static string[] names;
	public static string userName;
	public static Color playerColor;

	public Text startText;
	public GameObject startGameButton;
	public GameObject startGame;
	public GameObject startFleet;


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
		if(Network.isServer){
			for(int i = 0; i < PerlinStars.stars.Length; i++){
				GameObject go = PerlinStars.stars[i];
				GetComponent<NetworkView>().RPC("StarSync", RPCMode.All, go.name, Network.AllocateViewID());
			}
		}
		GetComponent<NetworkView>().RPC("StartGameRPC", RPCMode.All);
		CreateStartLocations();
		Camera.main.GetComponent<BasicCamera>().SetCamera(true);
	}

	public void CreateStartLocations(){
		GameObject[] locs = new GameObject[Network.connections.Length + 1];
		List<int> used = new List<int>();
		int random = 0;
		for(int i = 0; i < locs.Length; i++){
			random = RandomGenerator.GetInt(0, PerlinStars.stars.Length);
			while(used.Contains(random)){
				random = RandomGenerator.GetInt(0, PerlinStars.stars.Length);
			}
			used.Add(random);
			locs[i] = PerlinStars.stars[random];
		}
		GiveStartLocation(locs[0].name);
		for(int i = 1; i < locs.Length; i++){
			GetComponent<NetworkView>().RPC("GiveStartLocation", Network.connections[i-1], locs[i].name);
		}
	}

	[RPC]
	public void StarSync(string name, NetworkViewID id){
		GameObject g = GameObject.Find(name);
		NetworkView nv = g.AddComponent<NetworkView>() as NetworkView;
		nv.observed = g.GetComponent<PoliticalStar>();
		nv.viewID = id;
		nv.stateSynchronization = NetworkStateSynchronization.ReliableDeltaCompressed;
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

	[RPC]
	public void GiveStartLocation(string name){
		GameObject g = GameObject.Find(name);
		GameObject f = Network.Instantiate(startFleet, g.transform.position + Vector3.up, Quaternion.identity, 0) as GameObject;
		f.transform.Rotate(new Vector3(90, 0, 0));
		f.GetComponent<Fleet>().currentStar = g;
		f.GetComponent<Fleet>().owner = Network.player;
		GetComponent<Empire>().fleets.Add(f);
		GetComponent<Empire>().AddStar(g);
		Camera.main.GetComponent<BasicCamera>().PositionCamera(g.transform.position);

	}
}
