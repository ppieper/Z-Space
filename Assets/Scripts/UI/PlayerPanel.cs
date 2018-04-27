using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPanel : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GetComponent<RectTransform>().position = new Vector3(Screen.width, Screen.height/20, 0)/2;
	}
}
