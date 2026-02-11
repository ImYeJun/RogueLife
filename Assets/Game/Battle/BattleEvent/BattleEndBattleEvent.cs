public class BattleEndBattleEvent : BattleEvent
{
    private BattleResult result;

    public BattleEndBattleEvent(BattleResult result)
    {
        this.result = result;
    }

    public BattleResult Result { get => result; }
}