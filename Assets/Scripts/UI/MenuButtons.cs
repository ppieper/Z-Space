using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuButtons : MonoBehaviour {
	[SerializeField]
	private GameObject pauseMenu;

	// Load a level
	public void OnStart(string levelName) 
	{
		if (GameManager.Instance)
			GameManager.Instance.LoadLevel (levelName);
		else
			SceneManager.LoadScene(levelName);
	}

	// Quit the game
	public void OnQuit() 
	{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
	}

	// Quit to the main menu
	public void OnQuitToMenu() 
	{
		GameManager.Instance.QuitToMenu();
	}

	// Restart level
	public void OnRestart() 
	{
		GameManager.Instance.Reload();
	}
}
