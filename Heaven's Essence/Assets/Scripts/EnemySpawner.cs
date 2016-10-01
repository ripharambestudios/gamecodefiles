using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	public GameObject player;
	public GameObject enemyType;
	public float waveTime = 6f;
	public int numberOfEnemiesPerWave = 3;

	private int numberOfEnemies;

	// Use this for initialization
	void Start () {
		
		//A repeating call to the spawning method, at a rate decided by spawn time
		//Invoke("Spawn", waveTime);
		numberOfEnemies = 0;
	}

	void Update(){
		if (numberOfEnemies == 0) {
			StartCoroutine (BeginSpawn ()); //calls coroutine to allow for a delay between waves
		}
	}

	//Start spawn of the wave
	IEnumerator BeginSpawn(){
		numberOfEnemies += numberOfEnemiesPerWave;
		yield return new WaitForSeconds (waveTime);
		for (int i = 0; i < numberOfEnemiesPerWave; i++) {
			Spawn ();

			//yield return null;
		}
		numberOfEnemiesPerWave += 2;
	}

	//Code to randomly spawn enemies in the map
	//credit to unity3d.com and the team there as they provided the method to spawn
	void Spawn () {
		if (player.GetComponent<MainCharacterController> ().GetHealth () <= 0) {
			return;
		}
		Vector2 spawnLocation = new Vector2(Random.Range(-50,50), Random.Range(-30,30));
		//create new enemy and spawn him in somewhere random
		Instantiate(enemyType, spawnLocation, Quaternion.Euler(new Vector3(0,0,0)));
	}

	public void decrementNumOfEnemies(){
		numberOfEnemies--;
	}
}
