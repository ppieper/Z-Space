using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

	[SerializeField]
	private CanvasGroup canvasGroup;

	public void FadeToRed()
	{
		canvasGroup.alpha = 0;
		StartCoroutine(DoFade());
	}

	IEnumerator DoFade()
	{
		while (canvasGroup.alpha < .75f) 
		{
			canvasGroup.alpha += Time.unscaledDeltaTime / 2;
			yield return null;
		}
		canvasGroup.interactable = false;
		yield return null;
	}
}