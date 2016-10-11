using UnityEngine;
using System.Collections;

public class ShotGunBullet : MonoBehaviour {

	public float expandSize = 1.33f;
	public int speed = 10;
	private float timer = 0f;
    private float destroyTimer = 0f;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
        destroyTimer += Time.deltaTime;
		if (timer >= 1f) {
			//this.transform.localScale;
            
			timer = 0f;
		}

        if(destroyTimer > 5f)
        {
            Destroy(this.gameObject);
        }
	}
}
