using UnityEngine;
using System.Collections;
using System;

public class BigBoomAI : MonoBehaviour
{
    public float sightRadius = 800f;
    public float damage = 20f;
    public float waitTime = 0.5f;
    public float inverseLaunchSpeed = 10f;
    public float radius = 1.8f;
    public float bounceSpeed = .5f;
    public float edgeY = 54f;
    public float edgeX = 105f;
    public float knockBackDistance;
    public GameObject attackType;
    public float teleportTime = 2f;
    public int teleDistance = 5;
    public AudioClip explodeSound;
	public float waitTimeToExplode;

    private AudioSource source;
    static private int direction = 0;
    private GameObject target;
    private float distanceToTarget;
    private bool isAttacking;
    private Animator animator;
    private bool weakenedOnce;
    private bool correctPlacement;
    private bool canAttack;
    private System.Random randNum;


    // Use this for initialization
    void Start()
    {
        knockBackDistance = 2;
        isAttacking = false;
        target = GameObject.FindWithTag("Player");
        animator = this.GetComponent<Animator>();
        animator.SetInteger("Port", 0);
        randNum = new System.Random();
        weakenedOnce = false;
        correctPlacement = false;
        canAttack = true;
        source = this.gameObject.AddComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale != 0)
        {
            if (target != null && canAttack)
            {
                if (this.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > 0)
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
                distanceToTarget = Vector2.Distance(this.transform.position, target.transform.position);

                if (distanceToTarget <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth>().IsBelowThirtyFivePercent() || weakenedOnce))
                {
                    isAttacking = true;
                    StartCoroutine(LaunchAttack());
                }
                else if (this.GetComponent<EnemyHealth>().IsBelowThirtyFivePercent() && !weakenedOnce)
                {
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    StartCoroutine(WeakenedState());
                }
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
    IEnumerator LaunchAttack()
    {
        yield return null;
        float timer = teleportTime;
        //yield return new WaitForSeconds (waitTime);
        while (target != null && transform.position != target.transform.position && distanceToTarget <= sightRadius)
        {
            timer += Time.deltaTime;

            if (timer >= teleportTime)
            {
                enemyPlacement();
                //add check if on top of other enemies to move off slightly
				yield return new WaitForSeconds(waitTimeToExplode);
                animator.SetInteger("Port", 1);
                if (canAttack)
                {
                    Instantiate(attackType, transform.position, Quaternion.identity);
                    source.PlayOneShot(explodeSound, .075f);
                }
                timer = 0f;
            }
            yield return null;

        }
        animator.SetInteger("Port", 2);
        yield return new WaitForSeconds(.5f);
        animator.SetInteger("Port", 0);
		isAttacking = false;
	}     

    private void enemyPlacement()
    {
        int distanceForPlacement = 1;
        correctPlacement = false;
        while (!correctPlacement)
        {
            this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            // check for colliders when there is a plannet it is landing on
            if (direction == 0)
            { // on top
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y + (teleDistance * distanceForPlacement), 0);
            }
            else if (direction == 1)
            { // on right
                transform.position = new Vector3(target.transform.position.x + (teleDistance * distanceForPlacement), target.transform.position.y, 0);
            }
            else if (direction == 2)
            { // on bottom
                transform.position = new Vector3(target.transform.position.x, target.transform.position.y - (teleDistance * distanceForPlacement), 0);
            }
            else
            { //on left
                transform.position = new Vector3(target.transform.position.x - (teleDistance * distanceForPlacement), target.transform.position.y, 0);
            }

            Collider2D[] collidersPlanets = Physics2D.OverlapCircleAll(this.transform.position, radius, 1 << LayerMask.NameToLayer("Obstacles"));
            if (collidersPlanets.Length == 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                correctPlacement = true;
            }
            else
            {
                distanceForPlacement++;
            }
        }

        //check for bounce off
        if (this.gameObject.transform.position.x <= -edgeX && gameObject.activeSelf) 
        {
            canAttack = false;
            int randomDegree = randNum.Next(-30, 30);
            StartCoroutine(BounceOff(this.gameObject, randomDegree));
        }
        else if (this.gameObject.transform.position.x >= edgeX && gameObject.activeSelf)
        {
            canAttack = false;
            int randomDegree = randNum.Next(150, 210);
            StartCoroutine(BounceOff(this.gameObject, randomDegree));
        }

        if (this.gameObject.transform.position.y <= -edgeY && gameObject.activeSelf)
        {
            canAttack = false;
            int randomDegree = randNum.Next(60, 120);
            StartCoroutine(BounceOff(this.gameObject, randomDegree));
        }
        else if (this.gameObject.transform.position.y >= edgeY && gameObject.activeSelf)
        {
            canAttack = false;
            int randomDegree = randNum.Next(240, 300);
            StartCoroutine(BounceOff(this.gameObject, randomDegree));
        }


        if (direction < 3)
        {
            direction++;
        }
        else {
            direction = 0;
        }
    }


    public void setCanAttack(bool booleanSent)
    {
        canAttack = booleanSent;
    }


    IEnumerator BounceOff(GameObject enemy, int randomDegree)
    {
        //yield return null;
        float numAddX = Mathf.Cos(randomDegree * (Mathf.PI / 180)) * 50;
        float numAddY = Mathf.Sin(randomDegree * (Mathf.PI / 180)) * 50;
        float endX = numAddX + enemy.transform.position.x;
        float endY = numAddY + enemy.transform.position.y;
        Vector2 endLocation = new Vector2(endX, endY);
        Vector2 nextPosition = enemy.transform.position;
        Vector2 look = endLocation - nextPosition;
        float distanceCovered = 0;
        int maxDistance = 100;
        int layerDepth = 1;
        int obsticalMask = layerDepth << 12; //obsticale on 12th layer
        RaycastHit2D impactObsticale = Physics2D.Raycast(nextPosition, endLocation, maxDistance, obsticalMask);


        float distanceToGo = 50f;
        while (distanceCovered < distanceToGo)
        {
            nextPosition += look.normalized * bounceSpeed; //try time.detlatime to see if that can make it better
            distanceCovered += Math.Abs(Vector2.Distance(enemy.transform.position, nextPosition));

            if (Physics2D.Linecast(enemy.transform.position, nextPosition, obsticalMask)) // if it his an obsticale it stops moving
            {
                impactObsticale = Physics2D.Linecast(enemy.transform.position, nextPosition, obsticalMask);
                distanceCovered = distanceToGo;
                nextPosition = enemy.transform.position;
            }

            enemy.transform.position = nextPosition;
            yield return null;
        }
        yield return new WaitForSeconds(.25f); // cooldown
        canAttack = true;
    }

    public void startKnockBack(float degree)
    {
        StartCoroutine(BounceOff(degree, 1f));
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
        Vector2 endLocation = new Vector2(endX, endY);
        Vector2 nextPosition = this.gameObject.transform.position;
        Vector2 look = endLocation - nextPosition;
        float distanceCovered = 0;
        int maxDistance = 100;
        int layerDepth = 1;
        int obsticalMask = layerDepth << 12; //obsticale on 12th layer
        RaycastHit2D impactObsticale = Physics2D.Raycast(nextPosition, endLocation, maxDistance, obsticalMask);


        float distanceToGo = knockBackDistance;
        while (distanceCovered < distanceToGo)
        {
            nextPosition += look.normalized * knockBackSpeed;
            distanceCovered += Math.Abs(Vector2.Distance(this.gameObject.transform.position, nextPosition));

            if (Physics2D.Linecast(this.gameObject.transform.position, nextPosition, obsticalMask)) // if it his an obsticale it stops moving
            {
                impactObsticale = Physics2D.Linecast(this.gameObject.transform.position, nextPosition, obsticalMask);
                distanceCovered = distanceToGo;
                nextPosition = this.gameObject.transform.position;
            }

            this.gameObject.transform.position = nextPosition;
            yield return null;
        }
        yield return new WaitForSeconds(.5f); // cooldown
        canAttack = true;
    }

    /// <summary>
    /// Reset information dealing with the start of the enemy.
    /// Used for when the enemy is returned to the pool of objects.
    /// </summary>
    public void ResetInfo()
    {
        weakenedOnce = false;
        knockBackDistance = 2;
        isAttacking = false;
        target = GameObject.FindWithTag("Player");
        animator = this.GetComponent<Animator>();
        animator.SetInteger("Port", 0);
        randNum = new System.Random();
        weakenedOnce = false;
        correctPlacement = false;
        canAttack = true;
    }
}