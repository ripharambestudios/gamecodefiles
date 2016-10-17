using UnityEngine;
using System.Collections;

public class BigBoomAI : MonoBehaviour {

	public float sightRadius = 800f;
	public float damage = 20f;
	public float waitTime = 0.5f;
	public float inverseLaunchSpeed = 10f;

	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;
    private Animator animator;

	public GameObject attackType;

	public float teleportTime = 2f;

	public int teleDistance = 5;

	private bool weakenedOnce = false;

	// Use this for initialization
	void Start () {
		isAttacking = false;
		target = GameObject.FindWithTag ("Player");
        animator = this.GetComponent<Animator>();
        animator.SetInteger("Port", 0);
    }

	// Update is called once per frame
	void Update () {
		distanceToTarget = Vector2.Distance(this.transform.position, target.transform.position);

		if (distanceToTarget <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() || weakenedOnce)) 
		{
			Debug.Log ("Seen");
			isAttacking = true;
			StartCoroutine(LaunchAttack());
		}
		else if (this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() && !weakenedOnce)
		{
			StartCoroutine(WeakenedState());

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
		while (transform.position != target.transform.position && distanceToTarget <= sightRadius)
		{
			timer += Time.deltaTime;
 
			if (timer >= teleportTime)
			{
				Debug.Log ("Teleporting");
				transform.position = new Vector3(target.transform.position.x - teleDistance, target.transform.position.y, 0);
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
}
