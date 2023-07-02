using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color baseColor;
    [SerializeField] private Color offsetColor;
    [Tooltip("When the horse already goes here, it will be considered done.")]
    [SerializeField] private Color tileDone;
    [SerializeField] private GameObject highlight;
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private GameObject helperImg;
    public bool isDone;
    public int index;

    public void Init(bool isOffset,int ind)
    {
        backgroundRenderer.color = isOffset ? baseColor : offsetColor;
        index = ind;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.gameFinished) return;

        bool isValidPos = GridManager.Instance.HandleTileClicked(transform.position);

        if (isValidPos)
        {
            SetTileDone();
            GameManager.Instance.MoveHorse(transform.position,index);
            GridManager.Instance.CheckIfGameFinished();
        }
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    public void SetTileDone()
    {
        if (isDone) return;
        isDone = true;
        backgroundRenderer.color = tileDone;
        GridManager.Instance.UpateFinishedTilesCount();
    }

    public void ResetTile()
    {
        isDone = false;
        backgroundRenderer.color = baseColor;
    }
}
