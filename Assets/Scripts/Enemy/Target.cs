
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;
	public float shield = 5f;
	public GameObject deathExplosion;
	private bool dead = false;

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

        if (health <= 0f && !dead)
        {
			dead = true;
            Die();
        }
    }

    void Die ()
    {
		// display an explosion and play the sound effect
		if (deathExplosion) {
			GameObject explosion = Instantiate(deathExplosion, gameObject.transform.position, gameObject.transform.rotation);
			explosion.transform.SetParent(GameObject.FindGameObjectWithTag("WorldDynamic").transform);
			Destroy (explosion, 1f);
			AudioSource audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.clip = Resources.Load("Sound/explosion") as AudioClip;
			audioSource.Play();

		}
		// remove enemy from enemy manager
		Enemy enemy = gameObject.GetComponent<Enemy>();
		EnemyManager.Instance.RemoveEnemy(enemy);

        // destroy the indicators so they don't show up
		Destroy(enemy);
		// disable all the associated meshrenderers while we wait for object to be GC'd
		MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach(MeshRenderer renderer in renderers)
		{
			renderer.enabled = false;
		}
		// disable collisider
		gameObject.GetComponent<BoxCollider>().enabled = false;
		Destroy(gameObject, 2f);

    }
}
