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
    private Dictionary<Vector2, Tile> tiles;
    private Vector2[] cornerArr = new Vector2[]
                                { new Vector2(0, 0), new Vector2(0, 7), new Vector2(7, 0), new Vector2(7, 7) };

    [SerializeField] private int[] tileIndex;
    [SerializeField] private int[] validIndexes;
    private int finishedTilesCount = 0;
    
    private void Awake()
    {
        Instance = this;
        tileIndex = new int[width * height];
        validIndexes = new int[8];
    }

    private void Start()
    {
        for (int i = 0; i < width * height; i++) {
            tileIndex[i] = i;
        }
    }

    internal void InstantiateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        int counter = 0;

        for (int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                Tile spawnedTile = Instantiate(tilePrefab, new Vector3(i,j),Quaternion.identity);
                spawnedTile.name = $"Tile {i} : {j}";

                var isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);

                spawnedTile.Init(isOffset, counter);
                tiles[new Vector2(i,j)] = spawnedTile;
                counter++;
            }
        }
    }

    public void ResetTiles()
    {
        foreach (Tile tile in tiles.Values)
        {
            tile.ResetTile();
        }
    }

    public Vector3 GetWholeGridPosition()
    {
        return new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if (tiles.TryGetValue(position, out Tile tile)) return tile;
        return null;
    }

    public Vector3 GetRandomCornerTilePosition()
    {
        return cornerArr[UnityEngine.Random.Range(0,cornerArr.Length)];
    }

    /// <summary>
    /// Index 0=0,0 Index 1=0,7 Index 2=7,0 Index 3=7,7
    /// </summary>
    /// <param name="corner"></param>
    /// <returns></returns>
    //public Vector3 GetCornerTilePositionAtIndex(int corner)
    //{
    //    if (corner > cornerArr.Length) return Vector2.zero;
    //    return cornerArr[corner];
    //}

    public bool HandleTileClicked(Vector2 pos)
    {
        if (GetTileAtPosition(pos) == null) return false;

        int newIndex = GetTileAtPosition(pos).index;

        if (validIndexes.Contains(newIndex))
        {
            return true;
        }

        return false;
    }

    public void CheckIfGameFinished()
    {
        if (finishedTilesCount == tileIndex.Length)
        {
            OnGameFinsihed?.Invoke();
        }
    }

    public void UpateFinishedTilesCount()
    {
        finishedTilesCount++;
    }

    public void UpdateHorseNextValidPositions(int currentIndex)
    {
        validIndexes[0] = currentIndex + 6;
        validIndexes[1] = currentIndex + 10;
        validIndexes[2] = currentIndex + 15;
        validIndexes[3] = currentIndex + 17;
        validIndexes[4] = currentIndex - 6;
        validIndexes[5] = currentIndex - 10;
        validIndexes[6] = currentIndex - 15;
        validIndexes[7] = currentIndex - 17;
    }

    public void ShowVisualHelperForNextValidPosition()
    {

    }

    public void HideVisualHelpers()
    {

    }
}
