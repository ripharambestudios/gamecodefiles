using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{

    public GameObject weaponPosition;
    public GameObject attackSpawn;

    //energy attack type
    public GameObject projectileEnergy;
    public GameObject attackTypeEnergy;
    //demonic sonic attack type
    public GameObject projectileBeam;
    public GameObject attackTypeBeam;
    //bomb guy attack type
    public GameObject projectileBomb;
    public GameObject attackTypeBomb;
    //spooky guy attack type
    public GameObject projectileSpeed;
    public GameObject attackTypeSpeed;
    //fallen guy attack type
    public GameObject projectileShotgun;
    public GameObject attackTypeShotgun;

    //public float damage = 10;
    public float speedOfProjectile = 1f;
    public float rateOfFire = 4.0f;

    private GameObject projectile;
    private GameObject attackType;
    private float _rateOfFire;
    private Vector2 attackAngle = Vector2.zero;
    private Vector2 aimLocation = Vector2.zero;
    private bool canAttack = true;


    // Use this for initialization
    void Start()
    {
        
        projectile = projectileEnergy;
        attackType = attackTypeEnergy;
    }

    public void Aim(Vector2 aimTarget)
    {
        aimLocation = aimTarget;
        attackSpawn.transform.position = weaponPosition.transform.position;
        attackSpawn.transform.LookAt(aimLocation);
        attackAngle = attackSpawn.transform.forward;

    }

    public void Fire()
    {
        if (canAttack)
        {
            if (projectile.name == projectileBeam.name)
            {
                canAttack = false;
                StartCoroutine(fireBeam((Vector2)attackSpawn.transform.position, attackAngle));
                StartCoroutine(Cooldown(_rateOfFire));
            }
            else {
				if (projectile.name == projectileBomb.name) {
					speedOfProjectile = .25f;
					rateOfFire = 1.0f;

				} else if (projectile.name == projectileSpeed.name) {
					speedOfProjectile = 1.2f;
					rateOfFire = 6.0f;
				} else if (projectile.name == projectileShotgun.name) {
					speedOfProjectile = .7f;
					rateOfFire = 3.0f;
				} else {
					speedOfProjectile = 1f;
					rateOfFire = 4.0f;
				}
				_rateOfFire = 1 / rateOfFire;
				canAttack = false;
				StartCoroutine (fireProjectile ((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
				StartCoroutine (Cooldown (_rateOfFire));
            }
        }
    }

    IEnumerator fireBeam(Vector2 start, Vector2 next)
    {
        yield return null;
        //destroy object if it doesn't collide with anything after timeout amout of time
        GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
        createProjectile.transform.parent = this.transform;
        //get the sign of the direction of the aim
        float signOfLook = 1;
        if (createProjectile.transform.position.y > next.y)
        {
            signOfLook = Mathf.Sign(next.y); //this will be negative if the mouse is below bullet, rotating it appropriately
        }
        float angle = Vector3.Angle(Vector3.right, new Vector3(next.x, next.y, 0));
        angle *= signOfLook;
        if(this.transform.position.y < 0 && next.y < 0)
        {
            angle = angle * -1;
        }
        //rotate shot
        createProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Transform middle = this.transform.GetChild(1);
        Vector2 nextPosition = start;
        float maxLaser = 100f;

        RaycastHit2D impact;
        int layerDepth = 1;
        int layerMask = layerDepth << 9; //enemies on 9th layer
        RaycastHit2D[] hits = Physics2D.RaycastAll(createProjectile.transform.position, next, maxLaser, layerMask);
        foreach (RaycastHit2D hit in hits)
        {
            Instantiate(attackType, hit.point, Quaternion.identity);
        }
        yield return new WaitForSeconds(.2f);
        yield return null;
        Destroy(createProjectile);
    }

    IEnumerator fireProjectile(Vector2 start, Vector2 next, float attackSpeed)
    {
        yield return null;
        //destroy object if it doesn't collide with anything after timeout amout of time
        float timeout = 3f;

        GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
        createProjectile.transform.parent = this.transform;
        //get the sign of the direction of the aim
        float signOfLook = 1;
        if (createProjectile.transform.position.y > next.y)
        {
            signOfLook = Mathf.Sign(next.y); //this will be negative if the mouse is below bullet, rotating it appropriately
        }
        float angle = Vector3.Angle(Vector3.right, new Vector3(next.x, next.y, 0));
        angle *= signOfLook;
        if (this.transform.position.y < 0 && next.y < 0)
        {
            angle = angle * -1;
        }
        //rotate shot
        createProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Vector2 nextPosition = start;
        bool hit = false;
        while (timeout > 0f && !hit)
        {
            timeout -= Time.deltaTime;
            nextPosition += next * attackSpeed;

            RaycastHit2D impact;
            int layerDepth = 1;
            int layerMask = layerDepth << 9; //enemies on 9th layer
            if (Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask))
            {
                impact = Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask);
                Instantiate(attackType, impact.point, Quaternion.identity);
                hit = true;
            }

            createProjectile.transform.position = nextPosition;
            yield return null;
        }
        Destroy(createProjectile);
    }


    //method for checking weapon cooldown on seperate thread
    IEnumerator Cooldown(float cooldown)
    {
        float timer = 0;
        while (timer < cooldown)
        {
            timer += Time.deltaTime;
            yield return null;

        }
        canAttack = true;
    }

    public void EnemyAbsorbed(string attackTypeString)
    {
        if (attackTypeString == "Energy")
        {
            projectile = projectileEnergy;
            attackType = attackTypeEnergy;
        }
        else if (attackTypeString == "DemonicSonic(Clone)")
        {
            projectile = projectileBeam;
            attackType = attackTypeBeam;
        }
        else if (attackTypeString == "BoomEnemy(Clone)")
        {
            projectile = projectileBomb;
            attackType = attackTypeBomb;
        }
        else if (attackTypeString == "SpookyGuy(Clone)")
        {
            projectile = projectileSpeed;
            attackType = attackTypeSpeed;
        }
        else if (attackTypeString == "FallenGuy(Clone)")
        {
            projectile = projectileShotgun;
            attackType = attackTypeShotgun;
        }
    }
}
