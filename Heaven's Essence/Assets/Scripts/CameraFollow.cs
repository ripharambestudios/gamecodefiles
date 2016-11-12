using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
    public GameObject topLeft;
    public GameObject topRight;
    public GameObject bottomLeft;
    public GameObject bottomRight;
    
    public float trackingSpeed = 5;

    private GameObject map;
    private Camera cam;
    private float camHeight;
    private float camWidth;
    private Vector2 mapMinBound;
    private Vector2 mapMaxBound;
    private EdgeCollider2D[] mapEdges;

    void Start()
    {
        map = GameObject.FindGameObjectWithTag("Background");
        cam = Camera.main;
        camHeight = 2f * cam.orthographicSize;
        camWidth = camHeight * cam.aspect;
        mapEdges = map.GetComponents<EdgeCollider2D>();
        mapMinBound = new Vector2(map.transform.position.x, map.transform.position.y);
        //mapMaxBound = mapMinBound + new Vector2(map)
    }

    // Update is called once per frame at the end of the update
    void LateUpdate()
    {
        //change camera speed at a rate between the position of the camera and its target, with a multiplier of time last called and tracking speed
        //delta time is a small number
        if (target != null)
        {
            
            if (this.transform.position.x < topLeft.transform.position.x )
            {
                this.transform.position = new Vector2(topLeft.transform.position.x + camWidth/2, this.transform.position.y);
            }
            else if(this.transform.position.y > topLeft.transform.position.y)
            {
                this.transform.position = new Vector2(this.transform.position.x, topLeft.transform.position.y - camHeight/2);
            }
            else if(this.transform.position.x > bottomRight.transform.position.x)
            {
                this.transform.position = new Vector2(bottomRight.transform.position.x - camWidth / 2, this.transform.position.y);
            }
            else if(this.transform.position.y < bottomRight.transform.position.y)
            {
                this.transform.position = new Vector2(this.transform.position.x, bottomRight.transform.position.y + camHeight / 2);
            }
            else
            {
                this.transform.position = Vector2.Lerp(this.transform.position, target.transform.position, Time.deltaTime * trackingSpeed);
            }
        }
    }
}
