﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanResults : MonoBehaviour {
	
	private GameObject scannableObject;
	private Vector3 objectScreenPos;
	private GameObject playerObject;
	private Vector3 playerPos;
	private int distance;
	private string scanResult;
	private bool scanActive = false;

	[TextArea(0,1)]
	// Input text to display on player's scanner
	public string scanText;
	// Input font to use for the text display
	public Font scanFont;
	// Input color of the text displayed
	public Color color;

	// initializations
	private void Start() 
	{
		scannableObject = gameObject;
		objectScreenPos = scannableObject.transform.position;
		playerObject = GameObject.Find("FPSChar1");
		playerPos = playerObject.transform.position;
	}    

	// called multiple times per frame
	// display the results if scanner is on and object is in front of camera
	private void OnGUI()
	{
		if (scanActive) 
		{
			// if the camera is facing the object, the z axis will be non-negative
			if (objectScreenPos.z >= 0) {
				GUI.skin.font = scanFont;
				GUI.contentColor = color;
				GUI.Label (new Rect (objectScreenPos.x, Screen.height - objectScreenPos.y, 500, 30), scanResult);
			}
		}
	}    

	// called once per frame
	// check to see if the player is currently scanning
	void Update()
	{
		// check to see if the Scan key is held down 
		if (Input.GetButton("Scan"))
		{
			// always ensure that the location is up to date before the GUI is displayed
			objectScreenPos = Camera.main.WorldToScreenPoint(scannableObject.transform.position);
			scanActive = true;
		} else 
		{
			scanActive = false;	
		}
	}

	// called after Update(), once per frame
	// Use LateUpdate to compute the distance after the player or object has moved, and add it to the results
	private void LateUpdate()
	{
		// save some resources by only doing the distance computations/string concatenation while user is scanning
		if (!scanActive) 
		{
			return;
		}

		// if the camera is facing the object, the z axis will be non-negative
		if (objectScreenPos.z >= 0)
		{
			playerPos = playerObject.transform.position;
			distance = (int)Vector3.Distance(scannableObject.transform.position, playerObject.transform.position);
			scanResult = scanText + " - " + distance.ToString() + "km";
		}
	}    
}
