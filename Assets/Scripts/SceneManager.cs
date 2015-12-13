using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public void MainScene () {
		Application.LoadLevel ("MainScene");
	}
	
	public void ScoreScene () {
		Application.LoadLevel ("ScoreScene");
	}
}
