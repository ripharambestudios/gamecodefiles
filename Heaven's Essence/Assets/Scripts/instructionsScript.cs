using UnityEngine;
using System.Collections;


public class instructionsScript : MonoBehaviour
{
    public GameObject mainPanel;
    public GameObject instructionsPanel;

    public GameObject controllerText;

    public GameObject keyboardText;

	private GameObject Player;

    private string[] controllerNames;

    // Use this for initialization
    void Start()
    {
        controllerNames = Input.GetJoystickNames();
		Player = GameObject.Find ("MainCharacter");
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

		if (Player != null) {
			if (Player.GetComponent<MainCharacterController> ().useController) {
				keyboardText.SetActive (false);
				controllerText.SetActive (true);
			} else {
				keyboardText.SetActive (true);
				controllerText.SetActive (false);
			}

		} 
		else {
			
			controllerNames = Input.GetJoystickNames ();
			if (controllerNames.Length > 0 && controllerNames [0] != "") {
				keyboardText.SetActive (false);
				controllerText.SetActive (true);
			} else {
				keyboardText.SetActive (true);
				controllerText.SetActive (false);
			}
		}
    }

	public void deactivatePanel()
	{
		instructionsPanel.SetActive(false);
		mainPanel.SetActive(true);
	}

}
