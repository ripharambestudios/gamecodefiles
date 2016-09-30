﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
	public float trackingSpeed = 5;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame at the end of the update
	void LateUpdate () {
		//change camera speed at a rate between the position of the camera and its target, with a multiplier of time last called and tracking speed
		//delta time is a small number
		this.transform.position = Vector2.Lerp (this.transform.position, target.transform.position, Time.deltaTime*trackingSpeed);
	}
}