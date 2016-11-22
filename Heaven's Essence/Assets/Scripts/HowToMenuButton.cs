using UnityEngine;
using System.Collections;

public class HowToMenuButton : MonoBehaviour
{
    public GameObject HowToPlayPanel;
    public GameObject mainPanel;
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
    }
}
