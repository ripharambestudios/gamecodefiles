using UnityEngine;
using System.Collections;

public class AlternatePlayerBoomAttack : MonoBehaviour {
    public GameObject actualExplosion;

    private int splitAmount;
	// Use this for initialization
	void Start () {
        splitAmount = 1;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Fire2") > 0)
        {

            if (splitAmount == 1)
            {
                Instantiate(actualExplosion, this.transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            else if(splitAmount > 1)
            {
                Instantiate(actualExplosion, this.transform.position + new Vector3(0, 5, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(-5, -5, 0), Quaternion.identity);
                Instantiate(actualExplosion, this.transform.position + new Vector3(5, -5, 0), Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
    }

    public void setSplitAmount(int newNum)
    {
        splitAmount = newNum;
    }
}
