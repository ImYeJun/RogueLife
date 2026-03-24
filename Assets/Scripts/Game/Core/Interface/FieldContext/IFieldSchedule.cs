using Battle.BattleResultCommands;

public interface IFieldSchedule {
    public void PendBattleResult(BattleResult battleResult);
    public void RequestBattleTransition();
}