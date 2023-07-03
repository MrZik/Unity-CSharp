using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightButton : MonoBehaviour
{
    [SerializeField] private AudioClip lightOnClip;
    public int Index {  get; private set; }

    public AudioClip GetOnSound() { return lightOnClip; }

    private void OnMouseDown()
    {
        if (GameManager.Instance.GameState != GameState.PlayerTurn) return;

        AudioHandler.Instance.PlaySfx(lightOnClip);
        SequenceChecker.Instance.CheckPlayerInput(Index);
    }

    internal void SetIndex(int ind)
    {
        Index = ind;
    }
}
