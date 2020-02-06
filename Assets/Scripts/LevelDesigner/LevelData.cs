using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Designer/New Level", fileName = "LevelData")]
public class LevelData:ScriptableObject
{
    public int level;
    public int moves;
    public int powerUpMoves;
    public WallType wallType;
    public List<bool> cells;

    public void Init(int level)
    {
        this.level = level;
        moves = 20;
        powerUpMoves = 5;
        wallType = WallType.Small;
        cells = new List<bool>(FieldController.MaxCell);
        for (var i = 0; i < FieldController.MaxCell; i++)
        {
            cells.Add(false);
        }
    }
}