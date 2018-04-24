using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
	private Color defaultColor;
	[SerializeField]
	private Color enemyTargettedColor;
	private Camera camera;
	private Vector3 forward;
	private RaycastHit hit;
	private Image image;

	// Use this for initialization
	void Start () {
		camera  = Camera.main;
		image = GetComponent<Image>();
		defaultColor = image.color;
	}
	
	// Update is called once per frame
	void Update () {
		if (Physics.Raycast (camera.transform.position, camera.transform.forward, out hit, Constants.weaponRange)) 
		{
			if(hit.transform.tag == "Enemy")
				image.color = enemyTargettedColor; // change crosshair color when enemy is targetted
		} else 
		{
			image.color = defaultColor; // otherwise use default color
		}
	}
}
