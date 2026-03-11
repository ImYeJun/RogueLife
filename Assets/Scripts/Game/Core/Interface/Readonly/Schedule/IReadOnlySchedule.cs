using System.Collections.Generic;

public interface IReadOnlySchedule
{
    public ScheduleData Data { get; }
    public IReadOnlyDictionary<int, List<Node>> Map { get; }
    public Node CurrentNode { get; }
}