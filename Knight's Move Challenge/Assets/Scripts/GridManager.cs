using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    public event Action OnGameFinsihed;

    [Header("Tile Size: X and Y")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [Space]
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private Tile[] tilesArr;
    private List<Tile> activeHelperTiles = new();

    // 0, 7, 56, 63
    private readonly int[] cornerIndexes = new int[4] { 0, 7, 56, 63 };

    [SerializeField] private Vector2[] validPositions;
    [SerializeField] private int finishedTilesCount = 0;
    private Dictionary<Vector2, Tile> tileDictionar = new Dictionary<Vector2, Tile>();
    
    private void Awake()
    {
        Instance = this;
        tilesArr = new Tile[width * height];
        validPositions = new Vector2[8];
    }

    internal void InstantiateGrid()
    {
        int counter = 0;

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Tile spawnedTile = Instantiate(tilePrefab, new Vector3(i,j),Quaternion.identity);
                spawnedTile.name = $"Tile {i} : {j}";

                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);

                spawnedTile.Init(isOffset, counter);
                tileDictionar.Add(new Vector2(i,j),spawnedTile);
                tilesArr[counter] = spawnedTile;
                counter++;
            }
        }
    }

    public void ResetTiles()
    {
        for (int i = 0; i < tilesArr.Length; i++)
        {
            tilesArr[i].ResetTile();
        }
    }

    public void ResetFinishedTileCount()
    {
        finishedTilesCount = 0;
    }

    public Vector3 GetWholeGridPosition()
    {
        return new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtIndex(int index)
    {
        if (tilesArr.Contains(tilesArr[index])) return tilesArr[index];
        return null;
    }

    public Tile GetRandomCornerTile()
    {
        int index = cornerIndexes[UnityEngine.Random.Range(0, cornerIndexes.Length)];
        return tilesArr[index];
    }

    public bool IsTileValidPosition(int index,Vector2 pos)
    {
        if (GetTileAtIndex(index) == null) return false;

        if (validPositions.Contains(pos))
        {
            return true;
        }

        return false;
    }

    public void UpateFinishedTilesCount()
    {
        finishedTilesCount++;

        if (finishedTilesCount == tilesArr.Length)
        {
            OnGameFinsihed?.Invoke();
        }
    }

    public void UpdateHorseNextValidPositions(Vector2 pos)
    {
        // Clock wise
        // Top right
        validPositions[0] = new Vector2(pos.x + 1, pos.y + 2);
        validPositions[1] = new Vector2(pos.x + 2, pos.y + 1);

        // bottom right
        validPositions[2] = new Vector2(pos.x + 1, pos.y - 2);
        validPositions[3] = new Vector2(pos.x + 2, pos.y - 1);

        // bottom left
        validPositions[4] = new Vector2(pos.x - 1, pos.y - 2);
        validPositions[5] = new Vector2(pos.x - 2, pos.y - 1);

        // top left
        validPositions[6] = new Vector2(pos.x - 1, pos.y + 2);
        validPositions[7] = new Vector2(pos.x - 2, pos.y + 1);

        /* This uses + and - 
        // top right
        //validIndexes[1] = currentIndex + 10;
        //validIndexes[3] = currentIndex + 17;

        // bottom right
        //validIndexes[0] = currentIndex + 6;
        //validIndexes[2] = currentIndex + 15;

        // top left
        //validIndexes[4] = currentIndex - 6;
        //validIndexes[6] = currentIndex - 15;

        // bottom left
        //validIndexes[5] = currentIndex - 10;
        //validIndexes[7] = currentIndex - 17;
        */

        if (GameManager.Instance.IsStrictModeOn)
        {
            int count = 0;

            for (int i = 0; i < validPositions.Length; i++)
            {
                if (tileDictionar.TryGetValue(validPositions[i], out Tile tile))
                {
                    if(tile.isDone)
                    {
                        count++;
                    }
                }
                else
                {
                    count++;
                }
            }

            if(count == validPositions.Length)
            {
                GameManager.Instance.GameOver();
            }
        }
    }

    public void ShowVisualHelperForNextValidPosition()
    {
        for (int i = 0; i < validPositions.Length; i++)
        {
            if(tileDictionar.TryGetValue(validPositions[i],out Tile tile))
            {
                tile.ShowTileHelper();
                activeHelperTiles.Add(tile);
            }
        }
    }

    public void HideVisualHelpers()
    {
        for (int i = 0; i < activeHelperTiles.Count; i++)
        {
            activeHelperTiles[i].HideTileHelper();
        }
    }
}
