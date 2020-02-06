using UnityEngine;

public abstract class WallTypeManage:MonoBehaviour
{
    public WallAsset[] walls;
    private WallType _wallType;

    public virtual void SetWallType(WallType type)
    {
        _wallType = type;
    }

    public void ShowWall(bool value)
    {
        foreach (var wall in walls)
        {
            wall.gameObject.SetActive(wall.type == _wallType && value);
        }
    }
}
