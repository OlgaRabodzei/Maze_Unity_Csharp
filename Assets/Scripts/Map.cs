using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{

	public class Node
	{
		public bool isWall{ get; set; }

		public bool visited{ get; set; }

		public Node parent{ get; set; }

		public Vector3 position{ get; set; }

		public Node (bool _isWall, Vector3 _position)
		{
			this.isWall = _isWall;
			this.position = _position;
			this.visited = false;
			this.parent = null;
		}
	};

	public int columns = 10;
	public int rows = 10;
	public GameObject hero;
	public GameObject coin;
	public GameObject enemy;
	public GameObject wall;
	public GameObject ground;
	private List<Vector3> freePositions = new List<Vector3> ();
	private Node[,] grid;
	private const float tilesScale = 1f; // 0.9f;
	private Transform boardHolder;
	private Transform coinHolder;

	// Use this for initialization
	void Awake ()
	{
		grid = new Node[columns, rows];
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void CreateMap ()
	{
		
		//Creates the outer walls and floor.
		BoardSetup ();
		//Reset our gridpositions.
		Initialise ();
		//Create walls
		//TO_DO: Add maze generation.
		for (int i = 0; i < 20; i++) {
			LayoutObjectAtRandom (wall, boardHolder);
		}

		LayoutObjectAtRandom (hero, null);
		LayoutObjectAtRandom (enemy, null);
		coinHolder = new GameObject ("Coins").transform;
		LayoutObjectAtRandom (coin, coinHolder);
	}

	private void Initialise ()
	{
		Transform groundHolder = new GameObject ("Ground").transform;
		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				Vector3 pos = new Vector3 (x * tilesScale, y * tilesScale, 0f);
				GameObject instance = Instantiate (ground, pos, Quaternion.identity) as GameObject;
				instance.transform.SetParent (groundHolder);
				grid [x, y] = new Node (false, pos);
				freePositions.Add (pos);
			}
		}
	}

	private List<Node> GetAdjacentFor (Vector3 pos)
	{
		int x = (int)(pos.x / tilesScale);
		int y = (int)(pos.y / tilesScale);
		List<Node> adjacent = new List<Node> ();
		if (y - 1 > 0 && !grid [x, y - 1].isWall) {
			adjacent.Add (grid [x, y - 1]);
		}
		if (x - 1 > 0 && !grid [x - 1, y].isWall) {
			adjacent.Add (grid [x - 1, y]);
		}
		if (x + 1 < columns && !grid [x + 1, y].isWall) {
			adjacent.Add (grid [x + 1, y]);
		}
		if (y + 1 < rows && !grid [x, y + 1].isWall) {
			adjacent.Add (grid [x, y + 1]);
		}
		return adjacent;
	}

	public List<Vector3> SearchPath (Vector3 startPos, Vector3 finishPos)
	{
		Node start = grid [(int)(startPos.x / tilesScale), (int)(startPos.y / tilesScale)];
		Vector3 finish = new Vector3((int)(finishPos.x / tilesScale), (int)(finishPos.y / tilesScale));
		if (start.position == finish) {
			return null;
		}
		foreach (var n in grid) {
			n.visited = false;
			n.parent = null;
		}

		List<Vector3> path = new List<Vector3> ();
		Queue<Node> queue = new Queue<Node> ();
		
		queue.Enqueue (start);

		while (queue.Count != 0) {
			Node cur = queue.Dequeue ();
			cur.visited = true;

			if (cur.position == finish) {
				do {
					path.Add (cur.position);
					cur = cur.parent;
				} while (cur.parent != null);
				return path;
			}
			List<Node> adjacent = GetAdjacentFor (cur.position);
			foreach (Node node in adjacent) {
				if (node.visited == false) {
					node.parent = cur;
					queue.Enqueue (node);
				}
			}
		}
		return null;
	}

	//Sets up the outer walls and floor (background) of the game board.
	void BoardSetup ()
	{
		//Instantiate Board and set boardHolder to its transform.
		boardHolder = new GameObject ("Board").transform;

		//Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
		for (int x = -1; x < columns + 1; x++) {
			GameObject instance_down = Instantiate (wall, new Vector3 (x * tilesScale, -1 * tilesScale, 0f), Quaternion.identity) as GameObject;
			GameObject instance_up = Instantiate (wall, new Vector3 (x * tilesScale, rows * tilesScale, 0f), Quaternion.identity) as GameObject;
			
			instance_down.transform.SetParent (boardHolder);
			instance_up.transform.SetParent (boardHolder);
		}

		//Loop along y axis, starting from -1 to place floor or outerwall tiles.
		for (int y = 0; y < rows; y++) {
			GameObject instance_left = Instantiate (wall, new Vector3 (-1 * tilesScale, y * tilesScale, 0f), Quaternion.identity) as GameObject;
			GameObject instance_right = Instantiate (wall, new Vector3 (columns * tilesScale, y * tilesScale, 0f), Quaternion.identity) as GameObject;
				
			instance_left.transform.SetParent (boardHolder);
			instance_right.transform.SetParent (boardHolder);
		}
	}
	
	//RandomPosition returns a random position from our list freePositions.
	Vector3 RandomPosition (bool _isWall)
	{
		//Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List freePositions.
		int randomIndex = Random.Range (0, freePositions.Count);
		if (_isWall) {
			int x = (int)(freePositions [randomIndex].x / tilesScale);
			int y = (int)(freePositions [randomIndex].y / tilesScale);
			grid [x, y].isWall = _isWall;
		}
		//Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List freePositions.
		Vector3 randomPosition = freePositions [randomIndex];
		
		//Remove the entry at randomIndex from the list so that it can't be re-used.
		freePositions.RemoveAt (randomIndex);
		//Return the randomly selected Vector3 position.
		return randomPosition;
	}

	void LayoutObjectAtRandom (GameObject tile)
	{
		//Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
		Vector3 randomPosition = RandomPosition (false);

		//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
		GameObject instance = Instantiate (tile, randomPosition, Quaternion.identity) as GameObject;
	}

	void LayoutObjectAtRandom (GameObject tile, Transform holder)
	{
		Vector3 randomPosition;
		
		if (holder == boardHolder) {
			randomPosition = RandomPosition (true);
		} else {
			randomPosition = RandomPosition (false);
		}

		//Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
		GameObject instance = Instantiate (tile, randomPosition, Quaternion.identity) as GameObject;
		instance.transform.SetParent (holder);
	}

	public void ClearPosition (Vector3 pos)
	{
		freePositions.Add (pos);
	}

	public void AddCoin ()
	{
		LayoutObjectAtRandom (coin, coinHolder);
	}
}
