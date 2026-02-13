using System;

public class ScheduleSystem
{
    private System.Random random;
    private ScheduleGenerator scheduleGenerator;
    private Action onScheduleEnd;

    private Schedule currentSchedule;
    public Schedule CurrentSchedule { get => currentSchedule; }

    public ScheduleSystem(System.Random random, ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule nodeTypeResolveRule, IEngageBattle battleSystem, Action onScheduleEnd)
    {
        this.random = random;
        this.onScheduleEnd = onScheduleEnd;

        scheduleGenerator = new ScheduleGenerator(skeletonRule, nodeTypeResolveRule, battleSystem);
    }

    public void StartSchdule(Player player)
    {
        //TODO UI에게 일정 선택 요청 보내고 (보낼 때 player을 담아서 보냄), 그 옵저버로 SettleCurrentScheduleData 등록하기
    }

    public void SettleCurrentScheduleData(ScheduleData data, Player player)
    {
        currentSchedule = scheduleGenerator.GenerateSchedule(random, data);
        currentSchedule.OnEnd += EndSchedule;

        currentSchedule.EnterStartNode(player);
    }

    public void EndSchedule()
    {
        currentSchedule.OnEnd -= EndSchedule;
        onScheduleEnd?.Invoke();
    }
}
