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
	public float waveTime = 10f;
	public float demonicRadius = 216; // if map had box collider use x value times scale to get radius will cover larger y than necessary
	
	public Text waveText;
	public Text enemiesLeftText;

	private System.Random randNum;
	static private int numberOfEnemies;
	private List<char> enemyPossibilities = new List<char>() { 's', 's', 's', 's', 'b', 'b', 'b', 'g', 'g', 'f' }; //'s', 's', 's', 's', 'b', 'b', 'b', 'g', 'g', 'f'
    static private int waveNum;
    private int numberOfEnemiesPerWave = 4;

	private bool correctPlacement;
    

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
        numberOfEnemiesPerWave = (int)(8 * Math.Log(waveNum, Math.E) + 45);
        
    }

	void Update(){
		if (numberOfEnemies == 0) 
		{
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
	}

	//Code to randomly spawn enemies in the map
	//credit to unity3d.com and the team there as they provided the method to spawn
	void Spawn (int type) {
		char enemyChar = enemyPossibilities [type];
		if (player.GetComponent<MainCharacterController> ().GetHealth () <= 0) {
			return;
		}

		correctPlacement = false;
		Vector2 spawnLocation = new Vector2 ();
		while (!correctPlacement) 
		{
			spawnLocation = new Vector2 (randNum.Next (-105, 105), randNum.Next (-55, 55));
		
			//create new enemy and spawn him in somewhere random
			if (enemyChar == 's') 
			{		
				GameObject newDemonic = (GameObject)Instantiate (enemyType, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
				Collider2D[] collidersPlanets = Physics2D.OverlapCircleAll (newDemonic.transform.position, demonicRadius, 1 << LayerMask.NameToLayer ("Obsticale"));
				if (collidersPlanets.Length == 0) 
				{
					correctPlacement = true;
				}
				else 
				{
					Destroy (newDemonic);
				}
			}
			//bomb guy
			else if (enemyChar == 'b') 
			{
				Instantiate (enemyType2, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
				correctPlacement = true;
			}
			//spooky guy
			else if (enemyChar == 'g')
			{
				Instantiate (enemyType3, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
				correctPlacement = true;
			}
			//fallen guy
			else if (enemyChar == 'f') 
			{
				Instantiate (enemyType4, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
				correctPlacement = true;
			}
		}
		
	}

	public void decrementNumOfEnemies(){
		numberOfEnemies--;
		enemiesLeftText.text = "Enemies Remaining: " + numberOfEnemies.ToString();
		if (numberOfEnemies == 0) 
		{
			this.GetComponent<SnitchSpawner> ().startSpawn ();

		}

	}
}
