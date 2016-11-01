using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

	public GameObject player;
	public GameObject enemyType;
	public GameObject enemyType2;
	public GameObject enemyType3;
	public GameObject enemyType4;
	public float waveTime = 6f;
	
	public Text waveText;
	public Text enemiesLeftText;

	private System.Random randNum;
	static private int numberOfEnemies;
	private List<char> enemyPossibilities = new List<char>() { 's', 's', 's', 's', 'b', 'b', 'g', 'f', 'f', 'f' }; //'s', 's', 's', 's', 'b', 'b', 'b', 'g', 'g', 'f'
    static private int waveNum;
    private int healthReturned = -150;
    private int numberOfEnemiesPerWave = 4;


    // Use this for initialization
    void Start () {
		
		//A repeating call to the spawning method, at a rate decided by spawn time
		//Invoke("Spawn", waveTime);
		numberOfEnemies = 0;
		randNum = new System.Random ();
		randNum.Next ();
		waveNum = 1;
		enemiesLeftText.text = "Enemies Remaining: " + numberOfEnemies.ToString ();
		waveText.text = "";
        numberOfEnemiesPerWave = (int)(8 * Math.Log(waveNum, Math.E) + 5);
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
		enemiesLeftText.text = "Enemies Remaining: " + numberOfEnemies.ToString();
        waveText.enabled = true;
		waveText.text = "Wave " + waveNum;
		yield return new WaitForSeconds (waveTime);
		for (int i = 0; i < numberOfEnemiesPerWave; i++) {
			typeOfEnemy = randNum.Next (0,9);
			Spawn (typeOfEnemy);

			//yield return null;
		}
        waveText.enabled = false;
        //waveText.text = "";
        numberOfEnemiesPerWave = (int)(8 * Math.Log(waveNum, Math.E) + 5); //increases number of enemies quickly initially and then slows down as it gets further
		waveNum++;
        player.SendMessage("EnemyDamage", healthReturned, SendMessageOptions.DontRequireReceiver); //returns a set amount of health to the player to give a little amount of longevity to the player's life
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
		}
		//bomb guy
		else if (enemyChar == 'b') {
			Instantiate (enemyType2, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		}
		//spooky guy
		else if (enemyChar == 'g') {
			Instantiate (enemyType3, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		}
		//fallen guy
		else if (enemyChar == 'f') {
			Instantiate (enemyType4, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		}
		
	}

	public void decrementNumOfEnemies(){
		numberOfEnemies--;
		enemiesLeftText.text = "Enemies Remaining: " + numberOfEnemies.ToString();
							//this.GetComponent<TotalEnemies>().decrementNumEnemies ();
	}
}
