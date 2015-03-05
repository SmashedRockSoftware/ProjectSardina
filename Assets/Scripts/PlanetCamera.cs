using UnityEngine;
using System.Collections;

public class PlanetCamera : MonoBehaviour {
	//This is the camera script made by Tiny
	//This was made on 2/13/14
	//The basic idead is that it has inputs that affect where the camera translates to
	
	public int speed = 10; //This lets us change the speed in the inspector
	public int XLimit;//This locks it to the int + or - so it limits the camera
	public int NegXLimit;
	
	void MoveBuildCam(){
		Camera cam = GetComponent<Camera>() as Camera;
		if(transform.position.x >= XLimit) transform.position = new Vector3(XLimit, transform.position.y, transform.position.z); //This forced it to stay within bounds of xlimit verticly
		if(transform.position.x <= NegXLimit) transform.position = new Vector3(NegXLimit, transform.position.y, transform.position.z); //This forced it to stay within bounds of -xlimit verticly
		//cam.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * speed * -1; // This sets the size of the orthographic camera, essentially a zoom
		if(cam.orthographicSize < 1){ // Limits it so that it can't get too small
			cam.orthographicSize = 1;
		}
		if(cam.orthographicSize > 50){ // Same thing as above, but for getting too large
			cam.orthographicSize = 50;
		}

		if(transform.position.x <= XLimit && transform.position.x >= NegXLimit){ //If that this is in the right area let us control it
				
			float h = Input.GetAxis("Horizontal");//This is the number you edit to control the h axis
			transform.Translate(Vector3.right * speed * h * Time.deltaTime);//This is then adjusted by that value

		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Camera.main == null){ //Later we can determine a reason to disable this control
			MoveBuildCam(); //This calls the function
		}
	}
}




