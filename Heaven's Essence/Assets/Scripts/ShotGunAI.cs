﻿using UnityEngine;
using System.Collections;

public class ShotGunAI : MonoBehaviour {

	private float sightRadius = 10f;
	public float damage = 20f;
	public float waitTime = 0.5f;
    private float movementSpeed = 100f;
	private float launchSpeed = 1000f;
	private GameObject target;
	public float distanceToTarget;
	public bool isAttacking = false;
	private bool weakenedOnce = false;

    private bool stopped = false;
	public GameObject createProjectile;
	public GameObject attackType;

	public float attackTime = 3f;

    public int rotateSpeed = 3;

    private bool attacked = false;
	// Use this for initialization
	void Start () {
		isAttacking = false;
		target = GameObject.FindWithTag ("Player");
        transform.LookAt(target.transform.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);
    }

    // Update is called once per frame
    void Update()
    {
        distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

		if (distanceToTarget <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() || weakenedOnce))
        {
            Debug.Log(distanceToTarget + "Distance to Target ," + sightRadius + " ,Sight Radius");
            isAttacking = true;
            //setAttackingAnimation(true);
            StartCoroutine(LaunchAttack());
        }
		else if (this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() && !weakenedOnce)
		{
            stopped = true;
			StartCoroutine(WeakenedState());

		}


        //Vector2 velocity = new Vector2((transform.position.x - target.transform.position.x - 5) * inverseLaunchSpeed, (transform.position.y - target.transform.position.y - 5) * inverseLaunchSpeed);
        //GetComponent<Rigidbody2D>().velocity = -velocity;

        if (!stopped)
        {
            Vector3 dir = target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (distanceToTarget >= sightRadius - 1.0f)
            {
                Debug.Log("Forward");
                transform.position += transform.right * Time.deltaTime * movementSpeed;
            }
            else
            {
                Debug.Log("Rotate");
                //transform.position += transform.right * Time.deltaTime * speed;
                transform.RotateAround(target.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime * 500);
            }
        }
        }

	IEnumerator WeakenedState()
	{
		yield return new WaitForSeconds(5);
		weakenedOnce = true;
        stopped = false;
		yield return null;
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

		//this.transform.LookAt(target);
		//yield return new WaitForSeconds (waitTime);
		//yield return new WaitForSeconds (waitTime);
		while (distanceToTarget <= sightRadius && !attacked)
		{
			timer += Time.deltaTime;
            if (timer >= attackTime && !attacked)
			{
                attacked = true;
                createProjectile = (GameObject)Instantiate(attackType, transform.position + 1.0f * transform.right, transform.rotation);
                createProjectile.GetComponent<Rigidbody2D>().AddForce(createProjectile.transform.right * launchSpeed);
                stopped = true;
                Debug.Log("attacked");
                yield return new WaitForSeconds(1);
                stopped = false;
				timer = 0f;
			}
			yield return null;

		}
		isAttacking = false;
        attacked = false;

        Debug.Log("done attacking");
        //setAttackingAnimation(false);

    }

    void setAttackingAnimation(bool status)
    {
        this.GetComponent<EnemyAnimationScript>().isAttacking = status;
    }
}
