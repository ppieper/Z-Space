using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

  // list of states
  enum States { CRUISE, APPROACH, ATTACK, DIVERT };
    // CRUISE :: Default state that just roam around until it senses a player
    // APPROACH :: Approaches to the player target until it is on their crosshair
    // ATTACK :: Fires at the player at the target
    // DIVERT :: When dangerously close, the enemy turns away to prevent crashing

  // enemy count
  public static int enemyCount = 0;

  // AI Properties
  public float cruiseSpeed = 10.0f;    // speed of the AI when not in combat
  public float fov = 60.0f;            // angle of view cone of AI to notice the player
  public float viewRange = 2500.0f;    // effective range of the view cone of the AI to notice the player
  public float senseRange = 500.0f;    // effective range of the sense sphere of the AI to notice the player

  public float combatSpeed = 75.0f;    // speed of the AI when in combat
  public float turnSpeed = 200.0f;     // turn speed of the AI

  private Rigidbody rb;

  private States state;
  private States prev;

  // Use this for initialization
  void Start () {
    enemyCount++;
    rb = GetComponent<Rigidbody>();
    rb.velocity = transform.forward * cruiseSpeed;

    prev = state = States.CRUISE;
  }
	
	// Update is called once per frame
	void FixedUpdate () {
    if (state == States.CRUISE)
    {
      // ACTION :: Default state that just roam around until it senses a player
      rb.velocity = transform.forward * cruiseSpeed;

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
  }


  // debug on draw gizmo
  private void OnDrawGizmos()
  {
    Gizmos.color = Color.red;
    // draw sense sphere
    Gizmos.DrawWireSphere(transform.position, senseRange);
    // draw view range
    Gizmos.DrawRay(transform.position, transform.forward * viewRange);
  }
}
