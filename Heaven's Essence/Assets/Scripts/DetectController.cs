using UnityEngine;
using System.Collections;

public class DetectController : MonoBehaviour
{

    private GameObject Player;
    private bool PS4Controller;
    private bool XBoxController;
    private GameObject GameManager;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("MainCharacter");
        GameManager = GameObject.Find("Game Manager");
        PS4Controller = false;
        XBoxController = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && (Input.anyKeyDown || Input.GetAxisRaw("XBoxRHorizontal") != 0 || Input.GetAxisRaw("XBoxRVertical") != 0))
        {
            string[] names = Input.GetJoystickNames();
            for(int i = 0; i < names.Length; i++)
            {
                //Debug.Log(names[x].Length);
                if (names[i].Length == 19)
                {
                    //Debug.Log(names[x]);
                    PS4Controller = true;
                    XBoxController = false;
                    Player.GetComponent<MainCharacterController>().PS4Controller = true;
                    Player.GetComponent<MainCharacterController>().XBoxController = false;
                    Player.GetComponentInChildren<AnimatorScript>().PS4Controller = true;
                    Player.GetComponentInChildren<AnimatorScript>().XBoxController = false;
                    GameManager.GetComponent<PauseScript>().PS4Controller = true;
                    GameManager.GetComponent<PauseScript>().XBoxController = false;

                }
                else if (names[i].Length == 33)
                {
                    //Debug.Log(names[x]);
                    //set a controller bool to true
                    PS4Controller = false;
                    XBoxController = true;
                    Player.GetComponent<MainCharacterController>().PS4Controller = false;
                    Player.GetComponent<MainCharacterController>().XBoxController = true;
                    Player.GetComponentInChildren<AnimatorScript>().PS4Controller = false;
                    Player.GetComponentInChildren<AnimatorScript>().XBoxController = true;
                    GameManager.GetComponent<PauseScript>().PS4Controller = false;
                    GameManager.GetComponent<PauseScript>().XBoxController = true;
                }
            }
            

            //Detect Mouse Input
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(3))
            {
                Player.GetComponent<MainCharacterController>().useController = false;
                Player.GetComponentInChildren<AnimatorScript>().useController = false;
            }
            if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            {
                Player.GetComponent<MainCharacterController>().useController = false;
                Player.GetComponentInChildren<AnimatorScript>().useController = false;
            }

            //Detect Controller Input
            if (Input.GetAxisRaw("XBoxRHorizontal") != 0 || Input.GetAxisRaw("XBoxRVertical") != 0)
            {
                Player.GetComponent<MainCharacterController>().useController = true;
                Player.GetComponentInChildren<AnimatorScript>().useController = true;
            }
            //Check for any buttons pressed on the controller, assuming that if they do they want to use the controller
            if (Input.GetKey(KeyCode.Joystick1Button0) || Input.GetKey(KeyCode.Joystick1Button1) ||
                    Input.GetKey(KeyCode.Joystick1Button2) || Input.GetKey(KeyCode.Joystick1Button3) ||
                    Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5) ||
                    Input.GetKey(KeyCode.Joystick1Button6) || Input.GetKey(KeyCode.Joystick1Button7) ||
                    Input.GetKey(KeyCode.Joystick1Button8) || Input.GetKey(KeyCode.Joystick1Button9) ||
                    Input.GetKey(KeyCode.Joystick1Button10) || Input.GetKey(KeyCode.Joystick1Button11) ||
                    Input.GetKey(KeyCode.Joystick1Button12) || Input.GetKey(KeyCode.Joystick1Button13) ||
                    Input.GetKey(KeyCode.Joystick1Button14) || Input.GetKey(KeyCode.Joystick1Button15) ||
                    Input.GetKey(KeyCode.Joystick1Button16) || Input.GetKey(KeyCode.Joystick1Button17) ||
                    Input.GetKey(KeyCode.Joystick1Button18) || Input.GetKey(KeyCode.Joystick1Button19))
            {
                Player.GetComponent<MainCharacterController>().useController = true;
                Player.GetComponentInChildren<AnimatorScript>().useController = true;
            }

        }
    }
}
