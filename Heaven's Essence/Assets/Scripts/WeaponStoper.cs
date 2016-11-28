using UnityEngine;
using System.Collections;

public class WeaponStoper : MonoBehaviour
{

    public GameObject actualExplosion;
    public float radius = 3.2f;

    //Check for projectiles entering the planets and destroying them.
    //Separate checks for player and enemies
    void OnTriggerEnter2D(Collider2D coll)
    {
        print("Collider Entered");
        if (coll.gameObject.layer == 10)
        {
            if (coll.gameObject.name == "BombBall(Clone)")
            {
                Instantiate(actualExplosion, coll.transform.position, Quaternion.identity);
                Destroy(coll.gameObject);
            }
            else {
                Destroy(coll.gameObject);
            }
        }
        else if (coll.gameObject.layer == 11)
        {
            Destroy(coll.gameObject);
        }
    }
}

/*
//Collider2D[] collidersPlayerShots = Physics2D.OverlapCircleAll(this.transform.position, radius, 1 << LayerMask.NameToLayer("PlayerAttacks")); // for player attacks
        foreach (Collider2D coll in collidersPlayerShots)
        {
            if (coll.gameObject.layer == 10)
            {
                if (coll.gameObject.name == "BombBall(Clone)")
                {
                    Instantiate(actualExplosion, coll.transform.position, Quaternion.identity);
                    Destroy(coll.gameObject);
                }
                else {
                    Destroy(coll.gameObject);
                }
            }
        }

        //Collider2D[] collidersEnemyShots = Physics2D.OverlapCircleAll(this.transform.position, radius, 1 << LayerMask.NameToLayer("EnemyAttacks")); // for enemy attacks
        foreach (Collider2D coll in collidersEnemyShots)
        {
            if (coll.gameObject.layer == 11)
            {
                if (coll.gameObject.name == "SpookyGuyAttackObject(Clone)")
                {
                    coll.GetComponentInParent<DistantShooterAI>().decrementNumberOfProjectiles();
                }
                Destroy(coll.gameObject);
            }
        }
*/