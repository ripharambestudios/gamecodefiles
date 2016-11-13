using UnityEngine;
using System.Collections;

public class EdgeWrapping : MonoBehaviour {

	public int edgeY = 25;
	public int edgeX = 50;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.tag == "Player")
        {
			Rigidbody2D rigid = coll.GetComponent<Rigidbody2D> ();
			Debug.Log ("Crossed boundary: name is " + rigid.name + "X: " + rigid.transform.position.x + " Y: " + rigid.transform.position.y);
			if (rigid != null && rigid.tag == "Player")
            {
				if (rigid.transform.position.y >= edgeY)
                {
					rigid.transform.position = new Vector3 (rigid.transform.position.x, rigid.transform.position.y * -1, 0);
				}  
				else if (rigid.transform.position.y <= -edgeY)
                {
					rigid.transform.position = new Vector3 (rigid.transform.position.x, rigid.transform.position.y * -1 - 2, 0);
				} 

				if (rigid.transform.position.x >= edgeX)
                {
					rigid.transform.position = new Vector3 (rigid.transform.position.x * -1, rigid.transform.position.y, 0);
				}
                else if (rigid.transform.position.x <= -edgeX)
                {
					rigid.transform.position = new Vector3 (rigid.transform.position.x * -1 - 4, rigid.transform.position.y, 0);
				}
			}
		}
	}
}
