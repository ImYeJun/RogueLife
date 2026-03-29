using Battle.HurtSources;
using UnityEngine;

public class RequestHurtEntityBattleAction : IBattleAction, IEntityTargetedBattleAction
{
    private BattleHurtSource source;
    private int damage;
    private BattleEntity target;
    private bool hasNullified = false;

    public RequestHurtEntityBattleAction(BattleHurtSource source, int damage, BattleEntity target)
    {
        this.source = source;
        this.damage = damage;
        this.target = target;
    }

    public BattleHurtSource Source { get => source; }
    public int Damage { get => damage; }
    public BattleEntity Target { get => target; }
    public bool HasNullified { get => hasNullified; }

    public void Execute(BattleContext context)
    {
        if (hasNullified || damage <= 0) { return; }
        
        target.RequestHurt(damage, source);
    }

    public void AddDamage(int amount) { damage += amount; }
    public void ReduceDamage(int amount) { damage = Mathf.Max(damage - amount, 0); }
    public void Nullify() { hasNullified = true; }
}