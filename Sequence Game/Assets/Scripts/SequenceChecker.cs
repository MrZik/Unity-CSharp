using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceChecker : MonoBehaviour
{
    public static SequenceChecker Instance { get; private set; }

    [SerializeField] private SequenceCreator sequenceCreator;
    [SerializeField] private SequnceFeedback sequenceFeedback;
    [Space]
    [SerializeField] private int playerCurrentSequencePosition = 0;
    [SerializeField] private int playerCurrentStrike = 0;
    [Space]
    [SerializeField] private AudioClip gameOverClip;

    private WaitForSeconds interval;

    public int StrikeCount => playerCurrentStrike;
    private const int MAX_STIKES = 3;

    private void Awake()
    {
        Instance = this;
        interval = new WaitForSeconds(0.75f);
    }

    public void CheckPlayerInput(int index)
    {
        if (!sequenceCreator.SequnceListHasIndex(index))
        {
            // THAT BUTTON WAS NOT INCLUDED IN THE SEQUENCE
            // WRONG SEQUENCE
            playerCurrentStrike++;
            GameManager.Instance.SwitchState(GameState.WrongSequence);
            return;
        }

        if(index == sequenceCreator.GetSequnceNumberAtIndex(playerCurrentSequencePosition))
        {
            playerCurrentSequencePosition++;
            sequenceFeedback.UdateLightPosition(index);
            sequenceFeedback.TurnOnLight();

            if (playerCurrentSequencePosition >= sequenceCreator.TotalSequenceCount)
            {
                // SEQUENCE FINISHED

                GameManager.Instance.SwitchState(GameState.CorrectSequnce);
            }
            return;
        }

        // WRONG SEQUENCE
        playerCurrentStrike++;
        GameManager.Instance.SwitchState(GameState.WrongSequence);
    }

    public void ResetPlayerCurrentSequence()
    {
        playerCurrentSequencePosition = 0;
    }

    public void HandleStrikeCount()
    {
        if (playerCurrentStrike >= MAX_STIKES)
        {
            // GAME OVER
            
            StartCoroutine(DelayNextInitialiation());
        }
    }

    public void ResetStrikeCount()
    {
        playerCurrentStrike = 0;
    }

    public bool IsStrikedOut()
    {
        return playerCurrentStrike >= MAX_STIKES;
    }

    private IEnumerator DelayNextInitialiation()
    {
        yield return interval;
        AudioHandler.Instance.PlaySfx(gameOverClip);
        GameManager.Instance.SwitchState(GameState.GameOver);
    }
}
