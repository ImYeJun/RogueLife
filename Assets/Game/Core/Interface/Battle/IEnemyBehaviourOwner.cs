using System.Collections.Generic;
using Battle.HurtSource;

public interface IEnemyBehaviourOwner
{
    public bool IsFirstAction { get; }
    public int PreviousActionCount { get; }
    public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentBuffs { get; }
    public IReadOnlyDictionary<BattleStatusEffectData, BattleStatusEffect> CurrentDebuffs { get; }
    public EnemyData Data { get; }

    public void ApplyStatusEffect(BattleStatusEffect newEffect);
    public void RequestHeal(int amount);
    public void RequestHurt(int amount, BattleHurtSource source);
}