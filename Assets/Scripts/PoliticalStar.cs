using UnityEngine;
using System.Collections;

public class PoliticalStar : MonoBehaviour, IFleetMoveHandler {

	public int owner = -1;

	private SpriteRenderer captured;

	// Use this for initialization
	void Start () {
		captured = transform.Find("CapturedSystem").GetComponent<SpriteRenderer>() as SpriteRenderer;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){
		if(stream.isWriting){
			int own = owner;
			stream.Serialize(ref own);
			float r = 0, g = 0, b = 0;
			r = captured.color.r;
			g = captured.color.g;
			b = captured.color.b;
			stream.Serialize(ref r);
			stream.Serialize(ref g);
			stream.Serialize(ref b);
			bool sr = captured.enabled;
			stream.Serialize(ref sr);
		}else{
			int own = -1;
			stream.Serialize(ref own);
			owner = own;
			float r = 0, g = 0, b = 0;
			stream.Serialize(ref r);
			stream.Serialize(ref g);
			stream.Serialize(ref b);
			captured.color = new Color(r, g, b);
			bool sr = false;
			stream.Serialize(ref sr);
			captured.enabled = sr;
		}
	}

	public void OnFleetMove(FleetMoveEventData data){
		if(owner == -1){
			if(Network.isServer){
				Color c = MultiplayerManager.playerColor;
				ConquerSystem(data.fleet.owner, c.r, c.g, c.b);
			}else{
				Color c = MultiplayerManager.playerColor;
				GetComponent<NetworkView>().RPC("ConquerSystem", RPCMode.Server, data.fleet.owner, c.r, c.g, c.b);
			}
		}else if(owner != int.Parse(data.fleet.owner.ToString())){
			Debug.Log("SIEGE!");
		}
	}

	[RPC]
	public void ConquerSystem(NetworkPlayer player, float r, float g, float b){
		owner = int.Parse(player.ToString());
		captured.enabled = true;
		captured.color = new Color(r, g, b);
	}
}
