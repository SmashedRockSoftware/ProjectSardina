using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class OrderEventData : BaseEventData{

	public GameObject target;

	public OrderEventData(GameObject t, EventSystem es) : base(es){
		target = t;
	}
}
