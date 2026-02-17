public interface IBattleStatusEffectOwner
{
    public void RequestHurt(int amount, BattleHurtSource source);
    public void RequestHeal(int amount);
}