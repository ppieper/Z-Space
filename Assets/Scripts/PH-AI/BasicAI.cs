using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

  // list of states
  enum States { CRUISE, APPROACH, ATTACK, DIVERT, OVERTAKE, REALIGN };
    // CRUISE :: Default state that just roam around until it senses a player
    // APPROACH :: Approaches to the player target until it is on their cross-hair
    // ATTACK :: Fires at the player at the target
    // DIVERT :: When dangerously close, the enemy turns away to prevent crashing
    // OVERTAKE :: When closing in to the player, chasing, overtake onto the side
    // REALIGN :: After overtaking, turn around to reapproach to the player

  // AI Properties
  public float cruiseSpeed = 10.0f;       // speed of the AI when not in combat
  // public float fov = 60.0f;               // angle of view cone of AI to notice the player
  // public float viewRange = 2500.0f;       // effective range of the view cone of the AI to notice the player
  public float senseRange = 2500.0f;      // effective range of the sense sphere of the AI to notice the player

  public float combatSpeed = 75.0f;       // speed of the AI when in combat
  public float turnSpeed = 200.0f;        // turn speed of the AI

  public float divertRange = 50.0f;       // effective range of when they should start to move away from crashing
  public float overtakeRange = 100.0f;    // effective range of when they start to attempt to overtake you
  public float sideOvertakeRange = 7.5f;  // potential range of the distance of the player by the side
  public float realignRange = 250.0f;     // effective range of when they move away from the player before returning back in

  private States state;                   // Current State of the AI
  private States prev;                    // Previous State of the AI (for Divert)

  private Rigidbody enemyOwnRB;           // Rigidbody of the AI
  private GameObject playerObject;        // Reference to the player ship for position and other what not

  private Ray detectRay;                  // Ray for detecting stuff
  private RaycastHit detectHit;           // Helper attribute for detectRay
  private Plane relativePlayerPlane;      // Plane for overtaking player
  private float distanceToPlayerPlane;    // Distance for trying to overtake player
  private float planarDistanceToPlayer;   // Distance on plane from player for successful overtake
  private string objectName;              // The output string of what the detectRay found


  // Use this for initialization
  void Start ()
  {
    // get the player object for reference
    playerObject = GameObject.Find("PH-Ship");
    // get the enemy ship stuff for movement
    enemyOwnRB = GetComponent<Rigidbody>();

    // initialize the states
    prev = state = States.CRUISE;

    // initialize the latter stuff (from UpdateStuff w/ initialization)
    detectRay = new Ray(this.transform.position, this.transform.forward);
    relativePlayerPlane = new Plane(this.transform.forward, playerObject.transform.position);

    UpdateStuff();
  }
	
	// Update is called once per frame
	void Update ()
  {
    if (state == States.CRUISE)
    {
      // ACTION :: Default state that just roam around until it senses a player


      // TRANSITION to DIVERT :: Check if they might almost crash
      if (false)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to APPROACH :: Check if they sense the player
      else if (false)
      {
        state = States.APPROACH;
        //  TRANSITION to ATTACK :: Check if they already have player in their cross-hair
        if (false) {
          state = States.ATTACK;
        }
      }
    }
    else if (state == States.APPROACH)
    {
      // ACTION :: Turn themselves to the player to attack
      //rb.AddForce(transform.forward * combatSpeed, ForceMode.Acceleration);


      // TRANSITION to DIVERT :: Check if they might almost crash
      if (false)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to ATTACK :: Check if they have player in their cross-hair
      else if (false)
      {
        state = States.ATTACK;
      }
    }
    else if (state == States.ATTACK)
    {
      // ACTION :: Fire at the player


      // TRANSITION to DIVERT :: Check if they might almost crash
      if (false)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to APPROACH :: Check if they have player is out of their cross-hair
      else if (false)
      {
        state = States.APPROACH;
      }
    }
    else if (state == States.DIVERT)
    {
      // ACTION :: Turn away from crashing


      // TRANSITION to [PREVIOUS STATE] :: Check if they successfully divert
      if (false)
      {
        state = prev;
      }
    }
    else if (state == States.OVERTAKE)
    {
      // ACTION :: When closing in to the player, chasing, overtake onto the side


      // TRANSITION to DIVERT :: Check if they might almost crash
      if (false)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to REALIGN :: Check if they are on the side of the player
      else if (false)
      {
        state = States.REALIGN;
      } // TRANSITION to APPROACH :: Check if the player is too fast to try to overtake
      else if (false)
      {
        state = States.APPROACH;
      }
    }
    else if (state == States.REALIGN)
    {
      // ACTION :: After overtaking, turn around to reapproach to the player


      // TRANSITION to DIVERT :: Check if they might almost crash
      if (false)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to APPROACH :: Check if they are away enough to then return back to action
      else if (false)
      {
        state = States.APPROACH;
      }
    }
  }

  // Update is called for physics
  void FixedUpdate()
  {
    if (state == States.CRUISE)
    {
      // ACTION :: Default state that just roam around until it senses a player
      //enemyOwnRB.AddForce(transform.forward*cruiseSpeed, ForceMode.Acceleration);

      this.transform.Translate(0, 0, cruiseSpeed * Time.deltaTime);
    }
    else if (state == States.APPROACH)
    {
      // ACTION :: Turn themselves to the player to attack
      //enemyOwnRB.AddForce(transform.forward*combatSpeed, ForceMode.Acceleration);

      this.transform.Translate(0, 0, combatSpeed * Time.deltaTime);
    }
    else if (state == States.ATTACK)
    {
      // ACTION :: Fire at the player

    }
    else if (state == States.DIVERT)
    {
      // ACTION :: Turn away from crashing

    }
    else if (state == States.OVERTAKE)
    {
      // ACTION :: When closing in to the player, chasing, overtake onto the side

    }
    else if (state == States.REALIGN)
    {
      // ACTION :: After overtaking, turn around to reapproach to the player
      
    }

    UpdateStuff();
  }


  private void UpdateStuff()
  {
    // set the relative player plane at the player with the normal of the enemy's forward
    relativePlayerPlane.SetNormalAndPosition(this.transform.forward, playerObject.transform.position);

    // set the point on plane for the proceeding 
    Vector3 pointOnPlane = relativePlayerPlane.ClosestPointOnPlane(this.transform.position);
    // calculate the current distance to plane
    distanceToPlayerPlane = Vector3.Distance(this.transform.position, pointOnPlane);
    // calculate the current distance on plane to player
    planarDistanceToPlayer = Vector3.Distance(pointOnPlane, playerObject.transform.position);

    // reset the ray to point
    detectRay.origin = this.transform.position;
    detectRay.direction = this.transform.forward;

    // get the object for detection
    if (Physics.Raycast(detectRay, out detectHit, rayLength))
      objectName = interationRayHit.transform.GameObject.name;
    else
      objectName = "-";
  }

  // debug on draw gizmo
  /*
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    // draw sense sphere
    Gizmos.DrawWireSphere(transform.position, senseRange);
    // draw view range
    Gizmos.DrawRay(transform.position, transform.forward * viewRange);
    // draw overtake range
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, overtakeRange);
    Gizmos.DrawWireSphere(transform.position, sideOTRange);
    // draw realign
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, realignRange);
  }
  */
}
