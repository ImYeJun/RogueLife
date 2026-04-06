using Battle.HurtSources;

public interface IBattleStatusEffectOwner
{
    public void TryHurt(int amount, BattleHurtSource source);
    public void RequestHeal(int amount, bool isFrontQueue = false);
    public BattleHurtSource GetAsHurtSource();
}