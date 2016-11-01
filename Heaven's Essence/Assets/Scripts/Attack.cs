using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    //Attack upgrades and souls text
    public Text soulsText;
    public Text upgradeLevels;

    private GameObject projectile;
    private GameObject attackType;
    private float _rateOfFire;
    private Vector2 attackAngle = Vector2.zero;
    private Vector2 aimLocation = Vector2.zero;
    private bool canAttack = true;

	private bool startedOnce = false; //for beam timer

	private float chargeTime = 0; //used to track how long right click is held for energy attack alternate attack
	private float maxCharge = 6f;  //time player needs to hold down for alt attack to fire
    //Number of souls player has obtained
    private int energySouls = 0;
    private int beamSouls = 0;
    private int bombSouls = 0;
    private int shotgunSouls = 0;
    private int spookyGuySouls = 0;
    //attack power of the different attacks
    private int energyAttackLevel = 1;
    private int beamAttackLevel = 0;
    private int bombAttackLevel = 0;
    private int shotgunAttackLevel = 0;
    private int spookyGuyAttackLevel = 0;
    //upgrade cost for each attack
    private int energyUpgradeCost = 50;
    private int beamUpgradeCost = 20;
    private int bombUpgradeCost = 20;
    private int shotgunUpgradeCost = 20;
    private int spookyGuyUpgradeCost = 20;
    //max number of upgrades
    private int maxNumOfUpgrades = 12;
    private int maxUpgradeForWeapon = 8;
    private int numberOfUpgrades = 0;
    //starting damage values
    private int energyInitialDamage;
    private int beamInitialDamage;
    private int bombInitialDamage;
    private int shotgunInitialDamage;
    private int spookyGuyInitialDamage;


    // Use this for initialization
    void Start()
    {
        //set attack to initial energy ball with start properties
        projectile = projectileEnergy;
        attackType = attackTypeEnergy;
        speedOfProjectile = 1f;
        rateOfFire = 4.0f;
        //save initial damage done by attacks
        energyInitialDamage = attackTypeEnergy.GetComponent<DoDamage>().damage;
        beamInitialDamage = attackTypeBeam.GetComponent<DoDamage>().damage;
        bombInitialDamage = attackTypeBomb.GetComponent<DoDamage>().damage;
        shotgunInitialDamage = attackTypeShotgun.GetComponent<DoDamage>().damage;
        spookyGuyInitialDamage = attackTypeSpeed.GetComponent<DoDamage>().damage;

        soulsText.text = "Souls: Energy= 0 Beam= 0 Bomb= 0 Speed= 0 Shotgun= 0";
        upgradeLevels.text = "Attack Levels: Energy= 1 Beam= 0 Bomb= 0 Speed= 0 Shotgun= 0";
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
			if (projectile.name == projectileBeam.name && beamAttackLevel > 0)
			{
                //may be wrong right now
				//canAttack = false;
				float beamTimer = 2f; //2 seconds
				if (!startedOnce) {
					StartCoroutine (BeamTimeLeft (beamTimer));
					startedOnce = true;
				}
                canAttack = false;
                
                StartCoroutine(fireBeam((Vector2)attackSpawn.transform.position, attackAngle));
                StartCoroutine(Cooldown(_rateOfFire));

            }
            else {
				if (projectile.name == projectileBomb.name && bombAttackLevel >0) {
					
                    
                } else if (projectile.name == projectileSpeed.name && spookyGuyAttackLevel >0) {
					
                    
                } else if (projectile.name == projectileShotgun.name && shotgunAttackLevel > 0) {
					
                    
                } else {
					
					
				}
				_rateOfFire = 1 / rateOfFire;
				canAttack = false;
				StartCoroutine (fireProjectile ((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
				StartCoroutine (Cooldown (_rateOfFire));
            }
        }
    }

    /// <summary>
    /// Currently not set in stone.
    /// </summary>
	public void AltFire()
    {
		chargeTime += .1f;
        if (canAttack)
        {
			if (projectile.name == projectileBomb.name) {
				//nothing, done in different script
			} else if (projectile.name == projectileEnergy.name && chargeTime >= maxCharge) {
				canAttack = false;
				speedOfProjectile = 1f;
				rateOfFire = 4f;
				_rateOfFire = 1 / rateOfFire;
				attackTypeEnergy.GetComponent<DoDamage> ().damage = energyInitialDamage * energyAttackLevel * 2;
				StartCoroutine (Cooldown (_rateOfFire));
				StartCoroutine (altEnergy ((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));

			} else if (projectile.name == projectileBeam.name && beamAttackLevel > 0) {

				float beamTimer = 1f; //1 seconds
				if (!startedOnce) {
					StartCoroutine (BeamTimeLeft (beamTimer));
					startedOnce = true;
				}
				StartCoroutine(altBeam((Vector2)attackSpawn.transform.position, attackAngle));
					
			} else if (projectile.name == projectileSpeed.name && spookyGuyAttackLevel > 0) {
				speedOfProjectile = 1.2f;
				rateOfFire = 6.0f;
				_rateOfFire = 1 / rateOfFire;
				canAttack = false;
				StartCoroutine (altSpeed ((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
				StartCoroutine (Cooldown (_rateOfFire));
			} else if (projectile.name == projectileShotgun.name && shotgunAttackLevel > 0) {
				speedOfProjectile = .7f;
				rateOfFire = 3.0f;
				_rateOfFire = 1 / rateOfFire;
				canAttack = false;
				StartCoroutine (altShotgun (speedOfProjectile));
				StartCoroutine (Cooldown (_rateOfFire));
			}
        }
		if (chargeTime >= maxCharge) {
			chargeTime = 0;
		}
    }

	//fire super powerful red beam for 2 seconds in any direction
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
		
	//fires variety of projectiles from the player
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


        Destroy(createProjectile);
    }

	//shoots bigger laser that instantly drains whole timer
	//HIT BOX IS NOT CORRECT CURRENTLY
	IEnumerator altBeam(Vector2 start, Vector2 next){
		yield return null;
		//destroy object if it doesn't collide with anything after timeout amout of time
		GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
		createProjectile.transform.localScale *= 10;
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
	//Needs to be adjusted
	IEnumerator altEnergy(Vector2 start, Vector2 next, float attackSpeed){
		
		//destroy object if it doesn't collide with anything after timeout amout of time
		float timeout = 3f;
		GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
		createProjectile.transform.parent = this.transform;
		createProjectile.transform.localScale *= maxCharge;
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

		Destroy(createProjectile);
	}

	//shoots double bullets, drains off ammunition twice as fast
	IEnumerator altSpeed(Vector2 start, Vector2 next, float attackSpeed){
		yield return null;
		//destroy object if it doesn't collide with anything after timeout amout of time
		float timeout = 3f;

		GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
		createProjectile.transform.parent = this.transform;
		GameObject createProjectile2 = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
		createProjectile2.transform.parent = this.transform;

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
		createProjectile2.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		Vector2 nextPosition = start;
		Vector2 nextPosition2 = start + new Vector2(0,-1);
		bool hit = false;
		bool hit2 = false;
		while ((createProjectile != null || createProjectile2 != null) &&  timeout > 0f && (!hit|| !hit2))
		{
			timeout -= Time.deltaTime;
			nextPosition += next * attackSpeed;
			nextPosition2 += next * attackSpeed;
			RaycastHit2D impact;
			RaycastHit2D impact2;
			int layerDepth = 1;
			int layerMask = layerDepth << 9; //enemies on 9th layer
			if (createProjectile != null && Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask) && !hit)
			{
				impact = Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask);
				Instantiate(attackType, impact.point, Quaternion.identity);
				hit = true;
			}
			if (createProjectile2 != null && Physics2D.Linecast(createProjectile2.transform.position, nextPosition2, layerMask) && !hit2)
			{
				impact2 = Physics2D.Linecast(createProjectile2.transform.position, nextPosition2, layerMask);
				Instantiate(attackType, impact2.point, Quaternion.identity);
				hit2 = true;
			}

			if (createProjectile != null) {	
				createProjectile.transform.position = nextPosition;
			}
			if (createProjectile2 != null) {
				createProjectile2.transform.position = nextPosition2;
			}
			if (hit) {
				Destroy (createProjectile);
			}
			if (hit2) {
				Destroy (createProjectile2);
			}
			yield return null;
		}
        
		if (createProjectile != null) {
			Destroy (createProjectile);
		}
		if (createProjectile2 != null) {
			Destroy (createProjectile2);
		}
	}

	//fires four shots, one above, one below, and to the left and right of the player
	IEnumerator altShotgun(float attackSpeed){
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
		
		startedOnce = false;
        
    }

    public void EnemyAbsorbed(string attackTypeString)
    {
        if (attackTypeString == "Energy")
        {

        }
        else if (attackTypeString == "DemonicSonic(Clone)")
        {
            energySouls += 10;
            beamSouls += 10;
        }
        else if (attackTypeString == "BoomEnemy(Clone)")
        {
            energySouls += 20;
            bombSouls += 10;
        }
        else if (attackTypeString == "SpookyGuy(Clone)")
        {

            energySouls += 25;
            spookyGuySouls += 10;
        }
        else if (attackTypeString == "FallenGuy(Clone)")
        {
            energySouls += 15;
            shotgunSouls += 10;
        }
        soulsText.text = "Souls: Energy= " + energySouls.ToString() + " Beam= " + beamSouls + " Bomb= " + bombSouls + " Speed= " + spookyGuySouls + " Shotgun= " + shotgunSouls;
    }

    public void SwitchAttacks(string attackTypeString)
    {

        if (attackTypeString == "Energy")
        {
            projectile = projectileEnergy;
            attackType = attackTypeEnergy;
            speedOfProjectile = 1f;
            rateOfFire = 4.0f;
        }
        else if (attackTypeString == "Beam" && beamAttackLevel >0)
        {
            projectile = projectileBeam;
            attackType = attackTypeBeam;
            
        }
        else if (attackTypeString == "Bomb" && bombAttackLevel >0)
        {
            projectile = projectileBomb;
            attackType = attackTypeBomb;
            speedOfProjectile = .4f;
            rateOfFire = 1.0f;

        }
        else if (attackTypeString == "Speed" && spookyGuyAttackLevel > 0)
        {
            projectile = projectileSpeed;
            attackType = attackTypeSpeed;
            speedOfProjectile = 1.2f;
            rateOfFire = 6.0f;

        }
        else if (attackTypeString == "Shotgun" && shotgunAttackLevel > 0)
        {
            projectile = projectileShotgun;
            attackType = attackTypeShotgun;
            speedOfProjectile = .7f;
            rateOfFire = 3.0f;
        }
        else
        {
            //case occurs when player selects weapon that is not unlocked
            projectile = projectileEnergy;
            attackType = attackTypeEnergy;
            speedOfProjectile = 1f;
            rateOfFire = 4.0f;
            Debug.Log("Attack is not unlocked");
            //instantiate here a warning that the player has not unlocked that attack yet
        }
    }

    public void UpgradeAttack(string upgradeType)
    {
        numberOfUpgrades += 1;
        if (numberOfUpgrades <= maxNumOfUpgrades)
        {
            if (upgradeType == "Energy" && energySouls >= energyUpgradeCost && energyAttackLevel < maxUpgradeForWeapon)
            {
                
                energyAttackLevel += 1;
                attackTypeEnergy.GetComponent<DoDamage>().damage = energyInitialDamage * energyAttackLevel;
                energySouls -= energyUpgradeCost;
                energyUpgradeCost *= 2;
            }
            else if (upgradeType == "Beam" && beamSouls >= beamUpgradeCost && beamAttackLevel < maxUpgradeForWeapon)
            {
                Debug.Log("Attack upgraded");
                beamAttackLevel += 1;
                attackTypeBeam.GetComponent<DoDamage>().damage = beamInitialDamage * beamAttackLevel;
                beamSouls -= beamUpgradeCost;
                beamUpgradeCost *= 2;

            }
            else if (upgradeType == "Bomb" && bombSouls >= bombUpgradeCost && bombAttackLevel < maxUpgradeForWeapon)
            {
                bombAttackLevel += 1;
                attackTypeBomb.GetComponent<DoDamage>().damage = bombInitialDamage * bombAttackLevel;
                bombSouls -= bombUpgradeCost;
                bombUpgradeCost *= 2;

            }
            else if (upgradeType == "Speed" && spookyGuySouls >= spookyGuyUpgradeCost && spookyGuyAttackLevel < maxUpgradeForWeapon)
            {
                spookyGuyAttackLevel += 1;
                attackTypeSpeed.GetComponent<DoDamage>().damage = spookyGuyInitialDamage * spookyGuyAttackLevel;
                spookyGuySouls -= spookyGuyUpgradeCost;
                spookyGuyUpgradeCost *= 2;

            }
            else if (upgradeType == "Shotgun" && shotgunSouls >= shotgunUpgradeCost && shotgunAttackLevel < maxUpgradeForWeapon)
            {
                shotgunAttackLevel += 1;
                attackTypeShotgun.GetComponent<DoDamage>().damage = shotgunInitialDamage * shotgunAttackLevel;
                shotgunSouls -= shotgunUpgradeCost;
                shotgunUpgradeCost *= 2;
            }
            else
            {
                //case occurs when player selects weapon that they do not have enough to upgrade
                Debug.Log("You do not have enough " + upgradeType + " type souls to upgrade this attack.");
                //instantiate here a warning that the player does not have enough souls for the attack upgrade
                numberOfUpgrades -= 1;
            }
            
            upgradeLevels.text = "Attack Levels: Energy= " + energyAttackLevel + " Beam= " + beamAttackLevel + " Bomb= "+ bombAttackLevel+ " Speed= " + spookyGuyAttackLevel + " Shotgun= " + shotgunAttackLevel;
        }
        else
        {
            //display that the player has reached their max number of upgrades
        }

    }

    public int GetEnergyNumberOfSouls()
    {
        return energySouls;
    }

    public int GetBombNumberOfSouls()
    {
        return bombSouls;
    }
    public int GetSpookyNumberOfSouls()
    {
        return spookyGuySouls;
    }
    public int GetShotgunNumberOfSouls()
    {
        return shotgunSouls;
    }
    public int GetBeamNumberOfSouls()
    {
        return beamSouls;
    }
}
