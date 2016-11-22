using UnityEngine;
using System.Collections;

public class instructionsScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject instructionsPanel;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void activatePanel()
    {
        instructionsPanel.SetActive(true);
        mainPanel.SetActive(false);
    }
}
