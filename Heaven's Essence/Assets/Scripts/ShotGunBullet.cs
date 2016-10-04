using UnityEngine;
using System.Collections;

public class ShotGunBullet : MonoBehaviour {

	public float expandSize = 1.33f;
	public int speed = 10;
	private float timer = 0f;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= 1f) {
			this.transform.localScale = (this.transform.localScale)*2;
			GetComponent<Rigidbody2D> ().AddForce (transform.up * speed * Time.smoothDeltaTime);
			timer = 0f;
		}
	}
}
