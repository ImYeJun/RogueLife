public interface IBattleScheduler {
    public void EndPlayerTurn();
    public void EndEnemyTurn();
    public void EndBattle(BattleResultType result);
}