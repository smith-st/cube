using System.Collections.Generic;
using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    public LevelData levelData;
    public int selectedLevel;
    public List<LevelData> levels;
    private FieldController _fieldController;

    public void UpdateCells()
    {
        if (levelData == null)
        {
            return;
        }

        if (_fieldController == null)
        {
            _fieldController = FindObjectOfType<FieldController>();
        }

        if (_fieldController != null)
        {
            _fieldController.UpdateCells(levelData.cells);
        }
    }
}
