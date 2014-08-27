using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PerceptualSchema {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {


	}

	//select UAV based on the shortest distance in X between UAV and different crowds
	public GameObject selectUAV(List<GameObject> UAVs, List<GameObject> Targets){

		GameObject target = cameraFindTarget (UAVs, Targets);

		float[] DistanceX = new float[UAVs.Count];
		
		for (int i = 0; i<UAVs.Count; i++) {
				DistanceX[i] = Mathf.Abs (UAVs[i].transform.position.x - target.transform.position.x);
//			Debug.Log("DistanceX[i]: "+DistanceX[i]);
		}

		float minDistance = Mathf.Min(DistanceX);
//		Debug.Log ("Min D:"+minDistance);

		minDistance = DistanceX[0];
		int switchCase = 1;
		if (DistanceX[1] < minDistance) {
			minDistance = DistanceX[1];
			switchCase = 2;
			if(DistanceX[2] < minDistance){
				minDistance = DistanceX[2];
				switchCase = 3;
			}
		}

		switch (switchCase) {
		case 1: 
//			Debug.Log ("UAV: Blue");
			return UAVs[0];
			break;
		case 2: 
//			Debug.Log ("UAV: Red");
			return UAVs[1];
			break;
		case 3:
//			Debug.Log ("UAV: Yellow");
			return UAVs[2];
			break;
		default: 
//			Debug.Log ("No UAV selected! Defult return RED UAV");
			return UAVs[1];
			break;
		}


	}

	//help UAV choose the target based on the distance in Z between UAV and different crowds
	public GameObject cameraFindTarget(List<GameObject> UAVs, List<GameObject> Targets){
		GameObject target = Targets[0];
		GameObject threatenLine = GameObject.Find ("Threaten Line");
		for (int j = 0; j<Targets.Count; j++) {
			if	(Mathf.Abs (threatenLine.transform.position.z - Targets[j].transform.position.z) < Mathf.Abs (threatenLine.transform.position.z - target.transform.position.z)){
				target = Targets[j];
			}				
		}
		return target;
		
	}
}
