using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HowToMenuButton : MonoBehaviour
{
    public GameObject HowToPlayPanel;
    public GameObject mainPanel;
    public GameObject mainMenuPlayButton;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void deactivatePanel()
    {
        mainPanel.SetActive(true);
        HowToPlayPanel.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(mainMenuPlayButton);
    }
}
