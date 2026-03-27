using System;

public interface IBattleActionScheduler {
    public void Enqueue(IBattleAction action);
    public void EnqueueFront(IBattleAction action);
    public void Pause();
    public void PushActionScope(BattleActionScope scope);

    public bool IsRunning { get; }
    public event Action OnQueueEmpty; 
}