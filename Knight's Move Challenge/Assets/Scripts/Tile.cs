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
    private bool offSet;

    public void Init(bool isOffset,int ind)
    {
        offSet = isOffset;
        backgroundRenderer.color = isOffset ? baseColor : offsetColor;
        index = ind;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.GameFinished) return;

        bool isValidPos = GridManager.Instance.IsTileValidPosition(index, GetTilePosition());

        if (isValidPos)
        {
            GameManager.Instance.MoveHorse(GetTilePosition(), index);

            // if this tile is already done but we still went to it = game over
            if (GameManager.Instance.IsStrictModeOn && isDone)
            {
                GameManager.Instance.GameOver();
            }

            SetTileDone();
            //GridManager.Instance.CheckIfGameFinished();
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

        backgroundRenderer.color = offSet ? baseColor : offsetColor;
        HideTileHelper();
    }

    public Vector2 GetTilePosition()
    {
        return new Vector2(transform.position.x,transform.position.y);
    }

    public void ShowTileHelper()
    {
        helperImg.SetActive(true);
    }

    public void HideTileHelper()
    {
        helperImg.SetActive(false);
    }
}
