using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscSlotUI : MonoBehaviour, IDropHandler
{
    [SerializeField] private float xPosition;
    [SerializeField] private float initialYPosition;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag == null) return;

        eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(xPosition, initialYPosition, 0);
    }

}
