#region

using System;
using System.Collections.Generic;

using UnityEngine;

using Object = UnityEngine.Object;

#endregion

public class PlayingUnitManager
{
    #region Static Fields

    private static readonly Object lockObj = new Object();

    private static PlayingUnitManager instance;

    #endregion

    #region Fields

    private PlayingUnit[,] playingUnitArray = new PlayingUnit[MyTool.ColumnCount, MyTool.RowCount];

    private Dictionary<int, PlayingUnit> playingUnitDic = new Dictionary<int, PlayingUnit>();

    #endregion

    #region Constructors and Destructors

    private PlayingUnitManager()
    {
    }

    #endregion

    #region Public Methods and Operators

    public static PlayingUnitManager GetInstance()
    {
        if (instance == null)
        {
            lock (lockObj)
            {
                instance = new PlayingUnitManager();
            }
        }
        return instance;
    }

    public void AddPlayingUnit(int unitId, CardType cardType)
    {
        GameObject playingUnitObj = (GameObject)Object.Instantiate(Resources.Load("PlayingUnit"));
        int rowId = 0;
        int columnId = MyTool.CalculateRowAndColumnByUnitId(unitId, out rowId);
        playingUnitObj.name = "PlayingUnit_" + unitId + "_" + rowId + "_" + columnId;
        playingUnitObj.transform.parent = GameObject.FindWithTag("PlayingPanel").transform;
        playingUnitObj.transform.localPosition = MyTool.CalculatePositionByRowAndColumn(rowId, columnId);
        playingUnitObj.transform.localScale = new Vector3(1, 1, 1);

        PlayingUnit playingUnit = playingUnitObj.GetComponent<PlayingUnit>();
        playingUnit.UnitId = unitId;
        playingUnit.RowIndex = rowId;
        playingUnit.ColumnIndex = columnId;
        playingUnit.CardType = cardType;

        //this.playingUnitDic.Add(unitId, playingUnit);
        this.playingUnitArray[columnId, rowId] = playingUnit;
    }

    public List<int> GetEmptyUnits()
    {
        List<int> emptyIndexes = new List<int>();
        for (int i = 0; i < MyTool.RowCount; i ++)
        {
            for (int j = 0; j < MyTool.ColumnCount; j ++)
            {
                if (this.playingUnitArray[j, i] == null)
                {
                    emptyIndexes.Add(MyTool.CalculateUnitIdByRowAndColumn(i, j));
                }
            }
        }
        return emptyIndexes;
    }

    public PlayingUnit GetPlayingUnitById(int unitId)
    {
        return this.playingUnitDic.ContainsKey(unitId) ? this.playingUnitDic[unitId] : null;
    }

    public PlayingUnit GetPlayingUnitByRowAndColumn(int rowId, int columnId)
    {
        if (rowId >= MyTool.RowCount || columnId >= MyTool.ColumnCount)
        {
            Debug.LogError("Invalid RowId or ColumnId");
        }
        return this.playingUnitArray[columnId, rowId];
    }

    /*public void MoveAndMerge(FingerGestures.SwipeDirection direction)
    {
        switch (direction)
        {
            case FingerGestures.SwipeDirection.Up:
                {
                    for (int i = 0; i < MyTool.ColumnCount; i ++)
                    {
                        // Move
                        for (int j = 1; j < MyTool.RowCount; j ++)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[i, j];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j - 1; k >= 0; k --)
                                {
                                    if (this.playingUnitArray[i, k] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(k, i);
                                    this.playingUnitArray[i, k] = currentMoveUnit;
                                }
                            }
                        }
                        //Merge
                        for (int j = 0; j < MyTool.RowCount - 1; j ++)
                        {
                            if (this.playingUnitArray[i, j] != null)
                            {
                                if (this.playingUnitArray[i, j + 1] != null)
                                {
                                    this.PlayingUnitMerge(i, j, i, j + 1);
                                    //if (this.playingUnitArray[i, j].Num == this.playingUnitArray[i, j + 1].Num)
                                    //{
                                    //    this.playingUnitArray[i, j].Num *= 2;
                                   //     this.playingUnitArray[i, j + 1].DestroySelf();
                                   //     this.playingUnitArray[i, j + 1] = null;
                                  //  }
                                }
                            }
                        }
                        // Move
                        for (int j = 1; j < MyTool.RowCount; j++)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[i, j];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    if (this.playingUnitArray[i, k] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(k, i);
                                    this.playingUnitArray[i, k] = currentMoveUnit;
                                }
                            }
                        }
                    }
                }
                break;
            case FingerGestures.SwipeDirection.Down:
                {
                    for (int i = 0; i < MyTool.ColumnCount; i++)
                    {
                        // Move
                        for (int j = MyTool.RowCount - 2; j >= 0; j--)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[i, j];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j + 1; k <= MyTool.RowCount - 1; k++)
                                {
                                    if (this.playingUnitArray[i, k] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(k, i);
                                    this.playingUnitArray[i, k] = currentMoveUnit;
                                }
                            }
                        }
                        //Merge
                        for (int j = MyTool.RowCount - 1; j > 0; j--)
                        {
                            if (this.playingUnitArray[i, j] != null)
                            {
                                if (this.playingUnitArray[i, j - 1] != null)
                                {
                                    this.PlayingUnitMerge(i, j, i, j-1);
                                    //if (this.playingUnitArray[i, j].Num == this.playingUnitArray[i, j - 1].Num)
                                    //{
                                    //    this.playingUnitArray[i, j].Num *= 2;
                                   //     this.playingUnitArray[i, j - 1].DestroySelf();
                                   //     this.playingUnitArray[i, j - 1] = null;
                                   // }
                                }
                            }
                        }
                        // Move
                        for (int j = MyTool.RowCount - 2; j >= 0; j--)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[i, j];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j + 1; k <= MyTool.RowCount - 1; k++)
                                {
                                    if (this.playingUnitArray[i, k] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(k, i);
                                    this.playingUnitArray[i, k] = currentMoveUnit;
                                }
                            }
                        }
                    }
                }
                break;
            case FingerGestures.SwipeDirection.Left:
                {
                    for (int i = 0; i < MyTool.RowCount; i++)
                    {
                        // Move
                        for (int j = 1; j < MyTool.ColumnCount; j++)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[j, i];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    if (this.playingUnitArray[k, i] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(i, k);
                                    this.playingUnitArray[k, i] = currentMoveUnit;
                                }
                            }
                        }
                        //Merge
                        for (int j = 0; j < MyTool.ColumnCount - 1; j++)
                        {
                            if (this.playingUnitArray[j, i] != null)
                            {
                                if (this.playingUnitArray[j + 1, i] != null)
                                {
                                    this.PlayingUnitMerge(j, i, j + 1, i);
                                    //if (this.playingUnitArray[j, i].Num == this.playingUnitArray[ j + 1, i].Num)
                                    //{
                                   //     this.playingUnitArray[j, i].Num *= 2;
                                    //    this.playingUnitArray[j + 1, i].DestroySelf();
                                    //    this.playingUnitArray[j + 1, i] = null;
                                   // }
                                }
                            }
                        }
                        // Move
                        for (int j = 1; j < MyTool.ColumnCount; j++)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[j, i];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j - 1; k >= 0; k--)
                                {
                                    if (this.playingUnitArray[k, i] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(i, k);
                                    this.playingUnitArray[k, i] = currentMoveUnit;
                                }
                            }
                        }
                    }
                }
                break;
            case FingerGestures.SwipeDirection.Right:
                {
                    for (int i = 0; i < MyTool.RowCount; i++)
                    {
                        // Move
                        for (int j = MyTool.ColumnCount - 2; j >= 0; j--)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[j, i];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j + 1; k <= MyTool.ColumnCount - 1; k++)
                                {
                                    if (this.playingUnitArray[k, i] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(i, k);
                                    this.playingUnitArray[k, i] = currentMoveUnit;
                                }
                            }
                        }
                        //Merge
                        for (int j = MyTool.ColumnCount - 1; j > 0; j--)
                        {
                            if (this.playingUnitArray[j, i] != null)
                            {
                                if (this.playingUnitArray[ j - 1, i] != null)
                                {
                                    this.PlayingUnitMerge(j, i, j - 1, i);
                                    //if (this.playingUnitArray[j, i].Num == this.playingUnitArray[ j - 1, i].Num)
                                   // {
                                     //   this.playingUnitArray[j,i].Num *= 2;
                                     //   this.playingUnitArray[ j - 1,i].DestroySelf();
                                    //    this.playingUnitArray[j - 1,i] = null;
                                   // }
                                }
                            }
                        }
                        // Move
                        for (int j = MyTool.ColumnCount - 2; j >= 0; j--)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[j, i];
                            if (currentMoveUnit != null)
                            {
                                for (int k = j + 1; k <= MyTool.ColumnCount - 1; k++)
                                {
                                    if (this.playingUnitArray[k, i] != null)
                                    {
                                        break;
                                    }
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(i, k);
                                    this.playingUnitArray[k, i] = currentMoveUnit;
                                }
                            }
                        }
                    }
                }
                break;
        }
    }*/

    public void MoveAndMerge(FingerGestures.SwipeDirection direction)
    {
        switch (direction)
        {
            case FingerGestures.SwipeDirection.Up:
                {
                    for (int i = 0; i < MyTool.ColumnCount; i++)
                    {
                        // Move
                        for (int j = 1; j < MyTool.RowCount; j++)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[i, j];
                            if (currentMoveUnit != null)
                            {
                                if (this.playingUnitArray[i, j - 1] == null)
                                {
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(j - 1, i);
                                    this.playingUnitArray[i, j - 1] = currentMoveUnit;
                                }
                                else if (this.playingUnitArray[i, j - 1].CardType == currentMoveUnit.CardType)
                                {
                                    this.PlayingUnitMerge(i, j, i, j-1);
                                }
                            }
                        }
                    }
                }
                break;
            case FingerGestures.SwipeDirection.Down:
                {
                    for (int i = 0; i < MyTool.ColumnCount; i++)
                    {
                        // Move
                        for (int j = MyTool.RowCount - 2; j >= 0; j--)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[i, j];
                            if (currentMoveUnit != null)
                            {
                                if (this.playingUnitArray[i, j + 1] == null)
                                {
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(j + 1, i);
                                    this.playingUnitArray[i, j + 1] = currentMoveUnit;
                                }
                                else if (this.playingUnitArray[i, j + 1].CardType == currentMoveUnit.CardType)
                                {
                                    this.PlayingUnitMerge(i, j, i, j+1);
                                }
                            }
                        }
                    }
                }
                break;
            case FingerGestures.SwipeDirection.Left:
                {
                    for (int i = 0; i < MyTool.RowCount; i++)
                    {
                        // Move
                        for (int j = 1; j < MyTool.ColumnCount; j++)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[j, i];
                            if (currentMoveUnit != null)
                            {
                                if (this.playingUnitArray[j-1, i] == null)
                                {
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(i, j-1);
                                    this.playingUnitArray[j - 1, i] = currentMoveUnit;
                                }
                                else if (this.playingUnitArray[j - 1, i].CardType == currentMoveUnit.CardType)
                                {
                                    this.PlayingUnitMerge(j, i, j-1, i);
                                }
                            }
                        }
                    }
                }
                break;
            case FingerGestures.SwipeDirection.Right:
                {
                    for (int i = 0; i < MyTool.RowCount; i++)
                    {
                        // Move
                        for (int j = MyTool.ColumnCount - 2; j >= 0; j--)
                        {
                            PlayingUnit currentMoveUnit = this.playingUnitArray[j, i];
                            if (currentMoveUnit != null)
                            {
                                if (this.playingUnitArray[j+1, i] == null)
                                {
                                    this.playingUnitArray[currentMoveUnit.ColumnIndex, currentMoveUnit.RowIndex] = null;
                                    currentMoveUnit.MoveTo(i, j+1);
                                    this.playingUnitArray[j + 1, i] = currentMoveUnit;
                                }
                                else if (this.playingUnitArray[j + 1, i].CardType == currentMoveUnit.CardType)
                                {
                                    this.PlayingUnitMerge(j, i, j+1, i);
                                }
                            }
                        }
                    }
                }
                break;
        }
    }

    public void PlayingUnitMerge(int attackRow, int attackColumn, int defenseRow, int defenseColumn)
    {
        PlayingUnit unitAttack = this.playingUnitArray[attackRow, attackColumn];
        PlayingUnit unitDefense = this.playingUnitArray[defenseRow, defenseColumn];
        if (unitAttack.CardType != unitDefense.CardType)
        {
            return;
        }
        switch (unitAttack.CardType)
        {
            case CardType.WuLong0:
                {
                    unitDefense.CardType = CardType.BuMa1;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.PuEr0:
                {
                    unitDefense.CardType = CardType.BuMa1;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.BuMa1:
                {
                    unitDefense.CardType = CardType.QiQi2;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.QiQi2:
                {
                    unitDefense.CardType = CardType.LanQi3;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.LanQi3:
                {
                    unitDefense.CardType = CardType.GuiXianRen4;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.GuiXianRen4:
                {
                    unitDefense.CardType = CardType.JiaLinXianRen5;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.JiaLinXianRen5:
                {
                    unitDefense.CardType = CardType.YaMuCha6;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.YaMuCha6:
                {
                    unitDefense.CardType = CardType.YaQiLuoBei7;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.YaQiLuoBei7:
                {
                    unitDefense.CardType = CardType.KeLin8;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.KeLin8:
                {
                    unitDefense.CardType = CardType.TianJinFan9;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.TianJinFan9:
                {
                    unitDefense.CardType = CardType.TianShen10;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
            case CardType.TianShen10:
                {
                    unitDefense.CardType = CardType.WuKong11;
                    unitAttack.DestroySelf();
                    this.playingUnitArray[attackRow, attackColumn] = null;
                }
                break;
        }
    }

    /*public void PlayingUnitMerge(int attackRow, int attackColumn, int defenseRow, int defenseColumn)
    {
        PlayingUnit unitAttack = this.playingUnitArray[attackRow, attackColumn];
        PlayingUnit unitDefense = this.playingUnitArray[defenseRow, defenseColumn];
        switch (unitAttack.CardType)
        {
                case CardType.AntiKing:
                {
                    if (unitDefense.CardType == CardType.MilitaryAdviser)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Marshal)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.Bandit:
                {
                    if (unitDefense.CardType == CardType.Bandit)
                    {
                        unitDefense.CardType = CardType.RebelArmy;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Scholar)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Soldier)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.Book:
                {
                    if (unitDefense.CardType == CardType.Farmer)
                    {
                        unitDefense.CardType = CardType.Scholar;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.Farmer:
                {
                    if (unitDefense.CardType == CardType.Farmer)
                    {
                        unitDefense.CardType = CardType.Bandit;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Weapon)
                    {
                        unitDefense.CardType = CardType.Soldier;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Book)
                    {
                        unitDefense.CardType = CardType.Scholar;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.General:
                {
                    if (unitDefense.CardType == CardType.General)
                    {
                        unitDefense.CardType = CardType.Marshal;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.RebelArmy)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.StaffOfficer)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.Marshal:
                {
                    if (unitDefense.CardType == CardType.AntiKing)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.MilitaryAdviser)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.MilitaryAdviser:
                {
                    if (unitDefense.CardType == CardType.Marshal)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.AntiKing)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.RebelArmy:
                {
					if(unitDefense.CardType == CardType.RebelArmy)
					{
						unitDefense.CardType = CardType.AntiKing;
						unitAttack.DestroySelf();
						this.playingUnitArray[attackRow, attackColumn] = null;
					}
                    if (unitDefense.CardType == CardType.StaffOfficer)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.General)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.Scholar:
                {
                    if (unitDefense.CardType == CardType.Scholar)
                    {
                        unitDefense.CardType = CardType.StaffOfficer;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Soldier)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Bandit)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.Soldier:
                {
                    if (unitDefense.CardType == CardType.Soldier)
                    {
                        unitDefense.CardType = CardType.General;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Bandit)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.Scholar)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.StaffOfficer:
                {
                    if (unitDefense.CardType == CardType.StaffOfficer)
                    {
                        unitDefense.CardType = CardType.MilitaryAdviser;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.General)
                    {
                        unitDefense.CardType = unitAttack.CardType;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                    else if (unitDefense.CardType == CardType.RebelArmy)
                    {
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
                case CardType.Weapon:
                {
                    if (unitDefense.CardType == CardType.Farmer)
                    {
                        unitDefense.CardType = CardType.Soldier;
                        unitAttack.DestroySelf();
                        this.playingUnitArray[attackRow, attackColumn] = null;
                    }
                }
                break;
        }
    }*/

    public void RemoveAllPlayingUnits()
    {
        this.playingUnitDic.Clear();
        Array.Clear(this.playingUnitArray, 0, this.playingUnitArray.Length);
    }

    public bool RemovePlayingUnit(PlayingUnit playingUnit)
    {
        return playingUnit != null && this.RemovePlayingUnitById(playingUnit.UnitId);
    }

    public bool RemovePlayingUnitById(int unitId)
    {
        if (!this.playingUnitDic.ContainsKey(unitId))
        {
            this.playingUnitDic.Remove(unitId);
            int rowId = 0;
            int columnId = MyTool.CalculateRowAndColumnByUnitId(unitId, out rowId);
            this.playingUnitArray[columnId, rowId] = null;
        }
        return true;
    }

    #endregion
}