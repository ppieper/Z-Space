
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{


	public float range = Constants.weaponRange;
    public System.Random rnd = new System.Random();
    public int damageMin = 1;
    public int damageMax = 2;
    public int powerDamageMin = 15;
    public int powerDamageMax = 20;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlash2;
    public ParticleSystem laser1;
    public ParticleSystem laser2;
    public GameObject impactEffect;
    public GameObject impactEffect2;
    public GameObject FDT;
    public GameObject FDTPower;
    public int totalAmmo = 20;
    public int totalCurrency = 0;
    public Text ammoText;
    public Text currencyText;
    public float fireRate = 15f;
    public float fireRatePower = 1f;
    private float nextTimeToFire = 0f;
    private float nextTimeToFire1 = 0f;

    // Update is called once per frame
    void Update()
    {

		if (Input.GetButton("Fire1") && !GameManager.Instance.isPaused && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();

        }
        if (Input.GetButtonDown("Fire2") && !GameManager.Instance.isPaused && Time.time >= nextTimeToFire1)
        {
            nextTimeToFire1 = Time.time + 1f / fireRatePower;
            ShootPowerWeapon();

        }
        ammoText.text = totalAmmo.ToString();
		currencyText.text = totalCurrency.ToString();
    }


    void Shoot()
    {
            RaycastHit hit;
            muzzleFlash.Play();
            muzzleFlash2.Play();
            laser1.Play();
            laser2.Play();
            //totalAmmo = (totalAmmo - 1);

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                //currencyText.text = "Test " + totalCurrency.ToString();




                //muzzleFlash.Play();
                Debug.Log(hit.transform.name);

                Target target = hit.transform.GetComponentInParent<Target>();


                //

                if (target != null) //if the shot hits
                {

                    //AmmoIncreaseDrop();
                    int dmg = rnd.Next(damageMin, damageMax);  //dmg range
                    target.TakeDamage(dmg);
                    Debug.LogFormat("{0} was dealt {1} damage", target, dmg);
                    TextMesh textObject = GameObject.Find("FDT").GetComponentInParent<TextMesh>();
                    textObject.text = dmg.ToString();
                    GameObject FDTgo = Instantiate(FDT, hit.point, Quaternion.identity); //temp code

                    textObject.text = " ";

                    Destroy(FDTgo, 0.15f);//temp code

                    //above is creating and destroying the combat text

					if (target.health <= 0)
					{
						Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
						if (enemy) 
						{
							AmmoIncreaseDrop(enemy);
							totalCurrency = (totalCurrency + enemy.bounty);
						}
					}
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
            }
    }

    void ShootPowerWeapon()
    {
        if (totalAmmo > 0)
        {
            RaycastHit hit;
            muzzleFlash.Play();
            muzzleFlash2.Play();
            totalAmmo = (totalAmmo - 1);

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                //currencyText.text = "Test " + totalCurrency.ToString();




                //muzzleFlash.Play();
                Debug.Log(hit.transform.name);

                Target target = hit.transform.GetComponentInParent<Target>();


                //

                if (target != null) //if the shot hits
                {

                    //AmmoIncreaseDrop();
                    int dmg = rnd.Next(powerDamageMin, powerDamageMax);  //dmg range
                    target.TakeDamage(dmg);
                    Debug.LogFormat("{0} was dealt {1} damage", target, dmg);
                    TextMesh textObject = GameObject.Find("FDTPower").GetComponentInParent<TextMesh>();
                    textObject.text = dmg.ToString();
                    GameObject FDTgo = Instantiate(FDTPower, hit.point, Quaternion.identity); //temp code

                    textObject.text = " ";

                    Destroy(FDTgo, 0.30f);//temp code

                    //above is creating and destroying the combat text

                    if (target.health <= 0)
                    {
						Enemy enemy = hit.transform.GetComponentInParent<Enemy>();
						if (enemy) 
						{
							AmmoIncreaseDrop(enemy);
							totalCurrency = (totalCurrency + enemy.bounty);
						}
                    }
                }

                GameObject impactGO = Instantiate(impactEffect2, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
            }
        }
    }

    public void AmmoIncreaseDrop(Enemy enemy)
    {
		if(enemy)
			totalAmmo = (totalAmmo + enemy.ammoDrop);
    }
}
