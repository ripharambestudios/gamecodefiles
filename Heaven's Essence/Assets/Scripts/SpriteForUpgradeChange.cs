using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteForUpgradeChange : MonoBehaviour {

    public Sprite upgradeLevel0;
    public Sprite upgradeLevel1;
    public Sprite upgradeLevel2;
    public Sprite upgradeLevel3;
    public Sprite upgradeLevel4;
    public Sprite upgradeLevel5;
    public Sprite upgradeLevel6;
    public Sprite upgradeLevel7;

    // Use this for initialization
    void Start () {
        this.GetComponent<Image>().sprite = upgradeLevel0;
	}
	
	public void setSpriteLevel(int level)
    {
        switch(level)
        {
            case 0:
                this.GetComponent<Image>().sprite = upgradeLevel0;
                break;
            case 1:
                this.GetComponent<Image>().sprite = upgradeLevel1;
                break;
            case 2:
                this.GetComponent<Image>().sprite = upgradeLevel2;
                break;
            case 3:
                this.GetComponent<Image>().sprite = upgradeLevel3;
                break;
            case 4:
                this.GetComponent<Image>().sprite = upgradeLevel4;
                break;
            case 5:
                this.GetComponent<Image>().sprite = upgradeLevel5;
                break;
            case 6:
                this.GetComponent<Image>().sprite = upgradeLevel6;
                break;
            case 7:
                this.GetComponent<Image>().sprite = upgradeLevel7;
                break;
        }
        
    }
}
