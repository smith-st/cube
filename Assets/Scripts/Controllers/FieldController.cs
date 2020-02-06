using System.Collections.Generic;
using EasyButtons;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    public const int MaxCell = 100;
    private List<Cell> _cells;
    private bool _applyId;

    public Vector3 GetCellPosition(int id)
    {
        SetIDForCells();
        if (id >= 1 && id <= MaxCell)
        {
            return _cells[id - 1].transform.position;
        }
        return Vector3.zero;
    }

    public void UpdateWallType(WallType type)
    {
        SetIDForCells();
        foreach (var cell in _cells)
        {
            cell.SetWallType(type);
        }
    }

    public void UpdateCells(List<bool> cellsData)
    {
        SetIDForCells();
        for (var i = 0; i < _cells.Count; i++) {
            _cells [i].ShowWall(cellsData[i]);
        }
    }

    [Button]
    public void SetIDForCells()
    {
        if (_applyId)
        {
            return;
        }
        var cellComponents = gameObject.GetComponentsInChildren<Cell>();
        _cells = new List<Cell>(MaxCell);
        _cells.AddRange(cellComponents);
        _cells.Sort (Compare);
        for (var i = 0; i < _cells.Count; i++) {
            _cells [i].SetId(i + 1);
        }

        _applyId = true;
    }


    private static int Compare(Cell a, Cell b)
    {
        if (a.transform.position.z > b.transform.position.z) {
            return 1;
        } else if (a.transform.position.z < b.transform.position.z) {
            return -1;
        } else {
            if (a.transform.position.x > b.transform.position.x) {
                return 1;
            } else if (a.transform.position.x < b.transform.position.x) {
                return -1;
            } else {
                return 0;
            }
        }
    }
}
