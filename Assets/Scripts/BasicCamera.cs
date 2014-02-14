using UnityEngine;
using System.Collections;

public class BasicCamera : MonoBehaviour {
//This is the camera script made by Tiny
//This was made on 2/13/14
//The basic idead is that it has inputs that affect where the camera translates to
	
	public int speed = 10; //This lets us change the speed in the inspector
	public int ZLimit;//This locks it to the int + or - so it limits the camera
	public int NegZLimit;
	public int XLimit;//This locks it to the int + or - so it limits the camera
	public int NegXLimit;

	void MoveBuildCam(){
		if(transform.position.z >= ZLimit) transform.Translate(Vector3.up * speed * -1 * Time.deltaTime); //This forced it to stay within bounds of zlimit horizontaly
		if(transform.position.z <= NegZLimit) transform.Translate(Vector3.up * speed * 1 * Time.deltaTime); //This forced it to stay within bounds of -zlimit horizontaly
		if(transform.position.x >= XLimit) transform.Translate(Vector3.right * speed * -1 * Time.deltaTime); //This forced it to stay within bounds of xlimit verticly
		if(transform.position.x <= NegXLimit) transform.Translate(Vector3.right * speed * 1 * Time.deltaTime); //This forced it to stay within bounds of -xlimit verticly
		Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * speed * -1; // This sets the size of the orthographic camera, essentially a zoom
		if(Camera.main.orthographicSize < 1){ // Limits it so that it can't get too small
			Camera.main.orthographicSize = 1;
		}
		if(Camera.main.orthographicSize > 100){ // Same thing as above, but for getting too large
			Camera.main.orthographicSize = 100;
		}

		if(transform.position.z <= ZLimit && transform.position.z >= NegZLimit){ //If that this is in the right area let us control it
			if(transform.position.x <= XLimit && transform.position.x >= NegXLimit){ //If that this is in the right area let us control it
				
				float h = Input.GetAxis("Horizontal");//This is the number you edit to control the h axis
				transform.Translate(Vector3.right * speed * h * Time.deltaTime * Camera.main.orthographicSize/10);//This is then adjusted by that value
				
				float v = Input.GetAxis("Vertical");//This is the number you edit to control the v axis
				transform.Translate(Vector3.up * speed * v * Time.deltaTime * Camera.main.orthographicSize/10);//This is then adjusted by that value
			}
			
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(true){ //Later we can determine a reason to disable this control
			MoveBuildCam(); //This calls the function
		}
	}
}





	
	


