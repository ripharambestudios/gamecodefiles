using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class howToPlayScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject howToPlayPanel;
    public GameObject playButton;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (howToPlayPanel.activeInHierarchy)
        {
            if (EventSystem.current.GetComponent<EventSystem>().currentSelectedGameObject == null)
            {

                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(playButton);

            }
        }

    }
    public void deactivatePanel()
    {
        howToPlayPanel.SetActive(true);
        mainPanel.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(playButton);
    }
}
