
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 50f;
    public GameObject FDT;
    public void TakeDamage (float amount)
    {
        var text = Instantiate(FDT);
        var position = gameObject.transform.position;

        text.gameObject.transform.position = new Vector3(position.x, position.y + 1f, position.z);
        //GameObject FDTgo = Instantiate(FDT, new Vector3(0, 2, 0), Quaternion.identity);
        health -= amount;
        Destroy(text);
        if (health <= 0f)
        {
            Die();

        }

    }

    void Die ()

    {
        Destroy(gameObject);
    }
}
