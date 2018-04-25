using UnityEngine;

public class PauseController : MonoBehaviour {
	[SerializeField]
	private GameObject menu;
	
	// Update is called once per frame
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			if (!GameManager.Instance.isPaused) 
				Pause();
			else 
				Unpause();
		}
	}
	public void Pause()
	{
		menu.SetActive(true);
		Time.timeScale = 0;
		GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
	}

	public void Unpause()
	{
		menu.SetActive(false);
		Time.timeScale = 1;
		GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
	}
}