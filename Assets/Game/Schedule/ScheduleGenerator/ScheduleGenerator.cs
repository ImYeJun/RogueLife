using System;
using System.Collections.Generic;

public class ScheduleGenerator
{
    private ScheduleGenerationContext generationContext;
    private ScheduleSkeletonGenerator skeletonGenerator;
    private SchedulePathCollector pathCollector;
    private NodeGenerator nodeGenerator;
    private SchedulePathCountRule pathCountRule;

    public ScheduleGenerator(ScheduleSkeletonRule skeletonRule, SchedulePathRule pathRule, BattleSystem battleSystem, SchedulePathCountRule pathCountRule)
    {
        generationContext = new ScheduleGenerationContext();
        skeletonGenerator = new ScheduleSkeletonGenerator(skeletonRule);
        pathCollector = new SchedulePathCollector(pathRule, generationContext);
        nodeGenerator = new NodeGenerator(battleSystem);

        this.pathCountRule = pathCountRule;
    }

    public Schedule GenerateSchedule(Random random, ScheduleData scheduleData)
    {
        generationContext.ResetContext();

        Schedule result;
        while (true)
        {
            if (TryGenerateSchedule(random, scheduleData, out result))
            {
                break;
            }
        }

        return result;
    }

    private bool TryGenerateSchedule(Random random, ScheduleData scheduleData, out Schedule schedule)
    {
        ScheduleSkeleton scheduleSkeleton = skeletonGenerator.GenerateSkeleton(random);

        pathCollector.Collect(scheduleSkeleton.StartNode, new SchedulePath());

        ResolveSkeletonNodeType();

        if (IsAppropriatePathCount())
        {
            schedule = ConvertSkeletonToReal(scheduleSkeleton);
            return true;
        }
        else
        {
            schedule = null;
            return false;
        }
    }

    private bool IsAppropriatePathCount()
    {
        return 
            generationContext.CompletePathCount >= pathCountRule.MinCompeletePath &&
            generationContext.CompletePathCount <= pathCountRule.MaxCompletePath;
    }

    private void ResolveSkeletonNodeType()
    {
        throw new NotImplementedException();
    }

    private Schedule ConvertSkeletonToReal(ScheduleSkeleton skeleton)
    {
        throw new NotImplementedException();
    }
}