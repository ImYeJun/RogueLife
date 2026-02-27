using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CompositeCardTargetType : CardTargetType
{
    [SerializeReference, SubclassSelector] private List<CardTargetType> requiredTypes;

    public override bool IsValid(CardTarget target, BattleContext context)
    {
        if (target is not CompositeCardTarget compositeTarget)
        {
            return false;
        }

        if (requiredTypes.Count != compositeTarget.Targets.Count)
        {
            return false;
        }

        for (int i = 0; i < requiredTypes.Count; i++)
        {
            CardTargetType requiredType = requiredTypes[i];
            CardTarget actualTarget = compositeTarget.Targets[i];

            if (!requiredType.IsValid(actualTarget, context))
            {
                return false;
            }
        }

        return true;
    }
}