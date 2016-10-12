using UnityEngine;
using System.Collections;

public class EnemyAnimationScript : MonoBehaviour {

    public bool isAttacking { get; set; }

	private Animator animator;
	private GameObject player;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
        isAttacking = false;
	}

	// Update is called once per frame
	void Update () {
		//Attempt at making animation depend on mouse placement, will come back to.
		Vector3 lookDirection = player.transform.position;
		lookDirection.z = 0f;

		var xDiff = Mathf.Abs (this.transform.position.x - lookDirection.x);
		var yDiff = Mathf.Abs (this.transform.position.y - lookDirection.y);


		if (xDiff < yDiff) {
			//up
			if (lookDirection.y > this.transform.position.y) {
				animator.SetInteger ("Direction", 2);
				fire (2);
				//this.gameObject.transform.GetChild (0).transform.position = new Vector3 (.5f, 0, 0) + this.gameObject.transform.position;
			}
			//down
			else {
				animator.SetInteger ("Direction", 0);
				fire (0);
				//this.gameObject.transform.GetChild (0).transform.position = new Vector3 (-.5f, 0, 0) + this.gameObject.transform.position;
			}

			//this.gameObject.transform.GetChild (0).GetComponent<BoxCollider2D> ().enabled = false;
			//this.gameObject.GetComponentInParent<BoxCollider2D> ().offset = new Vector2 (0, 0);

		} 

		else {

			//this.gameObject.transform.GetChild (0).GetComponent<BoxCollider2D> ().enabled = true;

			//right
			if (lookDirection.x > this.transform.position.x) {
				animator.SetInteger ("Direction", 1);
				fire (1);
				//this.gameObject.transform.GetChild (0).transform.position = new Vector3 (1.25f, .5f, 0) + this.gameObject.transform.position;       // this should be (.25, .1, 0) but scalling makes it need to be this
				//this.gameObject.GetComponentInParent<BoxCollider2D> ().offset = new Vector2 (-.5f, 0);

			}
			//left
			else {
				animator.SetInteger ("Direction", 3);
				fire (3);
				//this.gameObject.transform.GetChild (0).transform.position =  new Vector3 (-1.25f, .5f, 0) + this.gameObject.transform.position;
				//this.gameObject.GetComponentInParent<BoxCollider2D> ().offset = new Vector2 (.5f, 0);
			}
		}


	}

	void fire(int value)
	{
		if (isAttacking) {
			animator.SetInteger ("Direction", value);
			animator.SetBool ("Shoot", true);
		} 
		else {
			animator.SetBool ("Shoot", false);
		}
	}
}
