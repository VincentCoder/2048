using System;
using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class PlayingUnit : MonoBehaviour
{
    public int UnitId { get; set; }

    public int RowIndex { get; set; }

    public int ColumnIndex { get; set; }

    private UISprite SelfSprite { get; set; }

    private CardType cardType;

    public CardType CardType
    {
        get
        {
            return this.cardType;
        }
        set
        {
            this.cardType = value;
            this.ResetSprite();
        }
    }

    private void Awake()
    {
        this.SelfSprite = this.gameObject.GetComponent<UISprite>();
    }

    public void ResetSprite()
    {
        if (this.SelfSprite != null)
        {
            this.SelfSprite.spriteName = "Box_" + this.CardType;
        }
    }

    public void MoveTo(Vector3 pos)
    {
        //TweenPosition tweenPosition = TweenPosition.Begin(this.gameObject, 0.1f, pos);
        //tweenPosition.onFinished = new List<EventDelegate>();
        this.transform.localPosition = pos;
    }

    public void MoveTo(int rowId, int columnId)
    {
        this.UnitId = MyTool.CalculateUnitIdByRowAndColumn(rowId, columnId);
        this.RowIndex = rowId;
        this.ColumnIndex = columnId;
        this.transform.localPosition = MyTool.CalculatePositionByRowAndColumn(rowId, columnId);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }

}
