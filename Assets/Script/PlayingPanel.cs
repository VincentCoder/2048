#region

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#endregion

public class PlayingPanel : MonoBehaviour
{
    #region Fields

    private Queue gesturesQueue;

    #endregion

    #region Methods

    private void AddOneRandomUnit()
    {
        int[] indexArray = PlayingUnitManager.GetInstance().GetEmptyUnits().ToArray();
        if (indexArray.Length >= 1)
        {
            PlayingUnitManager.GetInstance()
                .AddPlayingUnit(indexArray[Random.Range(0, indexArray.Length)], Random.Range(0,2) == 0?CardType.WuLong0:CardType.PuEr0);
            /*indexArray = PlayingUnitManager.GetInstance().GetEmptyUnits().ToArray();
            int randomIndex = Random.Range(0, 100);
            if (randomIndex < 20)
            {
                PlayingUnitManager.GetInstance().AddPlayingUnit(indexArray[Random.Range(0, indexArray.Length)], CardType.Weapon);
            }
            else if (randomIndex >= 20 && randomIndex < 70)
            {
                PlayingUnitManager.GetInstance().AddPlayingUnit(indexArray[Random.Range(0, indexArray.Length)], CardType.Book);
            }*/
        }
        //PlayingUnitManager.GetInstance()
            //.AddPlayingUnit(indexArray[Random.Range(0, indexArray.Length)], Random.Range(0, 100) < 70 ? 2 : 4);
    }

    private void DoMove(SwipeGesture gesture)
    {
        switch (gesture.Direction)
        {
            case FingerGestures.SwipeDirection.Left:
                {
                    PlayingUnitManager.GetInstance().MoveAndMerge(FingerGestures.SwipeDirection.Left);
                }
                break;
            case FingerGestures.SwipeDirection.Right:
                {
                    PlayingUnitManager.GetInstance().MoveAndMerge(FingerGestures.SwipeDirection.Right);
                }
                break;
            case FingerGestures.SwipeDirection.Down:
                {
                    PlayingUnitManager.GetInstance().MoveAndMerge(FingerGestures.SwipeDirection.Down);
                }
                break;
            case FingerGestures.SwipeDirection.Up:
                {
                    PlayingUnitManager.GetInstance().MoveAndMerge(FingerGestures.SwipeDirection.Up);
                }
                break;
        }
        this.AddOneRandomUnit();
    }

    private void Init()
    {
        int randomIndex1 = Random.Range(0, MyTool.RowCount * MyTool.ColumnCount);
        PlayingUnitManager.GetInstance().AddPlayingUnit(randomIndex1, Random.Range(0,2) == 0?CardType.WuLong0:CardType.PuEr0);
        int randomIndex2;
        while (true)
        {
            randomIndex2 = Random.Range(0, MyTool.RowCount * MyTool.ColumnCount);
            if (randomIndex1 != randomIndex2)
            {
                break;
            }
        }
        PlayingUnitManager.GetInstance().AddPlayingUnit(randomIndex2, Random.Range(0, 2) == 0?CardType.WuLong0:CardType.PuEr0);


        //PlayingUnitManager.GetInstance().AddPlayingUnit(4, 2);
        //PlayingUnitManager.GetInstance().AddPlayingUnit(12, 2);
        //PlayingUnitManager.GetInstance().AddPlayingUnit(7, 2);
    }

    private void OnSwipe(SwipeGesture gesture)
    {
        this.gesturesQueue.Enqueue(gesture);
        this.DoMove(gesture);
    }

    private void Start()
    {
        this.Init();
        this.gesturesQueue = new Queue();
        //this.Invoke("Test", 3f);
    }

    private void Test()
    {
        PlayingUnitManager.GetInstance().MoveAndMerge(FingerGestures.SwipeDirection.Right);
    }

    #endregion
}