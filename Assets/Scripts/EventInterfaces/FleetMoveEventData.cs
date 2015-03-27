using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class FleetMoveEventData : BaseEventData {

	public Fleet fleet;

	public FleetMoveEventData(Fleet fleet, EventSystem es) : base(es){
		this.fleet = fleet;
	}

}
