using UnityEngine;
using System.Collections;

public class TotalEnemies : MonoBehaviour {

	private int totalNumEnemies = 0;
	private bool waveHasSpawned =false;


	/*
	 * Currently does not work.  
	 * Spawner needs to see when all waves are dead before spawning next wave.
	*/

	
	// Update is called once per frame
	public void incrementNumEnemies(int enemies){
		totalNumEnemies += enemies;
	}

	public void decrementNumEnemies(){
		totalNumEnemies--;
	}

	public bool enemiesGone(){
		if (totalNumEnemies == 0) {
			return true;
		} else {
			return false;
		}
	}
}
