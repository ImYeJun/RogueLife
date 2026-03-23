public class BattleEndBattleEvent : BattleEvent
{
    private BattleResultType result;

    public BattleEndBattleEvent(BattleResultType result)
    {
        this.result = result;
    }

    public BattleResultType Result { get => result; }
}