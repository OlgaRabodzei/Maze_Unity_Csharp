using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Xml;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;

public class FileManager : MonoBehaviour {
    public Text scoreTable;

	// Use this for initialization
	void Start () {
        if (EditorSceneManager.GetActiveScene().name == "ScoreScene") {
            Load();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Save()	{
		XmlDocument xmlDoc = new XmlDocument();
		XmlNode rootNode;
		if (System.IO.File.Exists ("Score.xml")) {
			xmlDoc.Load ("Score.xml");
			rootNode = xmlDoc.FirstChild;

		} else {
			rootNode = xmlDoc.CreateElement("Score");
			xmlDoc.AppendChild(rootNode);
		}

		XmlNode userNode;
		XmlAttribute attribute;

		userNode = xmlDoc.CreateElement ("User");
		// User name
		userNode.InnerText = GameManager.instance.user;
		// Coins count
		attribute = xmlDoc.CreateAttribute("score");
		attribute.Value = GameManager.instance.score.ToString();
		userNode.Attributes.Append(attribute);
		// Cause of end the game
		attribute = xmlDoc.CreateAttribute("gameOver");
        if (GameManager.instance.gameOver) {
            attribute.Value = "Game over";
        }
        else {
            attribute.Value = "Exit";
        }
		userNode.Attributes.Append(attribute);
		// Date and time of the playing
		attribute = xmlDoc.CreateAttribute("date");
		attribute.Value = System.DateTime.Now.ToString();
		userNode.Attributes.Append(attribute);

		rootNode.AppendChild(userNode);
		xmlDoc.Save("Score.xml");
	}

	public void Load()	{
		if (System.IO.File.Exists ("Score.xml")) {
            XmlTextReader reader = new XmlTextReader("Score.xml");
            scoreTable.text = "";
            while (reader.Read()) {
                if (reader.IsStartElement("User") && !reader.IsEmptyElement) {
                    int n = reader.AttributeCount;
                    string[] attr = new string[n]; 
                    for (int i = 0; i < n; i++) {
                        attr[i] = reader.GetAttribute(i);
                    }
                    scoreTable.text += String.Format("{0,-20}", reader.ReadString());
                    for (int i = 0; i < n; i++) {
                        scoreTable.text += String.Format("{0, -10}",attr[i]);
                    }
                    scoreTable.text += "\n";
                }
            }
            reader.Close();
		}
	}
}



