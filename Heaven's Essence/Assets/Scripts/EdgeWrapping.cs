using UnityEngine;
using System.Collections;
using System;

public class EdgeWrapping : MonoBehaviour {

	public float edgeY = 54f;
	public float edgeX = 105f;
	public float bounceSpeed = .5f;

	private System.Random randNum;
	// Use this for initialization
	void Start () {
		randNum = new System.Random ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll)
	{

		if (coll.tag == "Player") 
		{
			Rigidbody2D rigid = coll.GetComponent<Rigidbody2D> ();
			//Debug.Log ("Crossed boundary: name is " + rigid.name + "X: " + rigid.transform.position.x + " Y: " + rigid.transform.position.y);
			if (rigid != null && rigid.tag == "Player")
			{
				if (rigid.transform.position.y >= edgeY) 
				{
					rigid.transform.position = new Vector3 (rigid.transform.position.x, rigid.transform.position.y * -1, 0);
				} 
				else if (rigid.transform.position.y <= -edgeY)
				{
					rigid.transform.position = new Vector3 (rigid.transform.position.x, rigid.transform.position.y * -1 - 2, 0);
				} 

				if (rigid.transform.position.x >= edgeX) 
				{
					rigid.transform.position = new Vector3 (rigid.transform.position.x * -1, rigid.transform.position.y, 0);
				} 
				else if (rigid.transform.position.x <= -edgeX)
				{
					rigid.transform.position = new Vector3 (rigid.transform.position.x * -1 - 4, rigid.transform.position.y, 0);
				}
			}
		}

		else if (coll.gameObject.layer == LayerMask.NameToLayer ("Enemies")) 
		{
			
			if (coll.gameObject.transform.position.x <= -edgeX) 
			{
				coll.gameObject.SendMessage ("setCanAttack", false, SendMessageOptions.DontRequireReceiver);
				int randomDegree = randNum.Next (-30, 30);
				StartCoroutine(BounceOff(coll.gameObject, randomDegree));
			} 
			else if(coll.gameObject.transform.position.x >= edgeX)
			{
				coll.gameObject.SendMessage ("setCanAttack", false, SendMessageOptions.DontRequireReceiver);
				int randomDegree = randNum.Next (150, 210);
				StartCoroutine(BounceOff(coll.gameObject, randomDegree));
			}

			if (coll.gameObject.transform.position.y <= -edgeY) 
			{
				coll.gameObject.SendMessage ("setCanAttack", false, SendMessageOptions.DontRequireReceiver);
				int randomDegree = randNum.Next (60, 120);
				StartCoroutine(BounceOff(coll.gameObject, randomDegree));
			} 
			else if(coll.gameObject.transform.position.y >= edgeY)
			{
				coll.gameObject.SendMessage ("setCanAttack", false, SendMessageOptions.DontRequireReceiver);
				int randomDegree = randNum.Next (240, 300);
				StartCoroutine(BounceOff(coll.gameObject, randomDegree));
			}
		}
	}

	IEnumerator BounceOff(GameObject enemy, int randomDegree)
	{
		//yield return null;
		float numAddX = Mathf.Cos(randomDegree * (Mathf.PI / 180)) * 50;
		float numAddY = Mathf.Sin(randomDegree * (Mathf.PI / 180)) * 50;
		float endX = numAddX + enemy.transform.position.x;
		float endY = numAddY + enemy.transform.position.y;
		Vector2 endLocation = new Vector2 (endX, endY);
		Vector2 nextPosition = enemy.transform.position;
		Vector2 look = endLocation - nextPosition;
		float distanceCovered = 0;
		int maxDistance = 100;
		int layerDepth = 1;
		int obsticalMask = layerDepth << 12; //obsticale on 12th layer
		RaycastHit2D impactObsticale = Physics2D.Raycast(nextPosition, endLocation, maxDistance, obsticalMask);


		float distanceToGo = 50f;
		while(distanceCovered < distanceToGo){
			nextPosition += look.normalized * bounceSpeed; //try time.detlatime to see if that can make it better
			distanceCovered += Math.Abs (Vector2.Distance (enemy.transform.position, nextPosition));

			if (Physics2D.Linecast (enemy.transform.position, nextPosition, obsticalMask)) // if it his an obsticale it stops moving
			{
				impactObsticale = Physics2D.Linecast (enemy.transform.position, nextPosition, obsticalMask);
				distanceCovered = distanceToGo;
				nextPosition = enemy.transform.position;
			}

			enemy.transform.position = nextPosition;
			yield return null;
		}
		yield return new WaitForSeconds (.25f); // cooldown
		enemy.SendMessage ("setCanAttack", true, SendMessageOptions.DontRequireReceiver);
	}


}
