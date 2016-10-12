using UnityEngine;
using System.Collections;

public class ShotGunAI : MonoBehaviour {

	public float sightRadius = 3f;
	public float damage = 20f;
	public float waitTime = 0.5f;
    public float movementSpeed = 100f;
	public float launchSpeed = 2000f;
	Vector2 changes = new Vector2 (5, 5);
	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;

	private float attackDistance = 10f;
    private bool stopped = false;
	public GameObject createProjectile;
	public GameObject attackType;

	public float attackTime = 2f;

    public int rotateSpeed = 3;

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
        distanceToTarget = Vector2.Distance(this.transform.position, target.transform.position);

        if (distanceToTarget <= sightRadius && !isAttacking)
        {
            Debug.Log("Seen");
            isAttacking = true;
            setAttackingAnimation(true);
            StartCoroutine(LaunchAttack());
        }


        //Vector2 velocity = new Vector2((transform.position.x - target.transform.position.x - 5) * inverseLaunchSpeed, (transform.position.y - target.transform.position.y - 5) * inverseLaunchSpeed);
        //GetComponent<Rigidbody2D>().velocity = -velocity;

        if (!stopped)
        {
            transform.LookAt(target.transform.position);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self);

            if (distanceToTarget > 10f)
            {//move if distance from target is greater than 1

                //transform.Translate(new Vector3(inverseLaunchSpeed * Time.deltaTime, 0, 0));
                GetComponent<Rigidbody2D>().AddRelativeForce(transform.right * movementSpeed);

            }
            else
            {

                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0;
            }
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0;
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
		//this.transform.LookAt(target);
		//yield return new WaitForSeconds (waitTime);
		//yield return new WaitForSeconds (waitTime);
		while (distanceToTarget <= sightRadius)
		{
			timer += Time.deltaTime;
            			
			if (timer >= attackTime)
			{
                stopped = true;
                //yield return new WaitForSeconds(0.15f);
				GameObject bullet = (GameObject)Instantiate(attackType, transform.position + 1.0f*transform.right, transform.rotation);
                bullet.GetComponent<Rigidbody2D>().AddForce(bullet.transform.right * launchSpeed);
                if (Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask))
                {

                    impact = Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask);
                    impact.collider.gameObject.SendMessage("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
                    
                }
                yield return new WaitForSeconds(1);
                stopped = false;
				timer = 0f;
			}
			yield return null;

		}
		isAttacking = false;
        setAttackingAnimation(false);

    }

    void setAttackingAnimation(bool status)
    {
        this.GetComponent<EnemyAnimationScript>().isAttacking = status;
    }
}
