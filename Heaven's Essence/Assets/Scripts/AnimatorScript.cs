using UnityEngine;
using System.Collections;

public class AnimatorScript : MonoBehaviour {
	
	private Animator animator;
	private Vector2 look;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * Attempt at making animation depend on mouse placement, will come back to.
		Vector3 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lookDirection.z = 0f;
		look = lookDirection - this.transform.position;
		float angle = Vector2.Angle (lookDirection, this.transform.position);
		var vertical = lookDirection.y;
		var horizontal = lookDirection.x;
		Debug.Log ("Vertical : " + look.y);
		Debug.Log ("Horizontal: " + look.x);
		Debug.Log ("Angle is: " + angle);
		*/

		var vertical = Input.GetAxis ("Vertical");
		var horizontal = Input.GetAxis ("Horizontal");

		//up
		if (vertical > 0) {
			animator.SetInteger ("Direction", 2);
		}
		//down
		else if (vertical < 0) {
			animator.SetInteger ("Direction", 0);
		}
		//right
		else if (horizontal > 0) {
			animator.SetInteger ("Direction", 1);
		}
		//left
		else if (horizontal < 0) {
			animator.SetInteger ("Direction", 3);
		}

		if (Input.GetAxis ("Fire1") > 0) {
			animator.SetBool ("Shoot", true);
		} 
		else {
			animator.SetBool ("Shoot", false);
		}
	}
}
