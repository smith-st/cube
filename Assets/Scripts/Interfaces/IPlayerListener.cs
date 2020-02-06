namespace DefaultNamespace
{
    public interface IPlayerListener
    {
        void PlayerMoved();
        void PlayerReachedFinish();
        void PlayerCollectBonus(BonusController bonus);
    }
}