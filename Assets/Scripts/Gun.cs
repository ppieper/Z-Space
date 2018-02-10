
using UnityEngine;

public class Gun : MonoBehaviour {

    
    public float range = 100f;
    public System.Random rnd = new System.Random();
    public int damageMin = 150;
    public int damageMax = 200;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public GameObject FDT;  //temp code
	// Update is called once per frame
	void Update () {
		
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();

        }

	}


    void Shoot ()
    {
        muzzleFlash.Play();
        RaycastHit hit;
       if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range) )
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null) //if the shot hits
            {

                
    int dmg = rnd.Next(damageMin, damageMax);  //dmg range
                target.TakeDamage(dmg);
                Debug.LogFormat("{0} was dealt {1} damage", target, dmg);
                TextMesh textObject = GameObject.Find("FDT").GetComponent<TextMesh>();
                textObject.text = dmg.ToString();
                GameObject FDTgo = Instantiate(FDT, hit.point, Quaternion.identity); //temp code
                
                textObject.text = " ";
                Destroy(FDTgo, 0.15f);  //temp code
                
                
                //above is creating and destroying the combat text
            }

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 1f);
        }
    }
}
