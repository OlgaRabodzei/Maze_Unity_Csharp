using UnityEngine;
using System.Collections;
using System.Xml;

public class FileManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Save()	{
		XmlDocument xmlDoc = new XmlDocument();
		XmlNode rootNode = xmlDoc.CreateElement("Score");
		xmlDoc.AppendChild(rootNode);

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
		attribute.Value = GameManager.instance.gameOver.ToString();
		userNode.Attributes.Append(attribute);
		// Date and time of the playing
		attribute = xmlDoc.CreateAttribute("date");
		attribute.Value = System.DateTime.Now.ToString();
		userNode.Attributes.Append(attribute);

		rootNode.AppendChild(userNode);
		xmlDoc.Save("Score.xml");
	}

	public void Load()	{
	}
}
