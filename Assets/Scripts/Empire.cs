using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Empire : MonoBehaviour {

	public List<GameObject> fleets = new List<GameObject>();
	public List<GameObject> stars = new List<GameObject>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddStar(GameObject star){
		stars.Add(star);
		FleetMoveEventData data = new FleetMoveEventData(fleets[0].GetComponent<Fleet>(), EventSystem.current);
		ExecuteEvents.Execute<IFleetMoveHandler>(star, data, (x,y)=>x.OnFleetMove(data));
	}

	public void OrderFleets(GameObject g){
		OrderEventData data = new OrderEventData(g, EventSystem.current);
		for(int i = 0; i < fleets.Count; i++){
			ExecuteEvents.Execute<IOrderHandler>(fleets[i].gameObject, data, (x,y)=>x.OnOrder(data));
		}
	}

	public void SelectFleetsInArea(Bounds b){
		for(int i = 0; i < fleets.Count; i++){
			if(b.Contains(fleets[i].transform.position)){
				ExecuteEvents.Execute<ISelectableHandler>(fleets[i].gameObject, null, (x,y)=>x.Select());
			}
		}
	}

	public void DeselectFleets(){
		for(int i = 0; i < fleets.Count; i++){
			ExecuteEvents.Execute<IDeselectableHandler>(fleets[i].gameObject, null, (x,y)=>x.Deselect());
		}
	}
}
