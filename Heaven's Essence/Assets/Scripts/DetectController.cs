using UnityEngine;
using System.Collections;

public class DetectController : MonoBehaviour
{

    private GameObject Player;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("MainCharacter");
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale != 0 && (Input.anyKeyDown || Input.GetAxisRaw ("RHorizontal") != 0 || Input.GetAxisRaw ("RVertical") != 0)) {
			//Detect Mouse Input
			if (Input.GetMouseButton (0) || Input.GetMouseButton (1) || Input.GetMouseButton (3)) {
				Player.GetComponent<MainCharacterController> ().useController = false;
				Player.GetComponentInChildren<AnimatorScript> ().useController = false;
			}
			if (Input.GetAxisRaw ("Mouse X") != 0 || Input.GetAxisRaw ("Mouse Y") != 0) {
				Player.GetComponent<MainCharacterController> ().useController = false;
				Player.GetComponentInChildren<AnimatorScript> ().useController = false;
			}

			//Detect Controller Input
			if (Input.GetAxisRaw ("RHorizontal") != 0 || Input.GetAxisRaw ("RVertical") != 0) {
				Player.GetComponent<MainCharacterController> ().useController = true;
				Player.GetComponentInChildren<AnimatorScript> ().useController = true;
			}
			//Check for any buttons pressed on the controller, assuming that if they do they want to use the controller
			if (Input.GetKey (KeyCode.Joystick1Button0) || Input.GetKey (KeyCode.Joystick1Button1) ||
			        Input.GetKey (KeyCode.Joystick1Button2) || Input.GetKey (KeyCode.Joystick1Button3) ||
			        Input.GetKey (KeyCode.Joystick1Button4) || Input.GetKey (KeyCode.Joystick1Button5) ||
			        Input.GetKey (KeyCode.Joystick1Button6) || Input.GetKey (KeyCode.Joystick1Button7) ||
			        Input.GetKey (KeyCode.Joystick1Button8) || Input.GetKey (KeyCode.Joystick1Button9) ||
			        Input.GetKey (KeyCode.Joystick1Button10)) {
				Player.GetComponent<MainCharacterController> ().useController = true;
				Player.GetComponentInChildren<AnimatorScript> ().useController = true;
			}
		}
    }
}
