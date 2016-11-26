using UnityEngine;
using System.Collections;
using System;

public class ShotgunKnockback : MonoBehaviour {
	public float knockBackSpeed = 2;
	public int upgradeLevel;
	private GameObject Player;
	private Vector3 playerShootPosition;
	private float damage;

	void Start()
	{
		damage = 5;
		Player = GameObject.FindWithTag ("Player");
		playerShootPosition = Player.transform.position;
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Enemies")) 
		{
			coll.gameObject.SendMessage ("DealDamage", damage, SendMessageOptions.DontRequireReceiver);
			this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
			this.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
			float degree = Mathf.Atan2 (coll.gameObject.transform.position.y - playerShootPosition.y, coll.gameObject.transform.position.x - playerShootPosition.x) * (180 / Mathf.PI);
			coll.gameObject.SendMessage ("setCanAttack", false, SendMessageOptions.DontRequireReceiver);
			coll.gameObject.SendMessage ("setKnockBackAmount", upgradeLevel * 2, SendMessageOptions.DontRequireReceiver);
			coll.gameObject.SendMessage("startKnockBack", degree, SendMessageOptions.DontRequireReceiver);
		}
	}
}
