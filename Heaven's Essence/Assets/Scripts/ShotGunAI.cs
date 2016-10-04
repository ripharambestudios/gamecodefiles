using UnityEngine;
using System.Collections;

public class ShotGunAI : MonoBehaviour {

	public float sightRadius = 8f;
	public float damage = 20f;
	public float waitTime = 0.5f;
	public float inverseLaunchSpeed = 10f;
	Vector2 changes = new Vector2 (5, 5);
	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;

	private float attackDistance = 8f;

	public GameObject createProjectile;
	public GameObject attackType;

	public float attackTime = 2f;

	// Use this for initialization
	void Start () {
		isAttacking = false;
		target = GameObject.FindWithTag ("Player");
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
		Vector3 look = endLocation - nextPosition;
		look.x += 3;
		look.y += 3;

		float timer = attackTime;

		int maxDistance = 10000;
		int layerDepth = 1;
		int layerMask = layerDepth << 8; //player on 8th layer
		RaycastHit2D impact = Physics2D.Raycast(nextPosition, endLocation, maxDistance, layerMask);
		Debug.Log (impact.point + "TARGETING PLAYER SHOTGUN");
		bool hasHit = false;
		//this.transform.LookAt(target);
		yield return new WaitForSeconds (waitTime);
		//yield return new WaitForSeconds (waitTime);
		while (distanceToTarget <= sightRadius)
		{
			timer += Time.deltaTime;
			Vector2 velocity = new Vector2 ((transform.position.x - target.transform.position.x - 5) * inverseLaunchSpeed, (transform.position.y - target.transform.position.y - 5) * inverseLaunchSpeed);
			GetComponent<Rigidbody2D> ().velocity = -velocity;
			if (timer >= attackTime && distanceToTarget <= attackDistance)
			{
				Instantiate(attackType, transform.position, Quaternion.identity);
				timer = 0f;
			}
			yield return null;

		}
		isAttacking = false;
	}    
}
