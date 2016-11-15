using UnityEngine;
using System.Collections;

public class DistantShooterAI : MonoBehaviour
{

    public float sightRadius = 30f; //max distance can be from player and still in view
    public float damage = 20f;
    public float projectileSpeed = 10f; //bigger number means slower enemy
    public float attackCooldown = 1f;
    public float rateOfFire = 50f;
    public int numberOfProjectiles = 2;
    public GameObject projectile;
    public GameObject launchPosition;

    private GameObject target;

    private float actualRateOfFire;
    private bool isAttacking = false;
    private float distanceToTarget;
    private float minTether;
    private float maxTether;
    private int numberOfProjectilesLaunched;
    private bool weakenedOnce = false;
    private bool canMove = true;
    private bool canAttack = true;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");

        minTether = 17f;
        maxTether = 21f;
        if (sightRadius != maxTether)
        {
            sightRadius = maxTether;
        }
        numberOfProjectilesLaunched = 0;
        actualRateOfFire = 1 / rateOfFire;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null && canAttack)
        {
            distanceToTarget = Vector2.Distance(this.transform.position, target.transform.position);
            Vector2 tether = new Vector2(target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y);
            float tetherMagnitude = Mathf.Sqrt((tether.x * tether.x) + (tether.y * tether.y));

            if (tetherMagnitude <= sightRadius && !isAttacking && (!this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() || weakenedOnce) && canAttack)
            {
                isAttacking = true;
                StartCoroutine(VolleyOfAttacks(distanceToTarget)); // test
                                                                   //StartCoroutine (DistantAttack (distanceToTarget));
            }
            else if (this.GetComponent<EnemyHealth>().IsBelowTwentyPercent() && !weakenedOnce)
            {
                canMove = false;
                StartCoroutine(WeakenedState());

            }
            if (canMove)
            {
                //motion slightly jerky still
                if (tetherMagnitude < minTether)
                {

                    this.GetComponent<Rigidbody2D>().velocity = -tether;
                }
                else if (tetherMagnitude > maxTether)
                {

                    this.GetComponent<Rigidbody2D>().velocity = tether;
                }
                else
                {
                    this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                }
            }
        }

    }

    IEnumerator WeakenedState()
    {
        yield return new WaitForSeconds(5);
        weakenedOnce = true;
        canMove = true;
        yield return null;
    }

    IEnumerator VolleyOfAttacks(float distance)
    {
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            yield return new WaitForSecondsRealtime(actualRateOfFire);
            //numberOfProjectilesLaunched += 2;

            StartCoroutine(DistantAttack(distance));
        }
    }

    IEnumerator DistantAttack(float distance)
    {
        //yield return null;
        //destroy object if it doesn't collide with anything after timeout amout of time
        float timeout = 2.5f;
        setAttackingAnimation(true);
        Vector2 aim = new Vector2(target.transform.position.x - this.transform.position.x, target.transform.position.y - this.transform.position.y);
        //Debug.Log("Bullet Making");
        GameObject createProjectile = (GameObject)Instantiate(projectile, launchPosition.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        GameObject createProjectile2 = (GameObject)Instantiate(projectile, launchPosition.transform.position + new Vector3(0, -1, 0), Quaternion.Euler(new Vector3(0, 0, 0)));
        numberOfProjectilesLaunched += 2;
        createProjectile.transform.parent = this.transform;
        createProjectile2.transform.parent = this.transform;
        
        
        Vector2 nextPosition = launchPosition.transform.position;
        Vector2 nextPosition2 = launchPosition.transform.position + new Vector3(0, -1, 0);
        float signOfLook = 1;
        if (createProjectile.transform.position.y > nextPosition.y)
        {
            signOfLook = Mathf.Sign(nextPosition.y); //this will be negative if the mouse is below bullet, rotating it appropriately
        }
        float angle = Vector3.Angle(Vector3.right, new Vector3(nextPosition.x, nextPosition.y, 0));
        
        //angle *= signOfLook;
        /*
        if (this.transform.position.y < 0 && nextPosition.y < 0)
        {
            angle = angle * -1;
        }
        */
        //rotate shot
        createProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        createProjectile2.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        bool hit = false;
        while (timeout > 0f && !hit)
        {
            timeout -= Time.deltaTime;
            int layerDepth = 1;
            int layerMask = layerDepth << 8; //player on 8th layer

            if (createProjectile != null)
            {
                nextPosition += aim.normalized * projectileSpeed * Time.fixedDeltaTime;
                RaycastHit2D impact;
                if (Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask))
                {

                    impact = Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask);
                    impact.collider.gameObject.SendMessage("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
                    hit = true;

                }
                createProjectile.transform.position = nextPosition;

            }

            if (createProjectile2 != null)
            {
                nextPosition2 += aim.normalized * projectileSpeed * Time.fixedDeltaTime;
                RaycastHit2D impact2;
                if (Physics2D.Linecast(createProjectile2.transform.position, nextPosition2, layerMask))
                {

                    impact2 = Physics2D.Linecast(createProjectile2.transform.position, nextPosition2, layerMask);
                    impact2.collider.gameObject.SendMessage("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
                    hit = true;

                }
                createProjectile2.transform.position = nextPosition2;
                if (hit)
                {
                    DestroyImmediate(createProjectile.gameObject);
                    DestroyImmediate(createProjectile2.gameObject);
                    numberOfProjectilesLaunched -= 2;
                }
                yield return null;
            }
        }

        if (!hit)
        {
            DestroyImmediate(createProjectile.gameObject);
            DestroyImmediate(createProjectile2.gameObject);

            numberOfProjectilesLaunched -= 2;
        }


        //Debug.Log(numberOfProjectilesLaunched);


        //Debug.Log(numberOfProjectilesLaunched);
        if (numberOfProjectilesLaunched <= 0)
        {
            //Debug.Log("stopAttack");
            isAttacking = false;
            setAttackingAnimation(false);
        }
        yield return new WaitForSeconds(attackCooldown);
    }

    void setAttackingAnimation(bool status)
    {
        this.GetComponent<EnemyAnimationScript>().isAttacking = status;
    }

    public void setCanAttack(bool booleanSent)
    {
        canAttack = booleanSent;
    }
}
