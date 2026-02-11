public class BattlePhase : IBattlePhaseContext, IBattleEventObserver
{
    private BattleContext context;
    private int currentRemainPhase;

    public void Increase(int amount)
    {
    }

    public void Decrease(int amount)
    {
    }

    public void OnBattleEvent(BattleEvent battleEvent)
    {
        throw new System.NotImplementedException();
    }
}