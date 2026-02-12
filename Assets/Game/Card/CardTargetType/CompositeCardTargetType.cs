using System.Collections.Generic;

public class CompositeCardTargetType : CardTargetType
{
    private List<CardTargetType> requiredTypes;

    public CompositeCardTargetType(List<CardTargetType> requiredTypes)
    {
        this.requiredTypes = requiredTypes;
    }

    public List<CardTargetType> RequiredTypes { get => requiredTypes; }

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