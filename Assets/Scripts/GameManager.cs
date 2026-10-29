using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("GENERIC")]
    public int tileSize = 10; // a buffer putting distance between voxels
    //[Header("MAP")]
    enum GridItems { EMPTY, WALL, APPLE, PLAYER_HEAD, PLAYER_BODY };
    const int gridSize = 22;
    GridItems[,] mapGrid = new GridItems[gridSize,gridSize];  //2d array of the grid map. Access is [y][x], however for our purposes we could still just usit as [x][y]
    //new doesn't need a delete in C# because of garbage collection, but we can still use it to initialize the array
    //however x must remember to write delete when x convert to C++.
    GameObject[,] voxels = new GameObject[gridSize, gridSize]; //this will hold all of the initialised cubes to show the map


    [Header("APPLE")]
    bool isEaten = true;


    void Start()
    {
        //FILL GRID ON INIT
        //fill as empty
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                mapGrid[x, y] = GridItems.EMPTY;
            }
        }
        //place walls
        for (int y = 0; y < gridSize; y++) //fill first and last rows as walls (y)
        {
            mapGrid[0, y] = GridItems.WALL;
            mapGrid[gridSize - 1, y] = GridItems.WALL;
        }
        for (int x = 1; x < gridSize; x++) //fill first and last columns as walls (x)
        {
            mapGrid[x, 0] = GridItems.WALL;
            mapGrid[x, gridSize - 1] = GridItems.WALL;
        }
        //place player
        PlaceItem(GridItems.PLAYER_HEAD, (gridSize-2) / 2, (gridSize-2) / 2 );
        //place apple
        RegenerateApple();

        //generate all instances of the cube object
        InstantiateAllVoxels();
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



    void InstantiateAllVoxels()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                voxels[x,y] = GameObject.CreatePrimitive(PrimitiveType.Cube);   //the line that creates the cube, and stores it in my voxels[,] array
                voxels[x, y].transform.position = new Vector3(x*tileSize, 0, y*tileSize);  //actually put the y on the z axis, cause x want it to be flat on the floor
            }
        }

    }

    void RenderMap()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                switch (mapGrid[x,y])
                {
                    case GridItems.EMPTY:
                        voxels[x, y].GetComponent<Renderer>().material.color = Color.grey;
                        break;

                    case GridItems.WALL:
                        voxels[x,y].GetComponent<Renderer>().material.color = Color.white;
                        break;

                    case GridItems.APPLE:
                        voxels[x, y].GetComponent<Renderer>().material.color = Color.red;
                        break;

                    case GridItems.PLAYER_HEAD:
                        voxels[x, y].GetComponent<Renderer>().material.color = Color.blue;
                        break;

                    case GridItems.PLAYER_BODY:
                        voxels[x, y].GetComponent<Renderer>().material.color = Color.green;
                        break;
                }
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        RenderMap();
    }
}
