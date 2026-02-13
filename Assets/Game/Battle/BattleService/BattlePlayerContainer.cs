public class BattlePlayerContainer : IBattleEventObserver, IBattlePlayerContainerContext
{
    private BattlePlayer player;

    public BattlePlayer Player { get => player; }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        if (battleEvent is BattleStartEvent payload)
        {
            player = payload.BattlePlayer;
        }
    }
}