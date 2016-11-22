using UnityEngine;
using System.Collections;

public class LoadLevelScript : MonoBehaviour {
	//loads level from given level number
	public void LoadLevel(int level){
		UnityEngine.SceneManagement.SceneManager.LoadScene (level);
        Time.timeScale = 1;
	}

	//closes game if exit button is pushed
	public void ExitGame(){
		Application.Quit ();
	}
}
