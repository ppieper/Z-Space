
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;
	public float shield = 5f;
    public GameObject FDT;

	void Start()
	{
		// if shields are disabled, make sure we don't start off with any shield
		if (GetComponent<Enemy> () && !GetComponent<Enemy>().hasShield)
			shield = 0;
	}

    public void TakeDamage (float amount)
    {
        var text = Instantiate(FDT);
        var position = gameObject.transform.position;

        text.gameObject.transform.position = new Vector3(position.x, position.y + 1f, position.z);

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
        Destroy(text);

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die ()
    {
		EnemyManager.Instance.RemoveEnemy(gameObject.GetComponent<Enemy>());
        Destroy(gameObject);
    }
}
