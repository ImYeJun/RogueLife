using System;

[Serializable]
public abstract class BattleBelongingsBehaviour : IBattleEventObserver
{
    public abstract void OnBattleEvent(BattleEvent battleEvent);
    public abstract BattleBelongingsBehaviour Clone();
}