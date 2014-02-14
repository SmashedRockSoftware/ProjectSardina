using UnityEngine;
using System.Collections;

public class BuildMenu : MonoBehaviour {

	public Camera buildcam;
	public int speed = 10;
	







	void MoveBuildCam(){
		if(transform.position.z >= 46) transform.Translate(Vector3.forward * speed * -1 * Time.deltaTime);
		if(transform.position.z <= -46) transform.Translate(Vector3.forward * speed * 1 * Time.deltaTime);
		if(transform.position.x >= 43) transform.Translate(Vector3.right * speed * -1 * Time.deltaTime);
		if(transform.position.x <= -43) transform.Translate(Vector3.right * speed * 1 * Time.deltaTime);
		
		if(transform.position.z <= 46 && transform.position.z >= -46){
			if(transform.position.x <= 43 && transform.position.x >= -43){
				
				float h = Input.GetAxis("Horizontal");
				transform.Translate(Vector3.right * speed * h * Time.deltaTime);
				
				float v = Input.GetAxis("Vertical");
				transform.Translate(Vector3.forward * speed * v * Time.deltaTime);
			}
			
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(true){
			MoveBuildCam();
		}
	}
}





	
	


