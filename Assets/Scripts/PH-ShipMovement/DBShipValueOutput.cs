using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DBShipValueOutput : MonoBehaviour {

  public Text speedOutput;

  private Rigidbody rb;


  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update () {
    float velocity = rb.velocity.magnitude;

    // debug
	if (speedOutput) {
		speedOutput.text = "SPEED: " + (int)velocity;
	}
  }
}
