using UnityEngine;
using System.Collections;

public class EnemyAnimationScript : MonoBehaviour {

    public bool isAttacking { get; set; }

	private GameObject attackPoint;
	private GameObject particleSystem;
	private Animator animator;
	private GameObject player;
	// Use this for initialization
	void Start () {
		animator = this.GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");
        isAttacking = false;
		if (this.gameObject.tag == "EnemyFallen" || this.gameObject.tag == "EnemySpook")
		{
			particleSystem = this.gameObject.transform.GetChild (0).gameObject;
		}
		if (this.gameObject.tag == "EnemySpook") 
		{
			attackPoint = this.gameObject.transform.GetChild (1).gameObject;
		}
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
					if (this.gameObject.tag == "EnemySpook") 
					{
						attackPoint.transform.position = new Vector3 (0, 1.25f, 0) + this.gameObject.transform.GetChild (0).transform.position;
					}
					if (this.gameObject.tag == "EnemyFallen" || this.gameObject.tag == "EnemySpook") 
					{
						particleSystem.transform.position = new Vector3 (0, .73f, 0) + this.gameObject.transform.position;
						particleSystem.transform.rotation = Quaternion.Euler (215f, 0, 0);
					}
				}
				//down
				else 
				{
					animator.SetInteger ("Direction", 0);
					fire (0);
					if (this.gameObject.tag == "EnemySpook") 
					{
						attackPoint.transform.position = new Vector3(0, -1f, 0) + this.gameObject.transform.GetChild(0).transform.position;
					}
					if (this.gameObject.tag == "EnemyFallen" || this.gameObject.tag == "EnemySpook") 
					{
						particleSystem.transform.position = new Vector3 (0, .73f, 1) + this.gameObject.transform.position;
						particleSystem.transform.rotation = Quaternion.Euler (215f, 0, 0);
					}
				}
			} 
			else 
			{
				//right
				if (lookDirection.x > this.transform.position.x)
				{
					animator.SetInteger ("Direction", 1);
					fire (1);
					if (this.gameObject.tag == "EnemySpook") 
					{
						attackPoint.transform.position = new Vector3(.5f, 0, 0) + this.gameObject.transform.GetChild(0).transform.position;
					}
					if (this.gameObject.tag == "EnemyFallen" || this.gameObject.tag == "EnemySpook") 
					{
						particleSystem.transform.position = new Vector3 (-.66f, .73f, 0) + this.gameObject.transform.position;
						particleSystem.transform.rotation = Quaternion.Euler (335f, 265f, 0);
					}
				}
				//left
				else 
				{
					animator.SetInteger ("Direction", 3);
					fire (3);
					if (this.gameObject.tag == "EnemySpook") 
					{
						attackPoint.transform.position = new Vector3(-.5f, 0, 0) + this.gameObject.transform.GetChild(0).transform.position;
					}
					if (this.gameObject.tag == "EnemyFallen" || this.gameObject.tag == "EnemySpook") 
					{
						particleSystem.transform.position = new Vector3 (.66f, .73f, 0) + this.gameObject.transform.position;
						particleSystem.transform.rotation = Quaternion.Euler (215f, 265f, 0);
					}
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
