using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Fleet : MonoBehaviour, IOrderHandler, IPointerClickHandler, IDeselectableHandler, ISelectableHandler{

	public GameObject target = null;
	public List<GameObject> curPath = null;

	public NetworkPlayer owner;
	public List<Ship> ships = new List<Ship>();
	public GameObject currentStar;
	public float range = 10f;
	public float speed = 1f;
	public int chargeTime = 5;

	private bool isSelected = false;
	private bool inFlight = false;
	private SpriteRenderer select;
	private float distance;
	private float totalTime;
	private float fracJourney;
	private int[] chargeFinish;
	private int[] nextMoveDay;
	private int posInMove = 1;
	private LineRenderer pathLine;

	// Use this for initialization
	void Start () {
		SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>() as SpriteRenderer[];
		select = sr[1];
		pathLine = GetComponentInChildren<LineRenderer>() as LineRenderer;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(target == currentStar){
			target = null;
		}
		if(target != null){
			pathLine.enabled = isSelected;
			if(inFlight && !Pause.GetPause()){
				totalTime = distance/speed;
				pathLine.SetPosition(curPath.Count, transform.position);
				if(Calender.IsDate(nextMoveDay)){
					gameObject.transform.position = Vector3.Lerp(currentStar.transform.position + Vector3.up, curPath[0].transform.position + Vector3.up, posInMove/totalTime);
					posInMove++;
					nextMoveDay = Calender.GetFutureDate(1);
				}
				if(curPath.Count == 1 && transform.position == curPath[0].transform.position + Vector3.up){
					currentStar = curPath[0];
					target = null;
					curPath = null;
					inFlight = false;
					posInMove = 1;
					pathLine.SetVertexCount(0);
					if(GetComponent<NetworkView>().isMine){
						FleetMoveEventData data = new FleetMoveEventData(this, EventSystem.current);
						ExecuteEvents.Execute<IFleetMoveHandler>(currentStar, data, (x,y)=>x.OnFleetMove(data));
					}
				}else if(transform.position == curPath[0].transform.position + Vector3.up){
					currentStar = curPath[0];
					curPath.RemoveAt(0);
					distance = Vector3.Distance(currentStar.transform.position, curPath[0].transform.position);
					inFlight = false;
					chargeFinish = Calender.GetFutureDate(chargeTime);
					posInMove = 1;
					pathLine.SetVertexCount(curPath.Count + 1);
					if(GetComponent<NetworkView>().isMine){
						FleetMoveEventData data = new FleetMoveEventData(this, EventSystem.current);
						ExecuteEvents.Execute<IFleetMoveHandler>(currentStar, data, (x,y)=>x.OnFleetMove(data));
					}
				}
			}else if(!inFlight){
				if(Calender.IsDate(chargeFinish)){
					inFlight = true;
					nextMoveDay = Calender.GetFutureDate(1);
					distance = Vector3.Distance(currentStar.transform.position, curPath[0].transform.position);
				}
			}
		}
	}

	public void OnPointerClick(PointerEventData data){
		if(data.button == PointerEventData.InputButton.Left && GetComponent<NetworkView>().isMine){
			isSelected = !isSelected;
			select.enabled = isSelected;
		}
	}

	public void Select(){
		if(GetComponent<NetworkView>().isMine){
			isSelected = true;
			select.enabled = true;
		}
	}

	public void Deselect(){
		if(GetComponent<NetworkView>().isMine){
			isSelected = false;
			select.enabled = false;
		}
	}

	public void OnOrder(OrderEventData data){
		if(isSelected && !inFlight  && GetComponent<NetworkView>().isMine){
			target = data.target;
			curPath = FindPath(target);
			chargeFinish = Calender.GetFutureDate(chargeTime);
			DrawLine();
		}
	}

	private void DrawLine (){
		pathLine.SetVertexCount(curPath.Count + 1);
		pathLine.SetPosition(curPath.Count, currentStar.transform.position - Vector3.up);
		int nextOpen = curPath.Count - 1;
		for(int i = 0; i < curPath.Count; i++){
			pathLine.SetPosition(nextOpen, curPath[i].transform.position - Vector3.up);
			nextOpen--;
		}
	}

	private List<GameObject> FindPath (GameObject goal){
		List<PathPosition> open = new List<PathPosition>();
		List<PathPosition> closed = new List<PathPosition>();
		PathPosition start = new PathPosition(currentStar, null, 0, Vector3.Distance(currentStar.transform.position, goal.transform.position), Vector3.Distance(currentStar.transform.position, goal.transform.position));
		open.Add(start);
		open = AddAllInRange(open, closed, range, 0, start, goal);
		PathPosition s = FindLowestF(open, closed);
		PathPosition cur = new PathPosition(s.go, s.parent, s.G, s.H, s.F);
		open.Remove(s);
		closed.Add(s);
		while(!cur.IsEqual(goal) && open.Count > 0){
			open = AddAllInRange(open, closed, range, cur.G, cur, goal);
			PathPosition p = FindLowestF(open, closed);
			open.Remove(p);
			closed.Add(p);
			cur = new PathPosition(p.go, p.parent, p.G, p.H, p.F);
		}
		List<GameObject> path = new List<GameObject>();
		path.Add(cur.go);
		while(!path[0].Equals(currentStar)){
			cur = cur.parent;
			path.Insert(0, cur.go);
		}
		path.RemoveAt(0);
		return path;
	}

	private List<PathPosition> AddAllInRange (List<PathPosition> list, List<PathPosition> closed, float range, float currentG, PathPosition current, GameObject goal){
		float dist = 0;
		List<PathPosition> fin = new List<PathPosition>();
		foreach(PathPosition p in list){
			fin.Add(p);
		}
		for(int i = 0; i < PerlinStars.stars.Length; i++){
			if(!Contains(list, PerlinStars.stars[i]) && !Contains(closed, PerlinStars.stars[i])){
				dist = Vector3.Distance(current.go.transform.position, PerlinStars.stars[i].transform.position);
				if(dist <= range){
					PathPosition p = new PathPosition(PerlinStars.stars[i], current, currentG + dist, Vector3.Distance(PerlinStars.stars[i].transform.position, goal.transform.position), 0);
					p.F = p.H + p.G;
					fin.Add(p);
				}
			}
		}
		return fin;
	}

	private PathPosition FindLowestF(List<PathPosition> list, List<PathPosition> closed){
		PathPosition p = list[0];
		for(int i = 1; i < list.Count; i++){
			if(!Contains(closed, list[i].go) && list[i].F < p.F){
				p = list[i];
			}
		}
		return p;
	}

	private bool Contains(List<PathPosition> list, GameObject p){
		for(int i = 0; i < list.Count; i++){
			if(list[i].IsEqual(p)){
				return true;
			}
		}
		return false;
	}
}
