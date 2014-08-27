using UnityEngine;
using System.Collections;

public class Approaching : MonoBehaviour {

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

	//approaching's motor schema. When the target crowd passes the approaching line, the UAV will follow it in X direction.
	public void approachMS(GameObject target, GameObject UAV){
		GameObject threatenLine = GameObject.Find ("Threaten Line");
		GameObject approachLine = GameObject.Find ("Approach Line");
		GameObject backWallR = GameObject.Find ("Wall_Back1");
		GameObject backWallL = GameObject.Find ("Wall_Back2");
		float UAVrange = 0.5f;
		
		float leftLimit = UAVrange + backWallL.transform.position.x + backWallL.transform.localScale.z/2.0f;
		float rightLimit = backWallR.transform.position.x - backWallL.transform.localScale.z/2.0f - UAVrange;

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

		float threatenL = Uz - threatenLine.transform.position.z;
		float approachL = Uz - approachLine.transform.position.z;
		
		float followDistance = 0.2f;
		float movement = 0.0f;
		float moveSpeed = 2.0f;
		float eyelevel = 1.5f;
		float watchStay = 2.0f;
		
		float dBetweenUandT = Mathf.Abs (diffz);
		
		if (dBetweenUandT > threatenL && dBetweenUandT <= approachL) { 
			Debug.Log ("Approach");
			StayInDoorRange stay = new StayInDoorRange();
			if (stay.stayInDoorRange(Ux,Uy,Uz)){
				if (diffx < -followDistance) {
					movement = Time.deltaTime * -moveSpeed;
				} else if (diffx > followDistance) {
					movement = Time.deltaTime * moveSpeed;
				} else {
					movement = 0.0f;
				}
				UAV.transform.Translate (movement, 0, 0, target.transform);
			} else {
			if (Ux >= rightLimit) {
				movement = Time.deltaTime * -0.5f;
				UAV.transform.Translate (movement, 0, 0, target.transform);
			} else if (Ux <= leftLimit) {
				movement = Time.deltaTime * 0.5f;
				UAV.transform.Translate (movement, 0, 0, target.transform);
			} 
			}
		if (Uy > eyelevel) {
				movement = Time.deltaTime * -moveSpeed;
			} else if (Uy < eyelevel) {
				movement = Time.deltaTime * moveSpeed;
			} else {
				movement = 0.0f;
			}
			UAV.transform.Translate (0, movement, 0, target.transform);


		}

	}
	
}
