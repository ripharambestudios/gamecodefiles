using UnityEngine;
using System.Collections;

public class SnitchAI : MonoBehaviour {

	private GameObject target;

	private float distanceToTarget;
	private float minTether;
	private bool canMove = true;
	private int healthReGen = -150;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");

		minTether = 17f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (target != null)
		{
			distanceToTarget = Vector2.Distance (this.transform.position, target.transform.position);
			Vector2 tether = new Vector2 (target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y);
			float tetherMagnitude = Mathf.Sqrt ((tether.x * tether.x) + (tether.y * tether.y));

			if (canMove) 
			{
				//motion slightly jerky still
				if (tetherMagnitude < minTether)
				{ 			
					this.GetComponent<Rigidbody2D> ().velocity = -tether * 1.25f; 
				} 
				else 
				{		
					this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
				}
			}
		}
	}
		
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player") 
		{
			col.gameObject.SendMessage ("EnemyDamage", healthReGen, SendMessageOptions.DontRequireReceiver);
			Destroy (this.gameObject);
		}
	}
}