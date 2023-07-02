using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text movesText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text winLoseText;
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;

    public void UpdateMovesText(int count)
    {
        movesText.text = $"Moves: {count}";
    }

    public void ShowGameOverScreen(bool playerWon)
    {
        gameOverScreen.SetActive(true);

        if (playerWon)
        {
            winLoseText.text = "YOU WIN!";
            winLoseText.color = winColor;
        }
        else
        {
            winLoseText.text = "GAME OVER!";
            winLoseText.color = loseColor;
        }
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }

    public void UpdateStrictMode(bool enable)
    {
        GameManager.Instance.SetStrictMode(enable);
    }
}
