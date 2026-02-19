using System.Collections.Generic;
using System.Linq;

public class CompositeCardTarget : CardTarget
{
    private List<CardTarget> targets;

    public CompositeCardTarget(List<CardTarget> targets)
    {
        this.targets = targets;
    }

    public List<CardTarget> Targets { get => targets; }

    public T GetTarget<T>() where T : CardTarget
    {
        return targets.OfType<T>().FirstOrDefault();
    }
}