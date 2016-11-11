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

		if (player != null) {
			Vector3 lookDirection = player.transform.position;
			lookDirection.z = 0f;

			var xDiff = Mathf.Abs (this.transform.position.x - lookDirection.x);
			var yDiff = Mathf.Abs (this.transform.position.y - lookDirection.y);


			if (xDiff < yDiff)
			{
				//up
				if (lookDirection.y > this.transform.position.y) 
				{
					animator.SetInteger ("Direction", 2);
					fire (2);
				}
				//down
				else 
				{
					animator.SetInteger ("Direction", 0);
					fire (0);
				}
			} 
			else 
			{
				//right
				if (lookDirection.x > this.transform.position.x)
				{
					animator.SetInteger ("Direction", 1);
					fire (1);
				}
				//left
				else 
				{
					animator.SetInteger ("Direction", 3);
					fire (3);
				}
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
