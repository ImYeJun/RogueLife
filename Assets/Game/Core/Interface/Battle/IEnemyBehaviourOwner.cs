using System.Collections.Generic;

public interface IEnemyBehaviourOwner
{
    public IReadOnlyList<BattleStatusEffect> CurrentBuffs { get; }
    public IReadOnlyList<BattleStatusEffect> CurrentDebuffs { get; }
    public EnemyData Data { get; }

    public void RequestApplyBuff(BattleStatusEffect buff);
    public void RequestHeal(int amount);
    public void RequestHurt(int amount, HurtSource source);
}