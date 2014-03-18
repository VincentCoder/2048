using UnityEngine;
using System.Collections;

public class MyTool
{
    public static int RowCount = 5;

    public static int ColumnCount = 5;

    public static int PanelWidth = 640;

    public static int UnitWidth = 128;

    public static int UnitGap = 0;

    public static int CalculateUnitIdByRowAndColumn(int rowId, int columnId)
    {
        if (rowId >= RowCount || columnId >= ColumnCount)
        {
            Debug.LogError("Invalid RowId or ColumnId");
        }
        return rowId * ColumnCount + columnId;
    }

    public static int CalculateRowAndColumnByUnitId(int unitId, out int rowId)
    {
        if (unitId >= RowCount * ColumnCount)
        {
            Debug.LogError("Invalid UnitId");
        }
        rowId = unitId / ColumnCount;
        return unitId % ColumnCount;
    }

    public static Vector3 CalculatePositionByRowAndColumn(int rowId, int columnId)
    {
        int x = UnitGap + UnitWidth / 2 + columnId * (UnitGap + UnitWidth) - PanelWidth / 2;
        int y = PanelWidth / 2 - (UnitGap + UnitWidth / 2 + rowId * (UnitGap + UnitWidth));
        return new Vector3(x, y, 0);
    }

}
