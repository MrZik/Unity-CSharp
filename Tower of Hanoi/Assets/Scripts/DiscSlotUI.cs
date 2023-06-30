using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscSlotUI : MonoBehaviour, IDropHandler
{
    public event Action<int,int> OnDiscAddedToSlot;
    public int slotIndex;
    [SerializeField] private float xPosition;
    [SerializeField] private float initialYPosition;
    [SerializeField] private float yPositionIncrement = 35f;
    [field: SerializeField] public Vector2 SlotPosition { get; private set; }
    [Space]
    [SerializeField] private AudioClip onSuccessDropClip;
    [SerializeField] private AudioClip onFailDropClip;
    [Space]
    [SerializeField] private List<DiscUI> discsInPlace = new();
    
    private void Awake()
    {
        SlotPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (!eventData.pointerDrag.TryGetComponent(out DiscUI disc)) return;

        PlaceDisc(disc);
    }

    public void PlaceDisc(DiscUI disc)
    {
        if (discsInPlace.Count == 0)
        {
            AudioHandler.Instance.PlayAudio(onSuccessDropClip);
            discsInPlace.Add(disc);
            disc.UpdateData(this, new Vector2(xPosition, initialYPosition));

            OnDiscAddedToSlot?.Invoke(discsInPlace.Count, slotIndex);
            
            return;
        }

        if(disc.DiscIndex > discsInPlace[^1].DiscIndex)
        {
            AudioHandler.Instance.PlayAudio(onFailDropClip);
            disc.ReturnToPreviousPosition();
            return;
        }

        AudioHandler.Instance.PlayAudio(onSuccessDropClip);

        float yPos = yPositionIncrement + discsInPlace[^1].GetAnchoredPosition().y;
        discsInPlace.Add(disc);
        UpdateInteractableDisc();

        disc.UpdateData(this, new Vector2(xPosition, yPos));
        
        OnDiscAddedToSlot?.Invoke(discsInPlace.Count, slotIndex);
    }

    public void ReturnDiscToSlot(DiscUI disc)
    {
        if (discsInPlace.Count == 0)
        {
            discsInPlace.Add(disc);
            disc.UpdateData(this, new Vector2(xPosition, initialYPosition));
        }
        else
        {
            float yPos = yPositionIncrement + discsInPlace[^1].GetAnchoredPosition().y;
            discsInPlace.Add(disc);
            disc.UpdateData(this, new Vector2(xPosition, yPos));
        }

        UpdateInteractableDisc();
    }

    public void UpdateInteractableDisc()
    {
        for (int i = 0; i < discsInPlace.Count; i++)
        {
            if (discsInPlace[i] != discsInPlace[^1])
            {
                discsInPlace[i].UpdateInteractable(false);
            }
            else
            {
                discsInPlace[i].UpdateInteractable(true);
            }
        }
    }

    public void ClearDiscList()
    {
        discsInPlace.Clear();
    }

    public int DiscInSlotCount()
    {
        return discsInPlace.Count;
    }

    public void DisableInteractabilityOnAllDisc()
    {
        for (int i = 0; i < discsInPlace.Count; i++)
        {
            discsInPlace[i].UpdateInteractable(false);
        }
    }

    public void RemoveDisc(DiscUI discUI)
    {
        discsInPlace.Remove(discUI);
    }
}
