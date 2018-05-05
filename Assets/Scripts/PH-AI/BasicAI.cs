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
  // list of possible objects
  enum DetectOutput { NONE, PLAYER, OBSTACLE };
    // NONE :: Detect nothing in range of overtakeRange nor divertRange
    // PLAYER :: Detect the player in divertRange and nothing else in overtakeRange
    // OBSTACLE :: Detect an object overtakeRange regardless of divertRange notices player

  // AI Properties
  public float cruiseSpeed = 10.0f;            // speed of the AI when not in combat
  // public float fov = 60.0f;                    // angle of view cone of AI to notice the player
  // public float viewRange = 2500.0f;            // effective range of the view cone of the AI to notice the player
  public float senseRange = 2500.0f;           // effective range of the sense sphere of the AI to notice the player

  public float combatSpeed = 75.0f;            // speed of the AI when in combat
  public float turnSpeed = 200.0f;             // turn speed of the AI

  public float divertRange = 75.0f;            // effective range of when they should start to move away from crashing (Should be bigger than overtakeRange)
  public float overtakeRange = 50.0f;          // effective range of when they start to attempt to overtake you
  public float sideOvertakeRange = 7.5f;       // potential range of the distance of the player by the side
  public float realignRange = 250.0f;          // effective range of when they move away from the player before returning back in

  private States state;                        // Current State of the AI
  private States prev;                         // Previous State of the AI (for Divert)

  private Rigidbody enemyOwnRB;                // Rigidbody of the AI
  private GameObject playerObject;             // Reference to the player ship for position and other what not

  private Ray detectRay;                       // Ray for detecting stuff
  private RaycastHit detectHit;                // Helper attribute for detectRay
  private Plane relativePlayerPlane;           // Plane for overtaking player
  private float distanceToPlayerPlane;         // Distance for trying to overtake player
  private float planarDistanceToPlayer;        // Distance on plane from player for successful overtake
  private DetectOutput detectResult;           // The output string of what the detectRay found
  private bool playerOnCrosshair;              // Check if the player is on the enemy's cross-hair (independent of other ranges)
  private float detectFromOverlapBias = 5.0f;  // Evoked bias for if detect player in one range, should also move ray towards to fix it


  void Awake()
  {
    playerObject = GameObject.Find("PH-Ship");
  }

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
      // no additional code required

      // TRANSITION to DIVERT :: Check if they might almost crash
      if (detectResult == DetectOutput.OBSTACLE)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to APPROACH :: Check if they sense the player
      else if (Vector3.Distance(playerObject.transform.position, this.transform.position) <= senseRange)
      {
        state = States.APPROACH;
        //  TRANSITION to ATTACK :: Check if they already have player in their cross-hair
        if (playerOnCrosshair) {
          state = States.ATTACK;
        }
      }
    }
    else if (state == States.APPROACH)
    {
      // ACTION :: Turn themselves to the player to attack
      // no additional code required

      // TRANSITION to DIVERT :: Check if they might almost crash
      if (detectResult == DetectOutput.OBSTACLE)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to ATTACK :: Check if they have player in their cross-hair
      else if (playerOnCrosshair)
      {
        state = States.ATTACK;
      }
    }
    else if (state == States.ATTACK)
    {
      // ACTION :: Fire at the player
      // No additional code required

      // TRANSITION to DIVERT :: Check if they might almost crash
      if (detectResult == DetectOutput.OBSTACLE)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to APPROACH :: Check if they have player is out of their cross-hair
      else if (!playerOnCrosshair)
      {
        state = States.APPROACH;
      }
    }
    else if (state == States.DIVERT)
    {
      // ACTION :: Turn away from crashing
      // no additional code required

      // TRANSITION to [PREVIOUS STATE] :: Check if they successfully divert
      if (detectResult != DetectOutput.OBSTACLE)
      {
        state = prev;
      }
    }
    else if (state == States.OVERTAKE)
    {
      // ACTION :: When closing in to the player, chasing, overtake onto the side


      // TRANSITION to DIVERT :: Check if they might almost crash
      if (detectResult == DetectOutput.OBSTACLE)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to REALIGN :: Check if they are on the side of the player
      else if (distanceToPlayerPlane >= detectFromOverlapBias)
      {
        state = States.REALIGN;
      } // TRANSITION to APPROACH :: Check if the player is too fast to try to overtake
      else if (detectResult == DetectOutput.NONE)
      {
        state = States.APPROACH;
      }
    }
    else if (state == States.REALIGN)
    {
      // ACTION :: After overtaking, turn around to reapproach to the player
      // no additional code required

      // TRANSITION to DIVERT :: Check if they might almost crash
      if (detectResult == DetectOutput.OBSTACLE)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to APPROACH :: Check if they are away enough to then return back to action
      else if (Vector3.Distance(playerObject.transform.position, this.transform.position) >= realignRange)
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
      // TODO: Add turning
    }
    else if (state == States.ATTACK)
    {
      // ACTION :: Fire at the player
      // TODO: Add attack mechanic while moving and turning
    }
    else if (state == States.DIVERT)
    {
      // ACTION :: Turn away from crashing
      // TODO: Add moving and turning
    }
    else if (state == States.OVERTAKE)
    {
      // ACTION :: When closing in to the player, chasing, overtake onto the side
      // TODO: maintain distance of the circle with movement and SHIFT
    }
    else if (state == States.REALIGN)
    {
      // ACTION :: After overtaking, turn around to reapproach to the player
      // TODO: Turn away from the player
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

    // check if the player is on the cross-hair of the enemy
    if (Physics.Raycast(detectRay, out detectHit))
    {
      // verify if it is actually the player ship
      if (detectHit.transform.gameObject.name == "PH-Ship")
      {
        playerOnCrosshair = true;
      }
      else
      {
        playerOnCrosshair = false;
      }
    }
    else
    {
      playerOnCrosshair = false;
    }

    // compute the possible output for detectResult
    // check if an object is within range of overtakeRange to check for player ship
    if (Physics.Raycast(detectRay, out detectHit, overtakeRange))
    {
      // verify if it is actually the player ship
      if (detectHit.transform.gameObject.name == "PH-Ship")
      {
        // if so, check what is ahead to divertRange
        // first change detectRay's origin pushing past the ship w/ some bias and add the length
        float offsetLength = detectHit.distance + detectFromOverlapBias;
        detectRay.origin += detectRay.direction * offsetLength;

        if (Physics.Raycast(detectRay, out detectHit, divertRange - offsetLength))
        {
          // if there is an object past player, return an OBSTACLE DetectOutput
          detectResult = DetectOutput.OBSTACLE;
        }
        else
        {
          // if there's nothing else past player, return an PLAYER DetectOutput
          detectResult = DetectOutput.PLAYER;
        }

        // revert back the origin to enemy
        detectRay.origin = this.transform.position;
      }
      else
      {
        // if not, return an OBSTACLE DetectOutput
        detectResult = DetectOutput.OBSTACLE;
      }
    }
    else
    {
      // if not within overtakeRange, check if within divertRange instead
      if (Physics.Raycast(detectRay, out detectHit, divertRange))
      {
        // verify if it is definitely not the player ship
        if (detectHit.transform.gameObject.name != "PH-Ship")
        {
          // if so, return an OBSTACLE DetectOutput
          detectResult = DetectOutput.OBSTACLE;
        }
        else
        {
          // if not, return a NONE DetectOutput
          detectResult = DetectOutput.NONE;
        }
      }
      else
      {
        // if not within divertRange, return a NONE DetectOutput
        detectResult = DetectOutput.NONE;
      }
    }
  }

  // debug on draw gizmo
  private void OnDrawGizmos()
  {
    Color temp = Color.cyan;

    if (state == States.CRUISE)
    {
      temp = Color.cyan;
      Gizmos.color = temp;

      // draw sense sphere
      Gizmos.DrawWireSphere(this.transform.position, senseRange);
    }
    else if (state == States.APPROACH)
    {
      temp = Color.yellow;
    }
    else if (state == States.ATTACK)
    {
      temp = Color.red;
    }
    else if (state == States.DIVERT)
    {
      temp = Color.magenta;
    }
    else if (state == States.OVERTAKE)
    {
      temp = Color.green;
    }
    else if (state == States.REALIGN)
    {
      temp = new Color(0.5F, 0.5F, 1.0F);
    }
    

    // General Gizmos
    Gizmos.color = temp;
    Gizmos.DrawWireSphere(this.transform.position, 10.0f);

    // draw ray of divert range
    Gizmos.DrawRay(this.transform.position + this.transform.forward * divertRange, this.transform.forward * (overtakeRange-divertRange));

    // Create a ring
    Vector3 prevVec, currVec;
    int curveFidelity = 32;
    float radStep = Mathf.PI * 2 / curveFidelity;
    currVec = this.transform.right * sideOvertakeRange + playerObject.transform.position;
    for (int i = 1; i <= curveFidelity; ++i)
    {
      prevVec = currVec;
      currVec = (this.transform.right * Mathf.Cos(radStep * i) + this.transform.up * Mathf.Sin(radStep * i)) * sideOvertakeRange + playerObject.transform.position;
      Gizmos.DrawLine(prevVec,currVec);
    }

    // draw ray of overtake range
    Gizmos.color = Color.white;
    Gizmos.DrawRay(this.transform.position, this.transform.forward * overtakeRange);

    // Note Enemy Position on the plane
    Vector3 pointOnPlane = relativePlayerPlane.ClosestPointOnPlane(this.transform.position);
    Gizmos.DrawLine(this.transform.position, pointOnPlane);
    Gizmos.DrawLine(pointOnPlane, playerObject.transform.position);
  }
}
