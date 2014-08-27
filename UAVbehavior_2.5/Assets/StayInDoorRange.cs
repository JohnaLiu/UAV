using UnityEngine;
using System.Collections;

public class StayInDoorRange : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
	
	}

	public bool stayInDoorRange(float x, float y, float z){
		GameObject sign = GameObject.Find ("ExitSign");
		GameObject floor = GameObject.Find ("Floor");
		GameObject backWallT = GameObject.Find ("Wall_Back");
		GameObject backWallR = GameObject.Find ("Wall_Back1");
		GameObject backWallL = GameObject.Find ("Wall_Back2");
		float UAVrange = 0.5f;

		float leftLimit = UAVrange + backWallL.transform.position.x + backWallL.transform.localScale.z/2.0f;
		float rightLimit = backWallR.transform.position.x - backWallL.transform.localScale.z/2.0f - UAVrange;
		float topLimit = backWallT.transform.position.y - floor.transform.position.y - UAVrange;
		float bottomLimit = floor.transform.position.y + UAVrange;
		float insideDoorLimit = sign.transform.position.z - UAVrange;
		float outsideDoorLimit = sign.transform.position.z + UAVrange;

		if (x > leftLimit && x < rightLimit && y > bottomLimit && y < topLimit && z > insideDoorLimit && z < outsideDoorLimit) {
			return true;
		} else {
			return false;
		}
	}
}
