using UnityEngine;
using System.Collections;
using System;

public class MeteorAI : MonoBehaviour {

	public float sightRadius = 60f; //max distance can be from player and still in view
	public float damage = 20f;
	public float waitTime =0.25f;
	public float attackCooldown = .25f;
	public float knockBackDistance;

	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;
	private float launchSpeed = 1f;
    private bool track = true;
    private bool weakenedOnce = false;
	private bool canAttack;

	// Use this for initialization
	void Start () {
		knockBackDistance = 2;
		target = GameObject.FindGameObjectWithTag ("Player");  //may need to tweak this
		canAttack = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (target.gameObject != null && canAttack)
        {
            distanceToTarget = Vector2.Distance(this.transform.position, target.transform.position);
            if (track)
            {
                Vector2 endLocation = target.transform.position;
                Vector2 nextPosition = this.transform.position;
                Vector2 look = endLocation - nextPosition;

                //THIS SHOULD ALL BE PUT INTO A HELPER METHOD FOR ROTATION
                //get the sign of the direction of the aim
                float signOfLook = 1;
                if (this.transform.position.y > target.transform.position.y)
                {
                    signOfLook = Mathf.Sign(look.y); //this will be negative if the player is below demonic sonic, rotating him appropriately
                }
                float angle = Vector3.Angle(Vector3.right, new Vector3(look.x, look.y, 0));
                angle *= signOfLook;
                //rotate demonic sonic
                this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            //should only attack if in range, isn't attacking, health isn't below ten percent health, or if it is it has already entered its weakened state and can attack again
            if (distanceToTarget <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() || weakenedOnce))
            {
                isAttacking = true;
                StartCoroutine(LaunchAttack(distanceToTarget));

            }
            else if (this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() && !weakenedOnce)
            {
                StartCoroutine(WeakenedState());
                
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
	IEnumerator LaunchAttack(float distance)
	{
		//yield return null;
		Vector2 endLocation = target.transform.position;
		Vector2 nextPosition = this.transform.position;
        Vector2 look = endLocation - nextPosition;
        track = false;
        float distanceCovered = 0;
		int maxDistance = 10000;
		int layerDepth = 1;
		int layerMask = layerDepth << 8; //player on 8th layer
        int obsticalMask = layerDepth << 12; //obsticale on 12th layer
		RaycastHit2D impact = Physics2D.Raycast(nextPosition, endLocation, maxDistance, layerMask);
        RaycastHit2D impactObsticale; // = Physics2D.Raycast(nextPosition, endLocation, maxDistance, obsticalMask);
		//Debug.Log (impact.point + "TARGETING PLAYER");
		yield return new WaitForSeconds (waitTime);
        
		bool hasHit = false;
		float distanceToGo = Math.Abs (distance) * 2f;
		if (distanceToGo > 50f) {
			distanceToGo = 50f;
		}
		while(distanceCovered < distanceToGo && canAttack)
		{
			nextPosition += look.normalized * launchSpeed; //try time.detlatime to see if that can make it better
			distanceCovered += Math.Abs (Vector2.Distance (this.transform.position, nextPosition));
			if (Physics2D.Linecast (this.transform.position, nextPosition, layerMask) && !hasHit) {
				impact = Physics2D.Linecast (this.transform.position, nextPosition, layerMask);
				impact.collider.gameObject.SendMessage ("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
				hasHit = true;
				//Debug.Log ("Player hit.");
			}

			if (Physics2D.Linecast (this.transform.position, nextPosition, obsticalMask)) // if it his an obsticale it stops moving
			{
				impactObsticale = Physics2D.Linecast (this.transform.position, nextPosition, obsticalMask);
				distanceCovered = distanceToGo;
				nextPosition = this.transform.position;
			}

			this.transform.position = nextPosition;
			yield return null;
		}
        track = true;
		yield return new WaitForSeconds (attackCooldown);
		isAttacking = false;
	}

	public void setCanAttack(bool booleanSent)
	{
		canAttack = booleanSent;
	}
		
	public void startKnockBack(float degree)
	{
		StartCoroutine (BounceOff(degree, 1f));
	}

	public void setKnockBackAmount(int distance)
	{
		knockBackDistance = distance;
	}


	IEnumerator BounceOff(float degree, float knockBackSpeed)
	{
		//yield return null;
		float numAddX = Mathf.Cos(degree * (Mathf.PI / 180)) * knockBackDistance;
		float numAddY = Mathf.Sin(degree * (Mathf.PI / 180)) * knockBackDistance;
		float endX = numAddX + this.gameObject.transform.position.x;
		float endY = numAddY + this.gameObject.transform.position.y;
		Vector2 endLocation = new Vector2 (endX, endY);
		Vector2 nextPosition = this.gameObject.transform.position;
		Vector2 look = endLocation - nextPosition;
		float distanceCovered = 0;
		int maxDistance = 100;
		int layerDepth = 1;
		int obsticalMask = layerDepth << 12; //obsticale on 12th layer
		RaycastHit2D impactObsticale = Physics2D.Raycast(nextPosition, endLocation, maxDistance, obsticalMask);


		float distanceToGo = knockBackDistance;
		while(distanceCovered < distanceToGo)
		{
			nextPosition += look.normalized * knockBackSpeed;
			distanceCovered += Math.Abs (Vector2.Distance (this.gameObject.transform.position, nextPosition));

			if (Physics2D.Linecast (this.gameObject.transform.position, nextPosition, obsticalMask)) // if it his an obsticale it stops moving
			{
				impactObsticale = Physics2D.Linecast (this.gameObject.transform.position, nextPosition, obsticalMask);
				distanceCovered = distanceToGo;
				nextPosition = this.gameObject.transform.position;
			}

			this.gameObject.transform.position = nextPosition;
			yield return null;
		}
		yield return new WaitForSeconds (.5f); // cooldown
		canAttack = true;
	}
}
