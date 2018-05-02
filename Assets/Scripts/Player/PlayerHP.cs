using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour {

    public float maxHP;
    private float currentHP;
    public Slider HPbar;
	private PlayerShield shield;


	// Use this for initialization
	void Start () {
		// make sure the GameManager gets the player's instance
		GameManager.Instance.SetPlayer(gameObject);

		shield = GetComponent<PlayerShield>();

        currentHP = maxHP;
        HPbar.value = calcHPpercentage();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Z) && !GameManager.Instance.isPaused)
            TakeDamage(10);
	}

    public void TakeDamage(float value)
    {

		if (shield.GetCurrentShield() != 0) 
			value = shield.TakeDamage(value);
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
		GameManager.Instance.GameOver();
    }
}
