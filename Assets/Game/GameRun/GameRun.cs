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

    public GameRun(int seed)
    {
        this.seed = seed;
        random = new Random(this.seed);

        player = new Player();
        runDiarySystem = new RunDiarySystem(new SpecialDiaryDatabase()); //TODO : Database SerializeField화 하기
        battleSystem = new BattleSystem();
        scheduleSystem = new ScheduleSystem(random, battleSystem, OnScheduleEnd);
    }

    public GameRun() : this(new Random().Next()){}

    public void StartGame()
    {
        finishedSchedulesCount = 0;
        scheduleSystem.StartSchdule();
    }

    public void OnScheduleEnd()
    {
        if (++finishedSchedulesCount >= Constant.MAX_SCHEDULE_REPETITION)
        {
            runDiarySystem.WriteDiary();
        }
        else
        {
            scheduleSystem.StartSchdule(); 
        }
    }
}