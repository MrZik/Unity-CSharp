using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image image;

    [field: SerializeField] public int DiscIndex { get; private set; }
    [field: SerializeField] public Vector2 PreviousPosition { get; private set; }

    [SerializeField] private RectTransform rectTransform;

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.65f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        if (!eventData.pointerEnter)
        {
            ReturnToPreviousPosition();
            return;
        }

        if (!eventData.pointerEnter.TryGetComponent(out DiscSlotUI slot))
        {
            ReturnToPreviousPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void SetAnchoredPosition(Vector2 newPos)
    {
        rectTransform.anchoredPosition = newPos;
        SetPreviousPosition(newPos);
    }

    public void SetPreviousPosition(Vector2 previousPos)
    {
        PreviousPosition = previousPos;
    }

    public void ReturnToPreviousPosition()
    {
        rectTransform.anchoredPosition = PreviousPosition;
    }

    public Vector2 GetAnchoredPosition()
    {
        return rectTransform.anchoredPosition;
    }

    public void UpdateInteractable(bool isInteractable)
    {
        canvasGroup.interactable = isInteractable;
        image.raycastTarget = isInteractable;
    }
}
