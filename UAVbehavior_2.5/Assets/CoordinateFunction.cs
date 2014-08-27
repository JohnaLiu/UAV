using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoordinateFunction : MonoBehaviour {
	List<GameObject> UAVs = new List<GameObject>();
	List<GameObject> Targets = new List<GameObject>();
	public Dictionary<GameObject,bool> block = new Dictionary<GameObject,bool>();

	// Use this for initialization
	void Start () {

		GameObject red = GameObject.Find ("Red");
		GameObject blue = GameObject.Find ("Blue");
		GameObject yellow = GameObject.Find ("Yellow");
		GameObject UAVr = GameObject.Find ("ar_drone");
		GameObject UAVb = GameObject.Find ("airrobot1");
		GameObject UAVy = GameObject.Find ("airrobot2");		

		UAVs.Add (UAVb);
		UAVs.Add (UAVr);
		UAVs.Add (UAVy);
//		Debug.Log (UAVs.Count);

		Targets.Add (red);
		Targets.Add (blue);
		Targets.Add (yellow);
//		Debug.Log (Targets.Count);
		block [red] = false;
		block [blue] = false;
		block [yellow] = false;
	}
	
	// Update is called once per frame
	void Update () {
		PerceptualSchema PS = new PerceptualSchema();
		GameObject target = PS.cameraFindTarget(UAVs,Targets);
		GameObject UAV = PS.selectUAV(UAVs,Targets);

		if (block [target] == true) { //after the block trigger is on, the crowd will be blocked untill it reaches the safe line.
			if (Mathf.Abs (target.transform.position.z - UAV.transform.position.z) < (UAV.transform.position.z - GameObject.Find ("Safe Line").transform.position.z)) {
								float mZ = Time.deltaTime * -3.0f;
								target.transform.Translate (0, 0, mZ, target.transform);				                
			}
			if(target.transform.position.z <= GameObject.Find ("Safe Line").transform.position.z){
				block [target] = false;
			}

		} else {
			Time.timeScale = 1.0f;

						for (int i=0; i<UAVs.Count; i++) {
								if (UAVs [i].name != UAV.name) {
										release (UAVs [i]);
//										Debug.Log (UAVs [i].name);
								}		
						}
						if (Input.GetKey (KeyCode.R)) {
								coordinate (target, UAV);
								//			release(UAV,UAVb,UAVr,UAVy); haven't finished
						} else if (Input.GetKey (KeyCode.B)) {
								coordinate (target, UAV);
						} else if (Input.GetKey (KeyCode.Y)) {
								coordinate (target, UAV);
						}
				}

	}

	//coordinate function, combining watching, approaching and threatening.
	private void coordinate(GameObject target, GameObject UAV){
		        GameObject blockLine = GameObject.Find ("Block Line");
				GameObject threatenLine = GameObject.Find ("Threaten Line");
				GameObject approachLine = GameObject.Find ("Approach Line");
				GameObject watchLine = GameObject.Find ("Watch Line");
				GameObject safeLine = GameObject.Find ("Safe Line");
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

		
		        float blockL = Uz - blockLine.transform.position.z;		
		        float threatenL = Uz - threatenLine.transform.position.z;
				float approachL = Uz - approachLine.transform.position.z;
				float watchL = Uz - watchLine.transform.position.z;
				float safeL = Uz - safeLine.transform.position.z; 

				float movement = 0.0f;
				float moveSpeed = 2.0f;
				float eyelevel = 1.5f;
				float watchStay = 2.0f;
		
				float dBetweenUandT = Mathf.Abs (diffz); 
		if (dBetweenUandT > watchL && dBetweenUandT <= safeL) {
//			Debug.Log ("Above Eye Level");
			if (Uy <= eyelevel) {
				movement = Time.deltaTime * moveSpeed;
			} else if (Uy > eyelevel && Uy <= watchStay) {
				movement = Time.deltaTime * moveSpeed;
			} else {
				movement = 0.0f;
			}
			UAV.transform.Translate (0, movement, 0, target.transform);			
		}else if (dBetweenUandT > approachL && dBetweenUandT <= watchL) {		
			Watching watchMS = new Watching ();
			watchMS.watchMS (target, UAV);
		}else if (dBetweenUandT > threatenL && dBetweenUandT <= approachL) {
			
			Approaching approachMS = new Approaching ();
			approachMS.approachMS (target, UAV);
		}else if (dBetweenUandT <= threatenL) {
			Threatening threatenMS = new Threatening ();
			block = threatenMS.threatenMS (target, UAV, block);
//			Debug.Log(block[target]);
		}else {
			StayInDoorRange stay = new StayInDoorRange();
			if (stay.stayInDoorRange(Ux,Uy,Uz)){
				float mX = Time.deltaTime * UnityEngine.Random.Range (-5.0F, 5.0F);
								float mY = Time.deltaTime * UnityEngine.Random.Range (-5.0F, 5.0F);
								float mZ = Time.deltaTime * UnityEngine.Random.Range (-5.0F, 5.0F);
								UAV.transform.Translate (mX, mY, 0, target.transform);
			} else {
				float mX = Time.deltaTime * (Ix - Ux) * 5.0f;
				float mY = Time.deltaTime * (Iy - Uy) * 5.0f;
				float mZ = Time.deltaTime * (Iz - Uz) * 5.0f;
				UAV.transform.Translate (mX, mY, 0, target.transform);
			}
		}
	}
	
	
	//release those UAVs, who are not selected to response to the target crowd
	public void release(GameObject notSelectedUAV){
		float movement = 0.0f;
		float moveSpeed = 2.0f;
		float stay = 2.5f;
		float Uy = notSelectedUAV.transform.position.y;
		if (Uy < stay) {
			movement = Time.deltaTime * moveSpeed;
		} else if (Uy > stay){
			movement = Time.deltaTime * -moveSpeed;
		}else{
			movement = 0.0f;
		}
		notSelectedUAV.transform.Translate (0, movement, 0);	
		
	}
}
