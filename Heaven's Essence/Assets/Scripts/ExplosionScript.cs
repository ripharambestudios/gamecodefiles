using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {

	public int damage = 50;
	public float radius = 1f;
	public float force = 10f;

	public float duration = 1f; //how long damage animation lasts in scene


	// Use this for initialization
	void Start () {
		this.transform.localScale *= (radius/2);
		Collider2D[] colliders = Physics2D.OverlapCircleAll (this.transform.position, radius);
		foreach (Collider2D hit in colliders) {
			Rigidbody2D rigid = hit.GetComponent<Rigidbody2D> ();
			if (rigid != null) {
				//rigid.transform.position = new Vector2(rigid.position.x - force * .05, rigid.position.y - force * .05);
			}
			hit.gameObject.SendMessage ("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
		StartCoroutine (Duration ());

	}

	IEnumerator Duration(){
		float timer = 2;
		while (timer < duration) {
			timer += Time.deltaTime;
			yield return null;
		}
		Destroy (this.gameObject);
	}
}
