using UnityEngine;
using System.Collections;

public class EdgeWrapping : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player") {
			Rigidbody2D rigid = coll.GetComponent<Rigidbody2D> ();
			Debug.Log ("Crossed boundary: name is " + rigid.name);
			if (rigid != null && rigid.tag == "Player") {
				if (rigid.transform.position.y >= 32) {
					rigid.transform.position = new Vector3 (rigid.transform.position.x, rigid.transform.position.y * -1 - 5, 0);
				} else if (rigid.transform.position.y <= -32) {
					rigid.transform.position = new Vector3 (rigid.transform.position.x, rigid.transform.position.y * -1 + 5, 0);
				} else if (rigid.transform.position.x >= 55) {
					rigid.transform.position = new Vector3 (-55, rigid.transform.position.y, 0);
				} else if (rigid.transform.position.x <= -59) {
					rigid.transform.position = new Vector3 (52, rigid.transform.position.y, 0);
				}
			}
		}
	}
}
