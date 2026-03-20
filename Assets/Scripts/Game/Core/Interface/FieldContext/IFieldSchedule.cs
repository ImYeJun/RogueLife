using Battle.BattleResultCommands;

public interface IFieldSchedule {
    public void PendBattleResult(BattleResultCommand battleResult);
    public void RequestBattleTransition();
}