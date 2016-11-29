using UnityEngine;
//using System.Collections;

public class EnemyMap : MonoBehaviour
{

    Texture2D map;

    public int height = 90;
    public int width = 160;
	private int scaleFactorX;
	private int scaleFactorY;
	private Vector2 anchor;
    private Rect region;
    private GameObject mapShower;
	GameObject player;

	public Sprite demonicIcon;
	public Sprite boomIcon;
	public Sprite fallenIcon;
	public Sprite spookIcon;
	public Sprite playerIcon;


	private Texture2D baseMap;
    // Use this for initialization
    void Start()
    {
        mapShower = GameObject.FindGameObjectWithTag("Map");
        map = new Texture2D(width, height, TextureFormat.RGBA32, false);
        map.filterMode = FilterMode.Point;

        region = new Rect(0f, 0f, map.width, map.height);
        anchor = new Vector2(map.width * .5f, map.height * .5f);
		player = GameObject.FindGameObjectWithTag ("Player");
		clearMap(Color.black);

		scaleFactorX = 1920 / width;
		scaleFactorY = 1080 / height;
    }

    // Update is called once per frame
    void Update()
    {
		clearMap(Color.black);
		placeEnemies();
        Sprite sprite = Sprite.Create(map, region, anchor);
        mapShower.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
    }


	void clearMap(Color color)
    {
        map = new Texture2D(width, height, TextureFormat.RGBA32, false);
        map.filterMode = FilterMode.Point;
        Color fillColor = color;
        Color[] fillPixels = new Color[map.width * map.height];

        for (int i = 0; i < fillPixels.Length; i++)
        {
            fillPixels[i] = fillColor;
        }

        map.SetPixels(fillPixels);
    }

	void placeEnemies(){
		//Color[] colors;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyDemonic"))
		{
			//Debug.Log("1 called");

			for (int i = 0; i < demonicIcon.texture.width; i++) {
				for (int j = 0; j < demonicIcon.texture.height; j++) {
					if(demonicIcon.texture.GetPixel(i,j) != Color.blue){
						Debug.Log (enemy.transform.position.x);
					map.SetPixel((int)enemy.transform.position.x + width/2 + i, (int)enemy.transform.position.y + height/2 +  j, demonicIcon.texture.GetPixel(i,j));
					}
				}
			}
			//map.SetPixel((int)enemy.transform.position.x + width/2, (int)enemy.transform.position.y + height/2, Color.red);
		}

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyFallen"))
		{
			for (int i = 0; i < fallenIcon.texture.width; i++) {
				for (int j = 0; j < fallenIcon.texture.height; j++) {
					if(fallenIcon.texture.GetPixel(i,j) != Color.blue){
						map.SetPixel((int)enemy.transform.position.x + width/2 + i, (int)enemy.transform.position.y + height/2+  j, fallenIcon.texture.GetPixel(i,j));
					}
				}
			}
		}

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyBoom"))
		{
			for (int i = 0; i < boomIcon.texture.width; i++) {
				for (int j = 0; j < boomIcon.texture.height; j++) {
					if(boomIcon.texture.GetPixel(i,j) != Color.blue){
						map.SetPixel((int)enemy.transform.position.x + width/2 + i, (int)enemy.transform.position.y + height/2+  j, boomIcon.texture.GetPixel(i,j));
					}
				}
			}
		}

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemySpook"))
		{
			for (int i = 0; i < spookIcon.texture.width; i++) {
				for (int j = 0; j < spookIcon.texture.height; j++) {
					if(spookIcon.texture.GetPixel(i,j) != Color.blue){
						map.SetPixel((int)enemy.transform.position.x + width/2 + i, (int)enemy.transform.position.y + height/2+  j, spookIcon.texture.GetPixel(i,j));
					}
				}
			}
		}

		if (player != null) 
		{
			for (int i = 0; i < playerIcon.texture.width; i++) {
				for (int j = 0; j < playerIcon.texture.height; j++) {
					if(playerIcon.texture.GetPixel(i,j) != Color.blue){
						map.SetPixel((int)player.transform.position.x + width/2 + i, (int)player.transform.position.y + height/2+  j, playerIcon.texture.GetPixel(i,j));
					}
				}
			}
		}
			

		map.Apply();

	}



}
