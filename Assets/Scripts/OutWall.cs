public class OutWall : WallTypeManage
{
    public override void SetWallType(WallType type)
    {
        base.SetWallType(type);
        ShowWall(true);
    }
}
