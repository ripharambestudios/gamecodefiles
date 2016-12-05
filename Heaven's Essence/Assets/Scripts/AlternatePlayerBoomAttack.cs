using UnityEngine;
using System.Collections;

public class AlternatePlayerBoomAttack : MonoBehaviour {
    public GameObject actualExplosion;
	public AudioClip bombExplodeSound;
	private AudioSource source;

    private int splitAmount;
	private bool hasExploded;
	// Use this for initialization
	void Start () {
        splitAmount = 1;
		source = gameObject.AddComponent<AudioSource> ();
		hasExploded = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Fire2") > 0)
        {
			if (splitAmount == 1 && !hasExploded)
            {
				hasExploded = true;
                GameObject Explosion = (GameObject)Instantiate(actualExplosion, this.transform.position, Quaternion.identity);
                source = Explosion.AddComponent<AudioSource>();
                this.gameObject.GetComponent<AudioSource>().enabled = false;
				source.PlayOneShot(bombExplodeSound, .075f);
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				this.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
				Destroy(this.gameObject);
            }
			else if(splitAmount == 3 && !hasExploded)
            {
				hasExploded = true;
                GameObject Explosion1 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
                GameObject Explosion2 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(-5, -5, 0), Quaternion.identity);
                GameObject Explosion3 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(5, -5, 0), Quaternion.identity);
                source = Explosion1.AddComponent<AudioSource>();
                source = Explosion2.AddComponent<AudioSource>();
                source = Explosion3.AddComponent<AudioSource>();
                this.gameObject.GetComponent<AudioSource>().enabled = false;
                source.PlayOneShot(bombExplodeSound, .15f);
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				this.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
				Destroy(this.gameObject);
            }
			else if (splitAmount == 5 && !hasExploded)
            {
				hasExploded = true;
                GameObject Explosion1 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                GameObject Explosion2 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(-5, -5, 0), Quaternion.identity);
                GameObject Explosion3 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(5, -5, 0), Quaternion.identity);
                GameObject Explosion4 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(5, 5, 0), Quaternion.identity);
                GameObject Explosion5 = (GameObject)Instantiate(actualExplosion, this.transform.position + new Vector3(-5, 5, 0), Quaternion.identity);
                source = Explosion1.AddComponent<AudioSource>();
                source = Explosion2.AddComponent<AudioSource>();
                source = Explosion3.AddComponent<AudioSource>();
                source = Explosion4.AddComponent<AudioSource>();
                source = Explosion5.AddComponent<AudioSource>();
                this.gameObject.GetComponent<AudioSource>().enabled = false;
                source.PlayOneShot(bombExplodeSound, .3f);
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				this.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
				Destroy(this.gameObject);
            }
        }
    }

    public void setSplitAmount(int newNum)
    {
        splitAmount = newNum;
    }
}
