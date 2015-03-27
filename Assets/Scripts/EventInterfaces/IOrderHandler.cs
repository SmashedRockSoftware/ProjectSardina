using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IOrderHandler : IEventSystemHandler{

	void OnOrder (OrderEventData data);

}
