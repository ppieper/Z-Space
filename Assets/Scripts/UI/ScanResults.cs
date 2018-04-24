using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanResults : MonoBehaviour {
	
	private Vector3 objectScreenPos;
	private GameObject playerObject;
	private int distance;
	private string scanResult;
	private bool scanActive = false;
	private const int SCAN_RANGE = 1000;

	[TextArea(0,1)]
	// Input text to display on player's scanner
	public string scanText;
	// Input bounty display on player's scanner
	public int bounty;
	// Is the entity an enemy ship? (we can have bounties on stationary objects, too)
	public bool isEnemy;
	// Input font to use for the text display 
	public Font scanFont;
	// Input color of the text displayed
	public Color color;

	// called in editor when reset button is pressed, or when script is first attached
	private void Reset() 
	{
		// set the default inspector values for convenience
		scanFont = Resources.Load("Fonts/nk57-monospace-cd-bd") as Font;
		// rgba 195 250 255 255
		color = new Color(.765F,.98F, 1F, 1F);
		// auto-set the isEnemy
		if (GetComponent("Enemy"))
		{
			isEnemy = true;
		}
	}    

	// initializations
	private void Start() 
	{
		objectScreenPos = new Vector3();
		playerObject = GameObject.FindGameObjectWithTag("Player");
	}    

	// called multiple times per frame
	// display the results if scanner is on and object is in front of camera
	private void OnGUI()
	{
		if (scanActive) 
		{
			// if the camera is facing the object, the z axis will be non-negative
			if (objectScreenPos.z >= 0) {
				// if enemy is out of range, we cannot scan
				if (isEnemy && distance > SCAN_RANGE) 
				{
					return;
				}
				// otherwise, display the scan result
				GUI.skin.font = scanFont;
				GUI.contentColor = color;
				GUIStyle style = GUI.skin.GetStyle ("Label");
				style.alignment = TextAnchor.UpperCenter;
				// center the label on the object 
				GUI.Label (new Rect (objectScreenPos.x-250, Screen.height - objectScreenPos.y-22.5F, 500, 45), scanResult);
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
			objectScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
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
			// calculate the distance between the player and the object
			distance = (int)Vector3.Distance(gameObject.transform.position, playerObject.transform.position);

			// if the scannable object is an enemy ship, ignore them until we get closer
			if (isEnemy && distance > SCAN_RANGE) 
			{
				return;
			}
			// format the scan result: "OBJECT - 0km 
			//                          BOUNTY: 0cr " (only if there is a bounty set)
			// 1 unit = 1 meter
			scanResult = scanText + " - " + (distance/1000F).ToString() + "km" + ((bounty != 0) ? "\nBOUNTY: " + bounty + "cr\n" : "");
		}
	}    
}
