using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIndicator : MonoBehaviour {

	public Color color;
	public Canvas canvas;
	public GameObject HPBar;
	public GameObject SBar;
	private Vector3 screenCenter;
	private Vector3 screenBounds;
	private Sprite target;
	private Sprite arrow;

	// Use this for initialization
	void Start () 
	{
		screenCenter = new Vector3(Screen.width, Screen.height, 0)/2;
		screenBounds = screenCenter * 0.95f;
		target = Resources.Load<Sprite> ("Sprites/Target_no_crosshair");
		arrow = Resources.Load<Sprite> ("Sprites/Arrow");
	}
	
	// LateUpdate is called once per frame, after Update()
	void LateUpdate () 
	{
		DrawIndicators();
	}

	void DrawIndicators()
	{
		List<Enemy> enemies = EnemyManager.Instance.GetEnemies();

		// draw the indicators for each enemy
		foreach (Enemy enemy in enemies) 
		{
			// get the indicator and its properties
			GameObject indicator = enemy.GetIndicator();

			// hide when the enemy is out of range
			if (!enemy.InFireRange ()) 
			{
				indicator.active = false;
				continue;
			}
			indicator.active = true;
			RectTransform trans = enemy.GetIndicatorTransform();
			Image image = enemy.GetIndicatorImage();
			GameObject healthBar = enemy.GetHealthBar();
			GameObject shieldBar = enemy.GetShieldBar();

			// get the enemy's screen position
			Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);

			// is the enemy offscreen, or onscreen?
			if (screenPos.z > 0 && 
				screenPos.x > 0 && screenPos.x < Screen.width &&
				screenPos.y > 0 && screenPos.y < Screen.height) 
			{
				// onscreen
				image.sprite = target;
				trans.sizeDelta = new Vector2 (100, 100);
				indicator.transform.position = screenPos;
				indicator.transform.rotation = Quaternion.Euler(0, 0, 0);
				healthBar.transform.localScale = new Vector3(.3f, .3f, .3f);
				if (enemy.hasShield)
					shieldBar.transform.localScale = new Vector3(.3f, .3f, .3f);
			} else 
			{
				// offscreen
				image.sprite = arrow;
				trans.sizeDelta = new Vector2 (48, 48);
				healthBar.transform.localScale = new Vector3(.2f, .2f, .2f);
				if (enemy.hasShield)
					shieldBar.transform.localScale = new Vector3(.2f, .2f, .2f);

				// invert if behind
				if (screenPos.z < 0) 
					PlaceOffScreen(-screenPos, ref indicator);
				else
					PlaceOffScreen(screenPos, ref indicator);
			}
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
}