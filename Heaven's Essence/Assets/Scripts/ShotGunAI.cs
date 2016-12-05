using UnityEngine;
using System.Collections;
using System;

public class ShotGunAI : MonoBehaviour
{

    private float sightRadius = 10f;
    public float damage = 20f;
    private float waitTime = 0.5f;
    private float movementSpeed = 100f;
    private float launchSpeed = 1000f;
    private GameObject target;
    private float distanceToTarget;
    private bool isAttacking = false;
    private bool weakenedOnce = false;

    private bool stopped;
    public GameObject attackType;

    public GameObject pool;

    private float attackTime = 3f;

    private int rotateSpeed = 3;
    public float knockBackDistance;


    private bool canAttack = true;

    // Use this for initialization
    void Start()
    {
        knockBackDistance = 2;
        isAttacking = false;
        stopped = false;
        target = GameObject.FindWithTag("Player");
        transform.LookAt(target.transform.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        pool = GameObject.FindWithTag("PoolFallen");
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.timeScale != 0)
        {
            if (target != null && canAttack && this.gameObject != null)

                if (target != null && canAttack && this.gameObject != null)
                {
                    distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

                    if (distanceToTarget <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth>().IsBelowThirtyFivePercent() || weakenedOnce) && gameObject.activeSelf)
                    {
                        isAttacking = true;
                        //setAttackingAnimation(true);
                        StartCoroutine(LaunchAttack());
                    }
                    else if (this.GetComponent<EnemyHealth>().IsBelowThirtyFivePercent() && !weakenedOnce && gameObject.activeSelf)
                    {
                        stopped = true;
                        StartCoroutine(WeakenedState());
                    }

                    //Vector2 velocity = new Vector2((transform.position.x - target.transform.position.x - 5) * inverseLaunchSpeed, (transform.position.y - target.transform.position.y - 5) * inverseLaunchSpeed);
                    //GetComponent<Rigidbody2D>().velocity = -velocity;

                    if (!stopped)

                    {
                        distanceToTarget = Vector2.Distance(transform.position, target.transform.position);

                        if (distanceToTarget <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth>().IsBelowThirtyFivePercent() || weakenedOnce))
                        {
                            isAttacking = true;
                            //setAttackingAnimation(true);
                            StartCoroutine(LaunchAttack());
                        }
                        else if (this.GetComponent<EnemyHealth>().IsBelowThirtyFivePercent() && !weakenedOnce)
                        {
                            stopped = true;
                            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            StartCoroutine(WeakenedState());
                        }

                        //Vector2 velocity = new Vector2((transform.position.x - target.transform.position.x - 5) * inverseLaunchSpeed, (transform.position.y - target.transform.position.y - 5) * inverseLaunchSpeed);
                        //GetComponent<Rigidbody2D>().velocity = -velocity;

                        if (!stopped)
                        {
                            Vector2 dir = target.transform.position - this.transform.position;
                            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                            if (distanceToTarget >= sightRadius)
                            {

                                this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(dir.x, dir.y) * Time.deltaTime * movementSpeed);

                                //transform.position += transform.right * Time.deltaTime * movementSpeed;
                            }
                            else if (!weakenedOnce)
                            {
                                //transform.position += transform.right * Time.deltaTime * speed;
                                transform.RotateAround(target.transform.position, Vector3.forward, rotateSpeed * Time.deltaTime * 100);
                            }
                        }
                    }
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
        bool attacked = false;
        yield return null;
        Vector2 endLocation = target.transform.position;
        Vector2 nextPosition = this.transform.position;
        Vector2 look = endLocation - nextPosition;

        float timer = attackTime;

        while (distanceToTarget <= sightRadius && !attacked)
        {
            if (Time.timeScale != 0)
            {
                timer += Time.deltaTime;
                if (timer >= attackTime && !attacked)
                {
                    setAttackingAnimation(true);
                    attacked = true;
                    GameObject createProjectile = (GameObject)Instantiate(attackType, transform.position + 1.0f * transform.right, transform.rotation);
                    createProjectile.GetComponent<Rigidbody2D>().AddForce(createProjectile.transform.right * launchSpeed);
                    stopped = true;
                    this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    yield return new WaitForSeconds(1);
                    stopped = false;
                    timer = 0f;
                }
            }
            setAttackingAnimation(false);
            yield return null;
        }
        isAttacking = false;
        attacked = false;
    }

    void setAttackingAnimation(bool status)
    {
        this.GetComponent<EnemyAnimationScript>().isAttacking = status;
    }


    public void setCanAttack(bool booleanSent)
    {
        canAttack = booleanSent;
    }

    public void setKnockBackAmount(int distance)
    {
        knockBackDistance = distance;
    }

    public void startKnockBack(float degree)
    {
        StartCoroutine(BounceOff(degree, 1f));
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
        stopped = false;
        target = GameObject.FindWithTag("Player");
        transform.LookAt(target.transform.position);
        transform.Rotate(new Vector3(0, -90, 0), Space.Self);
        pool = GameObject.FindWithTag("PoolFallen");
    }
}

