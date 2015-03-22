using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviour {

	public Text playerList;
	public Text chat;
	public GameObject seed;
	public MainMenu menu;
	public bool ready = false;
	public List<string> names = new List<string>();

	private int readyPlayers = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log("Player connected from " + player.ipAddress + ":" + player.port);
	}

	void OnConnectedToServer(){
		string[] data = getUserData();
		menu.ChangeWindow(2);
		GetComponent<NetworkView>().RPC("AddPlayerName", RPCMode.AllBuffered, data[0]);
		seed.SetActive(false);
	}

	void OnServerInitialized(){
		string[] data = getUserData();
		menu.ChangeWindow(2);
		GetComponent<NetworkView>().RPC("AddPlayerName", RPCMode.AllBuffered, data[0]);
	}

	void OnDisconnectedFromServer(NetworkDisconnection info){
		if(!Network.isServer){
			menu.ChangeWindow(1);
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player){
		Network.RemoveRPCs(player);
	}

	[RPC]
	void AddPlayerName(string name){
		if(Network.isServer){
			names.Add(name);
		}
		playerList.text += name + "\n";
	}

	[RPC]
	void RemovePlayerName(string name){
		playerList.text = playerList.text.Replace(name, "");
	}

	[RPC]
	void AddChatMessage(string message){
		chat.text = message + chat.text;
	}

	[RPC]
	void StartGameRPC(int seed){
		MultiplayerManager.seed = seed;
		if(Network.isServer){
			MultiplayerManager.names = names.ToArray();
		}else{
			MultiplayerManager.userName = getUserData()[0];
		}
		Application.LoadLevel(1);
	}

	[RPC]
	void AddReadyPlayer(int n, string name){
		if(Network.isServer){
			readyPlayers += n;
		}
		if(n == 1){
			playerList.text = playerList.text.Replace(name, "<color=green>" + name + "</color>");
		}else{
			playerList.text = playerList.text.Replace("<color=green>" + name + "</color>", name);
		}
	}

	public void StartGame(){
		string[] data = getUserData();
		if(Network.isServer){
			if(Network.connections.Length <= readyPlayers){
				int output = 0;
				if(int.TryParse(seed.GetComponent<InputField>().text, out output) == true){
					GetComponent<NetworkView>().RPC("StartGameRPC", RPCMode.All, output);
				}else{
					AddChatMessage("<color=yellow>Please enter a seed.</color>\n");
				}
			}else{
				AddChatMessage("<color=yellow>Not all players are ready.</color>\n");
			}
		}else{
			if(ready == false){
				GetComponent<NetworkView>().RPC("AddChatMessage", RPCMode.All, "<color=yellow>" + data[0] + " is ready.</color>\n");
				GetComponent<NetworkView>().RPC("AddReadyPlayer", RPCMode.AllBuffered, 1, data[0]);
				ready = true;
			}else{
				GetComponent<NetworkView>().RPC("AddChatMessage", RPCMode.All, "<color=yellow>" + data[0] + " is not ready.</color>\n");
				GetComponent<NetworkView>().RPC("AddReadyPlayer", RPCMode.AllBuffered, -1, data[0]);
				ready = false;
			}
		}
	}

	public void SendChatMessage(InputField inField){
		string[] data = getUserData();
		if(!inField.text.Equals("")){
			string finMessage = data[0] + ": " + inField.text + "\n";
		inField.text = "";
		GetComponent<NetworkView>().RPC("AddChatMessage", RPCMode.All, finMessage);
		}
	}

	public void Disconnect(){
		string[] data = getUserData();
		GetComponent<NetworkView>().RPC("RemovePlayerName", RPCMode.All, data[0] + "\n");
		if(Network.isServer){
			Network.Disconnect();
		}else{
			GetComponent<NetworkView>().RPC("AddChatMessage", RPCMode.All, "<color=yellow>" + data[0] + " has disconnected.</color>\n");
			Network.CloseConnection(Network.connections[0], true);
		}
		chat.text = "";
		playerList.text = "";
		seed.SetActive(true);
	}

	public void ConnectToServer(){
		string[] data = getUserData();
		Network.Connect(data[3], int.Parse(data[1]), "");
	}
	
	public void StartServer(){
		string[] data = getUserData();
		Network.incomingPassword = "";
		bool useNat = !Network.HavePublicAddress();
		Network.InitializeServer(int.Parse(data[2]) , int.Parse(data[1]), useNat);
	}

	public string[] getUserData(){
		InputField[] inputFieldArr = GetComponentsInChildren<InputField>();
		string[] userDataArr = new string[4];
		userDataArr[0] = inputFieldArr[0].text;
		userDataArr[1] = inputFieldArr[1].text;
		userDataArr[2] = inputFieldArr[2].text;
		userDataArr[3] = inputFieldArr[3].text;
		return userDataArr;
	}
}
