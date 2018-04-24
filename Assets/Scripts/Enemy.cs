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
	[SerializeField]
	private EnemyIndicator enemyIndicators;
	private float maxHealth;

	void Awake() 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		playerHealth = player.GetComponent<PlayerHP>();
		target = gameObject.GetComponent<Target>();

		maxHealth = target.health;

		InitializeIndicator();
		InitializeHealthBar();

		EnemyManager.Instance.AddEnemy(this);
	}

	// Update is called once per frame
	void Update () 
	{
		UpdateHealthBar();
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
											       healthBar.transform.position.y - 35, 
											       healthBar.transform.position.z);
		healthSlider = healthBar.GetComponent<Slider>();
		healthSlider.value = target.health / maxHealth;
		healthBar.SetActive(true);
	}

	private void UpdateHealthBar()
	{
		healthSlider.value = target.health / maxHealth;
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