using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Color color1;
    public Color color2;

    GameObject mapObject;
    SpriteRenderer mapRenderer;

    GameObject playerObj;
    public Color playerColor;
    Node playerNode;
    int playerPosx;
    int playerPosy;

    GameObject obstacleObj;
    public Color obstacleColor;
    public ObjectPool obstaclesPool;
    Node[,] obstaclesNodes; 

    private int maxWidth=15;
    private int maxHeight=17;

    Camera cam;

    Node[,] allNodes;
    float moveRate = 0.2f;
    float timer;


    [SerializeField] float waitForNextSpawn=.5f;
    [SerializeField] float delay = .5f;


    void Start()
    {
        cam = Camera.main;
        playerColor = Color.blue;

        CreateMap();
        CreatePlayer(playerColor);
        playerSetPos(0,0);
        
        CreateObstacles();

        cam.transform.position = new Vector3(allNodes[maxWidth / 2, maxHeight / 2].position.x, allNodes[maxWidth / 2, maxHeight / 2].position.y, -10);
        StartCoroutine(SpawnObstacles());
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > moveRate)
        {
            timer = 0;
            if (!GameObject.FindObjectOfType<GameOver>().gameOver)
            {
                MovePlayer();
            }
        }
        
    }

    void CreateMap() {
        mapObject = new GameObject("Map");
        mapRenderer = mapObject.AddComponent<SpriteRenderer>();
        Texture2D txt = new Texture2D(maxWidth, maxHeight);
        allNodes = new Node[maxWidth, maxHeight];

        for (int i = 0; i < maxWidth; i++) {
            for (int j = 0; j < maxHeight; j++) {
                Vector3 tempVector = Vector3.zero;
                tempVector.x = i;
                tempVector.y = j;
                Node node = new Node
                {
                    x = i,
                    y = j
                };
                node.position = tempVector;
                allNodes[i, j] = node;
                #region MapColor
                if (i % 2 != 0)
                {
                    if (j % 2 != 0)
                    {
                        txt.SetPixel(i,j, color1);
                    }
                    else {
                        txt.SetPixel(i, j, color2);
                    }

                }
                else {
                    if (j % 2 != 0)
                    {
                        txt.SetPixel(i, j, color2);
                    }
                    else
                    {
                        txt.SetPixel(i, j, color1);
                    }
                }
                #endregion
            }
        }
        txt.filterMode = FilterMode.Point;
        txt.Apply();
        Rect rect = new Rect(0, 0, maxWidth, maxHeight);
        Sprite sprite = Sprite.Create(txt, rect, Vector2.zero,1,0,SpriteMeshType.FullRect);
        mapRenderer.sprite = sprite;

    }

    void CreatePlayer(Color playerColor) {
        playerObj = new GameObject("Player");
        playerObj.AddComponent<BoxCollider2D>();
        playerObj.GetComponent<BoxCollider2D>().offset = new Vector2(.5f, .5f);
        playerObj.GetComponent<BoxCollider2D>().size = new Vector2(.80f, .80f);
        playerObj.AddComponent<Collisions>();
        playerObj.tag = "Player";
        SpriteRenderer playerSpriteRenderer = playerObj.AddComponent<SpriteRenderer>();

        

        Texture2D txt = new Texture2D(1,1);
        txt.SetPixel(0, 0,playerColor);
        txt.Apply();
        txt.filterMode = FilterMode.Point;
        Rect rect = new Rect(0, 0, 1, 1);
        Sprite sprite = Sprite.Create(txt, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect);
        playerSpriteRenderer.sprite = sprite;
        playerSpriteRenderer.sortingOrder = 1;

    }

    void MovePlayer() {
        if (Input.GetKey(KeyCode.A)) {
            if (playerPosx-1 >= allNodes[0, 0].position.x)
            {
                playerPosx--;
            }
            playerSetPos(playerPosx, playerPosy);
        }
        else if (Input.GetKey(KeyCode.D)) {
            if (playerPosx+1 <= allNodes[maxWidth-1, maxHeight-1].position.x)
            {

                playerPosx++;
            }
            playerSetPos(playerPosx, playerPosy);
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (playerPosy+1 <= allNodes[maxWidth-1, maxHeight-1].position.y)
            {

                playerPosy++;
            }
            playerSetPos(playerPosx, playerPosy);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (playerPosy-1 >= allNodes[0, 0].position.y)
            {
                playerPosy--;
            }
            playerSetPos(playerPosx, playerPosy);

        }
    }
    void playerSetPos(int x, int y) {

        //x ve y sinirlamarini koy.
        playerPosx = x;
        playerPosy = y;
        playerObj.transform.position=allNodes[playerPosx, playerPosy].position;
        playerNode = allNodes[playerPosx, playerPosy];
    }

    /// <summary>
    /// Create one object and its properties then fill the object pool.
    /// </summary>
    void CreateObstacles() {

        
        obstacleObj = new GameObject("Obstacle");
        obstacleObj.AddComponent<BoxCollider2D>();
        SpriteRenderer spriteRenderer = obstacleObj.AddComponent<SpriteRenderer>();
        obstacleObj.GetComponent<BoxCollider2D>().isTrigger = true;
        obstacleObj.GetComponent<BoxCollider2D>().offset= new Vector2(.5f,.5f);
        obstacleObj.GetComponent<BoxCollider2D>().size= new Vector2(.80f,.80f);
        obstacleObj.AddComponent<Rigidbody2D>();
        obstacleObj.GetComponent<Rigidbody2D>().bodyType =  RigidbodyType2D.Kinematic;

        Texture2D txt = new Texture2D(1, 1);
        txt.SetPixel(0, 0, Color.green);
        txt.Apply();
        txt.filterMode = FilterMode.Point;
        Rect rect = new Rect(0, 0, 1, 1);
        Sprite sprite = Sprite.Create(txt, rect, Vector2.zero, 1, 0, SpriteMeshType.FullRect);
        spriteRenderer.sprite = sprite;
        spriteRenderer.sortingOrder = 1;
        obstacleObj.SetActive(false);

        //Fill the Object Pool
        obstaclesPool = new ObjectPool(obstacleObj);
        obstaclesPool.FillPool(50);
    }

    IEnumerator SpawnObstacles() {
        while (true)
        {
            GameObject spawnedObstacle = obstaclesPool.PopPool();
            int xPos = Random.Range(0, maxWidth);
            spawnedObstacle.transform.position = allNodes[xPos, maxHeight - 1].position;
            delay = Mathf.Lerp(0.5f, 0.09f, Difficulty.GetDif());
            waitForNextSpawn = Mathf.Lerp(0.5f, 0.09f, Difficulty.GetDif());
            StartCoroutine(GoDown(spawnedObstacle, delay));
            yield return new WaitForSeconds(waitForNextSpawn);
        }
    }
    IEnumerator GoDown(GameObject obstacle,float delay)
    {
        while (obstacle.transform.position.y >= allNodes[0,0].y-1)// bottom nodes y axis  
        {
            obstacle.transform.Translate(Vector3.down);
            yield return new WaitForSeconds(delay);
        }
        obstaclesPool.AddPool(obstacle.gameObject);
    }


}
