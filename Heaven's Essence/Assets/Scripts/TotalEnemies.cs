using UnityEngine;
using System.Collections;

public class TotalEnemies : MonoBehaviour {

	private int totalNumEnemies = 0;

	// Use this for initialization
	void Start () {
	
	}
	
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
