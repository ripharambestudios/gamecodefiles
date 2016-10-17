using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainCharacterController : MonoBehaviour {

	public float moveSpeed = 10;
	public int health = 1000;
	public GameObject healthBar;
	public GameObject gameOverPanel;
	public Text HealthText;
	public Text ScoreText;

	private int currentHealth;
	private Vector2 characterVector;
	private Rigidbody2D player;
	private int totalScore = 0;


	// Use this for initialization
	void Start () {
		player = this.GetComponent<Rigidbody2D> ();
		player.gravityScale = 0;
		currentHealth = health;
		HealthText.text = "Health: ";   //+ currentHealth;
		gameOverPanel.SetActive (false);
		ScoreText.text = "Score: 0";
	}

	// Update is called once per frame
	void Update () {
		characterVector.y = Input.GetAxis ("Vertical");
		characterVector.x = Input.GetAxis ("Horizontal");

		player.transform.Translate (characterVector.x * moveSpeed * Time.deltaTime, characterVector.y * moveSpeed * Time.deltaTime, 0);

		//this line will need to change
		//this.transform.LookAt((Vector2)this.transform.position +characterVector);

		Vector3 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		lookDirection.z = 0f;
		RaycastHit2D hit;
		float distance = 100000;

		if (Physics2D.Raycast(this.transform.position, lookDirection, distance)) {
			hit = Physics2D.Raycast (this.transform.position, lookDirection, distance);
			this.GetComponent<Attack>().Aim (lookDirection);

		}
		if (Input.GetAxis ("Fire1") > 0) {
			this.GetComponent<Attack> ().Fire ();
		}

        if (Input.GetKeyUp("r"))
        {
            this.gameObject.SendMessage("EnemyAbsorbed", "Energy", SendMessageOptions.DontRequireReceiver);
        }

	}

	public void EnemyDamage(int damageDone){
		currentHealth -= damageDone;
		Debug.Log ("My current health: " + currentHealth);
		if (currentHealth <= 0) {
			currentHealth = 0;
			gameOverPanel.SetActive (true);
			Destroy (this.gameObject);
		}
		setHealthBar ();
	}

	public int GetHealth(){
		return currentHealth;
	}

	public void UpdateScore(int score){
		totalScore += score;
		ScoreText.text = "Score: " + totalScore;
	}

	private void setHealthBar()
	{
		if (currentHealth > 0) 
		{
			//float normalizedHealth = (float)currentHealth / (float)health;
			healthBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (currentHealth, 32);
			//float lostHealth = health - currentHealth;
		} 
		else 
		{
			healthBar.GetComponent<RectTransform> ().sizeDelta = new Vector2 (0, 0);
		}

	}

	/*
	void OnTriggerEnter2D(Collider2D coll)
	{
		Debug.Log("collision name = " + coll.gameObject.name);
		if (coll.gameObject.tag != null) {
			if (coll.gameObject.tag == "Enemy") {
				EnemyDamage (50);
			} else if (coll.gameObject.tag == "Explosion") {
				Debug.Log ("Explosion Collision");
				DestroyObject (coll.gameObject);
			}
		} else {
			Debug.Log ("Object has no tag: " + coll.gameObject.name);
		}		
	}*/
}
