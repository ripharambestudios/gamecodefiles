using UnityEngine;
using System.Collections;

public class DistantShooterAI : MonoBehaviour {

	public float sightRadius = 30f; //max distance can be from player and still in view
	public float damage = 20f;
	public float projectileSpeed = 10f; //bigger number means slower enemy
	public float attackCooldown = 1f;
	public float rateOfFire = 50f;
	public int numberOfProjectiles = 10000;
	public GameObject projectile;
	public GameObject launchPosition;

	private GameObject target;

	private float actualRateOfFire;
	private bool isAttacking = false;
	private float distanceToTarget;
	private float minTether;
	private float maxTether;
	private int numberOfProjectilesLaunched;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");

		minTether = 17f;
		maxTether = 21f;
		if (sightRadius != maxTether) {
			sightRadius = maxTether;
		}
		numberOfProjectilesLaunched = 0;
		actualRateOfFire = 1 / rateOfFire;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		distanceToTarget = Vector2.Distance (this.transform.position, target.transform.position);
		Vector2 tether = new Vector2 (target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y);
		float tetherMagnitude = Mathf.Sqrt ((tether.x * tether.x) + (tether.y * tether.y));
		if(tetherMagnitude <= sightRadius && !isAttacking){
			isAttacking = true;
			StartCoroutine (VolleyOfAttacks (distanceToTarget));
			
		}

		//motion slightly jerky still
		if (tetherMagnitude < minTether) { 
			
			this.GetComponent<Rigidbody2D> ().velocity = -tether ; 
		} else if (tetherMagnitude > maxTether) {
			
			this.GetComponent<Rigidbody2D> ().velocity = tether;
		} else {
			
			this.GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
		}
		/*
		if (distanceToTarget <= sightRadius && (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0)) {
			spookyGuyVector.y = Input.GetAxis ("Vertical");
			spookyGuyVector.x = Input.GetAxis ("Horizontal");
			if (Input.GetAxis ("Vertical") != 0) {
				tempLook.y = spookyGuyVector.y;
			}
			if (Input.GetAxis ("Horizontal") != 0) {
				tempLook.x = spookyGuyVector.x;
			}
			this.transform.Translate (spookyGuyVector.x * travelSpeed * Time.deltaTime, spookyGuyVector.y * travelSpeed * Time.deltaTime, 0);
			
		} else if (distanceToTarget <= sightRadius / 2) {
			Vector2 endLocation = target.transform.position;
			Vector2 nextPosition = this.transform.position;
			Vector2 look = endLocation - nextPosition;
			this.transform.Translate (look.x * travelSpeed * Time.deltaTime * -1, look.y * travelSpeed * Time.deltaTime * -1, 0);
			//this.transform.Translate (tempLook.x * travelSpeed / 2, tempLook.y * travelSpeed / 2, 0);
		}
		else if (distanceToTarget > sightRadius) {
			Vector2 endLocation = target.transform.position;
			Vector2 nextPosition = this.transform.position;
			Vector2 look = endLocation - nextPosition;
			this.transform.Translate (look.x * travelSpeed * Time.deltaTime, look.y * travelSpeed * Time.deltaTime, 0);
		}
		*/
	}

	IEnumerator VolleyOfAttacks(float distance){
		for (int i = 0; i < numberOfProjectiles; i++) {
			yield return new WaitForSeconds (actualRateOfFire);
			StartCoroutine (DistantAttack (distance));
			numberOfProjectilesLaunched += 2;
		}
	}

	IEnumerator DistantAttack(float distance){
		//yield return null;
		//destroy object if it doesn't collide with anything after timeout amout of time
		float timeout = 2.5f;
        setAttackingAnimation(true);
		Vector2 aim = new Vector2 (target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y);
		GameObject createProjectile = (GameObject)Instantiate (projectile, launchPosition.transform.position, Quaternion.Euler (new Vector3(0,0,0))); 
		GameObject createProjectile2 = (GameObject)Instantiate (projectile, launchPosition.transform.position + new Vector3 (0, -1, 0), Quaternion.Euler (new Vector3 (0, 0, 0))); 

			
		createProjectile.transform.parent = this.transform;
		createProjectile2.transform.parent = this.transform;
		Vector2 nextPosition = launchPosition.transform.position;
		Vector2 nextPosition2 = launchPosition.transform.position + new Vector3(0,-1,0);
		bool hit = false;
		while (timeout > 0f && !hit) {
			timeout -= Time.deltaTime;
			nextPosition += aim.normalized * projectileSpeed * Time.fixedDeltaTime;
			nextPosition2 += aim.normalized * projectileSpeed * Time.fixedDeltaTime;
			RaycastHit2D impact;
			RaycastHit2D impact2;
			int layerDepth = 1;
			int layerMask = layerDepth << 8; //enemies on 9th layer
			if (Physics2D.Linecast (createProjectile.transform.position, nextPosition, layerMask)) {
				
				impact = Physics2D.Linecast (createProjectile.transform.position, nextPosition, layerMask);
				impact.collider.gameObject.SendMessage ("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
				hit = true;
			}
			if (Physics2D.Linecast (createProjectile2.transform.position, nextPosition2, layerMask)) {

				impact2 = Physics2D.Linecast (createProjectile2.transform.position, nextPosition2, layerMask);
				impact2.collider.gameObject.SendMessage ("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
				hit = true;
			}

			createProjectile.transform.position = nextPosition;
			createProjectile2.transform.position = nextPosition2;
			yield return null;
		}
		Destroy (createProjectile);
		Destroy (createProjectile2);
		numberOfProjectilesLaunched -= 2;
		//yield return new WaitForSeconds (attackCooldown);
		if (numberOfProjectilesLaunched == 0) {
			isAttacking = false;
            setAttackingAnimation(false);
        }
	}

    void setAttackingAnimation(bool status)
    {
        this.GetComponent<EnemyAnimationScript>().isAttacking = status;
    }
}
