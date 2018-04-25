using UnityEngine;
using UnityEngine.EventSystems;

public class KeyboardNavigation : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;

	private bool buttonSelected;
	
	// Update is called once per frame
	void Update() 
	{
		if (buttonSelected == false && Input.GetAxisRaw ("Vertical") != 0) 
		{
			// make sure we don't select twice (mouse and keyboard/controller)
			if (eventSystem.alreadySelecting)
				return;
			eventSystem.SetSelectedGameObject(selectedObject);
			buttonSelected = true;
		}
	}
	private void OnDisable()
	{
		buttonSelected = false;
	}
}
