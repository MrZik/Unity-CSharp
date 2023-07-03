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
    private int moveCount = 0;

    [field: SerializeField] public bool GameFinished { get; private set; }
    public bool IsStrictModeOn { get; private set; }
    [Space]
    [SerializeField] private AudioClip horseMoveClip;
    [SerializeField] private AudioClip gameStartedClip;
    [SerializeField] private AudioClip uiClickClip;
    [SerializeField] private AudioClip gameFinishedClip;

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

        AudioHandler.Instance.PlaySfx(gameStartedClip);
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
        Tile cornerTile = gridManager.GetRandomCornerTile();
        gridManager.UpdateHorseNextValidPositions(cornerTile.GetTilePosition());

        horseMovement = Instantiate(horsePrefab,
                                    cornerTile.GetTilePosition(), Quaternion.identity);
        cornerTile.SetTileDone();

        horseMovement.UpdateIndex(cornerTile.index);

        if (IsStrictModeOn) return;
        gridManager.ShowVisualHelperForNextValidPosition();
    }

    private void ResetHorseDetails()
    {
        Tile cornerTile = gridManager.GetRandomCornerTile();
        gridManager.UpdateHorseNextValidPositions(cornerTile.GetTilePosition());
        horseMovement.transform.position = cornerTile.GetTilePosition();
        cornerTile.SetTileDone();

        horseMovement.UpdateIndex(cornerTile.index);

        if (IsStrictModeOn) return;
        gridManager.ShowVisualHelperForNextValidPosition();
    }

    private void OnDestroy()
    {
        gridManager.OnGameFinsihed -= GridManager_OnGameFinsihed;
    }

    private void GridManager_OnGameFinsihed()
    {
        AudioHandler.Instance.PlaySfx(gameFinishedClip);
        GameFinished = true;
        gridManager.HideVisualHelpers();
        uiHandler.ShowGameOverScreen(true);
    }

    public void MoveHorse(Vector2 newPos, int index)
    {
        AudioHandler.Instance.PlaySfx(horseMoveClip);
        moveCount++;
        horseMovement.transform.position = newPos;
        gridManager.UpdateHorseNextValidPositions(newPos);
        horseMovement.UpdateIndex(index);
        uiHandler.UpdateMovesText(moveCount);
        
        if (IsStrictModeOn) return;
        gridManager.HideVisualHelpers();
        gridManager.ShowVisualHelperForNextValidPosition();
    }

    public void RestartGame()
    {
        AudioHandler.Instance.PlaySfx(uiClickClip);
        GameFinished = false;
        moveCount = 0;

        uiHandler.UpdateMovesText(moveCount);
        uiHandler.HideGameOverScreen();

        gridManager.ResetTiles();
        gridManager.ResetFinishedTileCount();

        ResetHorseDetails();
        
    }

    public void SetStrictMode(bool enabled)
    {
        AudioHandler.Instance.PlaySfx(uiClickClip);
        IsStrictModeOn = enabled;

        RestartGame();
    }

    public void RestartForStrictMode()
    {
        AudioHandler.Instance.PlaySfx(uiClickClip);
        GameFinished = false;
        moveCount = 0;

        uiHandler.UpdateMovesText(moveCount);
        uiHandler.HideGameOverScreen();

        gridManager.ResetTiles();

        ResetHorseDetails();
    }

    public void GameOver()
    {
        AudioHandler.Instance.PlaySfx(gameFinishedClip);
        GameFinished = true;
        gridManager.HideVisualHelpers();
        uiHandler.ShowGameOverScreen(false);
    }
}
