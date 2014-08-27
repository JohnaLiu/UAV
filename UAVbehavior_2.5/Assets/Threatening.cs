using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Threatening : MonoBehaviour {

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

	//threatening's motor schema. When the target crowd acrosses the threaten line, the selected UAV will become aggrasive and if the crowd
	//keeps going forward and reaches the block line, the UAV will trigger the blocking
	public Dictionary<GameObject,bool> threatenMS(GameObject target, GameObject UAV, Dictionary<GameObject,bool> block){
		GameObject blockLine = GameObject.Find ("Block Line");
		GameObject threatenLine = GameObject.Find ("Threaten Line");
		GameObject approachLine = GameObject.Find ("Approach Line");
		GameObject safeLine = GameObject.Find ("Safe Line");
		GameObject backWallR = GameObject.Find ("Wall_Back1");
		GameObject backWallL = GameObject.Find ("Wall_Back2");
		float UAVrange = 0.5f;
		
		float leftLimit = UAVrange + backWallL.transform.position.x + backWallL.transform.localScale.z/2.0f;
		float rightLimit = backWallR.transform.position.x - backWallL.transform.localScale.z/2.0f - UAVrange;
		
		float Tx = target.transform.position.x;    //target's position
		float Ty = target.transform.position.y;
		float Tz = target.transform.position.z;
		
		
		float Ux = UAV.transform.position.x;       //UAV's position
		float Uy = UAV.transform.position.y;
		float Uz = UAV.transform.position.z;

		float diffx = Tx - Ux;                   //differences between x,y,z of Target and UAV
		float diffy = Ty - Uy;
		float diffz = Tz - Uz;
		float blockL = Uz - blockLine.transform.position.z;	       //block limit
		float threatenL = Uz - threatenLine.transform.position.z;
		float approachL = Uz - approachLine.transform.position.z;
		float safeL = Uz - safeLine.transform.position.z;
		
		float followDistance = 0.3f;
		float movement = 0.0f;
		float moveSpeed = 2.0f;
		float eyelevel = 1.5f;
		float watchStay = 2.0f;
		
		float dBetweenUandT = Mathf.Abs (diffz);
		
		
		if (dBetweenUandT > blockL && dBetweenUandT <= threatenL) {     //UAV is aggresive at beginning
			Debug.Log ("Threaten");
			Time.timeScale = 3.0f;
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
//			StayInDoorRange stay = new StayInDoorRange();
			if (stay.stayInDoorRange(Ux,Uy,Uz)){
				float mX = Time.deltaTime * UnityEngine.Random.Range (-2.0F, 2.0F);
				float mY = Time.deltaTime * UnityEngine.Random.Range (-2.0F, 2.0F);	
				UAV.transform.Translate (mX, mY, 0, target.transform);
			}
		} else if (dBetweenUandT <= blockL){   //block the crowd when it passes the block limit
			Debug.Log("BLOOOOOOOOCK!");
			block[target] = true;

		}
		return block;
	}

}
