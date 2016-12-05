using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
    public GameObject pausePanel;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyUp("p") || Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Joystick1Button7))
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
