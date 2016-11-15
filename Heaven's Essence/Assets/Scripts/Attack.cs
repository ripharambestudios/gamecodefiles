using UnityEngine;
using System.Collections;
using System;
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

    // the wing particle effect
    public GameObject wingParticleEffect;

    [Header("Laser Parts")]
    public GameObject laserStart;
    public GameObject laserMiddle;
    public GameObject laserEnd;

    //public float damage = 10;
    private float speedOfProjectile = 1f;
    private float rateOfFire = 4.0f;

    //Attack upgrades and souls text
    public Text energySoulsText;
    public Text beamSoulsText;
    public Text bombSoulsText;
    public Text speedSoulsText;
    public Text shotgunSoulsText;


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
    private int energySouls = 1000;
    private int beamSouls = 1000;
    private int bombSouls = 1000;
    private int shotgunSouls = 1000;
    private int speedSouls = 1000;
    //attack power of the different attacks
    private int energyAttackLevel = 1;
    private int beamAttackLevel = 0;
    private int bombAttackLevel = 0;
    private int shotgunAttackLevel = 0;
    private int speedAttackLevel = 0;
    //upgrade cost for each attack
    private int energyUpgradeCost = 5;
    private int beamUpgradeCost = 2;
    private int bombUpgradeCost = 2;
    private int shotgunUpgradeCost = 2;
    private int spookyGuyUpgradeCost = 2;
    //max number of upgrades
    private int maxNumOfUpgrades = 36;
    private int maxUpgradeForWeapon = 7;
    private int numberOfUpgrades = 0;
    //starting damage values
    private int energyInitialDamage;
    private int beamInitialDamage;
    private int bombInitialDamage;
    private int shotgunInitialDamage;
    private int spookyGuyInitialDamage;
    private float spookyGuyProjectileSpeed;

    //upgrade icons
    private GameObject energyShotUpgradeIcon;
    private GameObject shotgunAttackUpgradeIcon;
    private GameObject speedShotUpgradeIcon;
    private GameObject bombAttackUpgradeIcon;
    private GameObject beamAttackUpgradeIcon;

    //set up laser before hand
    private GameObject middleOfLaser;
    private GameObject laserStartParticles;
    private GameObject laserHitParticles;

    // Use this for initialization
    void Start()
    {
        //set attack to initial energy ball with start properties
        projectile = projectileEnergy;
        attackType = attackTypeEnergy;
        speedOfProjectile = 2f;
        rateOfFire = 10.0f;
        spookyGuyProjectileSpeed = 1.2f;

        //reset attack damage values otherwise they infinitely scale
        attackTypeEnergy.GetComponent<DoDamage>().damage = 5;
        attackTypeBeam.GetComponent<DoDamage>().damage = 1;
        attackTypeBomb.GetComponent<DoDamage>().damage = 6;
        attackTypeShotgun.GetComponent<DoDamage>().damage = 3;
        attackTypeSpeed.GetComponent<DoDamage>().damage = 2;
        //save initial damage done by attacks
        energyInitialDamage = attackTypeEnergy.GetComponent<DoDamage>().damage;
        beamInitialDamage = attackTypeBeam.GetComponent<DoDamage>().damage;
        bombInitialDamage = attackTypeBomb.GetComponent<DoDamage>().damage;
        shotgunInitialDamage = attackTypeShotgun.GetComponent<DoDamage>().damage;
        spookyGuyInitialDamage = attackTypeSpeed.GetComponent<DoDamage>().damage;

        energyShotUpgradeIcon = GameObject.Find("Attack level Basic");
        shotgunAttackUpgradeIcon = GameObject.Find("Attack level Shotgun");
        speedShotUpgradeIcon = GameObject.Find("Attack level Speed Shot");
        bombAttackUpgradeIcon = GameObject.Find("Attack level Bomb");
        beamAttackUpgradeIcon = GameObject.Find("Attack level Beam");

        //create laser object
        middleOfLaser = (GameObject)Instantiate(laserMiddle, attackSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        middleOfLaser.transform.parent = this.transform;
        middleOfLaser.SetActive(false);
        laserStartParticles = (GameObject)Instantiate(laserStart, attackSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        laserStartParticles.SetActive(false);
        laserHitParticles = (GameObject)Instantiate(laserStart, attackSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        laserHitParticles.SetActive(false);
    }

    void Update()
    {
        if (projectile.name == projectileBeam.name)
        {
            wingParticleEffect.GetComponent<ParticleSystem>().startColor = new Color(1, .34509f, .34509f, 1);
            //red skin
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(true);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (projectile.name == projectileEnergy.name)
        {
            wingParticleEffect.GetComponent<ParticleSystem>().startColor = new Color(.34509f, .768627f, 1, 1);
            //blue skin
            this.transform.GetChild(0).gameObject.SetActive(true);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (projectile.name == projectileShotgun.name)
        {
            wingParticleEffect.GetComponent<ParticleSystem>().startColor = new Color(.23529f, 1, .34509f, 1);
            //green skin
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(true);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(false);
        }
        else if (projectile.name == projectileSpeed.name)
        {
            wingParticleEffect.GetComponent<ParticleSystem>().startColor = new Color(1, .980392f, .34509f, 1);
            //yellow skin
            this.transform.GetChild(0).gameObject.SetActive(false);
            this.transform.GetChild(1).gameObject.SetActive(false);
            this.transform.GetChild(2).gameObject.SetActive(false);
            this.transform.GetChild(3).gameObject.SetActive(false);
            this.transform.GetChild(4).gameObject.SetActive(true);
        }
        else if (projectile.name == projectileBomb.name)
        {
            wingParticleEffect.GetComponent<ParticleSystem>().startColor = new Color(.81568f, .34509f, 1, 1);
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
                if (!startedOnce)
                {
                    startedOnce = true;
                    
                }
                canAttack = false;
                _rateOfFire = 0f;
                if (beamAttackLevel >= 1 && beamAttackLevel < 3)
                {
                    StartCoroutine(fireBeam((Vector2)attackSpawn.transform.position, attackAngle));
                    StartCoroutine(Cooldown(_rateOfFire));

                }
                else if (beamAttackLevel >= 3 && beamAttackLevel < 5)
                {
                    StartCoroutine(fireTripleBeam((Vector2)attackSpawn.transform.position, attackAngle));
                    StartCoroutine(Cooldown(_rateOfFire));

                }
                else if (beamAttackLevel >= 5)
                {
                    StartCoroutine(bigBeam((Vector2)attackSpawn.transform.position, attackAngle));
                    StartCoroutine(Cooldown(_rateOfFire));

                }

            }
            else {
                if (projectile.name == projectileBomb.name && bombAttackLevel > 0)
                {
                    _rateOfFire = 1 / rateOfFire;
                    canAttack = false;

                    if (bombAttackLevel >= 1 && bombAttackLevel < 3)
                    {
                        StartCoroutine(bombShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile, 1));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                    else if (bombAttackLevel >= 3 && bombAttackLevel < 5)
                    {
                        StartCoroutine(bombShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile, 3));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                    else if (bombAttackLevel >= 5)
                    {
                        StartCoroutine(bombShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile, 5));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                }
                else if (projectile.name == projectileSpeed.name && speedAttackLevel > 0)
                {
                    _rateOfFire = 1 / rateOfFire;
                    canAttack = false;

                    if (speedAttackLevel >= 1)
                    {
                        StartCoroutine(spookyGuyProjectile((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile, speedAttackLevel));  //pass attack level as penetration power of the shot
                        StartCoroutine(Cooldown(_rateOfFire));
                    }


                }
                else if (projectile.name == projectileShotgun.name && shotgunAttackLevel > 0)
                {

                    _rateOfFire = 1 / rateOfFire;
                    canAttack = false;

                    if (shotgunAttackLevel >= 1 && shotgunAttackLevel < 3)
                    {
                        StartCoroutine(shotgunShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                    else if (shotgunAttackLevel >= 3 && shotgunAttackLevel < 5)
                    {
                        StartCoroutine(shotgunShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                    else if (shotgunAttackLevel >= 5)
                    {
                        StartCoroutine(shotgunShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                }
                else
                {
                    _rateOfFire = 1 / rateOfFire;
                    canAttack = false;

                    if (energyAttackLevel >= 1 && energyAttackLevel < 3)
                    {
                        StartCoroutine(energyShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile, 1.0f));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                    else if (energyAttackLevel >= 3 && energyAttackLevel < 5)
                    {
                        StartCoroutine(energyShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile, 1.7f));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }
                    else if (energyAttackLevel >= 5)
                    {
                        StartCoroutine(energyShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile, 2.2f));
                        StartCoroutine(Cooldown(_rateOfFire));
                    }

                }
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
            if (projectile.name == projectileBomb.name)
            {
                //nothing, done in different script
            }


            //alt fire removed for time being
            /*
            else if (projectile.name == projectileEnergy.name && chargeTime >= maxCharge)
            {
                canAttack = false;
                speedOfProjectile = 1f;
                rateOfFire = 4f;
                _rateOfFire = 1 / rateOfFire;
                attackTypeEnergy.GetComponent<DoDamage>().damage = energyInitialDamage * energyAttackLevel * 2;
                StartCoroutine(Cooldown(_rateOfFire));
                StartCoroutine(altEnergy((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));

            }
            else if (projectile.name == projectileBeam.name && beamAttackLevel > 0)
            {

                float beamTimer = 1f; //1 seconds
                if (!startedOnce)
                {
                    StartCoroutine(BeamTimeLeft(beamTimer));
                    startedOnce = true;
                }
                StartCoroutine(altBeam((Vector2)attackSpawn.transform.position, attackAngle));

            }
            else if (projectile.name == projectileSpeed.name && spookyGuyAttackLevel > 0)
            {
                speedOfProjectile = 1.2f;
                rateOfFire = 6.0f;
                _rateOfFire = 1 / rateOfFire;
                canAttack = false;
                StartCoroutine(altSpeed((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
                StartCoroutine(Cooldown(_rateOfFire));
            }
            else if (projectile.name == projectileShotgun.name && shotgunAttackLevel > 0)
            {
                speedOfProjectile = .7f;
                rateOfFire = 3.0f;
                _rateOfFire = 1 / rateOfFire;
                canAttack = false;
                StartCoroutine(altShotgun(speedOfProjectile));
                StartCoroutine(Cooldown(_rateOfFire));
            }
            */
        }
        if (chargeTime >= maxCharge)
        {
            chargeTime = 0;
        }
    }

    //fire powerful red beam in any direction
    //CURRENTLY ATTACKS TO FAST, DOES TOO MUCH DAMAGE
    IEnumerator fireBeam(Vector2 start, Vector2 next)
    {
                
        //destroy object if it doesn't collide with anything after timeout amout of time
        if (!middleOfLaser.activeInHierarchy)
        {
            middleOfLaser.SetActive(true);
        }
        if(!laserStartParticles.activeInHierarchy)
        {
            laserStartParticles.SetActive(true);
        }
        laserStartParticles.transform.position = start;

        //get the sign of the direction of the aim
        float signOfLook = 1;
        if (middleOfLaser.transform.position.y > next.y)
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
        middleOfLaser.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float maxLaser = 100f;
        float currentLaserSize = maxLaser;

        int layerDepth = 1;
        int layerMask = layerDepth << 9; //enemies on 9th layer
        RaycastHit2D hit = Physics2D.Raycast(start, next, maxLaser, layerMask);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(start.x, start.y, 0);
        positions[1] = new Vector3(next.x * 1000, next.y * 1000, 0);
        if (hit.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hit.point;
            positions[1] = new Vector3(hit.point.x, hit.point.y, 0);

            currentLaserSize = Math.Abs(Vector2.Distance(hit.point, start));
            Instantiate(attackType, hit.point, Quaternion.Euler(new Vector3(0,0,0)));
        }
        currentLaserSize = currentLaserSize / .74f;

        middleOfLaser.GetComponent<LineRenderer>().SetPositions(positions);
        middleOfLaser.GetComponent<LineRenderer>().SetWidth(.6f, .45f);

        yield return null;
        
    }

    //fire powerful beam with two offshoots
    //OFFSHOOTS DON'T ACTUALLY HAVE RAYCASTS IN THE RIGHT DIRECTION, NEED TO HAVE THE NEXT VECTOR ADJUSTED
    IEnumerator fireTripleBeam(Vector2 start, Vector2 next)
    {
        //destroy object if it doesn't collide with anything after timeout amout of time
        GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
        GameObject createProjectileLeft = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0)));
        GameObject createProjectileRight = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0)));
        createProjectile.transform.parent = this.transform;
        createProjectileLeft.transform.parent = this.transform;
        createProjectileRight.transform.parent = this.transform;
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
        int rotationAmount = 25;
        createProjectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        createProjectileLeft.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        createProjectileRight.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        createProjectileLeft.transform.rotation = Quaternion.Euler(createProjectileLeft.transform.rotation.eulerAngles.x, createProjectileLeft.transform.rotation.eulerAngles.y, (createProjectileLeft.transform.rotation.eulerAngles.z) + rotationAmount);
        createProjectileRight.transform.rotation = Quaternion.Euler(createProjectileRight.transform.rotation.eulerAngles.x, createProjectileRight.transform.rotation.eulerAngles.y, (createProjectileRight.transform.rotation.eulerAngles.z) - rotationAmount);

        float maxLaser = 100f;

        int layerDepth = 1;
        int layerMask = layerDepth << 9; //enemies on 9th layer

        float size = (float)Math.Sqrt(next.x * next.x + next.y * next.y);

        Vector2 nextLeft = new Vector2((float)Math.Cos(createProjectileLeft.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size, (float)Math.Sin(createProjectileLeft.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size);
        Vector2 nextRight = new Vector2((float)Math.Cos(createProjectileRight.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size, (float)Math.Sin(createProjectileRight.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size);

        RaycastHit2D[] hits = Physics2D.RaycastAll(createProjectile.transform.position, next, maxLaser, layerMask);
        foreach (RaycastHit2D hit in hits)
        {
            Instantiate(attackType, hit.point, Quaternion.identity);
        }
        RaycastHit2D[] hitsLeft = Physics2D.RaycastAll(createProjectileLeft.transform.position, nextLeft, maxLaser, layerMask);
        foreach (RaycastHit2D hit in hitsLeft)
        {
            Instantiate(attackType, hit.point, Quaternion.identity);
            Debug.DrawLine(createProjectileLeft.transform.position, hit.point, new Color(255, 0, 0), 5);
        }
        RaycastHit2D[] hitsRight = Physics2D.RaycastAll(createProjectileRight.transform.position, nextRight, maxLaser, layerMask);
        foreach (RaycastHit2D hit in hitsRight)
        {
            Instantiate(attackType, hit.point, Quaternion.identity);
            Debug.DrawLine(createProjectileRight.transform.position, hit.point, new Color(0, 255, 0), 5);
        }
        yield return new WaitForSeconds(.01f);

        Destroy(createProjectile);
        Destroy(createProjectileLeft);
        Destroy(createProjectileRight);

    }

    //Shoots large laser beam that does more damage
    //HIT BOX IS NOT CORRECT CURRENTLY, LINE RAYCAST NEXT TO EACH OTHER
    IEnumerator bigBeam(Vector2 start, Vector2 next)
    {

        //destroy object if it doesn't collide with anything after timeout amout of time
        GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
        createProjectile.transform.localScale *= 5;
        //createProjectile.GetComponent<BoxCollider2D>();
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
        float maxLaser = 100f;

        int layerDepth = 1;
        int layerMask = layerDepth << 9; //enemies on 9th layer
        RaycastHit2D[] hits = Physics2D.RaycastAll(createProjectile.transform.position, next, maxLaser, layerMask); //STACK LINECASTING ALL RIGHT NEXT TO EACH OTHER FOR WHOLE WIDTH
        foreach (RaycastHit2D hit in hits)
        {
            Instantiate(attackType, hit.point, Quaternion.identity);
        }
        yield return new WaitForSeconds(.01f);

        Destroy(createProjectile);
    }


    //fires bomb projectile 
    IEnumerator bombShot(Vector2 start, Vector2 next, float attackSpeed, int splitAmount)
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
            createProjectile.GetComponent<AlternatePlayerBoomAttack>().setSplitAmount(splitAmount);
            timeout -= Time.deltaTime;
            nextPosition += next * attackSpeed;

            RaycastHit2D impact;
            int layerDepth = 1;
            int layerMask = layerDepth << 9; //enemies on 9th layer
            if (Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask))
            {
                impact = Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask);
                if (splitAmount == 1)
                {
                    Instantiate(attackType, impact.point, Quaternion.identity);
                }
                else if (splitAmount == 3)
                {
                    Instantiate(attackType, impact.point + new Vector2(0, 5), Quaternion.identity);
                    Instantiate(attackType, impact.point + new Vector2(-5, -5), Quaternion.identity);
                    Instantiate(attackType, impact.point + new Vector2(5, -5), Quaternion.identity);
                }
                else if (splitAmount == 5)
                {
                    Instantiate(attackType, impact.point + new Vector2(0, 0), Quaternion.identity);
                    Instantiate(attackType, impact.point + new Vector2(-5, -5), Quaternion.identity);
                    Instantiate(attackType, impact.point + new Vector2(5, -5), Quaternion.identity);
                    Instantiate(attackType, impact.point + new Vector2(5, 5), Quaternion.identity);
                    Instantiate(attackType, impact.point + new Vector2(-5, 5), Quaternion.identity);
                }
                hit = true;
            }
            createProjectile.transform.position = nextPosition;
            yield return null;
        }
        Destroy(createProjectile);
    }


    //charges up large shot
    //Needs to be adjusted
    IEnumerator energyShot(Vector2 start, Vector2 next, float attackSpeed, float scaleFactor)
    {

        //destroy object if it doesn't collide with anything after timeout amout of time
        float timeout = 3f;
        GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
        createProjectile.transform.parent = this.transform;
        //createProjectile.transform.localScale *= maxCharge;
        createProjectile.transform.localScale *= scaleFactor;

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
    IEnumerator spookyGuyProjectile(Vector2 start, Vector2 next, float attackSpeed, int penetrationPower)
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
        bool entered = false;
        while (createProjectile != null && timeout > 0f && !hit)
        {
            timeout -= Time.deltaTime;
            nextPosition += next * attackSpeed;

            RaycastHit2D impact;

            int layerDepth = 1;
            int layerMask = layerDepth << 9; //enemies on 9th layer
            if (createProjectile != null && Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask) && !hit)
            {
                //Penetration of the bullet increases with each level up, and only works when entered into the enemy the first time.
                if (penetrationPower > 0 && !entered)
                {
                    impact = Physics2D.Linecast(createProjectile.transform.position, nextPosition, layerMask);
                    Instantiate(attackType, impact.point, Quaternion.identity);
                    penetrationPower--;
                    entered = true;
                }
                if (penetrationPower <= 0)
                {
                    hit = true;
                }
            }
            else
            {
                //when the bullet leaves the enter then it can allow to attack again
                entered = false;
            }

            if (createProjectile != null)
            {
                createProjectile.transform.position = nextPosition;
            }

            if (hit)
            {
                Destroy(createProjectile);
            }
            yield return null;
        }

        if (createProjectile != null)
        {
            Destroy(createProjectile);
        }
    }

    //fires four shots, one above, one below, and to the left and right of the player
    IEnumerator shotgunShot(Vector2 start, Vector2 next, float attackSpeed)
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

    IEnumerator BeamTimeLeft(float beamTimer)
    {
        float timer = 0;
        while (timer < beamTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        startedOnce = false;

    }

    public void EnemyAbsorbed(string attackTypeString)
    {
        if (attackTypeString == "Energy")
        {
            //add none because this only happens if player hits r
        }
        else if (attackTypeString == "DemonicSonic(Clone)")
        {
            energySouls += 1;
            beamSouls += 1;
        }
        else if (attackTypeString == "BoomEnemy(Clone)")
        {
            energySouls += 2;
            bombSouls += 1;
        }
        else if (attackTypeString == "SpookyGuy(Clone)")
        {

            energySouls += 3;
            speedSouls += 1;
        }
        else if (attackTypeString == "FallenGuy(Clone)")
        {
            energySouls += 2;
            shotgunSouls += 1;
        }
        energySoulsText.text = energySouls.ToString();
        beamSoulsText.text = beamSouls.ToString();
        bombSoulsText.text = bombSouls.ToString();
        speedSoulsText.text = speedSouls.ToString();
        shotgunSoulsText.text = shotgunSouls.ToString();
    }

    public void SwitchAttacks(string attackTypeString)
    {

        if (attackTypeString == "Energy")
        {
            projectile = projectileEnergy;
            attackType = attackTypeEnergy;
            speedOfProjectile = 2f;
            rateOfFire = 10.0f;

        }
        else if (attackTypeString == "Beam" && beamAttackLevel > 0)
        {
            projectile = projectileBeam;
            attackType = attackTypeBeam;

        }
        else if (attackTypeString == "Bomb" && bombAttackLevel > 0)
        {
            projectile = projectileBomb;
            attackType = attackTypeBomb;
            speedOfProjectile = .4f;
            rateOfFire = 1.0f;

        }
        else if (attackTypeString == "Speed" && speedAttackLevel > 0)
        {
            projectile = projectileSpeed;
            attackType = attackTypeSpeed;
            speedOfProjectile = spookyGuyProjectileSpeed;
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
                energyShotUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(energyAttackLevel);
            }
            else if (upgradeType == "Beam" && beamSouls >= beamUpgradeCost && beamAttackLevel < maxUpgradeForWeapon)
            {
                beamAttackLevel += 1;
                attackTypeBeam.GetComponent<DoDamage>().damage = beamInitialDamage * beamAttackLevel;
                beamSouls -= beamUpgradeCost;
                beamUpgradeCost *= 2;
                beamAttackUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(beamAttackLevel);
            }
            else if (upgradeType == "Bomb" && bombSouls >= bombUpgradeCost && bombAttackLevel < maxUpgradeForWeapon)
            {
                bombAttackLevel += 1;
                attackTypeBomb.GetComponent<DoDamage>().damage = bombInitialDamage * bombAttackLevel;
                bombSouls -= bombUpgradeCost;
                bombUpgradeCost *= 2;
                bombAttackUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(bombAttackLevel);

            }
            else if (upgradeType == "Speed" && speedSouls >= spookyGuyUpgradeCost && speedAttackLevel < maxUpgradeForWeapon)
            {
                speedAttackLevel += 1;
                attackTypeSpeed.GetComponent<DoDamage>().damage = spookyGuyInitialDamage * speedAttackLevel;
                speedSouls -= spookyGuyUpgradeCost;
                spookyGuyUpgradeCost *= 2;
                spookyGuyProjectileSpeed = 1.2f + (speedAttackLevel * .1f);
                speedShotUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(speedAttackLevel);

            }
            else if (upgradeType == "Shotgun" && shotgunSouls >= shotgunUpgradeCost && shotgunAttackLevel < maxUpgradeForWeapon)
            {
                shotgunAttackLevel += 1;
                attackTypeShotgun.GetComponent<DoDamage>().damage = shotgunInitialDamage * shotgunAttackLevel;
                shotgunSouls -= shotgunUpgradeCost;
                shotgunUpgradeCost *= 2;
                shotgunAttackUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(shotgunAttackLevel);
            }
            else
            {
                //case occurs when player selects weapon that they do not have enough to upgrade
                Debug.Log("You do not have enough " + upgradeType + " type souls to upgrade this attack.");
                //instantiate here a warning that the player does not have enough souls for the attack upgrade
                numberOfUpgrades -= 1;
            }
            energySoulsText.text = energySouls.ToString();
            beamSoulsText.text = beamSouls.ToString();
            bombSoulsText.text = bombSouls.ToString();
            speedSoulsText.text = speedSouls.ToString();
            shotgunSoulsText.text = shotgunSouls.ToString();
        }
        else
        {
            //display that the player has reached their max number of upgrades
        }

    }

    public void DeactivateLaser()
    {
        middleOfLaser.SetActive(false);
        laserStartParticles.SetActive(false);
        laserHitParticles.SetActive(false);
    }

}
