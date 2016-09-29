using UnityEngine;
using System.Collections;

public class DistantShooterAI : MonoBehaviour {

	public float sightRadius = 40f; //max distance can be from player and still in view
	public float damage = 20f;
	public float travelSpeed = 10f; //bigger number means slower enemy
	public float attackCooldown = 1f;

	private GameObject target;
	private float flyingSpeed;
	private bool isAttacking = false;
	private float distanceToTarget;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		flyingSpeed = 1 / travelSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		distanceToTarget = Vector2.Distance (this.transform.position, target.transform.position);

		if(distanceToTarget <= sightRadius && !isAttacking){
			isAttacking = true;
			StartCoroutine (DistantAttack (distanceToTarget));

		}
	}

	IEnumerator DistantAttack(float distance){
		yield return null;
	}
}
