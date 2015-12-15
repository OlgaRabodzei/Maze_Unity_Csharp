using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private Map mapScript;

    public static GameManager instance = null;            
    public string user;
	public bool gameOver = false;
	public int score = 0;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        mapScript = GetComponent<Map>();
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = mapScript.GetCenter();
		user = PlayerName.user;
    }

	public void GameOver(){
		if (gameOver) {
			FileManager fileManager = GetComponent<FileManager> ();
			fileManager.Save ();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
		}
	}

    void OnApplicationQuit() {
        FileManager fileManager = GetComponent<FileManager>();
        fileManager.Save();
    }
}
