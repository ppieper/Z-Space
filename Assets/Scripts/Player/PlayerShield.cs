using UnityEngine;
using UnityEngine.UI;

public class PlayerShield : MonoBehaviour {


	public float maxShield;
	public Slider ShieldBar;
	private float currentShield;
	private float rechargeRate;

	// Use this for initialization
	void Awake () 
	{
		currentShield = maxShield;
		rechargeRate = maxShield/1000f;
		ShieldBar.value = 1.0f;
	}

	// Update is called once per frame
	void Update () 
	{
		// slowly recharge shield
		if (currentShield < maxShield && !GameManager.Instance.isPaused)
			currentShield += rechargeRate;
		ShieldBar.value = currentShield / maxShield;
	}

	public float GetCurrentShield()
	{
		return currentShield;
	}

	// the shield will absorb some damage 
	// returns the amount of damage that did not get absorbed
	public float TakeDamage(float damage)
	{
		float remainingDamage = 0;

		if (currentShield <= damage) {
			remainingDamage = damage - currentShield;
			currentShield = 0;
		} else 
		{
			currentShield -= damage;
		}
		return remainingDamage;
	}
}