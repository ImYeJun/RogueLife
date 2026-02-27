using System;

//TODO Close 했을 때 구독자 해지하는 로직 추가하기
public class BattleActionScope
{
    private int aliveCount = 1;
    public int AliveCount { get => aliveCount; }

    public event Action<BattleContext> OnScopeClose;

    public void Increase()
    {
        aliveCount++;
    }
    
    public void Decrease()
    {
        aliveCount--;
    }

    public void Close(BattleContext context)
    {
        OnScopeClose?.Invoke(context);
    }
}