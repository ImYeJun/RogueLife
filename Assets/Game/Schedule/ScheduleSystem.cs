using System;

public class ScheduleSystem
{
    private System.Random random;
    private ScheduleGenerator scheduleGenerator;
    private Action<ScheduleHistory> onScheduleEnd;

    private Schedule currentSchedule;
    public Schedule CurrentSchedule { get => currentSchedule; }

    public ScheduleSystem(System.Random random, ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule nodeTypeResolveRule, IEngageBattle battleSystem, Action<ScheduleHistory> onScheduleEnd)
    {
        this.random = random;
        this.onScheduleEnd = onScheduleEnd;

        scheduleGenerator = new ScheduleGenerator(skeletonRule, nodeTypeResolveRule, battleSystem);
    }

    public void StartSchdule(Player player, Action OnScheduleUnsettled)
    {
        //TODO UI에게 일정 선택 요청 보내고 (보낼 때 player을 담아서 보냄), 그 옵저버로 SettleCurrentScheduleData 등록하기, 만약에 조기 종료시 OnScheduleUnsettled실행
    }

    public void SettleCurrentScheduleData(ScheduleData data, Player player)
    {
        currentSchedule = scheduleGenerator.GenerateSchedule(random, data);
        currentSchedule.OnEnd += EndSchedule;

        currentSchedule.EnterStartNode(player);
    }

    public void EndSchedule(ScheduleHistory history)
    {
        currentSchedule.OnEnd -= EndSchedule;
        onScheduleEnd?.Invoke(history);
    }
}
