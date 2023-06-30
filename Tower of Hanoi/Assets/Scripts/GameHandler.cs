using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameHandler : MonoBehaviour
{
    //public static GameHandler Instance { get; private set; }
    [SerializeField] private GameTimer gameTimer;
    [Space]
    [SerializeField] private int maxDiscCanUse = 8;
    [SerializeField] private int minDiscCanUse = 3;
    [SerializeField] private int discsInUse = 3;
    [SerializeField] private int startSlot = 0; // 0,1,2 => 1,2,3
    [SerializeField] private int endSlot = 2;
    [Space]
    [Header("UI Elements")]
    [SerializeField] private GameObject finishedVisual;
    [SerializeField] private TextMeshProUGUI movesText;
    [SerializeField] private TextMeshProUGUI discCountText;
    [SerializeField] private TextMeshProUGUI minMovesText;
    [Space]
    [SerializeField] private AudioClip onGameFinsihedAudio;
    [SerializeField] private AudioClip uiClickAudio;
    [Space]
    [SerializeField] private List<DiscSlotUI> slots = new();
    [SerializeField] private List<DiscUI> discs = new();
    private int totalMoves = 0;

    private void Awake()
    {
       // Singleton();

        InitializeGame();
    }

    private void Singleton()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //else
        //{
        //    Destroy(Instance);
        //}
    }

    private void Start()
    {
        movesText.text = $"Moves: {totalMoves}";

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].OnDiscAddedToSlot += GameHandler_OnDiscAddedToSlot;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[endSlot].OnDiscAddedToSlot -= GameHandler_OnDiscAddedToSlot;
        }
    }

    private void GameHandler_OnDiscAddedToSlot(int count,int slotIndex)
    {
        totalMoves++;
        movesText.text = $"Moves: {totalMoves}";

        if (slotIndex != endSlot) return;

        if (count == discsInUse)
        {
            AudioHandler.Instance.PlayAudio(onGameFinsihedAudio);
            DisableDiscsInteractability();
            gameTimer.StopTimer();
            finishedVisual.SetActive(true);
        }

    }

    private void InitializeGame()
    {
        discCountText.text = discsInUse.ToString();
        EnableDiscsToUse();
        PlaceDiscsInInitialPositions();
        InitializeSlotsIndex();
        UpdateMinimumMoves();
        gameTimer.StartTimer();
        finishedVisual.SetActive(false);
    }

    private void EnableDiscsToUse()
    {
        for (int i = 0; i < discs.Count; i++)
        {
            discs[i].gameObject.SetActive(i < discsInUse);
        }
    }

    private void DisableDiscsInteractability()
    {
        slots[endSlot].DisableInteractabilityOnAllDisc();
    }

    private void PlaceDiscsInInitialPositions()
    {
        for(int i = discsInUse; i > 0;i--)
        {
            slots[startSlot].ReturnDiscToSlot(discs[i - 1]);
        }
    }

    private void InitializeSlotsIndex()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].slotIndex = i;
        }
    }

    public void ResetGame()
    {
        AudioHandler.Instance.PlayAudio(uiClickAudio);

        totalMoves = 0;
        movesText.text = $"Moves: {totalMoves}";

        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].ClearDiscList();
        }

        PlaceDiscsInInitialPositions();

        EnableDiscsToUse();

        UpdateMinimumMoves();

        gameTimer.ResetTime();

        finishedVisual.SetActive(false);
    }

    public void IncreaseDiscInUse()
    {
        AudioHandler.Instance.PlayAudio(uiClickAudio);
        discsInUse++;

        if(discsInUse > maxDiscCanUse)
        {
            discsInUse = maxDiscCanUse;
        }

        discCountText.text = discsInUse.ToString();

        ResetGame();
    }

    public void DecreaseDiscInUse()
    {
        AudioHandler.Instance.PlayAudio(uiClickAudio);
        discsInUse--;

        if (discsInUse < minDiscCanUse)
        {
            discsInUse = minDiscCanUse;
        }

        discCountText.text = discsInUse.ToString();

        ResetGame();
    }

    private void UpdateMinimumMoves()
    {
        minMovesText.text = $"Min Moves: {(Mathf.Pow(2, discsInUse) - 1)}";
    }
}
