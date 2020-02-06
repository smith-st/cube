public class Cell : WallTypeManage
{
    public int Id => _id;
    public bool IsFinish => _isFinishCell;


    private int _id;
    protected bool _isFinishCell = false;


    public void SetId(int value)
    {
        _id = value;
        gameObject.name = _id.ToString ();
    }
}
