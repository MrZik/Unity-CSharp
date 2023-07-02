using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text movesText;
    [SerializeField] private GameObject gameOverScreen;

    public void UpdateMovesText(int count)
    {
        movesText.text = $"Moves: {count}";
    }
}
