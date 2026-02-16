public interface IBattleStatusEffectOwner
{
    public void RequestHurt(int amount, BattleHurtSource source);
    public void RequestRemoveStatusEffect(BattleStatusEffect statusEffect);
    public void RequestHeal(int amount);
}