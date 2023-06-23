using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private float xPosition;
    [SerializeField] private float initialYPosition;
    [SerializeField] private float yPositionIncrement = 35f;
    [field: SerializeField] public Vector2 SlotPosition { get; private set; }

    [SerializeField] private List<DiscUI> discsInPlace = new();
    
    private void Awake()
    {
        SlotPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        if (!eventData.pointerDrag.TryGetComponent(out DiscUI disc)) return;

        disc.SetAnchoredPosition(new Vector2(xPosition, initialYPosition));

    }

    public void TryPlaceDisc(DiscUI disc)
    {
        if(discsInPlace.Count == 0)
        {
            discsInPlace.Add(disc);
            disc.SetAnchoredPosition(new Vector2(xPosition,initialYPosition));
            return;
        }

        if (disc.DiscIndex > discsInPlace[^1].DiscIndex)
        {
            disc.ReturnToPreviousPosition();
        }
        else
        {
            float yPos = yPositionIncrement + discsInPlace[^1].GetAnchoredPosition().y;
            discsInPlace.Add(disc);
            disc.SetAnchoredPosition(new Vector2(xPosition, yPos));
        }
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
}
