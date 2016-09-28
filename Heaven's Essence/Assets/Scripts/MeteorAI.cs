using UnityEngine;
using System.Collections;

public class MeteorAI : MonoBehaviour {

	public float sightRadius = 8f;
	public float damage = 20f;
	public float waitTime =0.5f;
	private float inverseLaunchSpeed = 10f;

	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;


	// Use this for initialization
	void Start () {
		target = GameObject.Find ("MainCharacter");  //may need to tweak this
	}
	
	// Update is called once per frame
	void Update () {
		distanceToTarget = Vector2.Distance (this.transform.position, target.transform.position);

		if(distanceToTarget <= sightRadius && !isAttacking){
			isAttacking = true;
			StartCoroutine (LaunchAttack ());

		}
	}

	//start method for enemy to launch at player
	IEnumerator LaunchAttack()
	{
		yield return null;
		Vector2 endLocation = target.transform.position;
		Vector2 nextPosition = this.transform.position;
		yield return new WaitForSeconds (waitTime);
		while(nextPosition != endLocation){
			
			RaycastHit2D impact;
			int layerDepth = 1;
			int layerMask = layerDepth << 8; //player on 8th layer
			if (Physics2D.Linecast (this.transform.position, nextPosition, layerMask)) {
				//impact = Physics2D.Linecast (createProjectile.transform.position, nextPosition, layerMask);
				//Instantiate (attackType, impact.point, Quaternion.identity);
				target.GetComponent<Rigidbody2D>().gameObject.SendMessage("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
				Debug.Log ("Player hit.");
			}
			nextPosition += endLocation / inverseLaunchSpeed;
			this.transform.position = nextPosition;
			yield return null;
		}
		isAttacking = false;
	}
}
