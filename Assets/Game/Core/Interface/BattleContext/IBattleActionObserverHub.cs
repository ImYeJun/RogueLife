public interface IBattleActionObserverHub {
    public void SubscribeInterrupter(IBattleActionInterrupter interrupter);
    public void UnsubscribeInterrupter(IBattleActionInterrupter interrupter);
    public void SubscribePreObserver(IBattleActionPreObserver preObesrver);
    public void UnsubscribePreObserver(IBattleActionPreObserver preObesrver);
    public void SubscribePostObserver(IBattleActionPostObserver postObserver);
    public void UnsubscribePostObserver(IBattleActionPostObserver postObserver);

}