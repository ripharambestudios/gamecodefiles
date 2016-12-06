using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyMap : MonoBehaviour
{
    public GameObject demonicIcon;
    public GameObject boomIcon;
    public GameObject fallenIcon;
    public GameObject spookIcon;
    public Image playerIcon;
    public int height = 170;
    public int width = 230;

	private int scaleFactorX;
	private int scaleFactorY;
    private GameObject mapShower;
	private GameObject player;
    private Image iconOfPlayer;


    // Use this for initialization
    void Start()
    {
		player = GameObject.FindGameObjectWithTag ("Player");

		scaleFactorX = 1920 / width;
		scaleFactorY = 1080 / height;
        iconOfPlayer = (Image)Instantiate(playerIcon, gameObject.transform);
        iconOfPlayer.transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
		placeEnemies();

    }

	void placeEnemies(){
		//Color[] colors;
		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyDemonic"))
		{
            //map.SetPixel((int)enemy.transform.position.x + width / 2 + i, (int)enemy.transform.position.y + height / 2 + j, demonicIcon.texture.GetPixel(i, j));

            
        }

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyFallen"))
		{
	

		}

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemyBoom"))
		{

		}

		foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("EnemySpook"))
		{
			
		}

		if (player != null) 
		{
            //iconOfPlayer.transform.position = new Vector3(gameObject.transform.position.x + player.transform.position.x +  width / 2, gameObject.transform.position.y +player.transform.position.y  + height / 2, 0);
            iconOfPlayer.transform.localPosition = new Vector3(player.transform.position.x, player.transform.position.y - 40);
        }
			


	}



}
