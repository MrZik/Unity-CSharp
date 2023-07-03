using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState GameState {get; private set;}
    [Header("Script References")]
    [SerializeField] private SequenceCreator sequenceCreator;
    [SerializeField] private SequnceFeedback sequenceFeedback;
    [SerializeField] private LightButtonHandler lightButtonsHandler;
    [SerializeField] private UIHandler uiHandler;

    private WaitForSeconds interval;

    private void Awake()
    {
        Instance = this;
        interval = new WaitForSeconds(1f);
    }

    private void Start()
    {
        SwitchState(GameState.NewGame);
    }

    public void SwitchState(GameState newState)
    {
        GameState = newState;
        uiHandler.UpdateStateText(GameState);

        switch (newState)
        {
            case GameState.NewGame:
                break;
            case GameState.Initialize:
                lightButtonsHandler.InitializeButtons();
                sequenceCreator.InitializeSequenceCreator();
                SequenceChecker.Instance.ResetPlayerCurrentSequence();

                uiHandler.UpdateSequenceCountText(sequenceCreator.TotalSequenceCount);
                
                // We are putting sequnce feedback here because
                // it will call the next state
                sequenceFeedback.InitializeSequenceFeedback();
                break;
            case GameState.CreateSequence:
                sequenceCreator.CreateSequnce();
                break;
            case GameState.PlaySequence:
                sequenceFeedback.PlaySequenceFeedback();
                break;
            case GameState.PlayerTurn:
                sequenceFeedback.TurnOffLight();
                break;
            case GameState.CorrectSequnce:
                sequenceFeedback.DecreaseSequenceInterval();
                sequenceCreator.IncreaseSequenceCount();
                break;
            case GameState.WrongSequence:
                uiHandler.UpdateStrikeCountText(SequenceChecker.Instance.StrikeCount);
                sequenceFeedback.IncreaseSequenceInterval();

                SequenceChecker.Instance.HandleStrikeCount();

                if (SequenceChecker.Instance.IsStrikedOut()) break;
                sequenceCreator.DecreaseSequenceCount();
                break;
            case GameState.GameOver:
                break;
            case GameState.Reset:
                break;
        }
    }

    public void ResetGame()
    {
        SwitchState(GameState.Reset);
        sequenceFeedback.ResetGamePressed();

        sequenceCreator.ResetSequenceCount();

        SequenceChecker.Instance.ResetStrikeCount();

        uiHandler.UpdateStrikeCountText(0);
        
        StartCoroutine(ResetGameCoroutine());
    }

    private IEnumerator ResetGameCoroutine()
    {
        yield return interval;

        SwitchState(GameState.Initialize);
    }
}

public enum GameState
{
    NewGame = 0,
    Initialize = 1,
    CreateSequence = 2,
    PlaySequence = 3,
    PlayerTurn = 4,
    CorrectSequnce = 5,
    WrongSequence = 6,
    GameOver = 7,
    Reset = 8
}
