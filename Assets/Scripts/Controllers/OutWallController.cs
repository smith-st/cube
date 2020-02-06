using UnityEngine;

public class OutWallController : MonoBehaviour
{
    [SerializeField]
    private OutWall[] _walls;

    public void UpdateWallType(WallType type)
    {
        foreach (var wall in _walls)
        {
            wall.SetWallType(type);
        }
    }
}
