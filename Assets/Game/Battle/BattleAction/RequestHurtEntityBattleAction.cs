using UnityEngine;

public class RequestHurtEntityBattleAction : IBattleAction, IEntityTargetedBattleAction
{
    private BattleHurtSource source;
    private int damage;
    private BattleEntity target;

    public RequestHurtEntityBattleAction(BattleHurtSource source, int damage, BattleEntity target)
    {
        this.source = source;
        this.damage = damage;
        this.target = target;
    }

    public BattleHurtSource Source { get => source; }
    public int Damage { get => damage; }
    public BattleEntity Target { get => target; }

    public void Execute(BattleContext context)
    {
        target.RequestHurt(damage, source);
    }

    public void AddDamage(int amount) { damage += amount; }
    public void ReduceDamage(int amount) { damage = Mathf.Min(damage - amount, 0); }
}