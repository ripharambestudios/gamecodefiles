using UnityEngine;
using System.Collections;

public class FacebookScript : MonoBehaviour
{
    /// <summary>
    /// Takes the user to our facebook page.
    /// </summary>
    public void OpenFacebook()
    {
        Application.OpenURL("https://www.facebook.com/ripharambestudiosHE/");
    }
}
