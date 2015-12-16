using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public void MainScene () {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
	}
	
	public void ScoreScene () {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ScoreScene");
	}

    public void Menu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

	public void Exit(){
		Application.Quit ();
	}
}
