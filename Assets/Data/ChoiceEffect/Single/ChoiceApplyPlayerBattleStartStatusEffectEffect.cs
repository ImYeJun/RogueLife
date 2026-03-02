using System;
using UnityEngine;

[Serializable]
public class ChoiceApplyPlayerBattleStartStatusEffectEffect : IChoiceEffect
{
    [SerializeField] private BattleStatusEffectEntity statusEffectEntity;
    
    [Header("전투 내부에서의 버프 수명")]
    [SerializeField] private bool isStatusEffectEternal; 
    [SerializeField] private int startDuration;
    [SerializeField] private int startStack;

    [Header("이 효과가 몇 번의 전투 동안 지속되는가?")]
    [SerializeField] private bool isBattleStartEffectEternal;
    [SerializeField, Min(1)] private int remainBattleCount = 1; 

    public ChoiceApplyPlayerBattleStartStatusEffectEffect() {}

    public bool IsInstant => true;

    public void Execute(FieldContext context, Node currentNode)
    {
        Battle.StartEffects.ApplyPlayerStatusEffectEffect battleStartEffect;

        if (isBattleStartEffectEternal)
        {
            battleStartEffect = isStatusEffectEternal ? 
                new Battle.StartEffects.ApplyPlayerStatusEffectEffect(statusEffectEntity, startStack) :
                new Battle.StartEffects.ApplyPlayerStatusEffectEffect(statusEffectEntity, startStack, startDuration);
        }
        else
        {
            battleStartEffect = isStatusEffectEternal ? 
                new Battle.StartEffects.ApplyPlayerStatusEffectEffect(remainBattleCount, statusEffectEntity, startStack) :
                new Battle.StartEffects.ApplyPlayerStatusEffectEffect(remainBattleCount, statusEffectEntity, startStack, startDuration);
        }

        context.BattleSystem.AddBattleStartEffect(battleStartEffect);
    }
}