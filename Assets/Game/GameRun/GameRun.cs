using System;

public class GameRun
{
    private int seed;
    private int finishedSchedulesCount;

    private Player player;
    private BattleSystem battleSystem;
    private ScheduleSystem scheduleSystem;
    private RunDiarySystem runDiarySystem;

    public GameRun()
    {
        player = new Player();
        runDiarySystem = new RunDiarySystem();
        battleSystem = new BattleSystem(player);
        scheduleSystem = new ScheduleSystem(battleSystem, OnScheduleEnd);

        seed = new Random().Next(0, Int32.MaxValue); //* This is just a stop gap.
    }

    public void StartGame()
    {
        finishedSchedulesCount = 0;
        scheduleSystem.StartSchdule(new ScheduleData()); //* This is just a stop gap since We haven't created test Data.
    }

    public void OnScheduleEnd()
    {
        if (++finishedSchedulesCount >= Constant.MAX_SCHEDULE_REPETITION)
        {
            runDiarySystem.WriteDiary();
        }
        else
        {
            scheduleSystem.StartSchdule(new ScheduleData()); //* This is just a stop gap since We haven't created test Data.
        }
    }
}