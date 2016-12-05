using UnityEngine;
using System.Collections;


public class instructionsScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject instructionsPanel;

    public GameObject controllerText;

    public GameObject keyboardText;

    private string[] controllerNames;

    // Use this for initialization
    void Start()
    {
        controllerNames = Input.GetJoystickNames();
        //Debug.Log(controllerNames[0]);


    }

    // Update is called once per frame
    void Update()
    {

    }
    public void activatePanel()
    {
        
        instructionsPanel.SetActive(true);
        mainPanel.SetActive(false);

        //keyboardText = GameObject.FindGameObjectWithTag("KeyboardText");
        //controllerText = GameObject.FindGameObjectWithTag("ControllerText");

        controllerNames = Input.GetJoystickNames();
        if (controllerNames.Length > 0 && controllerNames[0] != "")
        {
            keyboardText.SetActive(false);
            controllerText.SetActive(true);
        }

        else
        {
            keyboardText.SetActive(true);
            controllerText.SetActive(false);
        }


    }
}
