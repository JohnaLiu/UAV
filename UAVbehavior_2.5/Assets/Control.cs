
using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		GameObject.Find("Red");
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKey (KeyCode.R)) {
			GameObject target = GameObject.Find ("Red");
			move (target);
		} else if (Input.GetKey (KeyCode.B)) {
			GameObject target = GameObject.Find ("Blue");
			move (target);
		}else if (Input.GetKey (KeyCode.Y)) {
			GameObject target = GameObject.Find("Yellow");
			move (target);
		}


	}
	void move(GameObject target){
		if (Input.GetKey (KeyCode.S)){
			target.transform.Translate(0,0,-2*Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.W)){
			target.transform.Translate(0,0,2*Time.deltaTime);
		}
		
		if (Input.GetKey (KeyCode.A)){
			target.transform.Translate(-2*Time.deltaTime,0,0);
		}
		
		if (Input.GetKey (KeyCode.D)){
			target.transform.Translate(2*Time.deltaTime,0,0);
		}
		}
}
