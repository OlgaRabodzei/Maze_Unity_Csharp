using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    private Map mapScript;

    public static GameManager instance = null;            
   

    public string user;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        mapScript = GetComponent<Map>();
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = mapScript.GetCenter();
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

   
}
