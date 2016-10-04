using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject player;
	public GameObject enemyType;
	public GameObject enemyType2;
	public GameObject enemyType3;
	//public GameObject enemyType4;
	public float waveTime = 6f;
	public int numberOfEnemiesPerWave = 4;

	private System.Random randNum;
	private int numberOfEnemies;
	private List<char> enemyPossibilities = new List<char>() {'s', 's', 's', 's', 'b', 'b', 'b', 'g', 'g', 'g'}; //, 'g', 'g', 'f'   for spooky guy and fallen guy

	// Use this for initialization
	void Start () {
		
		//A repeating call to the spawning method, at a rate decided by spawn time
		//Invoke("Spawn", waveTime);
		numberOfEnemies = 0;
		randNum = new System.Random ();
		randNum.Next ();

	}

	void Update(){
		if (numberOfEnemies == 0) { 		//&& this.GetComponent<TotalEnemies>().enemiesGone()
			StartCoroutine (BeginSpawn ()); //calls coroutine to allow for a delay between waves
		}
	}

	//Start spawn of the wave
	IEnumerator BeginSpawn(){
		int typeOfEnemy;
		numberOfEnemies += numberOfEnemiesPerWave;
											//this.GetComponent<TotalEnemies>().incrementNumEnemies (numberOfEnemiesPerWave);
		yield return new WaitForSeconds (waveTime);
		for (int i = 0; i < numberOfEnemiesPerWave; i++) {
			typeOfEnemy = randNum.Next (0,9);
			Spawn (typeOfEnemy);

			//yield return null;
		}
		numberOfEnemiesPerWave += 2;
	}

	//Code to randomly spawn enemies in the map
	//credit to unity3d.com and the team there as they provided the method to spawn
	void Spawn (int type) {
		char enemyChar = enemyPossibilities [type];
		if (player.GetComponent<MainCharacterController> ().GetHealth () <= 0) {
			return;
		}
		Vector2 spawnLocation = new Vector2(randNum.Next(-50,50), randNum.Next(-30,30));
		//create new enemy and spawn him in somewhere random
		if (enemyChar == 's') {
			
			Instantiate (enemyType, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		} else if (enemyChar == 'b') {
			Instantiate (enemyType2, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		}

		else if (enemyChar == 'g') {
			Instantiate (enemyType3, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		}
		/*
		else if (enemyChar == 'f') {
			Instantiate (enemyType4`, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		}
		*/
	}

	public void decrementNumOfEnemies(){
		numberOfEnemies--;
							//this.GetComponent<TotalEnemies>().decrementNumEnemies ();
	}
}
