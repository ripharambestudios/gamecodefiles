using UnityEngine;
using System.Collections;

public class BigBoomAI : MonoBehaviour {

	public float sightRadius = 800f;
	public float damage = 20f;
	public float waitTime = 0.5f;
	public float inverseLaunchSpeed = 10f;

	public GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;


	public GameObject createProjectile;
	public GameObject attackType;

	public float teleportTime = 2f;

	public int teleDistance = 5;

	// Use this for initialization
	void Start () {
		isAttacking = false;
	}

	// Update is called once per frame
	void Update () {
		distanceToTarget = Vector2.Distance(this.transform.position, target.transform.position);

		if (distanceToTarget <= sightRadius && !isAttacking) 
		{
			Debug.Log ("Seen");
			isAttacking = true;
			StartCoroutine(LaunchAttack());
		}
	}

	//start method for enemy to launch at player
	IEnumerator LaunchAttack()
	{
		yield return null;
		Vector2 endLocation = target.transform.position;
		Vector2 nextPosition = this.transform.position;
		float timer = teleportTime;
		//yield return new WaitForSeconds (waitTime);
		while (transform.position != target.transform.position && distanceToTarget <= sightRadius)
		{
			timer += Time.deltaTime;
 
			if (timer >= teleportTime)
			{
				Debug.Log ("Teleporting");
				transform.position = new Vector3(target.transform.position.x - teleDistance, target.transform.position.y, 0);
				Instantiate(attackType, transform.position, Quaternion.identity);
				timer = 0f;
			}
			yield return null;

		}
		isAttacking = false;
	}    
}
