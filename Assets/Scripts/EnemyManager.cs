using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager {

	private static EnemyManager instance;

	private List<Enemy> enemies = new List<Enemy>();

	public static EnemyManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = new EnemyManager();
			}
			return instance;
		}
	}
	// get the current list of living enemies
	public List<Enemy> GetEnemies()
	{
		return enemies;
	}

	// add enemy to the enemy manager (call on creation)
	public void AddEnemy(Enemy enemy)
	{
		enemies.Add(enemy);
	}

	// remove enemy fromt he enemy manage (call on death)
	public void RemoveEnemy(Enemy enemy)
	{
		enemies.Remove(enemy);
	}
}