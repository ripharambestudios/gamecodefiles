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
                Instantiate(actualExplosion, this.transform.position, Quaternion.identity);
				source.PlayOneShot(bombExplodeSound, .075f);
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				this.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
				Destroy(this.gameObject, bombExplodeSound.length);
            }
			else if(splitAmount == 3 && !hasExploded)
            {
				hasExploded = true;
                Instantiate(actualExplosion, this.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(-5, -5, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(5, -5, 0), Quaternion.identity);
				source.PlayOneShot(bombExplodeSound, .15f);
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				this.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
				Destroy(this.gameObject, bombExplodeSound.length);
            }
			else if (splitAmount == 5 && !hasExploded)
            {
				hasExploded = true;
                Instantiate(actualExplosion, this.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(-5, -5, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(5, -5, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(5, 5, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(-5, 5, 0), Quaternion.identity);
				source.PlayOneShot(bombExplodeSound, .3f);
				this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
				this.gameObject.GetComponent<CircleCollider2D> ().enabled = false;
				Destroy(this.gameObject, bombExplodeSound.length);
            }
        }
    }

    public void setSplitAmount(int newNum)
    {
        splitAmount = newNum;
    }
}
