using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LevelDesigner))]
public class LevelDesignerEditor : Editor
{
    private LevelDesigner _levelDesigner;
    private bool _levelLoaded = false;

    private List<LevelData> allLevels
    {
        get => _levelDesigner.levels;
        set => _levelDesigner.levels = value;
    }

    private LevelData currentLevel
    {
        get => _levelDesigner.levelData;
        set => _levelDesigner.levelData = value;
    }

    public override void OnInspectorGUI()
    {
//        base.OnInspectorGUI();
        _levelDesigner = (LevelDesigner) target;
        if (allLevels == null || allLevels.Count == 0)
        {
            LoadAllLevels ();
            LoadLevel(1);
        }

        EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button("Принудительная перезагрузка уровней")){
            LoadAllLevels ();
            LoadLevel(1);
        }
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.LabelField($"Загружен уровень: {currentLevel.level}");
        EditorGUILayout.LabelField($"Название файла: {currentLevel.name}");

        EditorGUILayout.BeginHorizontal ();
        var levelsName = new string[allLevels.Count];
        var levels = new int[allLevels.Count];
        for (int i = 0; i < allLevels.Count; i++)
        {
            levels[i] = allLevels[i].level;
            levelsName[i] = $"Уровень: {allLevels[i].level}";
        }
        _levelDesigner.selectedLevel = EditorGUILayout.IntPopup ("Уровень ",_levelDesigner.selectedLevel, levelsName, levels);
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button("Загрузить уровень")){
            LoadLevel();
        }
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.Separator ();

        for (var i = 0; i < FieldController.MaxCell/10; i++)
        {
            EditorGUILayout.BeginHorizontal ();
            for (var j = 0; j < FieldController.MaxCell/10; j++)
            {
                var cellId = FieldController.MaxCell - 10 + 1 + j - 10*i;
                GUI.backgroundColor = CellHaveWall(cellId) ? Color.red : Color.white;
                if (GUILayout.Button(""))
                {
                    ChangeCellData(cellId);
                }
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal ();
        }

        EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button("Рандом")){
            for (var i = 1; i < FieldController.MaxCell-1; i++)
            {
                if (Random.Range(0, 100) > 75)
                {
                    currentLevel.cells[i] = true;
                }
                else
                {
                    currentLevel.cells[i] = false;
                }
            }
            EditorUtility.SetDirty (currentLevel);
            _levelDesigner.UpdateCells();
        }
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.Separator ();

        GUI.backgroundColor = Color.white;
        EditorGUILayout.BeginHorizontal ();
        currentLevel.moves = EditorGUILayout.IntSlider ("Количеcтво ходов",currentLevel.moves, 10, 100);
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();
        currentLevel.powerUpMoves = EditorGUILayout.IntSlider ("Количеcтво ходов PowerUp",currentLevel.powerUpMoves, 1, 100);
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.BeginHorizontal ();
        currentLevel.wallType = (WallType)EditorGUILayout.EnumPopup ("Тип стен", currentLevel.wallType);
        EditorGUILayout.EndHorizontal ();

        EditorGUILayout.Separator ();

        EditorGUILayout.BeginHorizontal ();
        if (GUILayout.Button("Создать новый уровень")){
            CreateNewLevel ();
        }
        EditorGUILayout.EndHorizontal ();

        if (allLevels.Count > 1)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"Удалить уровень {currentLevel.level}"))
            {
                if (EditorUtility.DisplayDialog("Подтверждение","Удалить уровень?", "Да", "Нет"))
                {
                    DeleteLevel();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Separator();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Сохранить все уровни"))
        {
            EditorSceneManager.SaveScene(SceneManager.GetActiveScene());
        }
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(currentLevel);
        }
    }

    private void DeleteLevel()
    {
        if (allLevels.Count <= 1)
        {
            return;
        }
        var number = currentLevel.level;
        AssetDatabase.DeleteAsset( $"Assets/Resources/Levels/{currentLevel.name}.asset");
        foreach (var item in allLevels)
        {
            if (item.level > number)
            {
                item.level--;
            }
        }
        LoadAllLevels();
        LoadLevel(Mathf.Max(1,number-1));
    }

    private void CreateNewLevel(){
        var asset = CreateInstance<LevelData>();
        AssetDatabase.CreateAsset(asset, $"Assets/Resources/Levels/{RandomString()}.asset");
        var number = allLevels[allLevels.Count - 1].level + 1;
        asset.Init (number);
        AssetDatabase.SaveAssets();
        LoadAllLevels();
        LoadLevel(number);
    }

    private void LoadLevel()
    {
        currentLevel = allLevels[_levelDesigner.selectedLevel-1];
        _levelDesigner.UpdateCells();
    }

    private void LoadLevel(int level)
    {
        _levelDesigner.selectedLevel = level;
        LoadLevel();
    }

    private void LoadAllLevels()
    {
        var levels = Resources.LoadAll<LevelData>("Levels");

        allLevels = new List<LevelData>(levels.Length);
        foreach (var item in levels)
        {
            allLevels.Add(item);
        }
        allLevels.Sort(Compare);
    }

    private static int Compare(LevelData a, LevelData b)
    {
        if (a.level > b.level) {
            return 1;
        }

        if (a.level < b.level) {
            return -1;
        }

        return 0;
    }

    private void ChangeCellData(int cellId){
        if (cellId == 1 || cellId == FieldController.MaxCell)
        {
            return;
        }

        if (currentLevel != null)
        {
            currentLevel.cells[cellId - 1] = !currentLevel.cells[cellId - 1];
            _levelDesigner.UpdateCells();
        }
    }

    private bool CellHaveWall(int cellId)
    {
        return currentLevel != null && currentLevel.cells[cellId - 1];
    }

    private string RandomString()
    {
        const string chars= "abcdefghijklmnopqrstuvwxyz0123456789";
        var str = "";
        for(var i=0; i<6; i++)
        {
            str += chars[Random.Range(0, chars.Length)];
        }
        return str;
    }
}
