namespace DefaultNamespace
{
    public interface IInputListener
    {
        void PlayerPressArrowButton(Direction direction);
        void RestartGame();
        void RestartLevel();
    }
}