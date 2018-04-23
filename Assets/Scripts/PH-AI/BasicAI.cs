using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

  // list of states
  enum States { CRUISE, APPROACH, ATTACK, DIVERT, OVERTAKE, REALIGN };
    // CRUISE :: Default state that just roam around until it senses a player
    // APPROACH :: Approaches to the player target until it is on their crosshair
    // ATTACK :: Fires at the player at the target
    // DIVERT :: When dangerously close, the enemy turns away to prevent crashing
    // OVERTAKE :: When closing in to the player, chasing, overtake onto the side
    // REALIGN :: After overtaking, turn around to reapproach to the player

  // enemy count
  public static int enemyCount = 0;

  // AI Properties
  public float cruiseSpeed = 10.0f;    // speed of the AI when not in combat
  public float fov = 60.0f;            // angle of view cone of AI to notice the player
  public float viewRange = 2500.0f;    // effective range of the view cone of the AI to notice the player
  public float senseRange = 500.0f;    // effective range of the sense sphere of the AI to notice the player

  public float combatSpeed = 75.0f;    // speed of the AI when in combat
  public float turnSpeed = 200.0f;     // turn speed of the AI

  public float overtakeRange = 25.0f;  // effective range of when they start to attempt to overttake you
  public float sideOTRange = 5.0f;     // potential range of the distance of the player by the side
  public float realignRange = 100.0f;  // effective range of when they move away from the player before returning back in

  private Rigidbody rb;                // RigidBody of the AI
  private Ray detect;                  // Ray for detecting stuff

  private States state;                // Current State of the AI
  private States prev;                 // Previous State of the AI (for Divert)

  // Use this for initialization
  void Start () {
    enemyCount++;
    rb = GetComponent<Rigidbody>();
    rb.velocity = transform.forward * cruiseSpeed;

    prev = state = States.CRUISE;
    RayUpdate();
  }
	
	// Update is called once per frame
	void Update () {
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
        //  TRANSITION to ATTACK :: Check if they already have player in their crosshair
        if (false) {
          state = States.ATTACK;
        }
      }
    }
    else if (state == States.APPROACH)
    {
      // ACTION :: Turn themselves to the player to attack
      rb.AddForce(transform.forward * combatSpeed, ForceMode.Acceleration);


      // TRANSITION to DIVERT :: Check if they might almost crash
      if (false)
      {
        prev = state;
        state = States.DIVERT;
      } // TRANSITION to ATTACK :: Check if they have player in their crosshair
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
      } // TRANSITION to APPROACH :: Check if they have player is out of their crosshair
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
      rb.AddForce(transform.forward * cruiseSpeed, ForceMode.Acceleration);

    }
    else if (state == States.APPROACH)
    {
      // ACTION :: Turn themselves to the player to attack
      rb.AddForce(transform.forward * combatSpeed, ForceMode.Acceleration);

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

    RayUpdate();
  }


  private void RayUpdate()
  {
    detect.direction = transform.forward;
    detect.origin = transform.position;
  }

  // debug on draw gizmo
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    // draw sense sphere
    Gizmos.DrawWireSphere(transform.position, senseRange);
    // draw view range
    Gizmos.DrawRay(transform.position, transform.forward * viewRange);
    // draw overrtake range
    Gizmos.color = Color.green;
    Gizmos.DrawWireSphere(transform.position, overtakeRange);
    Gizmos.DrawWireSphere(transform.position, sideOTRange);
    // draw realign
    Gizmos.color = Color.cyan;
    Gizmos.DrawWireSphere(transform.position, realignRange);
  }
}
