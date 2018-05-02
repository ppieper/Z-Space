using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour {

  public Camera cp;

  public float thrustStrength = 100.0f;
  public float breakForce = 50.0f;
  public float cruiseRatio = 1.0f;
  public float rollSpeed = 150.0f;
  public float turnSpeed = 225.0f;
  public float speedToTurnRatio = 0.75f;
  public float straightRatio = 0.1f;
  public float maxFOV = 135f;
  public float collisionKnockback = 50f;

  public bool inverseY = false;
  public bool forceDriven = true;
  public bool torqueDriven = false;
  public bool fovShift = false;

  /* DEBUG VALUES */
  public Text speedOutput;
  public Text pitchYawOutput;

  private Rigidbody rb;
  private Vector3 front;

  private float speed;
  private float projSpeed;
  private float invertY;
  private float trueRollSpeed;
  private float oldFOV;
  private float fovDiff;

  public AudioSource ambienceSound;
  public AudioSource engineSound;
  public float minEnginePitch;
  public float maxEnginePitch;

  // Use this for initialization
  void Start ()
  {
    rb = GetComponent<Rigidbody>();
    refresh();
    invertY = inverseY ? -1 : 1;
    trueRollSpeed = rollSpeed * 2;
    oldFOV = cp.fieldOfView;
    fovDiff = maxFOV - oldFOV;
	minEnginePitch = 0f;
	maxEnginePitch = 1.3f;
	}

  void Update()
  {
	if (!ambienceSound.isPlaying) 
	{
		ambienceSound.Play(); // the constant drone of the ship's engine
		ambienceSound.loop = true;
	}

	if (!engineSound.isPlaying) 
	{
		engineSound.Play(); // the ship's thrusters
		engineSound.loop = true;
	}
	else // adjust the sound of the ship's thrusters based on the speed at which it is traveling
	{
		float pitch = Mathf.Lerp(minEnginePitch, maxEnginePitch, forceDriven ? rb.velocity.magnitude/100 :
			                                                                   speed/100);
		engineSound.pitch = pitch; 
		//Debug.Log ("Pitch: " + pitch);
	}

  }

  // Update for many time to deal with physics movement
  void FixedUpdate()
  {
	if (GameManager.Instance.isPaused)
	  return;

    // BASIC KEY INPUT
    roll();
    camPitchYaw();

    refresh();

	thrust ();

    refresh();
    if(cp && fovShift)
      fovChange();

    // DB
    debugOutput();
  }

  // Bounce back on collisions & take some damage
  public void OnCollisionEnter(Collision obstacle) 
  {
	  // get the force vector
	  Vector3 bounceForce = transform.position - obstacle.transform.position;
	  // normalize force vector and multiply it by the configurable collisionKnockback value
	  bounceForce.Normalize();
	  rb.AddForce(bounceForce * collisionKnockback);
	  // player takes some damage
	  gameObject.GetComponent<PlayerHP>().TakeDamage (5);
  }


  // Private Functions
  // Thrust (up or down)
  private void thrust()
  {
    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))  // thrust forward
    {
	  if (forceDriven) 
	  {
		if (GameManager.Instance.IsPlayerOutOfBounds())
		  rb.AddForce (front * thrustStrength/2, ForceMode.Acceleration);
		else
		  rb.AddForce (front * thrustStrength, ForceMode.Acceleration);
	  }
	else 
	{
				if (GameManager.Instance.IsPlayerOutOfBounds())
		speed = Time.deltaTime * thrustStrength/2;
	  else
		speed += Time.deltaTime * thrustStrength;
	}
    }
    else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))  // thrust backward
    {
      if (projSpeed > 0)  // projected speed = dot product of front and velocity
      {
        if (forceDriven)
          rb.AddForce(front * breakForce * -1, ForceMode.Acceleration);
        else
          speed -= Time.deltaTime * breakForce;
      }
    }
    else  // let go
    {
      if (forceDriven)
        rb.AddForce(front * speed * cruiseRatio, ForceMode.Acceleration);
      else
        speed += Time.deltaTime * speed * cruiseRatio;
    }

    rb.velocity = front * speed;
  }

  // Key Functions
  // Roll (Left or Right)
  private void roll()
  {
    if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))  // Roll CCW
    {
      //Debug.Log("Roll Left!");
      if (torqueDriven)
        rb.AddTorque(front*trueRollSpeed);
      else
        transform.RotateAround(transform.position, transform.forward,
          Time.deltaTime*rollSpeed);
    }
    else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))  // Roll CW
    {
      //Debug.Log("Roll Right!");
      if (torqueDriven)
        rb.AddTorque(-front*trueRollSpeed);
      else
		    transform.RotateAround(transform.position, transform.forward,
          -Time.deltaTime*rollSpeed);
    }
  }

  // Camera control Pitch (Up and Down) and Yaw (Left and Right)
  private void camPitchYaw()
  {
    // take note of speed influence
    float speedRatio = speed/thrustStrength*speedToTurnRatio + (1.0f-speedToTurnRatio);

    // Get Mouse Input
    float pitchIn = Input.mousePosition.y - Screen.height/2;
    float yawIn = Input.mousePosition.x - Screen.width/2;

    float pitchOut = Reframe(pitchIn, -Screen.height/2, Screen.height/2, -1, 1);
    float yawOut = Reframe(yawIn, -Screen.width/2, Screen.width/2, -1, 1);

    // balance mitigate huge edge
    float mouseDist = Mathf.Sqrt(pitchOut*pitchOut + yawOut*yawOut);

    if (mouseDist > 1f)
    {
      pitchOut /= mouseDist;
      yawOut /= mouseDist;
    }

    // control aid for center
    if (-straightRatio < yawOut && yawOut < straightRatio
        && -straightRatio < pitchOut && pitchOut < straightRatio)
    {
      pitchOut = 0;
      yawOut = 0;
    }

    if (pitchYawOutput)
      pitchYawOutput.text = "Pitch: " + pitchOut + ", Yaw: " + yawOut;

    // Pitch Control
    // uses transform.right for torque
		transform.RotateAround(transform.position, transform.right,
                           -pitchOut*Time.deltaTime*turnSpeed*speedRatio*invertY);

    // Yaw Control
    // uses transform.up for torque
		transform.RotateAround(transform.position, transform.up,
                           yawOut*Time.deltaTime*turnSpeed*speedRatio);
  }

  // FOV Camera Change by speed
  private void fovChange()
  {
    cp.fieldOfView = oldFOV + fovDiff * speed/thrustStrength;
  }
  
  // DEBUG
  private void debugOutput()
  {
    if (speedOutput)
      speedOutput.text = "Speed: " + (int)speed;
  } 


  // Backend Functions
  // refesh values
  private void refresh()
  {
    front = transform.forward;
    speed = rb.velocity.magnitude;
    // projected speed = dot product of front and velocity = speed relative to forward
    projSpeed = Vector3.Dot(front,rb.velocity);
  }

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
