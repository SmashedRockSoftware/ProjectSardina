using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Multiplayer : MonoBehaviour {

	public Text playerList;
	public Text chat;
	public MainMenu menu;

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
	}

	void OnServerInitialized(){
		string[] data = getUserData();
		menu.ChangeWindow(2);
		GetComponent<NetworkView>().RPC("AddPlayerName", RPCMode.AllBuffered, data[0]);
	}

	[RPC]
	void AddPlayerName(string name){
		playerList.text += name + "\n";
	}

	[RPC]
	void RemovePlayerName(string name){
		playerList.text.Remove(playerList.text.IndexOf(name), name.Length);
	}

	[RPC]
	void AddChatMessage(string message){
		chat.text = message + chat.text;
	}

	public void SendChatMessage(InputField inField){
		string[] data = getUserData();
		string finMessage = data[0] + ": " + inField.text + "\n";
		inField.text = "";
		GetComponent<NetworkView>().RPC("AddChatMessage", RPCMode.All, finMessage);
	}

	public void Disconnect(){
		RemovePlayerName(getUserData()[0] + "\n");
		if(Network.isServer){
			Network.Disconnect();
		}else{
			Network.CloseConnection(Network.connections[0], true);
		}
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
