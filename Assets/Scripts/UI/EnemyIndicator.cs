using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour {

	public Color color;
	private Vector3 screenCenter;
	private Vector3 screenBounds;
	private List<GameObject> indicators = new List<GameObject>();
	[SerializeField]
	private Canvas canvas;

	// Use this for initialization
	void Start () 
	{
		screenCenter = new Vector3(Screen.width, Screen.height, 0)/2;
		screenBounds = screenCenter * 0.95f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		DrawIndicators();
	}

	void DrawIndicators()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		// erase the previous indicators
		clearIndicators();

		foreach (GameObject enemy in enemies) 
		{
			// create a temporary indicator object
			GameObject indicator = new GameObject("Indicator");
			indicator.transform.SetParent (canvas.transform);
			RectTransform trans = indicator.AddComponent<RectTransform>();
			Image image = indicator.AddComponent<Image>();
			image.color = color;

			// get the enemy's screen position
			Vector3 screenPos = Camera.main.WorldToScreenPoint (enemy.transform.position);

			// is the enemy offscreen, or onscreen?
			if (screenPos.z > 0 && 
				screenPos.x > 0 && screenPos.x < Screen.width &&
				screenPos.y > 0 && screenPos.y < Screen.height) 
			{
				// onscreen
				image.sprite = Resources.Load<Sprite> ("Sprites/Target_no_crosshair");
				trans.sizeDelta = new Vector2 (100, 100);
				indicator.transform.position = screenPos;
			} else 
			{
				// offscreen
				image.sprite = Resources.Load<Sprite> ("Sprites/Arrow");
				trans.sizeDelta = new Vector2 (48, 48);

				// invert if behind
				if (screenPos.z < 0) 
					PlaceOffScreen(-screenPos, ref indicator);
				else
					PlaceOffScreen(screenPos, ref indicator);
			}
			indicators.Add(indicator);
		}
	}

	void PlaceOffScreen(Vector3 screenPos, ref GameObject indicator)
	{
		indicator.transform.position = screenPos;

		// shift coordinates so that origin is at center of screen, instead of bottom left
		screenPos -= screenCenter;

		// angle from center screen to the enemy
		float angle = Mathf.Atan2 (screenPos.y, screenPos.x);
		angle -= 90 * Mathf.Deg2Rad;
	
		float cos = Mathf.Cos(angle);
		float sin = -Mathf.Sin(angle);

		// x = y/m
		// y = x*m
		float m = cos / sin;

		if (cos > 0)  // enemy above
		{
			screenPos = new Vector3 (screenBounds.y / m, screenBounds.y, 0);
		} else // enemy below
		{
			screenPos = new Vector3 (-screenBounds.y / m, -screenBounds.y, 0);
		}

		// out of bounds -- which side?
		if (screenPos.x > screenBounds.x) // out of bounds, right
		{
			screenPos = new Vector3 (screenBounds.x, screenBounds.x * m, 0);
		}
		else if (screenPos.x < -screenBounds.x) // out of bounds, left
		{
			screenPos = new Vector3 (-screenBounds.x, -screenBounds.x * m, 0);
		}

		// restore origin back to bottom left
		screenPos += screenCenter;

		// update offscreen sprite's position and rotation
		indicator.transform.position = screenPos;
		indicator.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
	}

	void clearIndicators()
	{
		// free the previous indicators from the heap
		foreach (GameObject indicator in indicators) 
		{
			Destroy(indicator);
		}
		indicators.Clear();
	}
}
