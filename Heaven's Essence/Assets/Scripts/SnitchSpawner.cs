using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class SnitchSpawner : MonoBehaviour {

	public GameObject player;
	public GameObject snitchType;
    public Text waveText;
    private System.Random randNum;
	public float timeOnMap;
    private float timerMap;
	private GameObject snitchSpawned;

    public GameObject ArrowType;

    private GameObject ArrowSpawned;

    public int radius = 5;
	public void startSpawn()
	{
		randNum = new System.Random();
		randNum.Next ();
		
		timeOnMap = 15f;
       
		Spawn ();
	}

	void Update()
	{
		timerMap -= Time.deltaTime;
		if (timerMap <= 0 && gameObject != null)
		{
			Destroy (snitchSpawned);
            Destroy(ArrowSpawned);
            timerMap = timeOnMap;
		}
        if(snitchSpawned == null && ArrowSpawned != null)
        {
            Destroy(ArrowSpawned);
        }
	}

	//Code to randomly spawn enemies in the map
	//credit to unity3d.com and the team there as they provided the method to spawn
	void Spawn ()
	{
        timerMap = timeOnMap;
		if (player.GetComponent<MainCharacterController> ().GetHealth () <= 0) {
			return;
		}

		Vector2 spawnLocation = new Vector2 (player.transform.position.x + randNum.Next (-20, 20), player.transform.position.y + randNum.Next (-20, 20));
        //Vector2 arrowSpawnLocation = player.transform.position + new Vector3(0,0,0);
        //create new enemy and spawn him in somewhere random
        //if(waveText.text == "Wave 1" || waveText.text == "Wave 2")
        //{
            ArrowSpawned = (GameObject)Instantiate(ArrowType, player.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        //}
        
		snitchSpawned = (GameObject)Instantiate (snitchType, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));

	}
}
