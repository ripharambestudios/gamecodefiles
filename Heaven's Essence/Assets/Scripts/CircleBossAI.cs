using UnityEngine;
using System.Collections;

public class CircleBossAI : MonoBehaviour {

    bool isAttacking;

    GameObject target;

    private float distanceToTarget;

    public float sightRadius = 10.0f;

    public float speed;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(Vector2.Distance(transform.position, target.transform.position));
	}
	
	// Update is called once per frame
	void Update () {
        distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        Debug.Log(distanceToTarget);
        if (distanceToTarget <= sightRadius && !isAttacking)
        {
            isAttacking = true;
            StartCoroutine(LaunchAttack());
        }

        moveBoss();
    }

    void moveBoss()
    {
        //Vector3 vectorToTarget = target.transform.position - transform.position;
        //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
        //transform.LookAt(target.transform.position);
        //transform.Rotate(new Vector3(0, -90, 0), Space.Self);

        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (distanceToTarget >= sightRadius)
        {
            Debug.Log("Forward");
            transform.position += transform.right * Time.deltaTime * speed;
        }
        else
        {
            Debug.Log("Rotate");
            //transform.position += transform.right * Time.deltaTime * speed;
            transform.RotateAround(target.transform.position, Vector3.forward, speed * Time.deltaTime * 100);
        }
    }
    IEnumerator LaunchAttack()
    {
        yield return null;

        while(transform.position != target.transform.position && distanceToTarget <= sightRadius)
        {

        }
    }
}
