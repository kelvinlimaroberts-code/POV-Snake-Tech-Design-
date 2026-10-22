using UnityEngine;


public class GameManager : MonoBehaviour
{
    //[Header("GENERIC")]
    //[Header("MAP")]
    enum GridItems { EMPTY, WALL, APPLE, PLAYER_HEAD, PLAYER_BODY };
    const int gridSize = 22;
    GridItems[,] mapGrid = new GridItems[gridSize,gridSize];  //2d array of the grid map. Access is [y][x], however for our purposes we could still just usit as [x][y]
    //new doesn't need a delete in C# because of garbage collection, but we can still use it to initialize the array
    //however i must remember to write delete when i convert to C++.
    public int tileSize = 10;
    public GameObject cubeBase;

    [Header("APPLE")]
    bool isEaten = true;


    void Start()
    {
        //FILL GRID ON INIT
        //fill as empty
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                mapGrid[i, j] = GridItems.EMPTY;
            }
        }
        //place walls
        for (int i = 0; i < gridSize; i++) //fill first and last rows as walls (y)
        {
            mapGrid[0, i] = GridItems.WALL;
            mapGrid[gridSize - 1, i] = GridItems.WALL;
        }
        for (int i = 1; i < gridSize; i++) //fill first and last columns as walls (x)
        {
            mapGrid[i, 0] = GridItems.WALL;
            mapGrid[i, gridSize - 1] = GridItems.WALL;
        }
        //place player
        PlaceItem(GridItems.PLAYER_HEAD, (gridSize-2) / 2, (gridSize-2) / 2 );
        //place apple
        RegenerateApple();

        //generate all instances of the cube object

    }

    void PlaceItem(GridItems item, int x, int y)
    {
        if (mapGrid[x,y] == GridItems.EMPTY)
        {
            mapGrid[x, y] = item;
        }
    }

    void RemoveItem(int x, int y)
    {
        if(mapGrid[x,y] != GridItems.WALL)
        {
            mapGrid[x, y] = GridItems.EMPTY;
        }
    }

    void GenerateRandomPos(ref int x, ref int y)
    {
        x = Random.Range(0, gridSize);
        y = Random.Range(0, gridSize);
    }

    void RegenerateApple()
    {
        if(isEaten)
        {
            isEaten = false;
            int appleX = 0;
            int appleY = 0;
            GenerateRandomPos(ref appleX, ref appleY);
            while (mapGrid[appleX,appleY] != GridItems.EMPTY)  // make sure that the desired new position for the apple is in an empty spot
            {
                GenerateRandomPos(ref appleX, ref appleY);
            }
            PlaceItem(GridItems.APPLE, appleX, appleY);
        }
    }

    void RenderMap()
    {

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
