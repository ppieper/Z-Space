using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DBShipValueOutput : MonoBehaviour {

  public Text SpeedOutput;

  private Rigidbody rb;


  void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  // Update is called once per frame
  void Update () {
    float velocity = rb.velocity.magnitude;

    SpeedOutput.text = "SPEED: " + (int)velocity;
  }
}
