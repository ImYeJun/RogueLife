public class BattleStatusEffectExecutedBattleEvent : BattleEvent{
    private readonly IReadOnlyBattleEntity owner;
    private readonly IReadOnlyBattleStatusEffect battleStatusEffect;

    public BattleStatusEffectExecutedBattleEvent(IReadOnlyBattleEntity owner, IReadOnlyBattleStatusEffect battleStatusEffect)
    {
        this.owner = owner;
        this.battleStatusEffect = battleStatusEffect;
    }

    public IReadOnlyBattleEntity Owner => owner;
    public IReadOnlyBattleStatusEffect BattleStatusEffect => battleStatusEffect;
}