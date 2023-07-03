using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text sequenceCountText;
    [SerializeField] private TMP_Text strikeCountText;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private GameObject clickToStart;
    [Space]
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColor;
    [Space]
    [SerializeField] private AudioClip resetClip;

    internal void UpdateSequenceCountText(int count)
    {
        sequenceCountText.text = count.ToString();
    }

    internal void UpdateStrikeCountText(int count)
    {
        strikeCountText.text = count.ToString();
    }

    internal void UpdateStateText(GameState state)
    {
        switch (state)
        {
            case GameState.Initialize:
                stateText.text = "";
                break;
            case GameState.PlaySequence:
                stateText.color = Color.white;
                stateText.text = "PLAYING SEQUENCE";
                break;
            case GameState.PlayerTurn:
                stateText.color = Color.white;
                stateText.text = "YOUR TURN";
                break;
            case GameState.CorrectSequnce:
                stateText.color = correctColor;
                stateText.text = "CORRECT!";
                break;
            case GameState.WrongSequence:
                stateText.color = wrongColor;
                stateText.text = "WRONG SEQUENCE!";
                break;
            case GameState.GameOver:
                stateText.color = wrongColor;
                stateText.text = "GAME OVER!";
                break;
            case GameState.Reset:
                stateText.color = Color.white;
                stateText.text = "RESETTING GAME";
                break;
        }
    }

    public void ResetGame()
    {
        AudioHandler.Instance.PlaySfx(resetClip);
        GameManager.Instance.ResetGame();
    }

    public void StartGame()
    {
        GameManager.Instance.SwitchState(GameState.Initialize);
        clickToStart.SetActive(false);
    }
}
