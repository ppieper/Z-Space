using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour {

  public float ThrustStrength = 100.0f;
  public float BreakForce = 50.0f;

  public Text debug;

  private Rigidbody rb;
  private Vector3 front;

  // Use this for initialization
  void Start ()
  {
    rb = GetComponent<Rigidbody>();
	}

  // Update for many time to deal with physics movement
  void FixedUpdate()
  {
    front = transform.forward;

    // BASIC KEY INPUT
    thrust();
    roll();
    camPitchYaw();
  }


  // Private Functions
  // Thrust (up or down)
  private void thrust()
  {
    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))  // thrust forward
    {
      //Debug.Log("Thrust Forward!");
      rb.AddForce(front * ThrustStrength, ForceMode.Acceleration);
    }
    else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))  // thrust backward
    {
      //Debug.Log("Thrust Backward!");
      rb.AddForce(front * BreakForce * -1, ForceMode.Acceleration);
    }
  }

  // Key Functions
  // Roll (Left or Right)
  private void roll()
  {
    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))  // Roll CCW
    {
      //Debug.Log("Roll Left!");
      transform.RotateAround(transform.position, transform.forward, 1);
    }
    else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))  // Roll CW
    {
      //Debug.Log("Roll Right!");
      transform.RotateAround(transform.position, transform.forward, 1);
    }
  }

  // Camera control Pitch (Up and Down) and Yaw (Left and Right)
  private void camPitchYaw()
  {
    // Get Mouse Input
    float pitchIn = Input.mousePosition.y - Screen.height/2;
    float yawIn = Input.mousePosition.x - Screen.width/2;

    float pitchOut = Reframe(pitchIn, -Screen.height/2, Screen.height/2, -1, 1);
    float yawOut = Reframe(yawIn, -Screen.width/2, Screen.width/2, -1, 1);

    debug.text = "Pitch: " + pitchOut + ", Yaw: " + yawOut;

    // Pitch Control
    // uses transform.right for torque
    transform.RotateAround(transform.position, transform.right, -pitchOut);

    // Yaw Control
    // uses transform.up for torque
    transform.RotateAround(transform.position, transform.up, yawOut);
  }

  // Backend Functions
  private float Reframe (float input, float oldMin, float oldMax, float newMin, float newMax)
  {
    float oldLen = oldMax - oldMin;
    float newLen = newMax - newMin;

    float oldIn = (input - oldMin) / oldLen;
    float newIn = oldIn * newLen + newMin;

    return CutRange(newIn,newMin,newMax);
  }

  private float CutRange (float input, float min, float max)
  {
    if (input < min)
      return min;
    if (input > max)
      return max;

    return input;
  }
}
