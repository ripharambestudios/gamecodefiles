using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public GameObject attackSpawn;

    [Header("Attack Types")]
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

    [Header("Particle Effects")]
    // the wing particle effect
    public GameObject wingParticleEffect;

    [Header("Audio Clips")]
    public AudioClip standardFireSound;
    public AudioClip beamFireSound;
    public AudioClip bombFireSound;
    public AudioClip fastShotFireSound;
    public AudioClip shotGunFireSound;
    public AudioClip bombExplodeSound;
	public AudioClip enemyAbsorbed;

    [Header("Laser Parts")]
    public GameObject laserParticles;
    public GameObject laserMiddle;
    private float speedOfProjectile = 1f;
    private float rateOfFire = 4.0f;

    [Header("Text")]
    //Attack upgrades and souls text
    public Text energySoulsText;
    public Text beamSoulsText;
    public Text bombSoulsText;
    public Text speedSoulsText;
    public Text shotgunSoulsText;
	public Text errorMessageText;

    private AudioSource source;

    private GameObject projectile;
    private GameObject attackType;
    private float _rateOfFire;
    private Vector2 attackAngle = Vector2.zero;
    private Vector2 aimLocation = Vector2.zero;
    private bool canAttack = true;

    private bool startedOnce = false; //for beam timer

    private float chargeTime = 0; //used to track how long right click is held for energy attack alternate attack
    private float maxCharge = 6f;  //time player needs to hold down for alt attack to fire
    private float laserTimer = 0f; //time for laser to do damage
    private float leftLaserTimer = 0f;
    private float rightLaserTimer = 0f;
    private float maxLaserTime = .5f;

    //Number of souls player has obtained
    private int energySouls = 0;
    private int beamSouls = 0;
    private int bombSouls = 0;
    private int shotgunSouls = 0;
    private int speedSouls = 0;
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
    private int maxNumOfUpgrades = 35;
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
    private GameObject leftSideLaser;
    private GameObject rightSideLaser;

    // Use this for initialization
    void Start()
    {
        //set attack to initial energy ball with start properties
        projectile = projectileEnergy;
        attackType = attackTypeEnergy;
        speedOfProjectile = 2f;
        rateOfFire = 6.5f;
        spookyGuyProjectileSpeed = 2.4f;

        source = gameObject.AddComponent<AudioSource>();

        //reset attack damage values otherwise they infinitely scale
        attackTypeEnergy.GetComponent<DoDamage>().damage = 5;
        attackTypeBeam.GetComponent<DoDamage>().damage = 1;
        attackTypeBomb.GetComponent<DoDamage>().damage = 6;
        attackTypeSpeed.GetComponent<DoDamage>().damage = 2;
        //save initial damage done by attacks
        energyInitialDamage = attackTypeEnergy.GetComponent<DoDamage>().damage;
        beamInitialDamage = attackTypeBeam.GetComponent<DoDamage>().damage;
        bombInitialDamage = attackTypeBomb.GetComponent<DoDamage>().damage;
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
        laserStartParticles = (GameObject)Instantiate(laserParticles, attackSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        laserStartParticles.SetActive(false);
        laserHitParticles = (GameObject)Instantiate(laserParticles, attackSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        laserHitParticles.SetActive(false);

        //create offshoot lasers
        leftSideLaser = (GameObject)Instantiate(laserMiddle, attackSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        leftSideLaser.transform.parent = this.transform;
        leftSideLaser.SetActive(false);
        rightSideLaser = (GameObject)Instantiate(laserMiddle, attackSpawn.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        rightSideLaser.transform.parent = this.transform;
        rightSideLaser.SetActive(false);
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
        laserTimer += Time.deltaTime;
        leftLaserTimer += Time.deltaTime;
        rightLaserTimer += Time.deltaTime;
    }


    public void Aim(Vector2 aimTarget)
    {
        aimLocation = aimTarget;
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
                    StartCoroutine(shotgunShot((Vector2)attackSpawn.transform.position, attackAngle, speedOfProjectile));
                    StartCoroutine(Cooldown(_rateOfFire));

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
        source.PlayOneShot(beamFireSound, .05f); // need assistance from chandler on this one
        //destroy object if it doesn't collide with anything after timeout amout of time
        if (!middleOfLaser.activeInHierarchy)
        {
            middleOfLaser.SetActive(true);
        }
        if (!laserStartParticles.activeInHierarchy)
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

        float maxLaser = 200f;
        int layerDepth = 1;
        int layerMask = layerDepth << 9; //enemies on 9th layer
        int layerMaskPlanet = layerDepth << 12; //the planets
        //raycast to both planets and enemies, to check which one the laser hits first
        RaycastHit2D hit = Physics2D.Raycast(start, next, maxLaser, layerMask);
        RaycastHit2D hitPlanet = Physics2D.Raycast(start, next, maxLaser, layerMaskPlanet);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(start.x, start.y, 0);
        positions[1] = new Vector3(next.x * 1000, next.y * 1000, 0);
        if (hit.collider != null && hitPlanet.collider != null)
        {
            float distanceToPlanet = Math.Abs(Vector2.Distance(start, hitPlanet.point));
            float distanceToEnemy = Math.Abs(Vector2.Distance(start, hit.point));
            if (distanceToEnemy <= distanceToPlanet)
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hit.point;
                positions[1] = new Vector3(hit.point.x, hit.point.y, 0);
                if (laserTimer >= maxLaserTime)
                {
                    laserTimer = 0f;
                    Instantiate(attackType, hit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
            }
            else
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hitPlanet.point;
                positions[1] = new Vector3(hitPlanet.point.x, hitPlanet.point.y, 0);
            }
        }
        else if (hit.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hit.point;
            positions[1] = new Vector3(hit.point.x, hit.point.y, 0);
            if (laserTimer >= maxLaserTime)
            {
                laserTimer = 0f;
                Instantiate(attackType, hit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
            }

        }
        else if (hitPlanet.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hitPlanet.point;
            positions[1] = new Vector3(hitPlanet.point.x, hitPlanet.point.y, 0);
        }

        middleOfLaser.GetComponent<LineRenderer>().SetPositions(positions);
        middleOfLaser.GetComponent<LineRenderer>().SetWidth(.6f, .45f);

        yield return null;

    }

    //fire powerful beam with two offshoots
    //offshoots slightly not correct
    IEnumerator fireTripleBeam(Vector2 start, Vector2 next)
    {
        source.PlayOneShot(beamFireSound, .075f); // need assistance from chandler on this one
        if (!middleOfLaser.activeInHierarchy)
        {
            middleOfLaser.SetActive(true);
        }
        if (!leftSideLaser.activeInHierarchy)
        {
            leftSideLaser.SetActive(true);
        }
        if (!rightSideLaser.activeInHierarchy)
        {
            rightSideLaser.SetActive(true);
        }
        if (!laserStartParticles.activeInHierarchy)
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
        int rotationAmount = 25;
        middleOfLaser.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        leftSideLaser.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        rightSideLaser.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        leftSideLaser.transform.rotation = Quaternion.Euler(leftSideLaser.transform.rotation.eulerAngles.x, leftSideLaser.transform.rotation.eulerAngles.y, (leftSideLaser.transform.rotation.eulerAngles.z) + rotationAmount);
        rightSideLaser.transform.rotation = Quaternion.Euler(rightSideLaser.transform.rotation.eulerAngles.x, rightSideLaser.transform.rotation.eulerAngles.y, (rightSideLaser.transform.rotation.eulerAngles.z) - rotationAmount);

        float maxLaser = 200f;

        int layerDepth = 1;
        int layerMask = layerDepth << 9; //enemies on 9th layer
        int layerMaskPlanet = layerDepth << 12;
        float size = (float)Math.Sqrt(next.x * next.x + next.y * next.y);

        Vector2 nextLeft = new Vector2((float)Math.Cos(leftSideLaser.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size, (float)Math.Sin(leftSideLaser.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size);
        Vector2 nextRight = new Vector2((float)Math.Cos(rightSideLaser.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size, (float)Math.Sin(rightSideLaser.transform.rotation.eulerAngles.z * (Math.PI / 180)) * size);

        RaycastHit2D hit = Physics2D.Raycast(middleOfLaser.transform.position, next, maxLaser, layerMask);
        RaycastHit2D hitPlanet = Physics2D.Raycast(middleOfLaser.transform.position, next, maxLaser, layerMaskPlanet);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(start.x, start.y, 0);
        positions[1] = new Vector3(next.x * 1000, next.y * 1000, 0);
        if (hit.collider != null && hitPlanet.collider != null)
        {
            float distanceToPlanet = Math.Abs(Vector2.Distance(start, hitPlanet.point));
            float distanceToEnemy = Math.Abs(Vector2.Distance(start, hit.point));
            if (distanceToEnemy <= distanceToPlanet)
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hit.point;
                positions[1] = new Vector3(hit.point.x, hit.point.y, 0);
                if (laserTimer >= maxLaserTime)
                {
                    laserTimer = 0f;
                    Instantiate(attackType, hit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
            }
            else
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hitPlanet.point;
                positions[1] = new Vector3(hitPlanet.point.x, hitPlanet.point.y, 0);
            }
        }
        else if (hit.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hit.point;
            positions[1] = new Vector3(hit.point.x, hit.point.y, 0);
            if (laserTimer >= maxLaserTime)
            {
                laserTimer = 0f;
                Instantiate(attackType, hit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
        else if (hitPlanet.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hitPlanet.point;
            positions[1] = new Vector3(hitPlanet.point.x, hitPlanet.point.y, 0);
        }

        RaycastHit2D hitLeft = Physics2D.Raycast(leftSideLaser.transform.position, nextLeft, maxLaser, layerMask);
        RaycastHit2D hitLeftPlanet = Physics2D.Raycast(leftSideLaser.transform.position, nextLeft, maxLaser, layerMaskPlanet);
        Vector3[] positionsLeft = new Vector3[2];
        positionsLeft[0] = new Vector3(start.x, start.y, 0);
        positionsLeft[1] = new Vector3(nextLeft.x * 1000, nextLeft.y * 1000, 0);
        if (hitLeft.collider != null && hitLeftPlanet.collider != null)
        {
            float distanceToPlanet = Math.Abs(Vector2.Distance(start, hitLeftPlanet.point));
            float distanceToEnemy = Math.Abs(Vector2.Distance(start, hitLeft.point));
            if (distanceToEnemy <= distanceToPlanet)
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hitLeft.point;
                positionsLeft[1] = new Vector3(hitLeft.point.x, hitLeft.point.y, 0);
                if (leftLaserTimer >= maxLaserTime)
                {
                    leftLaserTimer = 0f;
                    Instantiate(attackType, hitLeft.point, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
            }
            else
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hitLeftPlanet.point;
                positionsLeft[1] = new Vector3(hitLeftPlanet.point.x, hitLeftPlanet.point.y, 0);
            }
        }
        else if (hitLeft.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hitLeft.point;
            positionsLeft[1] = new Vector3(hitLeft.point.x, hitLeft.point.y, 0);
            if (leftLaserTimer >= maxLaserTime)
            {
                leftLaserTimer = 0f;
                Instantiate(attackType, hitLeft.point, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
        else if (hitLeftPlanet.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hitLeftPlanet.point;
            positionsLeft[1] = new Vector3(hitLeftPlanet.point.x, hitLeftPlanet.point.y, 0);
        }
        RaycastHit2D hitRight = Physics2D.Raycast(rightSideLaser.transform.position, nextRight, maxLaser, layerMask);
        RaycastHit2D hitRightPlanet = Physics2D.Raycast(rightSideLaser.transform.position, nextRight, maxLaser, layerMaskPlanet);
        Vector3[] positionsRight = new Vector3[2];
        positionsRight[0] = new Vector3(start.x, start.y, 0);
        positionsRight[1] = new Vector3(nextRight.x * 1000, nextRight.y * 1000, 0);
        if (hitRight.collider != null && hitRightPlanet.collider != null)
        {
            float distanceToPlanet = Math.Abs(Vector2.Distance(start, hitRightPlanet.point));
            float distanceToEnemy = Math.Abs(Vector2.Distance(start, hitRight.point));
            if (distanceToEnemy <= distanceToPlanet)
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hitRight.point;
                positionsRight[1] = new Vector3(hitRight.point.x, hitRight.point.y, 0);
                if (rightLaserTimer >= maxLaserTime)
                {
                    rightLaserTimer = 0f;
                    Instantiate(attackType, hitRight.point, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
            }
            else
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hitRightPlanet.point;
                positionsRight[1] = new Vector3(hitRightPlanet.point.x, hitRightPlanet.point.y, 0);
            }
        }
        else if (hitRight.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hitRight.point;
            positionsRight[1] = new Vector3(hitRight.point.x, hitRight.point.y, 0);
            if (rightLaserTimer >= maxLaserTime)
            {
                rightLaserTimer = 0f;
                Instantiate(attackType, hitRight.point, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
        else if (hitRightPlanet.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hitRightPlanet.point;
            positionsRight[1] = new Vector3(hitRightPlanet.point.x, hitRightPlanet.point.y, 0);
        }

        middleOfLaser.GetComponent<LineRenderer>().SetPositions(positions);
        middleOfLaser.GetComponent<LineRenderer>().SetWidth(.6f, .45f);

        leftSideLaser.GetComponent<LineRenderer>().SetPositions(positionsLeft);
        leftSideLaser.GetComponent<LineRenderer>().SetWidth(.6f, .35f);

        rightSideLaser.GetComponent<LineRenderer>().SetPositions(positionsRight);
        rightSideLaser.GetComponent<LineRenderer>().SetWidth(.6f, .35f);

        yield return null;

    }

    //Shoots large laser beam that does more damage
    //HIT BOX IS NOT CORRECT CURRENTLY, LINE RAYCAST NEXT TO EACH OTHER
    IEnumerator bigBeam(Vector2 start, Vector2 next)
    {
        source.PlayOneShot(beamFireSound, .1f); // need assistance from chandler on this one
        //destroy object if it doesn't collide with anything after timeout amout of time
        //destroy object if it doesn't collide with anything after timeout amout of time
        if (!middleOfLaser.activeInHierarchy)
        {
            middleOfLaser.SetActive(true);
        }
        if (!laserStartParticles.activeInHierarchy)
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

        float maxLaser = 200f;

        int layerDepth = 1;
        int layerMask = layerDepth << 9; //enemies on 9th layer
        int layerMaskPlanet = layerDepth << 12; //the planets
        //raycast to both planets and enemies, to check which one the laser hits first
        RaycastHit2D hit = Physics2D.Raycast(start, next, maxLaser, layerMask); //create a perpendicular line that contains the stacked raycasts as it aims
        RaycastHit2D hitPlanet = Physics2D.Raycast(start, next, maxLaser, layerMaskPlanet);
        Vector3[] positions = new Vector3[2];
        positions[0] = new Vector3(start.x, start.y, 0);
        positions[1] = new Vector3(next.x * 1000, next.y * 1000, 0);
        if (hit.collider != null && hitPlanet.collider != null)
        {
            float distanceToPlanet = Math.Abs(Vector2.Distance(start, hitPlanet.point));
            float distanceToEnemy = Math.Abs(Vector2.Distance(start, hit.point));
            if (distanceToEnemy <= distanceToPlanet)
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hit.point;
                positions[1] = new Vector3(hit.point.x, hit.point.y, 0);
                if (laserTimer >= maxLaserTime)
                {
                    laserTimer = 0f;
                    Instantiate(attackType, hit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
                }
            }
            else
            {
                if (!laserHitParticles.activeInHierarchy)
                {
                    laserHitParticles.SetActive(true);
                }
                laserHitParticles.transform.position = hitPlanet.point;
                positions[1] = new Vector3(hitPlanet.point.x, hitPlanet.point.y, 0);
            }
        }
        else if (hit.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hit.point;
            positions[1] = new Vector3(hit.point.x, hit.point.y, 0);
            if (laserTimer >= maxLaserTime)
            {
                laserTimer = 0f;
                Instantiate(attackType, hit.point, Quaternion.Euler(new Vector3(0, 0, 0)));
            }
        }
        else if (hitPlanet.collider != null)
        {
            if (!laserHitParticles.activeInHierarchy)
            {
                laserHitParticles.SetActive(true);
            }
            laserHitParticles.transform.position = hitPlanet.point;
            positions[1] = new Vector3(hitPlanet.point.x, hitPlanet.point.y, 0);
        }

        middleOfLaser.GetComponent<LineRenderer>().SetPositions(positions);
        middleOfLaser.GetComponent<LineRenderer>().SetWidth(3f, 2.25f);


        yield return null;
    }


    //fires bomb projectile 
    IEnumerator bombShot(Vector2 start, Vector2 next, float attackSpeed, int splitAmount)
    {
        yield return null;
        //destroy object if it doesn't collide with anything after timeout amout of time
        float timeout = 3f;

        source.PlayOneShot(bombFireSound, .035f);

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
            if (Time.timeScale != 0)
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
                        source.PlayOneShot(bombExplodeSound, .075f);
                        Instantiate(attackType, impact.point, Quaternion.identity);
                    }
                    else if (splitAmount == 3)
                    {
                        source.PlayOneShot(bombExplodeSound, .15f);
                        Instantiate(attackType, impact.point + new Vector2(0, 5), Quaternion.identity);
                        Instantiate(attackType, impact.point + new Vector2(-5, -5), Quaternion.identity);
                        Instantiate(attackType, impact.point + new Vector2(5, -5), Quaternion.identity);
                    }
                    else if (splitAmount == 5)
                    {
                        source.PlayOneShot(bombExplodeSound, .3f);
                        Instantiate(attackType, impact.point + new Vector2(0, 0), Quaternion.identity);
                        Instantiate(attackType, impact.point + new Vector2(-5, -5), Quaternion.identity);
                        Instantiate(attackType, impact.point + new Vector2(5, -5), Quaternion.identity);
                        Instantiate(attackType, impact.point + new Vector2(5, 5), Quaternion.identity);
                        Instantiate(attackType, impact.point + new Vector2(-5, 5), Quaternion.identity);
                    }
                    hit = true;
                }
                createProjectile.transform.position = nextPosition;
            }
            yield return null;
        }
        if (!hit && createProjectile != null)
        {
            if (splitAmount == 1)
            {
                source.PlayOneShot(bombExplodeSound, .075f);
                Instantiate(attackType, createProjectile.transform.position, Quaternion.identity);
            }
            else if (splitAmount == 3)
            {
                source.PlayOneShot(bombExplodeSound, .15f);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(0, 5), Quaternion.identity);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(-5, -5), Quaternion.identity);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(5, -5), Quaternion.identity);
            }
            else if (splitAmount == 5)
            {
                source.PlayOneShot(bombExplodeSound, .3f);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(0, 0), Quaternion.identity);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(-5, -5), Quaternion.identity);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(5, -5), Quaternion.identity);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(5, 5), Quaternion.identity);
                Instantiate(attackType, createProjectile.transform.position + new Vector3(-5, 5), Quaternion.identity);
            }
        }
        Destroy(createProjectile);
    }


    //charges up large shot
    //Needs to be adjusted
    IEnumerator energyShot(Vector2 start, Vector2 next, float attackSpeed, float scaleFactor)
    {

        //destroy object if it doesn't collide with anything after timeout amout of time
        float timeout = 1.5f;
        source.PlayOneShot(standardFireSound, .025f);

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
            if (Time.timeScale != 0)
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
            }
            yield return null;
        }

        Destroy(createProjectile);
    }

    //shoots double bullets, drains off ammunition twice as fast
    IEnumerator spookyGuyProjectile(Vector2 start, Vector2 next, float attackSpeed, int penetrationPower)
    {
        yield return null;
        //destroy object if it doesn't collide with anything after timeout amout of time
        float timeout = 1.5f;
        source.PlayOneShot(fastShotFireSound, .035f);

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
            if (Time.timeScale != 0)
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
        source.PlayOneShot(shotGunFireSound, .05f);

        GameObject createProjectile = (GameObject)Instantiate(projectile, start, Quaternion.Euler(new Vector3(0, 0, 0))); //make it kinda work: Euler (new Vector3(0,0,0))
        createProjectile.transform.parent = this.transform;
        createProjectile.gameObject.GetComponent<ShotgunKnockback>().upgradeLevel = shotgunAttackLevel; // changes shot gun level


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
            if (Time.timeScale != 0)
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
            }

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
		source.PlayOneShot (enemyAbsorbed, .1f);
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
            energySouls += 1;
            bombSouls += 1;
        }
        else if (attackTypeString == "SpookyGuy(Clone)")
        {

            energySouls += 1;
            speedSouls += 2;
        }
        else if (attackTypeString == "FallenGuy(Clone)")
        {
            energySouls += 1;
            shotgunSouls += 3;
        }


		energySoulsText.text = energySouls.ToString();
		if (energySouls >= energyUpgradeCost) 
		{
			energySoulsText.color = new Color (1, .80392f, 0, 1);
		} 
		else 
		{
			energySoulsText.color = new Color (0, 0, 0, 1);
		}

		beamSoulsText.text = beamSouls.ToString();
		if (beamSouls >= beamUpgradeCost) 
		{
			beamSoulsText.color = new Color (1, .80392f, 0, 1);
		} 
		else 
		{
			beamSoulsText.color = new Color (0, 0, 0, 1);
		}

		bombSoulsText.text = bombSouls.ToString();
		if (bombSouls >= bombUpgradeCost) 
		{
			bombSoulsText.color = new Color (1, .80392f, 0, 1);
		} 
		else 
		{
			bombSoulsText.color = new Color (0, 0, 0, 1);
		}

		speedSoulsText.text = speedSouls.ToString();
		if (speedSouls >= spookyGuyUpgradeCost) 
		{
			speedSoulsText.color = new Color (1, .80392f, 0, 1);
		} 
		else 
		{
			speedSoulsText.color = new Color (0, 0, 0, 1);
		}

		shotgunSoulsText.text = shotgunSouls.ToString();
		if (shotgunSouls >= shotgunUpgradeCost) 
		{
			shotgunSoulsText.color = new Color (1, .80392f, 0, 1);
		} 
		else 
		{
			shotgunSoulsText.color = new Color (0, 0, 0, 1);
		}
    }

	public void SwitchAttacks(string attackTypeString)
    {
        this.GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		if (attackTypeString == "Energy") 
		{
			projectile = projectileEnergy;
			attackType = attackTypeEnergy;
			speedOfProjectile = 2f;
			rateOfFire = 6.5f;
            DeactivateLaser();
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
			speedOfProjectile = .5f;
			rateOfFire = 1.0f;
            DeactivateLaser();
        } 
		else if (attackTypeString == "Speed" && speedAttackLevel > 0) 
		{
			projectile = projectileSpeed;
			attackType = attackTypeSpeed;
			speedOfProjectile = spookyGuyProjectileSpeed;
			rateOfFire = 15.0f;
            DeactivateLaser();
        } 
		else if (attackTypeString == "Shotgun" && shotgunAttackLevel > 0)
		{
			projectile = projectileShotgun;
			attackType = attackTypeShotgun;
			speedOfProjectile = 1.4f;
			rateOfFire = 4.5f;
            DeactivateLaser();
        } 
		else 
		{
			StartCoroutine (changeErrorText ("You don't have enough souls to upgrade and that weapon isn't unlocked"));
		}
    }

	public void UpgradeAttack(string upgradeType)
    {
        if (numberOfUpgrades <= maxNumOfUpgrades)
        {
            if (upgradeType == "Energy" && energySouls >= energyUpgradeCost && energyAttackLevel < maxUpgradeForWeapon)
            {
				numberOfUpgrades += 1;
                energyAttackLevel += 1;
                attackTypeEnergy.GetComponent<DoDamage>().damage = energyInitialDamage * energyAttackLevel;
                energySouls -= energyUpgradeCost;
                energyUpgradeCost *= 2;
                energyShotUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(energyAttackLevel);
            }
            else if (upgradeType == "Beam" && beamSouls >= beamUpgradeCost && beamAttackLevel < maxUpgradeForWeapon)
            {
				numberOfUpgrades += 1;
                beamAttackLevel += 1;
                attackTypeBeam.GetComponent<DoDamage>().damage = beamInitialDamage * beamAttackLevel;
                beamSouls -= beamUpgradeCost;
                beamUpgradeCost *= 2;
                beamAttackUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(beamAttackLevel);
            }
            else if (upgradeType == "Bomb" && bombSouls >= bombUpgradeCost && bombAttackLevel < maxUpgradeForWeapon)
            {
				numberOfUpgrades += 1;
                bombAttackLevel += 1;
                attackTypeBomb.GetComponent<DoDamage>().damage = bombInitialDamage * bombAttackLevel;
                bombSouls -= bombUpgradeCost;
                bombUpgradeCost *= 2;
                bombAttackUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(bombAttackLevel);

            }
            else if (upgradeType == "Speed" && speedSouls >= spookyGuyUpgradeCost && speedAttackLevel < maxUpgradeForWeapon)
            {
				numberOfUpgrades += 1;
                speedAttackLevel += 1;
                attackTypeSpeed.GetComponent<DoDamage>().damage = spookyGuyInitialDamage * speedAttackLevel;
                speedSouls -= spookyGuyUpgradeCost;
                spookyGuyUpgradeCost *= 2;
                spookyGuyProjectileSpeed = 1.2f + (speedAttackLevel * .1f);
                speedShotUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(speedAttackLevel);

            }
            else if (upgradeType == "Shotgun" && shotgunSouls >= shotgunUpgradeCost && shotgunAttackLevel < maxUpgradeForWeapon)
            {
				numberOfUpgrades += 1;
                shotgunAttackLevel += 1;
                shotgunSouls -= shotgunUpgradeCost;
                shotgunUpgradeCost *= 2;
                shotgunAttackUpgradeIcon.GetComponent<SpriteForUpgradeChange>().setSpriteLevel(shotgunAttackLevel);
            }
            else
            {
				StartCoroutine (changeErrorText ("You do not have enough " + upgradeType + " type souls to upgrade this attack."));
            }

            energySoulsText.text = energySouls.ToString();
			if (energySouls >= energyUpgradeCost) 
			{
				energySoulsText.color = new Color (1, .80392f, 0, 1);
			} 
			else 
			{
				energySoulsText.color = new Color (0, 0, 0, 1);
			}

            beamSoulsText.text = beamSouls.ToString();
			if (beamSouls >= beamUpgradeCost) 
			{
				beamSoulsText.color = new Color (1, .80392f, 0, 1);
			} 
			else 
			{
				beamSoulsText.color = new Color (0, 0, 0, 1);
			}

            bombSoulsText.text = bombSouls.ToString();
			if (bombSouls >= bombUpgradeCost) 
			{
				bombSoulsText.color = new Color (1, .80392f, 0, 1);
			} 
			else 
			{
				bombSoulsText.color = new Color (0, 0, 0, 1);
			}

            speedSoulsText.text = speedSouls.ToString();
			if (speedSouls >= spookyGuyUpgradeCost) 
			{
				speedSoulsText.color = new Color (1, .80392f, 0, 1);
			} 
			else 
			{
				speedSoulsText.color = new Color (0, 0, 0, 1);
			}

            shotgunSoulsText.text = shotgunSouls.ToString();
			if (shotgunSouls >= shotgunUpgradeCost) 
			{
				shotgunSoulsText.color = new Color (1, .80392f, 0, 1);
			} 
			else 
			{
				shotgunSoulsText.color = new Color (0, 0, 0, 1);
			}
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
        leftSideLaser.SetActive(false);
        rightSideLaser.SetActive(false);
    }

	IEnumerator changeErrorText(string text)
	{
		errorMessageText.text = text;
		errorMessageText.enabled = true;
		yield return new WaitForSeconds(3.0f);
		errorMessageText.enabled = false;
	}

}
