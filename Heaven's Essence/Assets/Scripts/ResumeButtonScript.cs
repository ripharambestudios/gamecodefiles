using UnityEngine;
using System.Collections;

public class ResumeButtonScript : MonoBehaviour {
    public GameObject pausePanel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void deactivatePauseMenuPanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
