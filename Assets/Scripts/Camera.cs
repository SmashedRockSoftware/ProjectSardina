using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
//This is the camera script made by Tiny
//This was made on 2/13/14
//The basic idead is that it has inputs that affect where the camera translates to

	public Camera buildcam; //lets us choose the camera and reference it
	public int speed = 10; //This lets us change the speed in the inspector

	void MoveBuildCam(){
		if(transform.position.z >= 46) transform.Translate(Vector3.up * speed * -1 * Time.deltaTime); //This forced it to stay within bounds of 46 horizontaly
		if(transform.position.z <= -46) transform.Translate(Vector3.up * speed * 1 * Time.deltaTime); //This forced it to stay within bounds of -46 horizontaly
		if(transform.position.x >= 43) transform.Translate(Vector3.right * speed * -1 * Time.deltaTime); //This forced it to stay within bounds of 43 verticly
		if(transform.position.x <= -43) transform.Translate(Vector3.right * speed * 1 * Time.deltaTime); //This forced it to stay within bounds of -43 verticly
		
		if(transform.position.z <= 46 && transform.position.z >= -46){ //If that this is in the right area let us control it
			if(transform.position.x <= 43 && transform.position.x >= -43){ //If that this is in the right area let us control it
				
				float h = Input.GetAxis("Horizontal");//This is the number you edit to control the h axis
				transform.Translate(Vector3.right * speed * h * Time.deltaTime);//This is then adjusted by that value
				
				float v = Input.GetAxis("Vertical");//This is the number you edit to control the v axis
				transform.Translate(Vector3.up * speed * v * Time.deltaTime);//This is then adjusted by that value
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





	
	


