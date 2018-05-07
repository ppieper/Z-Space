using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour {

	public GameObject projectile;
	public GameObject spawn;
	public GameObject player;
	public bool openFire = true;
	public int speed = 1;
	private float nextTimeToFire = 0f;
	public float enemyFireRate = 0.5f;

	// Use this for initialization
	void Start () {
		GameObject firebullet = Instantiate(projectile, transform.position, Quaternion.identity);
		firebullet.GetComponent<Rigidbody>().AddForce(transform.forward * 1000);


	}

	// Update is called once per frame
	void Update () {
		if (Time.time >= nextTimeToFire)
		{
			nextTimeToFire = Time.time + 1f / enemyFireRate;
			Shoot();
		}

	}


	void Shoot()
	{

		transform.position += transform.forward * speed * Time.deltaTime;
	}
}