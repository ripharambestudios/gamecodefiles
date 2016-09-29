using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

	public int startHealth = 100;

	private int currentHealth;

	// Use this for initialization
	void Start () {
		currentHealth = startHealth;
	}
	
	// Message sent from player that does damage to enemy
	public void DealDamage (int damage) {
		currentHealth -= damage;
		Debug.Log ("I am taking damage");
		if (currentHealth <= 0) {
			Destroy (this.gameObject);
		}
	}
}
