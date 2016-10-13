using UnityEngine;
using System.Collections;

public class Absorption : MonoBehaviour {

    private BoxCollider2D coll;

    // Use this for initialization
    void Start () {
        coll = this.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        int layerMask = 1;
        int layerDepth = layerMask >> 8;  //enemies on eigth layer
        if (coll.IsTouchingLayers(layerDepth))
        {
            float angle = 0;
            Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(coll.size.x, coll.size.y), angle, layerDepth);
            foreach(Collider2D hit in hits)
            {
                if (hit.GetComponent<EnemyHealth>().IsBelowTenPercent())
                {
                    string attackType = hit.name;
                    this.gameObject.SendMessage("EnemyAbsorbed", attackType, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
            
	}
}
