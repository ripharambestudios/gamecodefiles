using UnityEngine;
using System.Collections;

public class AnimatorScript : MonoBehaviour {
	
	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		//Attempt at making animation depend on mouse placement, will come back to.
		Vector3 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lookDirection.z = 0f;

		var xDiff = Mathf.Abs (this.transform.position.x - lookDirection.x);
		var yDiff = Mathf.Abs (this.transform.position.y - lookDirection.y);



		if (xDiff < yDiff) {
			//up
			if (lookDirection.y > this.transform.position.y) {
				animator.SetInteger ("Direction", 2);
			}
			//down
			else {
				animator.SetInteger ("Direction", 0);
			}
		} 

		else {
			//right
			if (lookDirection.x > this.transform.position.x) {
				animator.SetInteger ("Direction", 1);
			}
			//left
			else {
				animator.SetInteger ("Direction", 3);
			}
		}
			


		if (Input.GetAxis ("Fire1") > 0) {
			animator.SetBool ("Shoot", true);
		} 
		else {
			animator.SetBool ("Shoot", false);
		}

	}
}
