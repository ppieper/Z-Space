﻿
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;
	public float shield = 5f;
	public GameObject deathExplosion;

	void Start()
	{
		// if shields are disabled, make sure we don't start off with any shield
		if (GetComponent<Enemy> () && !GetComponent<Enemy>().hasShield)
			shield = 0;
	}

    public void TakeDamage (float amount)
    {
		// let shield absorb some damage
		if (shield != 0) 
		{
			if (amount >= shield) {
				amount = amount - shield;
				shield = 0;
			} else if (amount < shield) 
			{
				shield -= amount;
				amount = 0;
			}
		}
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die ()
    {
		if (deathExplosion) {
			GameObject explosion = Instantiate (deathExplosion);
			explosion.transform.SetParent(GameObject.FindGameObjectWithTag("WorldDynamic").transform);
			Destroy (explosion, 1f);
		}
		EnemyManager.Instance.RemoveEnemy(gameObject.GetComponent<Enemy>());
        Destroy(gameObject);

    }
}