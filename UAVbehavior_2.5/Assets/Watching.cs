using UnityEngine;
using System.Collections;

public class Watching : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject threatenLine = GameObject.Find ("Threaten Line");
		if (threatenLine == null) {
			Debug.Log ("Threaten Line not found.");	
		}
		GameObject approachLine = GameObject.Find ("Approach Line");
		if (approachLine == null) {
			Debug.Log ("Approach Line not found.");	
		}
		GameObject watchLine = GameObject.Find ("Watch Line");
		if (watchLine == null) {
			Debug.Log ("Watch Line not found.");	
		}
		GameObject safeLine = GameObject.Find ("Safe Line");
		if (safeLine == null) {
			Debug.Log ("Safe Line not found.");	
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	//watching's motor schema, let the UAV stay at the eye level to watch the target crowd
	public void watchMS(GameObject target, GameObject UAV){
		GameObject approachLine = GameObject.Find ("Approach Line");
		GameObject watchLine = GameObject.Find ("Watch Line");
//		GameObject safeLine = GameObject.Find ("Safe Line");

		float Ix = 12.0f;
		float Iy = 3.0f;
		float Iz = 4155.0f;
		
		float Tx = target.transform.position.x; //target's position
		float Ty = target.transform.position.y;
		float Tz = target.transform.position.z;
		
		
		float Ux = UAV.transform.position.x;       //UAV's position
		float Uy = UAV.transform.position.y;
		float Uz = UAV.transform.position.z;
		
		float diffx = Tx - Ux;
		float diffy = Ty - Uy;
		float diffz = Tz - Uz;

		float approachL = Uz - approachLine.transform.position.z;
		float watchL = Uz - watchLine.transform.position.z;
//		float safeL = Uz - safeLine.transform.position.z;
		
		float followDistance = 0.3f;
		float movement = 0.0f;
		float moveSpeed = 2.0f;
		float eyelevel = 1.5f;
		float watchStay = 2.0f;
		
		float dBetweenUandT = Mathf.Abs (diffz);  //distance between UAV and TArget


		if (dBetweenUandT > approachL && dBetweenUandT <= watchL) {		//Watching at eye level
			Debug.Log ("Watch");
			if (Uy > watchStay) {
				movement = Time.deltaTime * -moveSpeed;
			} else if (Uy < watchStay) {
				movement = Time.deltaTime * moveSpeed;
			} else {
				movement = 0.0f;
			}
			UAV.transform.Translate (0, movement, 0, target.transform);
		}  
	}


}
