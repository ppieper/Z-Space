using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	private Transform player;
	private PlayerHP playerHealth;
	private Target target;
	private GameObject indicator;
	private Image image;
	private RectTransform trans;
	private GameObject healthBar;
	private Slider healthSlider;
	private GameObject shieldBar;
	private Slider shieldSlider;
	[SerializeField]
	private EnemyIndicator enemyIndicators;
	private float maxHealth;
	private float maxShield;
	private float rechargeRate;
	public bool hasShield;
	public int bounty;
	public int ammoDrop;

	// called in editor when reset button is pressed, or when script is first attached
	private void Reset() 
	{
		// set the default inspector values for convenience
		enemyIndicators = FindObjectOfType<EnemyIndicator>();
	}    

	void Awake() 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = player.GetComponent<PlayerHP>();
		target = gameObject.GetComponent<Target>();

		maxHealth = target.health;
		maxShield = target.shield;
		rechargeRate = maxShield/1000f;

		InitializeIndicator();
		InitializeHealthBar();
		if(hasShield)
			InitializeShieldBar();

		EnemyManager.Instance.AddEnemy(this);
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateHealthBar();
		if(hasShield && !GameManager.Instance.isPaused)
			UpdateShieldBar();
	}
		
	// make sure we destroy the indicator too
	void OnDestroy()
	{
		Destroy(indicator);
	}

	// initialize indicator
	private void InitializeIndicator()
	{
		indicator = new GameObject("Indicator");
		indicator.transform.SetParent(enemyIndicators.canvas.transform);
		trans = indicator.AddComponent<RectTransform>();
		image = indicator.AddComponent<Image>();
		image.color = enemyIndicators.color;
	}

	// initialize healthbar portion of indicator
	private void InitializeHealthBar()
	{
		healthBar = Instantiate(enemyIndicators.HPBar);
		healthBar.transform.SetParent(indicator.transform);
		healthBar.transform.position = new Vector3(healthBar.transform.position.x, 
			                                       healthBar.transform.position.y - (hasShield ? 29 : 35),
											       healthBar.transform.position.z);
		healthSlider = healthBar.GetComponent<Slider>();
		healthSlider.value = target.health / maxHealth;
		healthBar.SetActive(true);
	}

	// initialize shieldbar portion of indicator
	private void InitializeShieldBar()
	{
		shieldBar = Instantiate(enemyIndicators.SBar);
		shieldBar.transform.SetParent(indicator.transform);
		shieldBar.transform.position = new Vector3(shieldBar.transform.position.x, 
		                                           shieldBar.transform.position.y - 35, 
		                                           shieldBar.transform.position.z);
		shieldSlider = shieldBar.GetComponent<Slider>();
		shieldSlider.value = target.shield / maxShield;
		shieldBar.SetActive(true);
	}

	private void UpdateHealthBar()
	{
		healthSlider.value = target.health / maxHealth;
	}

	private void UpdateShieldBar()
	{
		// slowly recharge shield
		if (target.shield < maxShield)
			target.shield += rechargeRate;
		shieldSlider.value = target.shield / maxShield;

	}

	public GameObject GetIndicator()
	{
		return indicator;
	}

	public RectTransform GetIndicatorTransform()
	{
		return trans;
	}

	public Image GetIndicatorImage()
	{
		return image;
	}

	public GameObject GetHealthBar()
	{
		return healthBar;
	}

	public GameObject GetShieldBar()
	{
		return shieldBar;
	}

	// are the enemy and player in firing range of one another?
	public bool InFireRange()
	{
		return Vector3.Distance(gameObject.transform.position, player.position) <= Constants.weaponRange;
	}

	// is the enemy in detection range of the player?
	public bool InDetectionRange()
	{
		return Vector3.Distance(gameObject.transform.position, player.position) <= Constants.detectionRange;
	}

}