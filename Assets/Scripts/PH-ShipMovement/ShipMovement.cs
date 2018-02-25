using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

  public float ThrustStrength = 100.0f;
  public float BreakForce = 50.0f;

  public float TurnTorque = 50.0f;
  public float TurnSpeed;
  public float RestoreRollSpeed;

  private Rigidbody rb;
  
  private float CurrentRollAngle;

  // Use this for initialization
  void Start ()
  {
    rb = GetComponent<Rigidbody>();
    CurrentRollAngle = 0;
	}
	
	// Update is called once per frame
	void Update ()
  {
    transform.RotateAround(transform.position, transform.forward, 20 * Time.deltaTime);
  }

  // Update for many time to deal with physics movement
  void FixedUpdate()
  {
    // BASIC KEY INPUT
    // Thrust (up or down)
    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))  // thrust forward
    {
      Debug.Log("Thrust Forward!");
      rb.AddForce(transform.forward * ThrustStrength, ForceMode.Acceleration);
    }
    else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))  // thrust backward
    {
      Debug.Log("Thrust Backward!");
      rb.AddForce(transform.forward * BreakForce * -1, ForceMode.Acceleration);
    }

    // Roll (left or right)
    
  }
}
