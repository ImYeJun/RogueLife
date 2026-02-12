using System.Collections.Generic;

public class CompositeCardTarget : CardTarget
{
    private List<CardTarget> targets;

    public CompositeCardTarget(List<CardTarget> targets)
    {
        this.targets = targets;
    }

    public List<CardTarget> Targets { get => targets; }
}