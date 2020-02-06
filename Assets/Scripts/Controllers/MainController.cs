using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class MainController : MonoBehaviour, IInputListener, IPlayerListener
{
    public InputController inputController;
    public FieldController fieldController;
    public OutController outController;
    public OutWallController outWallController;

    [Header("Prefabs")]
    public GameObject playerPrefab;
    public GameObject bonusPrefab;

    private LevelData _level;
    private List<LevelData> _levels;
    private PlayerController _player;
    private BonusController _bonus;
    private int _bonusCellId;
    private int _moves;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (inputController != null)
        {
            inputController.AddListener(this);
        }

        LoadLevels();
        StartLevel(1);
    }

    private void StartLevel(int level)
    {
        LoadLevel(level);
        outWallController.UpdateWallType(_level.wallType);
        fieldController.UpdateWallType(_level.wallType);
        fieldController.UpdateCells(_level.cells);
        _moves = _level.moves;
        outController.CountMovesChanged(_moves);
        outController.LevelChanged(level);
        outController.HideAllWindows();
        AddPlayer();
        AddBonus();
    }

    private void DestroyLevel()
    {
        if (_player != null)
        {
            Destroy(_player.gameObject);
        }

        if (_bonus != null)
        {
            Destroy(_bonus.gameObject);
        }
    }

    private void RestartLevel()
    {
        DestroyLevel();
        StartLevel(_level.level);
    }

    private void AddPlayer()
    {
        var cell = fieldController.GetCellPosition(1);
        var pos = new Vector3(cell.x, PlayerController.HalfSize, cell.z);
        _player = Instantiate(playerPrefab, pos, Quaternion.identity).GetComponent<PlayerController>();
        _player.AddListener(this);
    }

    private void AddBonus()
    {
        int id;
        do
        {
            id = Random.Range(2, FieldController.MaxCell - 1);
        } while (_level.cells[id-1]);

        var cell = fieldController.GetCellPosition(id);
        var pos = new Vector3(cell.x, PlayerController.HalfSize, cell.z);
        _bonus = Instantiate(bonusPrefab, pos, Quaternion.identity).GetComponent<BonusController>();
        _bonusCellId = id;
    }

    private void LoadLevels()
    {
        var levels = Resources.LoadAll<LevelData>("Levels");
        _levels = new List<LevelData>(levels.Length);
        foreach (var item in levels)
        {
            _levels.Add(item);
        }
    }
    private void LoadLevel(int level)
    {
        _level = _levels.First(l => l.level == level);
    }

    private void GameOver()
    {
        _player.FinishLevel();
        _player.DestroyPlayer();
        outController.LevelFail();
    }

    public void PlayerPressArrowButton(Direction direction)
    {
        Debug.Log($"Pressed {direction}");
        if (_player != null)
        {
            _player.MoveToDirection(direction);
        }
    }

    public void RestartGame()
    {
        DestroyLevel();
        StartLevel(1);
    }

    void IInputListener.RestartLevel()
    {
        RestartLevel();
    }

    public void PlayerMoved()
    {
        _moves--;
        outController.CountMovesChanged(_moves);
        if (_moves == 0)
        {
            GameOver();
        }
    }

    public void PlayerReachedFinish()
    {
        var currentLevel = _level.level;
        if (_levels.Count > currentLevel)
        {
            DestroyLevel();
            StartLevel(currentLevel + 1);
        }
        else
        {
            outController.FinishGame();
        }
    }

    public void PlayerCollectBonus(BonusController bonus)
    {
        bonus.Collect();
        var path = PathFinder.FindPath(_bonusCellId, _level.cells);
        if (path == null)
        {
            return;
        }
        path.RemoveAt(0);
        var positions = new List<Vector3>(_level.powerUpMoves);
        for (var i = 0; i < path.Count; i++)
        {
            if (i > _level.powerUpMoves-1)
            {
                break;
            }
            positions.Add(fieldController.GetCellPosition(path[i]));
        }
        _player.MoveOnPath(positions);
    }
}
