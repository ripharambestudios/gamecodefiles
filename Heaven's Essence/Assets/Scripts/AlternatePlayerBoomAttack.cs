using UnityEngine;
using System.Collections;

public class AlternatePlayerBoomAttack : MonoBehaviour {
    public GameObject actualExplosion;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Fire2") > 0)
        {
            Instantiate(actualExplosion, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
