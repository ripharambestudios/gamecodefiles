using UnityEngine;
using System.Collections;

public class BigBoomAI : MonoBehaviour {

	public float sightRadius = 800f;
	public float damage = 20f;
	public float waitTime = 0.5f;
	public float inverseLaunchSpeed = 10f;
	public float radius = 1.8f;

	static private int direction = 0;
	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;
    private Animator animator;

	public GameObject attackType;

	public float teleportTime = 2f;

	public int teleDistance = 5;

	private bool weakenedOnce = false;

	private bool correctPlacement = false;
	private bool canAttack = true;

	// Use this for initialization
	void Start () {
		isAttacking = false;
		target = GameObject.FindWithTag ("Player");
        animator = this.GetComponent<Animator>();
        animator.SetInteger("Port", 0);
    }

	// Update is called once per frame
	void Update () 
	{
		if (target != null && canAttack)
		{
			distanceToTarget = Vector2.Distance (this.transform.position, target.transform.position);

			if (distanceToTarget <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth> ().IsBelowTwentyPercent () || weakenedOnce)) 
			{
				isAttacking = true;
				StartCoroutine (LaunchAttack ());
			}
			else if (this.GetComponent<EnemyHealth> ().IsBelowTwentyPercent () && !weakenedOnce) 
			{
				StartCoroutine (WeakenedState ());
			}
		}
	}

	IEnumerator WeakenedState()
	{
		yield return new WaitForSeconds(5);
		weakenedOnce = true;
		yield return null;
	}


	//start method for enemy to launch at player
	IEnumerator LaunchAttack()
	{
		yield return null;
		float timer = teleportTime;
		//yield return new WaitForSeconds (waitTime);
		while ( target != null && transform.position != target.transform.position && distanceToTarget <= sightRadius)
		{
			timer += Time.deltaTime;
 
			if (timer >= teleportTime)
			{
				//Debug.Log ("Teleporting");

				enemyPlacement ();
				//add check if on top of other enemies to move off slightly
				yield return new WaitForSeconds (1f);
                animator.SetInteger("Port", 1);
				Instantiate(attackType, transform.position, Quaternion.identity);
				timer = 0f;
			}
			yield return null;

		}
        animator.SetInteger("Port", 2);
        yield return new WaitForSeconds(.5f);
        animator.SetInteger("Port", 0);
		isAttacking = false;
	}    


	private void enemyPlacement()
	{
		int distanceForPlacement = 1;
		correctPlacement = false;
		while (!correctPlacement)
		{
			this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			// check for colliders when there is a plannet it is landing on
			if (direction == 0) 
			{ // on top
				transform.position = new Vector3 (target.transform.position.x, target.transform.position.y + (teleDistance * distanceForPlacement), 0);
			} 
			else if (direction == 1)
			{ // on right
				transform.position = new Vector3 (target.transform.position.x + (teleDistance * distanceForPlacement), target.transform.position.y, 0);
			} 
			else if (direction == 2)
			{ // on bottom
				transform.position = new Vector3 (target.transform.position.x, target.transform.position.y - (teleDistance * distanceForPlacement), 0);
			} 
			else
			{ //on left
				transform.position = new Vector3 (target.transform.position.x - (teleDistance * distanceForPlacement), target.transform.position.y, 0);
			}

			Collider2D[] collidersPlanets = Physics2D.OverlapCircleAll (this.transform.position, radius, 1 << LayerMask.NameToLayer("Obsticale"));
			if (collidersPlanets.Length == 0) 
			{
				this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				correctPlacement = true;
			}
			else 
			{
				distanceForPlacement++;
			}
		}
			
		if (direction < 3) {
			direction++;
		} 
		else {
			direction = 0;
		}
	}


	public void setCanAttack(bool booleanSent)
	{
		canAttack = booleanSent;
	}
}
