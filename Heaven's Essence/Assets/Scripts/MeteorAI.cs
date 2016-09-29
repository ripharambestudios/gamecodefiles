using UnityEngine;
using System.Collections;
using System;

public class MeteorAI : MonoBehaviour {

	public float sightRadius = 40f; //max distance can be from player and still in view
	public float damage = 20f;
	public float waitTime =0.5f;
	public float inverseTravelSpeed = 10f; //bigger number means slower enemy
	public float attackCooldown = 1f;

	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;
	private float launchSpeed;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");  //may need to tweak this
		launchSpeed = 1 / inverseTravelSpeed;

	}
	
	// Update is called once per frame
	void Update () {
		distanceToTarget = Vector2.Distance (this.transform.position, target.transform.position);

		if(distanceToTarget <= sightRadius && !isAttacking){
			isAttacking = true;
			StartCoroutine (LaunchAttack (distanceToTarget));

		}
	}

	//start method for enemy to launch at player
	IEnumerator LaunchAttack(float distance)
	{
		//yield return null;
		Vector2 endLocation = target.transform.position;
		Vector2 nextPosition = this.transform.position;
		Vector2 look = endLocation - nextPosition;
		float distanceCovered = 0;
		int maxDistance = 10000;
		int layerDepth = 1;
		int layerMask = layerDepth << 8; //player on 8th layer
		RaycastHit2D impact = Physics2D.Raycast(nextPosition, endLocation, maxDistance, layerMask);
		Debug.Log (impact.point + "TARGETING PLAYER");
		yield return new WaitForSeconds (waitTime);
		bool hasHit = false;
		float distanceToGo = Math.Abs (distance) * 1.95f;
		if (distanceToGo > 50f) {
			distanceToGo = 50f;
		}
		while(distanceCovered < distanceToGo){
			nextPosition += look * launchSpeed;
			distanceCovered += Math.Abs (Vector2.Distance (this.transform.position, nextPosition));
			if (Physics2D.Linecast (this.transform.position, nextPosition, layerMask) && !hasHit) {
				impact = Physics2D.Linecast (this.transform.position, nextPosition, layerMask);
				impact.collider.gameObject.SendMessage ("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
				hasHit = true;
				Debug.Log ("Player hit.");
			}

			this.transform.position = nextPosition;
			yield return null;
		}
		yield return new WaitForSeconds (attackCooldown);
		isAttacking = false;
	}
}
