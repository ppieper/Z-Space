using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanResults : MonoBehaviour {
	
	private GameObject scannableObject;
	private Vector3 objectLocation;
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
		scannableObject = this.gameObject;
		objectLocation = new Vector3();
		playerObject = GameObject.Find("FPSChar1");
		playerPos = playerObject.transform.position;
	}    

	// display the results if scanner is on (called multiple times per frame)
	private void OnGUI()
	{
		if (scanActive) 
		{
			GUI.skin.font = scanFont;
			GUI.contentColor = color;
			GUI.Label (new Rect (objectLocation.x, Screen.height - objectLocation.y, 500, 30), scanResult);
		}
	}    

	// called once per frame, check to see if the player is currently scanning
	void Update()
	{
		// check to see if the Scan key is held down 
		if (Input.GetButton("Scan"))
		{
			scanActive = true;
		} else 
		{
			scanActive = false;	
		}
	}

	// called after Update()
	// Use LateUpdate to compute the distance after the player or object has moved, and add it to the results
	private void LateUpdate()
	{
		// save some resources by only doing the computations while user is scanning
		if (!scanActive) 
		{
			return;
		}

		// if the camera is facing away from the object, the z axis will be negative
		if (Camera.current.WorldToScreenPoint(scannableObject.transform.position).z >= 0)
		{
			objectLocation = Camera.current.WorldToScreenPoint(scannableObject.transform.position);
			playerPos = playerObject.transform.position;
			distance = (int)Vector3.Distance(scannableObject.transform.position, playerObject.transform.position);
			scanResult = scanText + " - " + distance.ToString() + "km";

		}
	}    
}
