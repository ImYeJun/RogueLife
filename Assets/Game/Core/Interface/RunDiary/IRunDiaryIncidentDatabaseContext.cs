using System.Collections.Generic;

public interface IRunDiaryIncidentDatabaseContext
{
    public List<IncidentData> AvailableIncidents { get; }
}