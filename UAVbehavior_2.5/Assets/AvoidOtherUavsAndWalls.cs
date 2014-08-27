using UnityEngine;
using System.Collections;
using System;

public class AvoidOtherUavsAndWalls : MonoBehaviour {
	
	public static bool isNearObstacle;
	public float speed = 1.5f;
	public float threshold = 0.5f;
	public float uavThreshold = 1.0f;
	public float moveAway = 0.5f;
	public float moveAwayUav = 1;
	
	private enum Wall
	{
		none,
		front,
		back,
		left,
		right
	}
	
	// Use this for initialization
	void Start () {
		isNearObstacle = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Avoid behaviour is always on. Hence we do not need to test the releaser for this behaviour
		Vector3 percept = detectObstacle (name, tag);
		//Debug.Log ("near wall -  " + isNearObstacle+percept);
		moveAwayFromObstacle (percept,name);
		
	}
	
	void moveAwayFromObstacle (Vector3 translationVector, String name)
	{
		// if player i.e., the uav is near obstacle move away from the obstacle
		// Assumption here is that air robot moves only in x and y direction 
		// and thus can collide only left or right wall															
		if(isNearObstacle){
			
			//get the player object
			GameObject player =	GameObject.Find(name);		
			
			// move away from the obstacle
			Vector3 newPosition = new Vector3 (translationVector.x * speed * Time.deltaTime, translationVector.y * speed * Time.deltaTime,  translationVector.z * speed * Time.deltaTime);
			player.transform.Translate (newPosition);
			isNearObstacle = false;
		}	
	}
	
	
	Vector3 detectObstacle(String name, String tag)
	{	
		Vector3 playerLocation = readLocation (name);	
		Vector3 translationVector = new Vector3(0,0,0);
		
		// return the percept specifying if the uav is near obstacle and the direction to move to
//		Wall nearWall = checkIfNearWall (playerLocation, name);
//		if (nearWall!=Wall.none) {
//			isNearObstacle = true;				 
//			
//			// get the direction in which uav should move to avoid the obstacle
//			if(nearWall == Wall.left){
//				translationVector.x = moveAway;
//			}else if(nearWall == Wall.right){
//				translationVector.x = -1 * moveAway;
//			}else if(nearWall == Wall.front){
//				translationVector.z = moveAway;
//			}else if(nearWall == Wall.back){
//				translationVector.z = -1 * moveAway;
//			}
//		}
//		
		// checl if the UAV is near any other uav. 
		//If so get the translation to vector to moveaway from the UAV 
		//and add it to the translation vector to moveaway from the walls.
		
		String nearUAV = checkIfNearOtherUAV (name,tag);
		
		if (nearUAV != null) {
			
			translationVector = translationVector + getTranslationVector(name,nearUAV);
		}
		
		// return the translation vector to moveaway from obstacles
		return translationVector;
	}
	
	Vector3 getTranslationVector (String name,String nearUAV)
	{
		Vector3 translationVector = new Vector3 (0, 0, 0);
		
		GameObject target = GameObject.Find (name);
		GameObject otherUAV = GameObject.Find (nearUAV);
		
		translationVector.x = moveAwayUav * (target.transform.position.x - otherUAV.transform.position.x);
		return translationVector;
	}
	
	// check if the uav is near any other uav
	String checkIfNearOtherUAV (String name, String tag)
	{
		GameObject target = GameObject.Find (name);
		GameObject[] uavs = GameObject.FindGameObjectsWithTag (target.tag);
		
		float distance = 0;
		float targetRadius = 0;
		float uavRadius = 0;
		
		String nearUav = null;
		
		foreach(GameObject uav in uavs)
		{
			if(uav.name != name)
			{
				distance = Vector3.Distance(uav.transform.position,target.transform.position);
				targetRadius = getRadius(target.transform.position,name);
				uavRadius = getRadius(uav.transform.position,uav.name);
				
				distance = distance - (targetRadius+uavRadius);
				if(distance < uavThreshold)
				{
					isNearObstacle = true;
					nearUav = uav.name;
					break;
				}
			}
		}
		return nearUav;
	}
	
	// check if the uav is near wall
	private Wall checkIfNearWall(Vector3 centroid, String name)
	{
		// Assumption here is that air robot moves only in x and y direction 
		// and thus can collide only left or right wall		
		GameObject floor = GameObject.Find ("Floor");
		GameObject wall=null;
		float distance = 0;					
		Wall nearWall = Wall.none;
		
		if (centroid.x < floor.transform.position.x) {
			wall =	GameObject.Find("Wall_Left");		
			nearWall = Wall.left;
		} else if (centroid.x > floor.transform.position.x) {
			wall = GameObject.Find("Wall_Right");	
			nearWall = Wall.right;
		}
		
		if (wall != null) 
		{
			distance = (float) Math.Pow ((wall.transform.position.x - centroid.x), 2);		
			distance = (float) Math.Pow (distance, 0.5);
			distance = distance - getRadius (centroid, name);
			//Debug.Log("Wall_Right " + distance);
			if (distance > threshold) {
				nearWall = Wall.none;
			}
		}
		
		distance = 0;
		if(nearWall == Wall.none)
		{
			if (centroid.z <  floor.transform.position.z) {
				wall =	GameObject.Find("Wall_Front");		
				nearWall = Wall.front;
			} else if (centroid.z >  floor.transform.position.z) {
				wall = GameObject.Find("Wall_Back");	
				nearWall = Wall.back;
			}
			if (wall != null) 
			{
				distance = (float) Math.Pow ((wall.transform.position.z - centroid.z), 2);		
				distance = (float) Math.Pow (distance, 0.5);
				distance = distance - getRadius (centroid, name);
				//Debug.Log("Wall_front"+distance);
				if (distance > threshold) {
					nearWall = Wall.none;
				}
			}
		}
		return nearWall;
	}
	
	// get the radius of uav periphery
	private float getRadius(Vector3 centroid, String name)
	{
		GameObject rotor1 = GameObject.Find (name+"_Rotor 1");
		
		double xSqr = 0.0;
		double ySqr = 0.0;
		double zSqr = 0.0;
		
		xSqr = Math.Pow((rotor1.transform.position.x - centroid.x),2);
		ySqr = Math.Pow((rotor1.transform.position.y - centroid.y),2);
		zSqr = Math.Pow((rotor1.transform.position.z - centroid.z),2);
		
		return (float) Math.Pow ((xSqr+ySqr+zSqr),0.5);
	}				
	
	public Vector3 readLocation(String name)
	{	
		GameObject player = GameObject.Find (name);
		
		// get the player location
		return player.transform.position;		
	}	
}
