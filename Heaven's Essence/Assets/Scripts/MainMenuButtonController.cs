using UnityEngine;
using System.Collections;

public class MainMenuButtonController : MonoBehaviour {
    public GameObject instructionPanel;
    public GameObject mainPanel;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void deactivatePanel()
    {
        mainPanel.SetActive(true);
        instructionPanel.SetActive(false);
    }
}
