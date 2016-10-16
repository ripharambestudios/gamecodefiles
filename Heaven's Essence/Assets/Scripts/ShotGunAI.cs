using UnityEngine;
using System.Collections;

public class ShotGunAI : MonoBehaviour {

	private float sightRadius = 10f;
	public float damage = 20f;
	public float waitTime = 0.5f;
    private float movementSpeed = 100f;
	private float launchSpeed = 2000f;
	Vector2 changes = new Vector2 (5, 5);
	private GameObject target;
	private float distanceToTarget;
	private bool isAttacking = false;

	private float attackDistance = 10f;
    private bool stopped = false;
	public GameObject createProjectile;
	public GameObject attackType;

	public float attackTime = 3f;

    public int rotateSpeed = 3;
    private float timeout = 2.5f;
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

        if (distanceToTarget < sightRadius && !isAttacking)
        {
            Debug.Log(distanceToTarget + "Distance to Target ," + sightRadius + " ,Sight Radius");
            isAttacking = true;
            //setAttackingAnimation(true);
            StartCoroutine(LaunchAttack());
        }


        //Vector2 velocity = new Vector2((transform.position.x - target.transform.position.x - 5) * inverseLaunchSpeed, (transform.position.y - target.transform.position.y - 5) * inverseLaunchSpeed);
        //GetComponent<Rigidbody2D>().velocity = -velocity;

        if (!stopped)
        {
            transform.LookAt(target.transform.position);
            transform.Rotate(new Vector3(0, -90, 0), Space.Self);

            if (distanceToTarget >= 10f)
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

		
		int layerDepth = 1;
		int layerMask = layerDepth << 8; //player on 8th layer
	
		
		//this.transform.LookAt(target);
		//yield return new WaitForSeconds (waitTime);
		//yield return new WaitForSeconds (waitTime);
		while (distanceToTarget <= sightRadius)
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
        timeout = 2.5f;
        Debug.Log("done attacking");
        //setAttackingAnimation(false);

    }

    void setAttackingAnimation(bool status)
    {
        this.GetComponent<EnemyAnimationScript>().isAttacking = status;
    }
}
