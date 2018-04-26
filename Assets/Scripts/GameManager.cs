using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager Instance = null;
	[HideInInspector]
	public bool isPaused;
	private GameObject player;
	[SerializeField]
	private GameObject pauseMenu;
	[SerializeField]
	private GameObject gameOverMenu;
	[SerializeField]
	private GameObject outOfBoundsWarning;
	private bool outOfBounds = false;
	private bool canUseMenu = true;

	void Awake()
	{
		if (Instance == null)
			Instance = this;
		else if (Instance != this)
			Destroy (gameObject);

		DontDestroyOnLoad(gameObject);
		HideMenusNotifications();

	}

	// Update is called once per frame
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Escape) && canUseMenu) 
		{
			if (!GameManager.Instance.isPaused) 
				Pause();
			else 
				Unpause();
		}
		if(player && PlayerOutOfBounds())
			EnableOutOfBoundsWarning();
		else
			DisableOutOfBoundsWarning();
			
	}

	public void Reload()
	{
		Cleanup();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		HideMenusNotifications();
		Unpause();
		canUseMenu = true;
	}

	public void LoadLevel(string levelName)
	{
		Cleanup();
		SceneManager.LoadScene(levelName);
		Unpause();
		canUseMenu = true;
	}

	public void QuitToMenu()
	{
		Cleanup();
		SceneManager.LoadScene("Menu");
		HideMenusNotifications();
		canUseMenu = false;
	}

	public void Cleanup()
	{
		EnemyManager.Instance.Cleanup();
		DisableOutOfBoundsWarning();
	}

	public void GameOver()
	{
		gameOverMenu.SetActive(true);
		isPaused = false;
		canUseMenu = false;
		gameOverMenu.GetComponentInChildren<GameOver>().FadeToRed();
		Time.timeScale = 0;
		isPaused = true;
	}

	public void Pause()
	{
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
		isPaused = true;
	}

	public void Unpause()
	{
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
		isPaused = false;
	}

	// return current player out of bounds state, which gets updated each frame
	public bool IsPlayerOutOfBounds() 
	{
		return outOfBounds;
	}

	// return true if player is out of bounds
	private bool PlayerOutOfBounds() 
	{
		if (Mathf.Abs (player.transform.position.x) >= Constants.mapBorder
		    || Mathf.Abs (player.transform.position.y) >= Constants.mapBorder
		    || Mathf.Abs (player.transform.position.z) >= Constants.mapBorder) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}

	// disable the out of bounds warning
	private void DisableOutOfBoundsWarning(bool disable = false)
	{
		if(outOfBounds)
		{
		CancelInvoke("FlashWarning");
		outOfBoundsWarning.SetActive(false);
			outOfBounds = false;
		return;
		}
	}

	// enable the out of bounds warning
	private void EnableOutOfBoundsWarning()
	{
		if (!outOfBounds) 
		{
			InvokeRepeating ("FlashWarning", 0, 1.0f);
			outOfBounds = true;
		}
	}

	// flash warning on player's screen
	private void FlashWarning()
	{
		outOfBoundsWarning.SetActive(!outOfBoundsWarning.activeInHierarchy);
	}

	private void HideMenusNotifications()
	{
		pauseMenu.SetActive(false);
		gameOverMenu.SetActive(false);
		outOfBoundsWarning.SetActive(false);
	}

	public GameObject GetPlayer()
	{
		return player;
	}

	public void SetPlayer(GameObject player)
	{
		this.player = player;
	}
}
