public interface IBattleActionObserverHub {
    public void SubscribeActionModifier(IBattleActionModifier modifier);
    public void UnsubscribeActionModifier(IBattleActionModifier modifier);
    public void SubscribePreObserver(IBattleActionPreObserver preObesrver);
    public void UnsubscribePreObserver(IBattleActionPreObserver preObesrver);
    public void SubscribePostObserver(IBattleActionPostObserver postObserver);
    public void UnsubscribePostObserver(IBattleActionPostObserver postObserver);

}