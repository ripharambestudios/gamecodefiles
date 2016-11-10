using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int startHealth = 100;
	public int scoreValue= 0;
	private GameObject enemyManager;

	private int currentHealth;
	private GameObject player;

	private double timeForFlash = .5f;

	// Use this for initialization
	void Start () {
		currentHealth = startHealth;
		enemyManager = GameObject.FindGameObjectWithTag ("Enemy Manager");
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Message sent from player that does damage to enemy
	public void DealDamage (int damage) {
		currentHealth -= damage;
		if (damage == 1000000) {
			scoreValue -= (scoreValue / 5);
		}
		Debug.Log ("I am taking damage");
		if (currentHealth <= 0) {
			enemyManager.GetComponent<EnemySpawner> ().decrementNumOfEnemies ();
			player.SendMessage ("UpdateScore", scoreValue, SendMessageOptions.DontRequireReceiver);
			Destroy (this.gameObject);
		}
	}

    public int GetEnemyHealth()
    {
        return currentHealth;
    }

    public bool IsBelowTwentyPercent()
    {
        int aTenth = startHealth / 10;
        if(currentHealth <= aTenth*2)
        {
			timeForFlash -= Time.deltaTime;
			if (timeForFlash <= 0)
			{
				if (this.gameObject.GetComponent<SpriteRenderer> ().color == new Color (1, 1, 1, 1))
				{
					this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, .792156f, 0, 1);
				} 
				else 
				{
					this.gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
				}
				timeForFlash = .5f;
			}
            return true;
        }
        else
        {
            return false;
        }
    }
}
