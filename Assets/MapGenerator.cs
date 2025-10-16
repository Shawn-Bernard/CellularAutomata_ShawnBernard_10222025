using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapGenerator : MonoBehaviour
{
    public int width;
    public int height;

    public string seed;
    public bool useRandomSeed;

    [Range(0,100)]
    public int randomFillPercent;

    [Range(0, 10)]
    public int mapSmooth;
    int[,] map;

    public int borderSize = 5;
    int[,] borderMap;

[SerializeField] bool ActiveGizmo;

    MeshGenerator meshGenerator => GetComponent<MeshGenerator>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            GenerateMap();
        }
    }

    void GenerateMap()
    {
        map = new int[width, height];
        RandomFillMap();
        for (int i = 0; i < mapSmooth; i++)
        {
            SmoothMap();
        }

        borderMap = new int[width + borderSize * 2,height + borderSize * 2];

        for (int x = 0; x < borderMap.GetLength(0); x++)
        {
            for (int y = 0; y < borderMap.GetLength(1); y++)
            {
                if (x >= borderSize && x < width && y >= borderSize && y < height)
                {
                    borderMap[x,y] = map[x - borderSize,y - borderSize];
                }
                else
                {
                    borderMap[x, y] = 1;
                }
            }
        }
                meshGenerator.GenerateMesh(borderMap, 1);
        
    }

    void RandomFillMap()
    {
        if (useRandomSeed)
        {
            seed = Time.time.ToString();
        }
        // pseudo random number generator
        System.Random prng = new System.Random(seed.GetHashCode());

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 || x == width-1 || y == 0 || y == height - 1)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = (prng.Next(0, 100) < randomFillPercent) ? 1 : 0;
                }
                
            }
        }
    }
    /// <summary>
    /// Runs a for loop that gets the neighbour walls and sets (1) wall or (2) air based on neighbours  
    /// </summary>
    void SmoothMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int neighrbourWallTiles = GetSurroundingWallCount(x, y);
                
                if (neighrbourWallTiles > 4)
                {
                    map[x, y] = 1;
                }
                else if (neighrbourWallTiles < 4)
                {
                    map[x, y] = 0;
                }
            }
        }
    }
    /// <summary>
    /// Runs a for loop around the x,y handed in and returns the walls counted
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="gridY"></param>
    /// <returns></returns>
    int GetSurroundingWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                {
                    if (neighbourX != gridX || neighbourY != gridY)
                    {
                        wallCount += map[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    private void OnDrawGizmos()
    {
        if (ActiveGizmo)
        if (Application.isPlaying)
        {
            if (map != null)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Gizmos.color = (map[x, y] == 1) ? Color.black : Color.white;
                        Vector3 position = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
                        Gizmos.DrawCube(position, Vector3.one);
                    }
                }
            }
            else
            {
                Debug.Log("Map is null");
            }
        }
        
    }
}
