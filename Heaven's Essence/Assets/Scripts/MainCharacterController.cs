using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class MainCharacterController : MonoBehaviour
{

	public float moveSpeed = 25;
	public int health = 1000;
	public GameObject healthBar;
	public GameObject gameOverPanel;
    public GameObject pausePanel;
	public Text HealthText;
	public Text ScoreText;
	public AudioClip playerHit;
	public AudioClip playerDeath;
	public GameObject soundObject;
	public AudioClip getHealthSound;
	public bool useController;
    public bool PS4Controller;
    public bool XBoxController;
    public GameObject reticle;

    private AudioSource source;
	private int currentHealth;
	private Vector2 characterVector;
	private Vector2 healthStartVector;
	private Rigidbody2D player;
	private int totalScore = 0;
	private List<string> attackTypes = new List<string>();
	private float timeForFlashRed = .1f;
	private float timeForFlashGreen = .25f;
    private bool laserActivated = false;
	private List<GameObject> pools;
    private GameObject aimReticle;

	// Use this for initialization
	void Start()
	{
		player = this.GetComponent<Rigidbody2D>();
		player.gravityScale = 0;
		currentHealth = health;
		//HealthText.text = "Health: ";   //+ currentHealth;
		gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
		ScoreText.text = "Score: " + totalScore;
		attackTypes.Add("Energy");
		attackTypes.Add("Beam");
		attackTypes.Add("Bomb");
		attackTypes.Add("Speed");
		attackTypes.Add("Shotgun");
		healthStartVector = healthBar.GetComponent<RectTransform>().sizeDelta;
		source = this.gameObject.AddComponent<AudioSource> ();
		pools = new List<GameObject>();
		pools.Add(GameObject.FindGameObjectWithTag("PoolDemonic"));
		pools.Add(GameObject.FindGameObjectWithTag("PoolSpook"));
		pools.Add(GameObject.FindGameObjectWithTag("PoolFallen"));
		pools.Add(GameObject.FindGameObjectWithTag("PoolBoom"));
        PS4Controller = false;
        XBoxController = false;
        //aimReticle = (GameObject)Instantiate(reticle, this.transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        //aimReticle.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
        if(Time.timeScale != 0)
        {
            characterVector.y = Input.GetAxis("Vertical");
            characterVector.x = Input.GetAxis("Horizontal");
            if(player.velocity.magnitude > 0)
            {
                player.velocity = Vector2.zero;
            }
            if (characterVector.x == 0 && characterVector.y == 0)
            {
                player.velocity = Vector2.zero;
            }
            player.transform.Translate(characterVector.x * moveSpeed * Time.deltaTime, characterVector.y * moveSpeed * Time.deltaTime, 0);
            if (!useController)
            {

                Vector3 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lookDirection.z = 0f;
                RaycastHit2D hit;
                float distance = 100000;
                if (Physics2D.Raycast(this.transform.position, lookDirection, distance))
                {
                    hit = Physics2D.Raycast(this.transform.position, lookDirection, distance);
                    this.GetComponent<Attack>().Aim(lookDirection);

                }
                if (Input.GetAxis("Fire1") > 0)
                {
                    this.GetComponent<Attack>().Fire();
                }
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    this.GetComponent<Attack>().DeactivateLaser();
                }
                if (Input.GetAxis("Fire2") > 0)
                {
                    this.GetComponent<Attack>().AltFire();
                }
                if (Input.GetKeyUp("r"))
                {
                    this.gameObject.SendMessage("EnemyAbsorbed", "Energy", SendMessageOptions.DontRequireReceiver);
                }

                if (Input.GetKeyUp(KeyCode.Alpha1) && !Input.GetKey(KeyCode.LeftShift))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[0]);
                    this.GetComponent<Attack>().SwitchAttacks(attackTypes[0]);
                    //check for numbers 1-5 and also for numbers plus control key.  send signal to upgrade attack or change attack
                }
                if (Input.GetKeyUp(KeyCode.Alpha2) && !Input.GetKey(KeyCode.LeftShift))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[1]);
                    this.GetComponent<Attack>().SwitchAttacks(attackTypes[1]);
                }
                if (Input.GetKeyUp(KeyCode.Alpha3) && !Input.GetKey(KeyCode.LeftShift))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[2]);
                    this.GetComponent<Attack>().SwitchAttacks(attackTypes[2]);
                }
                if (Input.GetKeyUp(KeyCode.Alpha4) && !Input.GetKey(KeyCode.LeftShift))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[3]);
                    this.GetComponent<Attack>().SwitchAttacks(attackTypes[3]);
                }
                if (Input.GetKeyUp(KeyCode.Alpha5) && !Input.GetKey(KeyCode.LeftShift))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[4]);
                    this.GetComponent<Attack>().SwitchAttacks(attackTypes[4]);
                }
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[0]);
                }
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[1]);
                }
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha3))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[2]);

                }
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha4))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[3]);

                }
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha5))
                {
                    this.GetComponent<Attack>().UpgradeAttack(attackTypes[4]);

                }
            }
            else
            {
                
                if (XBoxController)
                {
                    Vector3 playerDirection = new Vector3(10 * Input.GetAxis("XBoxRHorizontal") + this.transform.position.x, (-10) * Input.GetAxis("XBoxRVertical") + this.transform.position.y, 0);

                    if (Input.GetAxis("XBoxRHorizontal") != 0 || Input.GetAxis("XBoxRVertical") != 0)
                    {
                        this.GetComponent<Attack>().Aim(playerDirection);
                        this.GetComponent<Attack>().Fire();
                        laserActivated = true;
                    }

                    if ((Input.GetAxis("XBoxRHorizontal") == 0 && Input.GetAxis("XBoxRVertical") == 0) && laserActivated)
                    {
                        this.GetComponent<Attack>().DeactivateLaser();
                        laserActivated = false;
                    }
                    //x button and energy attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton2))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[0]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[0]);
                    }
                    //b button and beam attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton1))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[1]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[1]);
                    }
                    //right bumper and bomb attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton5))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[2]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[2]);
                    }
                    //y button and speed attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton3))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[3]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[3]);
                    }
                    //a button and shotgun attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton0))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[4]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[4]);
                    }
                }
                else if(PS4Controller)
                {
                    Vector3 playerDirection = new Vector3(10 * Input.GetAxis("PS4RHorizontal") + this.transform.position.x, (-10) * Input.GetAxis("PS4RVertical") + this.transform.position.y, 0);
                    
                    if (Input.GetAxis("PS4RHorizontal") != 0 || Input.GetAxis("PS4RVertical") != 0)
                    {
                        //aimReticle.transform.position = playerDirection;
                        //aimReticle.transform.localScale = new Vector3(5,5,1);
                        //aimReticle.SetActive(true);
                        
                        this.GetComponent<Attack>().Aim(playerDirection);
                        this.GetComponent<Attack>().Fire();
                        laserActivated = true;
                    }

                    if ((Input.GetAxis("PS4RHorizontal") == 0 && Input.GetAxis("PS4RVertical") == 0) && laserActivated)
                    {
                        this.GetComponent<Attack>().DeactivateLaser();
                        laserActivated = false;
                        //aimReticle.SetActive(false);
                    }
                    //square button and energy attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton0))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[0]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[0]);
                    }
                    //circle button and beam attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton2))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[1]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[1]);
                    }
                    //right bumper and bomb attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton5))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[2]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[2]);
                    }
                    //triangle button and speed attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton3))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[3]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[3]);
                    }
                    //x button and shotgun attack
                    if (Input.GetKeyUp(KeyCode.JoystickButton1))
                    {
                        this.GetComponent<Attack>().UpgradeAttack(attackTypes[4]);
                        this.GetComponent<Attack>().SwitchAttacks(attackTypes[4]);
                    }
                }
            }
        }
	}


	public void EnemyDamage(int damageDone)
	{
		currentHealth -= damageDone;
		StartCoroutine (flashRed());
		source.PlayOneShot (playerHit, .035f);
		if (currentHealth <= 0)
		{
			currentHealth = 0;
			gameOverPanel.SetActive(true);
			source.PlayOneShot (playerDeath, .75f);
			this.GetComponentInChildren<SpriteRenderer> ().enabled = false;
			this.GetComponentInChildren<BoxCollider2D> ().enabled = false;
			for(int i = 0; i < pools.Count; i++)
			{
				pools[i].GetComponent<PoolingSystem>().FalsePool();
			}
			Destroy(this.gameObject, playerDeath.length);
		}
		else if(currentHealth <= 400 && currentHealth > 200)
		{
			soundObject.GetComponent<AudioSource> ().pitch = 1.28f;
		}
		else if(currentHealth <= 200)
		{
			soundObject.GetComponent<AudioSource> ().pitch = 1.35f;
		}
		else if(currentHealth > 1000)
		{
			currentHealth = 1000;
		}
		setHealthBar();
	}

	public void ReturnHealth(int healthBack)
	{
		currentHealth += healthBack;
		source.PlayOneShot (getHealthSound, .1f);
		StartCoroutine (flashGreen());
		if(currentHealth > 1000)
		{
			currentHealth = 1000;
		}
		setHealthBar();
	}

	public int GetHealth()
	{
		return currentHealth;
	}

	public void UpdateScore(int score)
	{
		totalScore += score;
		ScoreText.text = "Score: " + totalScore;
	}

	private void setHealthBar()
	{
		if (currentHealth > 0)
		{
			//float normalizedHealth = (float)currentHealth / (float)health;
			//healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(currentHealth, 32);
			healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(healthStartVector.x * currentHealth / health, 22.34f);
			//float lostHealth = health - currentHealth;
		}
		else
		{
			healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
		}

	}

	IEnumerator flashRed()
	{
		this.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, .17647f, .17647f, 1);
		yield return new WaitForSeconds (timeForFlashRed);
		this.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
	}

	IEnumerator flashGreen()
	{
		this.GetComponentInChildren<SpriteRenderer> ().color = new Color (.17647f, 1, .17647f, 1);
		yield return new WaitForSeconds (timeForFlashGreen);
		this.GetComponentInChildren<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
	}
}