using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private GridManager gridManager;
    [SerializeField] private UIHandler uiHandler;
    [SerializeField] private HorseMovement horsePrefab;
    [SerializeField] private Transform backgroundImage;
    private Transform cameraT;
    private HorseMovement horseMovement;
    public bool gameFinished { get; private set; }
    private int moveCount = 0;

    private void Awake()
    {
        Instance = this;
        cameraT = Camera.main.transform;
    }

    private void Start()
    {
        gridManager.OnGameFinsihed += GridManager_OnGameFinsihed;

        gridManager.InstantiateGrid();

        AdjustCameraAndBackgroundImagePosition();

        InitializeHorse();
    }

    private void AdjustCameraAndBackgroundImagePosition()
    {
        Vector3 wholeGridPos = gridManager.GetWholeGridPosition();
        cameraT.position = wholeGridPos;
        wholeGridPos.z = 0;
        backgroundImage.position = wholeGridPos;
    }

    private void InitializeHorse()
    {
        Vector2 newPos = gridManager.GetRandomCornerTilePosition();
        horseMovement = Instantiate(horsePrefab,
                                    newPos, Quaternion.identity);

        Tile tile = gridManager.GetTileAtPosition(newPos);

        horseMovement.UpdateIndex(tile.index);
        tile.SetTileDone();
        gridManager.UpdateHorseNextValidPositions(tile.index);
    }

    private void ResetHorseDetails()
    {
        Vector2 newPos = gridManager.GetRandomCornerTilePosition();
        horseMovement.transform.position = newPos;

        Tile tile = gridManager.GetTileAtPosition(newPos);

        horseMovement.UpdateIndex(tile.index);
        tile.SetTileDone();
        gridManager.UpdateHorseNextValidPositions(tile.index);
    }

    private void OnDestroy()
    {
        gridManager.OnGameFinsihed -= GridManager_OnGameFinsihed;
    }

    private void GridManager_OnGameFinsihed()
    {
        gameFinished = true;
    }

    public void MoveHorse(Vector2 newPos, int index)
    {
        moveCount++;
        horseMovement.transform.position = newPos;
        horseMovement.UpdateIndex(index);
        gridManager.UpdateHorseNextValidPositions(index);
        uiHandler.UpdateMovesText(moveCount);
    }

    public void RestartGame()
    {
        moveCount = 0;
        uiHandler.UpdateMovesText(moveCount);
        gameFinished = false;
        gridManager.ResetTiles();

        ResetHorseDetails();
    }
}
