using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class DiscUI : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image image;

    [field: SerializeField] public int DiscIndex { get; private set; }
    [field: SerializeField] public Vector2 PreviousPosition { get; private set; }
    [field: SerializeField] public DiscSlotUI PreviousDiscSlot { get; private set; }

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private AudioClip onPickUpClip;
    [SerializeField] private AudioClip onFailDropClip;

    public async void OnBeginDrag(PointerEventData eventData)
    {
        PreviousDiscSlot.RemoveDisc(this);
        await Task.Yield();
        PreviousDiscSlot.UpdateInteractableDisc();

        canvasGroup.alpha = 0.65f;
        canvasGroup.blocksRaycasts = false;
        AudioHandler.Instance.PlayAudio(onPickUpClip);
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
            AudioHandler.Instance.PlayAudio(onFailDropClip);
            ReturnToPreviousPosition();
            return;
        }

        if (!eventData.pointerEnter.TryGetComponent(out DiscSlotUI slot))
        {
            ReturnToPreviousPosition();
            AudioHandler.Instance.PlayAudio(onFailDropClip);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void UpdateData(DiscSlotUI slot, Vector2 newPos)
    {
        rectTransform.anchoredPosition = newPos;
        PreviousDiscSlot = slot;
    }

    public void ReturnToPreviousPosition()
    {
        PreviousDiscSlot.ReturnDiscToSlot(this);
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
