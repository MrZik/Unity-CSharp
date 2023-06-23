using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private int discsInUse = 3;
    [SerializeField] private int startSlot = 0; // 0,1,2 => 1,2,3
    [SerializeField] private int endSlot = 2;
    [SerializeField] private List<DiscSlotUI> slots = new();
    [SerializeField] private List<DiscUI> discs = new();

    private void Awake()
    {
        EnableDiscsToUse();
        PlaceDiscsInInitialPositions();
    }

    private void EnableDiscsToUse()
    {
        for (int i = 0; i < discsInUse; i++)
        {
            discs[i].gameObject.SetActive(true);
        }
    }

    private void PlaceDiscsInInitialPositions()
    {
        for(int i = discsInUse; i > 0;i--)
        {
            slots[startSlot].TryPlaceDisc(discs[i - 1]);
        }

        slots[startSlot].UpdateInteractableDisc();
    }
}
