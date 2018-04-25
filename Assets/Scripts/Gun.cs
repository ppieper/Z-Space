
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{


    public float range = 100f;
    public System.Random rnd = new System.Random();
    public int damageMin = 150;
    public int damageMax = 200;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlash2;
    public GameObject impactEffect;
    public GameObject FDT;  //temp code
    public int totalAmmo = 20;
    public int totalCurrency = 0;
    public Text ammoText;
    public Text currencyText;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();

        }
        ammoText.text = "Ammo: " + totalAmmo.ToString();
        currencyText.text = "Currency: " + totalCurrency.ToString();
    }


    void Shoot()
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
                    int dmg = rnd.Next(damageMin, damageMax);  //dmg range
                    target.TakeDamage(dmg);
                    Debug.LogFormat("{0} was dealt {1} damage", target, dmg);
                    TextMesh textObject = GameObject.Find("FDT").GetComponentInParent<TextMesh>();
                    textObject.text = dmg.ToString();
                    GameObject FDTgo = Instantiate(FDT, hit.point, Quaternion.identity); //temp code

                    textObject.text = " ";



                    Destroy(FDTgo, 0.15f);//temp code


                    //above is creating and destroying the combat text
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 1f);
                if (target.health < 0)
                {
                    AmmoIncreaseDrop();
                    totalCurrency = (totalCurrency + 10);
                }


            }
        }
    }

    public void AmmoIncreaseDrop()
    {
        totalAmmo = (totalAmmo + 11);
    }
}
