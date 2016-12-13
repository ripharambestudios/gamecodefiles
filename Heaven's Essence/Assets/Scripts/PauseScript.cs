using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
    public GameObject pausePanel;
	public GameObject controlsPanel;
    public bool PS4Controller;
    public bool XBoxController;
    // Use this for initialization
    void Start () {
        PS4Controller = true;
        XBoxController = false;
    }

    // Update is called once per frame
    void Update ()
    {
        if(PS4Controller)
        {
            if ((Input.GetKeyUp("p") || Input.GetKeyUp(KeyCode.Escape)  || Input.GetKeyUp(KeyCode.Joystick1Button9)) && !controlsPanel.activeSelf)
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
        else if(XBoxController)
        {
            if ((Input.GetKeyUp("p") || Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Joystick1Button7)) && !controlsPanel.activeSelf)
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
        else
        {
            if ((Input.GetKeyUp("p") || Input.GetKeyUp(KeyCode.Escape)) && !controlsPanel.activeSelf)
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
