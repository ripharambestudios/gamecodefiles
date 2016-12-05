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

    public GameObject pool;
    private bool pulseGold;
    private float timeForFlashRed = .1f;
    private float timeForInvinciblity = 0;
    private bool invincible = false;
    public string poolName;



    // Use this for initialization
    void Start()
    {
        currentHealth = startHealth;
        enemyManager = GameObject.FindGameObjectWithTag("Enemy Manager");
        player = GameObject.FindGameObjectWithTag("Player");
        source = gameObject.AddComponent<AudioSource>();
        pool = GameObject.FindGameObjectWithTag(poolName);


    }

    // Message sent from player that does damage to enemy
    public void DealDamage(int damage)
    {
        if (IsBelowThirtyFivePercent() && !invincible)
            timeForInvinciblity = 1f;
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
                player.SendMessage("UpdateScore", scoreValue, SendMessageOptions.DontRequireReceiver);
                int numOfObjects = this.transform.childCount;
                for(int i =0; i < numOfObjects; i++)
                {
                    if(this.transform.GetChild(i).tag == "Enemy")
                    {
                        Destroy(this.transform.GetChild(i));
                    }
                }
                currentHealth = startHealth;
                this.gameObject.GetComponentInChildren<Light>().intensity = 0f;
                pool.GetComponent<PoolingSystem>().returnToPool(gameObject);
            }

            if (currentHealth > 0)
            {
                StartCoroutine(flashRed());
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
            if (timeForInvinciblity <= 0)
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(.5f, .5f, .5f, 1);

            if (pulseGold == true)
            {

                this.gameObject.GetComponentInChildren<Light>().intensity += .1f;

                if (this.gameObject.GetComponentInChildren<Light>().intensity >= 1.5f)
                    pulseGold = false;
            }
            else
            {
                this.gameObject.GetComponentInChildren<Light>().intensity -= .1f;

                if (this.gameObject.GetComponentInChildren<Light>().intensity <= 0)
                    pulseGold = true;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator flashRed()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, .17647f, .17647f, 1);
        yield return new WaitForSeconds(timeForFlashRed);
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
}