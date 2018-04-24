using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

	private Vector3 objectScreenPos;
	private GameObject playerObject;
	private GameObject targetObject;
	private Texture targetTexture;
	private float distance;

	private const int SCAN_RANGE = 1000;

	public Color color;

	// called in editor when reset button is pressed, or when script is first attached
	private void Reset() 
	{
		// rgba 194 24 7 255
		color = new Color(.761F,.09F, 0.03F, 1F);
	}    

	// initializations
	private void Start() 
	{
		objectScreenPos = new Vector3();
		playerObject = GameObject.Find("PH-Ship");
		// create the target sprite
		targetObject = new GameObject ("Target-HUD");
		targetObject.transform.parent = transform;
		targetTexture = Resources.Load<Texture>("Sprites/Target");
		SpriteRenderer targetRenderer = targetObject.AddComponent<SpriteRenderer>();
		targetRenderer.color = color;
	}    

	// called multiple times per frame
	// display the results if scanner is on and object is in front of camera
	private void OnGUI()
	{
		// ignore them until we get closer
		if (distance <= SCAN_RANGE) 
		{
			// if the camera is facing the object, the z axis will be non-negative
			if (objectScreenPos.z >= 0) 
			{
				// if enemy is out of range, we cannot scan
				if (distance > SCAN_RANGE) 
				{
					return;
				}
				// otherwise, display the target sprite
				float modifier = (1/distance)* 1000;       // these values
				float width = 150 + Mathf.Min(5,modifier); // need tweaking
				GUI.color = color;
				// center the sprite on the target
				GUI.DrawTexture (new Rect (objectScreenPos.x-width/2.0f, Screen.height - objectScreenPos.y-width/2.0f, width, width), 
					             targetTexture, ScaleMode.ScaleToFit);
				targetObject.SetActive (true);
			}
		} 
		else 
		{
			targetObject.SetActive (false);
		}
	}    

	// called after Update(), once per frame
	private void LateUpdate()
	{
		objectScreenPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);

		// if the camera is facing the object, the z axis will be non-negative
		if (objectScreenPos.z >= 0)
		{
			// calculate the distance between the player and the object
			distance = Vector3.Distance(gameObject.transform.position, playerObject.transform.position);
		}
	}    
}
