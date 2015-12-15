using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour {
    private const int COLUMNS = 10;
    private const int ROWS = 10;
    private const int MAX_COINS_COUNT = 10;
    private const int MAX_ENEMIES_COUNT = 2;
    private const int ADD_ENEMY_WHEN_SCORE = 10;
    private const int CHANGE_AI_WHEN_SCORE = 30;
    private Node[,] grid;
    private Transform boardHolder;
    private Transform coinHolder;
    private List<Vector3> freePositions = new List<Vector3>();
    private int coinsCount = 0;
    private int enemiesCount = 0;
    private List<Zombie> enemies;

    public static Map instance = null;
    public GameObject hero;
    public GameObject coin;
    public GameObject enemy;
    public GameObject wall;
    public GameObject ground;

    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        grid = new Node[COLUMNS, ROWS];
        enemies = new List<Zombie>();
        CreateMap();
    }

    void Start() {
        LayoutObjectAtRandom(hero);
        LayoutObjectAtRandom(enemy);
        enemiesCount++;
        coinHolder = new GameObject("Coins").transform;
        InvokeRepeating("CheckCoinsCount", 5, 5);
    }

    void Update() {
        if (enemiesCount < MAX_ENEMIES_COUNT && GameManager.instance.score == ADD_ENEMY_WHEN_SCORE) {
            LayoutObjectAtRandom(enemy);
            enemiesCount++;
        }
    }

    private void CreateMap() {
        BoardSetup();
        InitialiseGrid();
        WallsSetup();
    }

    public Vector3 GetCenter() {
        return new Vector3((COLUMNS-1) / 2.0f, (ROWS-1) / 2.0f, -10);
    }

    public List<Vector3> FindPath(Vector3 startPos, Vector3 finishPos) {
        Node start = grid[(int)Math.Round(startPos.x), (int)Math.Round(startPos.y)];
        Vector3 finish = new Vector3((int)Math.Round(finishPos.x), (int)Math.Round(finishPos.y));
        if (start.position == finish) {
            return null;
        }
        foreach (var n in grid) {
            n.visited = false;
            n.parent = null;
        }

        List<Vector3> path = new List<Vector3>();
        Queue<Node> queue = new Queue<Node>();

        queue.Enqueue(start);

        while (queue.Count != 0) {
            Node cur = queue.Dequeue();
            cur.visited = true;

            if (cur.position == finish) {
                do {
                    path.Add(cur.position);
                    cur = cur.parent;
                } while (cur.parent != null);
                return path;
            }
            List<Node> adjacent = GetAdjacentFor(cur.position);
            foreach (Node node in adjacent) {
                if (node.visited == false) {
                    node.parent = cur;
                    queue.Enqueue(node);
                }
            }
        }
        return null;
    }

    private List<Node> GetAdjacentFor(Vector3 pos) {
        int x = (int)(pos.x);
        int y = (int)(pos.y);
        List<Node> adjacent = new List<Node>();
        if (y - 1 >= 0 && !grid[x, y - 1].isWall) {
            adjacent.Add(grid[x, y - 1]);
        }
        if (x - 1 >= 0 && !grid[x - 1, y].isWall) {
            adjacent.Add(grid[x - 1, y]);
        }
        if (x + 1 < COLUMNS && !grid[x + 1, y].isWall) {
            adjacent.Add(grid[x + 1, y]);
        }
        if (y + 1 < ROWS && !grid[x, y + 1].isWall) {
            adjacent.Add(grid[x, y + 1]);
        }
        return adjacent;
    }

    private void AddObject(GameObject obj) { }

    //Sets up the outer walls and floor (background) of the game board.
    private void BoardSetup() {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject("Board").transform;

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -1; x < COLUMNS + 1; x++) {
            GameObject instance_down = Instantiate(wall, new Vector3(x, -1, 0f), Quaternion.identity) as GameObject;
            GameObject instance_up = Instantiate(wall, new Vector3(x, ROWS, 0f), Quaternion.identity) as GameObject;

            instance_down.transform.SetParent(boardHolder);
            instance_up.transform.SetParent(boardHolder);
        }

        //Loop along y axis, starting from -1 to place floor or outerwall tiles.
        for (int y = 0; y < ROWS; y++) {
            GameObject instance_left = Instantiate(wall, new Vector3(-1, y, 0f), Quaternion.identity) as GameObject;
            GameObject instance_right = Instantiate(wall, new Vector3(COLUMNS, y, 0f), Quaternion.identity) as GameObject;

            instance_left.transform.SetParent(boardHolder);
            instance_right.transform.SetParent(boardHolder);
        }
    }

    private void InitialiseGrid() {
        Transform groundHolder = new GameObject("Ground").transform;
        for (int x = 0; x < COLUMNS; x++) {
            for (int y = 0; y < ROWS; y++) {
                Vector3 pos = new Vector3(x, y, 0f);
                GameObject instance = Instantiate(ground, pos, Quaternion.identity) as GameObject;
                instance.transform.SetParent(groundHolder);
                grid[x, y] = new Node(false, pos);
                freePositions.Add(pos);
            }
        }
    }

    private void WallsSetup() {
        for (int i = 0; i < (COLUMNS - 2) * (ROWS - 2) / 4; i++) {
            LayoutObjectAtRandom(wall, boardHolder);
        }
    }

    public Vector3 GetRandomPosition() {
        int randomIndex = Random.Range(0, freePositions.Count);
        Vector3 randomPosition = freePositions[randomIndex];
        return randomPosition;
    }

    //RandomPosition returns a random position from our list freePositions.
    private Vector3 RandomPosition(bool _isWall) {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List freePositions.
        int randomIndex = Random.Range(0, freePositions.Count);
        if (_isWall) {
            int x = (int)(freePositions[randomIndex].x);
            int y = (int)(freePositions[randomIndex].y);
            grid[x, y].isWall = _isWall;
        }
        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List freePositions.
        Vector3 randomPosition = freePositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        freePositions.RemoveAt(randomIndex);
        //Return the randomly selected Vector3 position.
        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject tile) {
        //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
        Vector3 randomPosition = RandomPosition(false);

        //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
        GameObject instance = Instantiate(tile, randomPosition, Quaternion.identity) as GameObject;
        if (tile == enemy) {
            enemies.Add(instance.GetComponent<Zombie>());
        }
    }

    private void LayoutObjectAtRandom(GameObject tile, Transform holder) {
        Vector3 randomPosition;

        if (holder == boardHolder) {
            randomPosition = RandomPosition(true);
        }
        else {
            randomPosition = RandomPosition(false);
        }

        //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
        GameObject instance = Instantiate(tile, randomPosition, Quaternion.identity) as GameObject;
        instance.transform.SetParent(holder);
    }

    private void CheckCoinsCount() {
        if (coinsCount < MAX_COINS_COUNT) {
            LayoutObjectAtRandom(coin, coinHolder);
            coinsCount++;
        }
    }

    private void AddEnemy() {
        if (enemiesCount < MAX_ENEMIES_COUNT) {
            LayoutObjectAtRandom(enemy);
            enemiesCount++;
        }
    }

    public void UpdateEnemiesAI() {
        foreach (var zombie in enemies) {
            if (zombie.walkRandom && GameManager.instance.score == CHANGE_AI_WHEN_SCORE) {
                zombie.walkRandom = false;
            }
            if (GameManager.instance.score > CHANGE_AI_WHEN_SCORE) {
                Zombie.SpeedUp(zombie);
            }
        }
    }

    public void DeleteCoin(GameObject coin) {
        freePositions.Add(coin.transform.position);
        Destroy(coin);
        coinsCount--;
    }

    private class Node {
        public bool isWall { get; set; }

        public bool visited { get; set; }

        public Node parent { get; set; }

        public Vector3 position { get; set; }

        public Node(bool _isWall, Vector3 _position) {
            this.isWall = _isWall;
            this.position = _position;
            this.visited = false;
            this.parent = null;
        }
    };
}
