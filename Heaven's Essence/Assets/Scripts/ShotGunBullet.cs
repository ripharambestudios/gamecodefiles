using UnityEngine;
using System.Collections;

public class ShotGunBullet : MonoBehaviour {

	public float expandSize = 3f;
	public int speed = 10;
	private float timer = 0f;
    private float destroyTimer = 0f;
    private float maxSize = 5f;
    private int waitTime = 2;
    public float damage = 20f;
    // Use this for initialization
    void Start () {
        StartCoroutine(Scale());
	}
	
	// Update is called once per frame
	void Update () {
		

        destroyTimer += Time.deltaTime;

        if(destroyTimer > 5f)
        {
            Destroy(this.gameObject);
        }
	}

    IEnumerator Scale()
    {
        while (destroyTimer <= 5f)
        {
            timer += Time.deltaTime;

            while (maxSize > transform.localScale.x)
            {
                timer += Time.deltaTime;
                transform.localScale += new Vector3(1, 1, 1) * (Time.deltaTime*5) * expandSize;
                yield return null;
            }

            timer = 0;
            yield return null;
        }

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Result hit shotgun");
        if (coll.gameObject.tag != null)
        {
            if (coll.gameObject.tag == "Player")
            {
                coll.gameObject.SendMessage("EnemyDamage", damage, SendMessageOptions.DontRequireReceiver);
                Destroy(this.gameObject);
            }
        }
    }
}

