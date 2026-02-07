using System;

public class SchedulePathCollector
{
    private SchedulePathRule rule;
    private ScheduleGenerationContext generationContext;

    public SchedulePathCollector(SchedulePathRule rule, ScheduleGenerationContext generationContext)
    {
        this.rule = rule;
        this.generationContext = generationContext;
    }

    public void Collect(NodeSkeleton startNode, SchedulePath schedulePath)
    {
        throw new NotImplementedException();
    }
}