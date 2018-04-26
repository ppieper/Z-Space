using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour {

    public float maxHP;
    public float currentHP;
    public Slider HPbar;


	// Use this for initialization
	void Start () {
		// make sure the GameManager gets the player's instance
		GameManager.Instance.SetPlayer(gameObject);

        maxHP = 100f;
        currentHP = maxHP;

        HPbar.value = calcHPpercentage();

		// reposition to bottom of screen
		RectTransform trans = HPbar.GetComponent<RectTransform>();
		trans.position = new Vector3(Screen.width, Screen.height/20, 0)/2;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
            TakeDamage(10);
	}

    public void TakeDamage(float value)
    {
        currentHP -= value;
        HPbar.value = calcHPpercentage();
        if (currentHP <= 0)
            Die();

    }

    float calcHPpercentage()
    {
        return currentHP / maxHP;

    }


    void Die()
    {
        currentHP = 0;
        Debug.Log("Your ship has been destroyed.");
		GameManager.Instance.GameOver();
    }
}
