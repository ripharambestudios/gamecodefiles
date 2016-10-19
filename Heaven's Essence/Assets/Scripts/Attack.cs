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
	private int ammunition;
	private bool startedOnce = false;
	private bool ammoSet = false;

    // Use this for initialization
    void Start()
    {
        
        projectile = projectileEnergy;
        attackType = attackTypeEnergy;
    }

    void Update()
    {
        if(projectile.name == projectileBeam.name)
        {
            //red skin
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(true);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if(projectile.name == projectileEnergy.name)
        {
            //blue skin
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if(projectile.name == projectileShotgun.name)
        {
            //green skin
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if(projectile.name == projectileSpeed.name)
        {
            //yellow skin
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if(projectile.name == projectileBomb.name)
        {
            //purple skin
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(true);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
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
                //canAttack = false;
				float beamTimer = 2f; //2 seconds
				if (!startedOnce) {
					StartCoroutine (BeamTimeLeft (beamTimer));
					startedOnce = true;
				}
                StartCoroutine(fireBeam((Vector2)attackSpawn.transform.position, attackAngle));
                //StartCoroutine(Cooldown(_rateOfFire));
            }
            else {
				if (projectile.name == projectileBomb.name) {
					speedOfProjectile = .4f;
					rateOfFire = 1.0f;
					if (!ammoSet) {
						ammunition = 20;
						ammoSet = true;
					}
				} else if (projectile.name == projectileSpeed.name) {
					speedOfProjectile = 1.2f;
					rateOfFire = 6.0f;
					if (!ammoSet) {
						ammunition = 100;
						ammoSet = true;
					}
				} else if (projectile.name == projectileShotgun.name) {
					speedOfProjectile = .7f;
					rateOfFire = 3.0f;
					if (!ammoSet) {
						ammunition = 30;
						ammoSet = true;
					}
				} else {
					speedOfProjectile = 1f;
					rateOfFire = 4.0f;
					ammunition = 1000;
				}
				_rateOfFire = 1 / rateOfFire;
				canAttack = false;
				StartCoroutine (fireProjectile ((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
				StartCoroutine (Cooldown (_rateOfFire));
            }
        }
    }

    public void AltFire()
    {
        if (canAttack)
        {
			
			if (projectile.name == projectileBomb.name) {
				//nothing, done in different script
			} else if (projectile.name == projectileEnergy.name) {
				canAttack = false;
				StartCoroutine (altEnergy ((Vector2)attackSpawn.transform.position, attackAngle));

			} else if (projectile.name == projectileBeam.name) {
				canAttack = false;
				StartCoroutine (altBeam ((Vector2)attackSpawn.transform.position, attackAngle));

			} else if (projectile.name == projectileSpeed.name) {
				canAttack = false;
				StartCoroutine (altSpeed ((Vector2)attackSpawn.transform.position, attackAngle));
			} else if (projectile.name == projectileShotgun.name) {
				canAttack = false;
				StartCoroutine (altShotgun ());
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
        float maxLaser = 100f;

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
		ammunition -= 1;
		Debug.Log ("AMMO LEFT " + ammunition);
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
		while (createProjectile != null && timeout > 0f && !hit)
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

		if (ammunition <= 0) {
			ammoSet = false;
			projectile = projectileEnergy;
			attackType = attackTypeEnergy;
		}
        Destroy(createProjectile);
    }

	//shoots bigger laser that instantly drains whole timer
	IEnumerator altBeam(Vector2 start, Vector2 next){
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
		float maxLaser = 100f;

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

	//charges up large shot
	IEnumerator altEnergy(Vector2 start, Vector2 next){
		yield return null;
	}

	//shoots double bullets, drains off ammunition twice as fast
	IEnumerator altSpeed(Vector2 start, Vector2 next){
		yield return null;
	}

	//
	IEnumerator altShotgun(){
		yield return null;
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

	IEnumerator BeamTimeLeft(float beamTimer){
		float timer = 0;
		while (timer < beamTimer) {
			timer += Time.deltaTime;
			yield return null;
		}
		projectile = projectileEnergy;
		attackType = attackTypeEnergy;
		startedOnce = false;
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
