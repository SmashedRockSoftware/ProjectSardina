using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class FleetOrders : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public RectTransform dragPanel;

	public Vector3 startPos;
	public Vector3 currentPos;

	public void OnPointerClick(PointerEventData data){
		if(data.button == PointerEventData.InputButton.Left){
			GameObject.FindGameObjectWithTag("GameController").GetComponent<Empire>().DeselectFleets();
		} 
	}

	public void OnBeginDrag(PointerEventData data){
		startPos = data.worldPosition;
		dragPanel.position = startPos;
	}

	public void OnDrag(PointerEventData data){
		currentPos = data.worldPosition;
		dragPanel.localScale = new Vector3((currentPos.x - startPos.x), (currentPos.z - startPos.z));
	}

	public void OnEndDrag(PointerEventData data){
		Bounds b = new Bounds(new Vector3((currentPos.x + startPos.x)/2, 0, (currentPos.z + startPos.z)/2), new Vector3(Mathf.Abs(currentPos.x - startPos.x), 10, Mathf.Abs(currentPos.z - startPos.z)));
		GameObject.FindGameObjectWithTag("GameController").GetComponent<Empire>().SelectFleetsInArea(b);
		dragPanel.localScale = new Vector3(0, 0);
	}
}
