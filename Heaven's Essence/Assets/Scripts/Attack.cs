using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	public GameObject weaponPosition;
	public GameObject attackSpawn;
	public GameObject projectile;
	public GameObject attackType;

	//public float damage = 10;
	public float projectileSpeed = 10f;
	public float rateOfFire = 50.0f;

	private float _rateOfFire;
	private Vector2 attackAngle = Vector2.zero;
	private Vector2 aimLocation = Vector2.zero;
	private bool canAttack = true;
	//public bool isEnemyShot = false;

	// Use this for initialization
	void Start () {
		_rateOfFire = 1 / rateOfFire;
	}

	public void Aim(Vector2 aimTarget){
		aimLocation = aimTarget;
		attackSpawn.transform.position = weaponPosition.transform.position;
		attackSpawn.transform.LookAt (aimLocation);
		attackAngle = attackSpawn.transform.forward;

	}

	public void Fire(){
		if (canAttack) {
			canAttack = false;
			StartCoroutine (fireProjectile ((Vector2)attackSpawn.transform.position, attackAngle, projectileSpeed));
			StartCoroutine (Cooldown (_rateOfFire));
		}
	}

	IEnumerator fireProjectile(Vector2 start, Vector2 next, float attackSpeed){
		yield return null;
		//destroy object if it doesn't collide with anything after timeout amout of time
		float timeout = 3f;

		GameObject createProjectile = (GameObject)Instantiate (projectile, start, Quaternion.Euler (new Vector3(0,0,0))); //make it kinda work: Euler (new Vector3(0,0,0))
		Vector2 nextPosition = start;
		bool hit = false;
		while (timeout > 0f && !hit) {
			timeout -= Time.deltaTime;
			nextPosition += next * attackSpeed;

			RaycastHit2D impact;
			int layerDepth = 1;
			int layerMask = layerDepth << 9; //enemies on 9th layer
			if (Physics2D.Linecast (createProjectile.transform.position, nextPosition, layerMask)) {
				impact = Physics2D.Linecast (createProjectile.transform.position, nextPosition, layerMask);
				Instantiate (attackType, impact.point, Quaternion.identity);
				hit = true;
			}

			createProjectile.transform.position = nextPosition;
			yield return null;
		}
		Destroy (createProjectile);
	}


	//method for checking weapon cooldown on seperate thread
	IEnumerator Cooldown(float cooldown){
		float timer = 0;
		while (timer < cooldown) {
			timer += Time.deltaTime;
			yield return null;

		}
		canAttack = true;
	}
}
