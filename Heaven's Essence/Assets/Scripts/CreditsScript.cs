using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CreditsScript : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject creditsPanel;
    public GameObject backButton;
    public GameObject mainMenuPlayButton;

    void Start()
    {
        creditsPanel.SetActive(false);
    }

    void Update()
    {

        if (creditsPanel.activeInHierarchy)
        {
            if (EventSystem.current.GetComponent<EventSystem>().currentSelectedGameObject == null)
            {

                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(backButton);

            }
        }
    }

    public void activateCredits()
    {
        creditsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(backButton);
    }

    public void deactivateCredits()
    {
        creditsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(mainMenuPlayButton);
    }
}
