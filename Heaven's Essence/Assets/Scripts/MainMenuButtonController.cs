using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenuButtonController : MonoBehaviour
{
    public GameObject instructionPanel;
    public GameObject mainPanel;
    public GameObject mainMenuPlayButton;

    public GameObject StoredSelected;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (mainPanel.activeInHierarchy)
        {
            if (EventSystem.current.GetComponent<EventSystem>().currentSelectedGameObject != StoredSelected)
            {
                if (EventSystem.current.GetComponent<EventSystem>().currentSelectedGameObject == null)
                {

                    EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(StoredSelected);

                }
            }
        }

    }

    public void deactivatePanel()
    {
        mainPanel.SetActive(true);
        instructionPanel.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(mainMenuPlayButton);
    }
}
