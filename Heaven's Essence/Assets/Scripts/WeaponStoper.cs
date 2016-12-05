using UnityEngine;
using System.Collections;

public class WeaponStoper : MonoBehaviour
{

    public GameObject actualExplosion;
	public AudioClip bombExplodeSound;
	private AudioSource source;

	void Start()
	{
		source = this.gameObject.AddComponent<AudioSource> ();
	}
    //Check for projectiles entering the planets and destroying them.
    //Separate checks for player and enemies
    void OnTriggerEnter2D(Collider2D coll)
    {
        
        if (coll.gameObject.layer == 10)
        {
            if (coll.gameObject.name == "BombBall(Clone)")
            {
				Instantiate(actualExplosion, coll.transform.position, Quaternion.identity);
				source.PlayOneShot(bombExplodeSound, .075f);
                Destroy(coll.gameObject);
            }
            else 
			{
                Destroy(coll.gameObject);
            }
        }
        else if (coll.gameObject.layer == 11)
        {
            Destroy(coll.gameObject);
        }
    }
}
