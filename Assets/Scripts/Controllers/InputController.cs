using DefaultNamespace;

public class InputController : ControllerWithListener<IInputListener>
{
    public void PressTop()
    {
        _listener?.PlayerPressArrowButton(Direction.Top);
    }

    public void PressBottom()
    {
        _listener?.PlayerPressArrowButton(Direction.Bottom);
    }

    public void PressRight()
    {
        _listener?.PlayerPressArrowButton(Direction.Right);
    }

    public void PressLeft()
    {
        _listener?.PlayerPressArrowButton(Direction.Left);
    }

    public void RestartGame()
    {
        _listener?.RestartGame();
    }

    public void RestartLevel()
    {
        _listener?.RestartLevel();
    }
}
