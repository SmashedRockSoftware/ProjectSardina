using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public interface IFleetMoveHandler : IEventSystemHandler {

	void OnFleetMove(FleetMoveEventData data);

}
