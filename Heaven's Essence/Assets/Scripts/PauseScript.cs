using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
    public GameObject pausePanel;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("p") || Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeInHierarchy == false)
            {
                pauseMenuPanel();
            }
            else
            {
                deactivatePauseMenuPanel();
            }
        }
        
    }

    public void pauseMenuPanel()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        
    }
    public void deactivatePauseMenuPanel()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }
}
