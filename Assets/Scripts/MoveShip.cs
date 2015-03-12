using UnityEngine;
using System.Collections;

public class MoveShip : MonoBehaviour {

	bool move;
	Vector3 target;
	float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(move == true && V3Equal(transform.position, target) != true){
		transform.position = Vector3.MoveTowards(transform.position, target, speed);
		}else{
		transform.position = target;
			move = false;
		}
	}

	public void ShipMover (Vector3 targetI, float speedI) {
		move = true;
		speed = speedI;
		target = targetI;
	}

	public bool V3Equal(Vector3 a, Vector3 b){
		return Vector3.SqrMagnitude(a - b) < 0.0001;
	}
}
