using System;
using Battle.Cards.Casters;
using UnityEngine;

[Serializable]
public abstract class CardBattleBehaviour
{
    protected ICardBehaviourOwner owner;

    protected CardBattleBehaviour() {}
    protected CardBattleBehaviour(ICardBehaviourOwner owner)
    {
        this.owner = owner;
    }

    public abstract CardTargetType TargetType { get; }
    public abstract CardTargetType ReflectionTargetType { get; }

    public abstract void OnDraw(BattleContext context);
    public abstract void Execute(BattleContext context, CardCaster caster, CardTarget target);
    public abstract void ExecuteReflection(BattleContext context, CardCaster caster, CardTarget target);
    public abstract bool IsAbleToUse(BattleContext context, CardTarget target);
    public abstract bool IsAbleToUseReflect(BattleContext context, CardTarget target);
    public abstract CardBattleBehaviour Clone(ICardBehaviourOwner owner);
    public bool IsTargetValid(CardTarget target, BattleContext context, bool isReflectionApplied) 
    {
        CardTargetType type = isReflectionApplied ? ReflectionTargetType : TargetType;

        if (type is null)
        {
            Debug.LogError("[CardBattleBehaviour] Hey! You forgot to set CardTargetType in the Inspector!");
            return false;
        }
        if (!type.IsValid(target, context))
        {
            Debug.LogWarning("[CardBattleBehaviour] The given target is not valid.");
            return false;
        }
        return true;
    }
}

[Serializable]
public abstract class CardBattleBehaviour<T, Q> : CardBattleBehaviour 
    where T : CardTarget 
    where Q : CardTarget
{
    [SerializeReference, SubclassSelector] protected CardTargetType targetType;
    [SerializeReference, SubclassSelector] protected CardTargetType reflectionTargetType;

    protected CardBattleBehaviour() {}
    protected CardBattleBehaviour(ICardBehaviourOwner owner) : base(owner)
    {
    }

    public override CardTargetType TargetType { get => targetType; }
    public override CardTargetType ReflectionTargetType { get => reflectionTargetType; }

    public override sealed void Execute(BattleContext context, CardCaster caster, CardTarget target)
    {
        OnExecute(context, caster, (T)target); 
    }
    protected abstract void OnExecute(BattleContext context, CardCaster caster, T target);

    public override sealed void ExecuteReflection(BattleContext context, CardCaster caster, CardTarget target)
    {
        OnExecuteReflection(context, caster, (Q)target);
    }
    protected abstract void OnExecuteReflection(BattleContext context, CardCaster caster, Q target);
}