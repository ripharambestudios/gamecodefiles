using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {

	public GameObject player;
	public GameObject enemyPool;
	public GameObject enemyPool2;
	public GameObject enemyPool3;
	public GameObject enemyPool4;
	public float waveTime = 10f;
	public float demonicRadius = 216; // if map had box collider use x value times scale to get radius will cover larger y than necessary
	
	public Text waveText;
	public Text enemiesLeftText;

	private System.Random randNum;
	static private int numberOfEnemies;
	private List<char> enemyPossibilities = new List<char>() { 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f', 'f' }; //'s', 's', 's', 's', 'b', 'b', 'b', 'g', 'g', 'f'
    static private int waveNum;
    private int numberOfEnemiesPerWave = 4;

	private bool correctPlacement;

    private GameObject demonicPool;
    

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
        numberOfEnemiesPerWave = (int)(8 * Math.Log(waveNum, Math.E) + 7); // what you add is the first round amount
        demonicPool = GameObject.FindGameObjectWithTag("PoolDemonic");
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
		for (int i = 0; i < numberOfEnemiesPerWave; i++) 
		{
			typeOfEnemy = randNum.Next (0, 9);
			Spawn (typeOfEnemy);
		}
        waveText.enabled = false;
        numberOfEnemiesPerWave = (int)(8 * Math.Log(waveNum, Math.E) + 7); //increases number of enemies quickly initially and then slows down as it gets further
		waveNum++;
	}

	//Code to randomly spawn enemies in the map
	//credit to unity3d.com and the team there as they provided the method to spawn
	void Spawn (int type) {
		char enemyChar = enemyPossibilities [type];
		if (player.GetComponent<MainCharacterController> ().GetHealth () <= 0) 
		{
			return;
		}
			
		Vector3 spawnLocation = new Vector3 ();
		spawnLocation = new Vector3 (randNum.Next (-105, 105), randNum.Next (-55, 55), 0);
		GameObject spawn;
		// demonic sonic
		if (enemyChar == 's')
		{
			spawn = enemyPool.GetComponent<PoolingSystem> ().GetObject ();
			spawn.transform.parent = null;
			spawn.transform.position = spawnLocation;
			spawn.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
			spawnLocation = new Vector3 (randNum.Next (-105, 105), randNum.Next (-55, 55), 0);
		}
			//bomb guy
		else if (enemyChar == 'b')
		{
			spawn = enemyPool2.GetComponent<PoolingSystem> ().GetObject ();
			spawn.transform.position = spawnLocation;
			spawn.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		}
			//fallen guy
		else if (enemyChar == 'f')
		{
			spawn = enemyPool3.GetComponent<PoolingSystem> ().GetObject ();
			spawn.transform.position = spawnLocation;
			spawn.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		}
			//spooky guy
		else if (enemyChar == 'g')
		{
			spawn = enemyPool4.GetComponent<PoolingSystem> ().GetObject ();
			spawn.transform.position = spawnLocation;
			spawn.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
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
