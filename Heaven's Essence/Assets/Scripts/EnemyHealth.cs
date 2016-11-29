﻿using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int startHealth = 100;
	public int scoreValue= 0;
	private GameObject enemyManager;
	public AudioClip enemyHitSound;

	private AudioSource source;

	private int currentHealth;
	private GameObject player;

	public GameObject pool;
	private float timeForFlashGold = .5f;
	private float timeForFlashRed = .1f;
	public string poolName;

	// Use this for initialization
	void Start ()
	{
		currentHealth = startHealth;
		enemyManager = GameObject.FindGameObjectWithTag ("Enemy Manager");
		player = GameObject.FindGameObjectWithTag ("Player");
		source = gameObject.AddComponent<AudioSource> ();
		pool = GameObject.FindGameObjectWithTag(poolName);

	}

	// Message sent from player that does damage to enemy
	public void DealDamage (int damage)
	{
		source.PlayOneShot (enemyHitSound, .05f);
		currentHealth -= damage;
		if (damage == 1000000) {
			scoreValue -= (scoreValue / 5);
		}

		if (currentHealth <= 0) {
			enemyManager.GetComponent<EnemySpawner> ().decrementNumOfEnemies ();
			player.SendMessage ("UpdateScore", scoreValue, SendMessageOptions.DontRequireReceiver);
			int numOfObjects = this.transform.childCount;
			//for(int i =0; i < numOfObjects; i++)
			//{
			//    Destroy(this.transform.GetChild(i));
			//}
			//Destroy (this.gameObject);
			pool.GetComponent<PoolingSystem>().returnToPool(gameObject);
		}

		if (currentHealth > 0)
		{
			StartCoroutine (flashRed ());
		}
	}

	public int GetEnemyHealth()
	{
		return currentHealth;
	}

	public bool IsBelowTwentyPercent()
	{
		int aTenth = startHealth / 10;
		if(currentHealth <= aTenth*2)
		{

			timeForFlashGold -= Time.deltaTime;
			if (timeForFlashGold <= 0)
			{
				if (this.gameObject.GetComponent<SpriteRenderer> ().color == new Color (1, 1, 1, 1))
				{
					this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, .925f, 0, 1);
				} 
				else 
				{
					this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				}
				timeForFlashGold = .5f;
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	IEnumerator flashRed()
	{
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, .17647f, .17647f, 1);
		yield return new WaitForSeconds (timeForFlashRed);
		this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
	}
}
