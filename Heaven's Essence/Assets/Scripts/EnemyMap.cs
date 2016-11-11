using UnityEngine;
//using System.Collections;

public class EnemyMap : MonoBehaviour
{

    Texture2D map;

    public int height = 90;
    public int width = 160;
    Vector2 anchor;
    Rect region;
    GameObject mapShower;
    // Use this for initialization
    void Start()
    {
        Debug.Log("Map Initialization started");
        mapShower = GameObject.FindGameObjectWithTag("Map");
        map = new Texture2D(width, height, TextureFormat.RGBA32, false);
        map.filterMode = FilterMode.Point;

        region = new Rect(0f, 0f, map.width, map.height);
        anchor = new Vector2(map.width * .5f, map.height * .5f);
    }

    // Update is called once per frame
    void Update()
    {
        clearMap();
        updateMap();
        Sprite sprite = Sprite.Create(map, region, anchor);
        mapShower.GetComponent<UnityEngine.UI.Image>().sprite = sprite;

    }


    void clearMap()
    {
        map = new Texture2D(width, height, TextureFormat.RGBA32, false);
        map.filterMode = FilterMode.Point;
        Color fillColor = Color.black;
        Color[] fillPixels = new Color[map.width * map.height];

        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = fillColor;
        }

        map.SetPixels(fillPixels);
    }
    void updateMap()
    {
        Debug.Log("Test of Texture call");
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyDemonic"))
        {
            //Debug.Log("1 called");
            map.SetPixel((int)enemy.transform.position.x + width/2, (int)enemy.transform.position.y + height/2, Color.red);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyFallen"))
        {
            //Debug.Log("2 called");
			map.SetPixel((int)enemy.transform.position.x + width / 2, (int)enemy.transform.position.y + height / 2, Color.green);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyBoom"))
        {
            //Debug.Log("3 called");
			map.SetPixel((int)enemy.transform.position.x + width / 2, (int)enemy.transform.position.y + height / 2, Color.magenta);
        }

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemySpook"))
        {
            //Debug.Log("4 called");
			map.SetPixel((int)enemy.transform.position.x + width / 2, (int)enemy.transform.position.y + height / 2, Color.yellow);
        }

		map.SetPixel((int)GameObject.FindGameObjectWithTag("Player").transform.position.x + width/2, (int)GameObject.FindGameObjectWithTag("Player").transform.position.y + height/2, Color.blue);

        map.Apply();

    }


}
