﻿using UnityEngine;
using System.Collections;
using System;

public class SnitchSpawner : MonoBehaviour {

	public GameObject player;
	public GameObject snitchType;

	private System.Random randNum;
	static private int waveNum;
	public float timeOnMap;
	private GameObject snitchSpawned;

	public void startSpawn()
	{
		randNum = new System.Random();
		randNum.Next ();
		//waveNum = 1;
		timeOnMap = 15f;
		Spawn ();
	}

	void Update()
	{
		timeOnMap -= Time.deltaTime;
		if (timeOnMap <= 0 && this.gameObject != null)
		{
			Destroy (snitchSpawned);
		}
	}

	//Code to randomly spawn enemies in the map
	//credit to unity3d.com and the team there as they provided the method to spawn
	void Spawn ()
	{
		if (player.GetComponent<MainCharacterController> ().GetHealth () <= 0) {
			return;
		}

		Vector2 spawnLocation = new Vector2 (player.transform.position.x + randNum.Next (-20, 20), player.transform.position.y + randNum.Next (-20, 20));
		//create new enemy and spawn him in somewhere random

		snitchSpawned = (GameObject)Instantiate (snitchType, spawnLocation, Quaternion.Euler (new Vector3 (0, 0, 0)));
		//waveNum++;
	}
}