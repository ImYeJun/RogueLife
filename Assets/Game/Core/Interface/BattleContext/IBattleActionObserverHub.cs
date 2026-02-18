using System;

public interface IBattleActionObserverHub {
    public void SubscribeActionModifier<T>(Action<T, BattleContext> modifier, PipelinePhaseStep step = PipelinePhaseStep.MAIN) where T : IBattleAction;
    public void SubscribePreObserver<T>(Action<T, BattleContext> preObserver, PipelinePhaseStep step = PipelinePhaseStep.MAIN) where T : IBattleAction;
    public void SubscribePostObserver<T>(Action<T, BattleContext> postObserver, PipelinePhaseStep step = PipelinePhaseStep.MAIN) where T : IBattleAction;
    public void UnsubscribeActionModifier<T>(Action<T, BattleContext> modifier) where T : IBattleAction;
    public void UnsubscribePreObserver<T>(Action<T, BattleContext> preObserver) where T : IBattleAction;
    public void UnsubscribePostObserver<T>(Action<T, BattleContext> postObserver) where T : IBattleAction;
}