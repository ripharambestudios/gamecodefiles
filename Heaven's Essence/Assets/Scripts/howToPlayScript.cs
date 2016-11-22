using UnityEngine;
using System.Collections;

public class howToPlayScript : MonoBehaviour {
    public GameObject mainPanel;
    public GameObject howToPlayPanel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void deactivatePanel()
    {
        howToPlayPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
}
