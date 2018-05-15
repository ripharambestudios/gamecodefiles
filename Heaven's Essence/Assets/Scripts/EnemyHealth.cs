using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{

    public int startHealth = 100;
    public int scoreValue = 0;
    private GameObject enemyManager;
    public AudioClip enemyHitSound;

    private AudioSource source;

    private int currentHealth;
    private GameObject player;
    private new ParticleSystem particleSystem; 
    public GameObject pool;
    private bool pulse = true;
    private float timeForFlashRed = .1f;
    private float timeForInvinciblity = 0;
    private bool invincible;
    private bool soul; 
    public string poolName;
    public GameObject Soul;
    public Color spriteColor; 
    



    // Use this for initialization
    void Start()
    {
        currentHealth = startHealth;
        enemyManager = GameObject.FindGameObjectWithTag("Enemy Manager");
        player = GameObject.Find("MainCharacter");
        source = gameObject.AddComponent<AudioSource>();
        pool = GameObject.FindGameObjectWithTag(poolName);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        invincible = false;
        Soul.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, .5f); 
        Soul.SetActive(false);
        if (this.gameObject.tag != "EnemyBoom")
        {
            particleSystem = GetComponentInChildren<ParticleSystem>();
        }
    }

    // Message sent from player that does damage to enemy
    public void DealDamage(int damage)
    {

        
        if (IsBelowThirtyFivePercent() && !invincible)
        {
            timeForInvinciblity = 1f;
            Flash(); 

        }
        invincible = true;

        if (timeForInvinciblity > 0)
        {
            timeForInvinciblity -= Time.deltaTime;
        }
        else
        {
            source.PlayOneShot(enemyHitSound, .05f);
            currentHealth -= damage;
            if (damage == 1000000)
            {
                scoreValue -= (scoreValue / 5);
            }

            if (currentHealth <= 0)
            {
                enemyManager.GetComponent<EnemySpawner>().decrementNumOfEnemies();
                player.SendMessage("UpdateScore", scoreValue);
                int numOfObjects = this.transform.childCount;
                for (int i = 0; i < numOfObjects; i++)
                {
                    if(this.transform.GetChild(i).tag == "Enemy" && gameObject.activeSelf)
                    {
                        Destroy(this.transform.GetChild(i).gameObject);
                    }
                }
                currentHealth = startHealth;

                pool.GetComponent<PoolingSystem>().returnToPool(gameObject);
            }

            if (currentHealth > 0 && this.gameObject.activeInHierarchy)
            {
				if (this.gameObject.GetComponent<SpriteRenderer> ().enabled == true && this.gameObject.activeInHierarchy) 
				{
					StartCoroutine (flashRed ());
				}
            }
        }
    }

    public int GetEnemyHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Checks health of enemy to see if they can be absorbed.
    /// Enemy health must be 35 percent or lower to be absorbed.
    /// </summary>
    /// <returns></returns>
    public bool IsBelowThirtyFivePercent()
    {
        int aTenth = startHealth / 10;
        if (currentHealth <= aTenth * 3.5f)
        {
            Soul.SetActive(true);
            if (this.gameObject.tag != "EnemyBoom")
            {
                particleSystem.Stop();

            }
            if (timeForInvinciblity <= 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);
                
                
            }

            Flash();

            return true;
        }
        else
        {
            return false;
        }
    }
    public void Flash()
    {
       if (pulse == true) {

            if(Soul.GetComponent<SpriteRenderer>().color.a >= 1)
            {
                pulse = false;
                Soul.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Soul.GetComponent<SpriteRenderer>().color.a - .1f);


            }
            else

                Soul.GetComponent<SpriteRenderer>().color = new Color (spriteColor.r, spriteColor.g, spriteColor.b, Soul.GetComponent<SpriteRenderer>().color.a +.1f);
        
               
        }
        else
        {
            if (Soul.GetComponent<SpriteRenderer>().color.a <= .2f) {
                pulse = true;
                Soul.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Soul.GetComponent<SpriteRenderer>().color.a + .1f);

            }

            else
                Soul.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, Soul.GetComponent<SpriteRenderer>().color.a - .1f);






        }
    }

    IEnumerator flashRed()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, .17647f, .17647f, 1);
        yield return new WaitForSeconds(timeForFlashRed);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    /// <summary>
    /// Reset information dealing with the start of the enemy.
    /// Used for when the enemy is returned to the pool of objects.
    /// </summary>
    public void ResetInfo()
    {
        currentHealth = startHealth;
        enemyManager = GameObject.FindGameObjectWithTag("Enemy Manager");
        player = GameObject.FindGameObjectWithTag("Player");
        source = gameObject.AddComponent<AudioSource>();
        pool = GameObject.FindGameObjectWithTag(poolName);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        invincible = false;
    }
}