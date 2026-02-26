#nullable enable

using UnityEngine;

public class GameRunManager : SingletonManager<GameRunManager>
{
    public GameRun? CurrentRun { get; private set; }
    
    [SerializeField] private DatabaseManager databaseManager;
    [SerializeField] private ScheduleSkeletonRule scheduleSkeletonRule;
    [SerializeField] private ScheduleNodeTypeResolveRule scheduleNodeTypeResolveRule;
    //TODO Make Rules as SO

    public void StartNewRun()
    {
        var databases = databaseManager.Databaes;
        var rules = (scheduleSkeletonRule, scheduleNodeTypeResolveRule);

        CurrentRun = new GameRun(rules, databases);
    }
}