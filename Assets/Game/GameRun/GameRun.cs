using System;

public class GameRun
{
    private Random random;
    private int seed;
    private int finishedSchedulesCount;

    private Player player;
    private BattleSystem battleSystem;
    private ScheduleSystem scheduleSystem;
    private RunDiarySystem runDiarySystem;

    public GameRun(int seed, ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule typeResolveRule)
    {
        this.seed = seed;
        random = new Random(this.seed);

        player = new Player();
        runDiarySystem = new RunDiarySystem(new SpecialDiaryDatabase()); //TODO : Database SerializeField화 하기
        battleSystem = new BattleSystem(random);
        scheduleSystem = new ScheduleSystem(random, skeletonRule, typeResolveRule, battleSystem, OnScheduleEnd);
    }
    public GameRun(ScheduleSkeletonRule skeletonRule, ScheduleNodeTypeResolveRule typeResolveRule) : this(new Random().Next(), skeletonRule, typeResolveRule){}

    public void StartGame()
    {
        finishedSchedulesCount = 0;
        scheduleSystem.StartSchdule(player);
    }

    public void OnScheduleEnd()
    {
        if (++finishedSchedulesCount >= Constant.MAX_SCHEDULE_REPETITION)
        {
            runDiarySystem.WriteDiary();
        }
        else
        {
            scheduleSystem.StartSchdule(player); 
        }
    }
}