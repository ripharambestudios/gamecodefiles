using UnityEngine;
using System.Collections;

public class destroyScript : MonoBehaviour {

	public float destroyTime;
	// Use this for initialization
	void Start () {
		destroyTime = 2;
	}


	// Update is called once per frame
	void Update () {
		destroyTime -= Time.deltaTime;
		if (destroyTime <= 0) {
			Destroy (this.gameObject);
		}
	}
		
}
