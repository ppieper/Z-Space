using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	private GameObject playerObject;
	private float distance;

	// Speed at which the mob turns
	public float turnSpeed = 1.5F;
	// Determine at what angle they should start moving
	public float moveAngleThreshold = 5f;
	// Movement speed
	public float moveSpeed = 20f;
	// Range at which the mob starts becoming aggressive
	public float aggroRange = 300f;
	// Distance at which the mob starts circling the player 
	public float circleDistance = 30f;

	// Use this for initialization - get the player object
	void Start () {
		playerObject = GameObject.Find("PH-Ship");
	}
	
	// Update is called once per frame
	void Update () {
		// calculate distance to player
		distance = Vector3.Distance (playerObject.transform.position, this.transform.position);

		// check if the enemy is within ___ meters of the player
		if (distance < aggroRange) 
		{
			// smoothly rotate to face the player
			Vector3 direction = playerObject.transform.position - this.transform.position;
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation,
				Quaternion.LookRotation (direction), turnSpeed * Time.deltaTime);

			// start moving towards the player, stop within a certain distance, and start circling
			if (direction.magnitude > moveAngleThreshold && distance > circleDistance) 
			{
				this.transform.Translate (0, 0, moveSpeed * Time.deltaTime);
			}

			// TODO: implement circling, and eventually shooting at the player, obstacle avoidance, 
			//       patrol waypoints, etc.

		}
	}
}
