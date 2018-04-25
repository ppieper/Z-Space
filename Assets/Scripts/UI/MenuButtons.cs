using UnityEngine.SceneManagement;
using UnityEngine;

public class MenuButtons : MonoBehaviour {
	[SerializeField]
	private GameObject pauseMenu;

	// Load a level
	public void OnStart(string levelName) 
	{
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
		EnemyManager.Instance.Cleanup();
		SceneManager.LoadScene("Menu");
	}

	// Restart level
	public void OnRestart() 
	{
		EnemyManager.Instance.Cleanup();
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}
