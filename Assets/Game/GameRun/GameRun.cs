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
        scheduleSystem.StartSchdule(player, OnScheduleDataUnsettled);
    }

    public void OnScheduleDataUnsettled()
    {
        runDiarySystem.WriteDiary(player.Deck, player.BelongingsBag, false);
    }

    public void OnScheduleEnd(ScheduleHistory history)
    {
        finishedSchedulesCount++;
        runDiarySystem.RecordScheduleHistory(finishedSchedulesCount, history);
        
        if (history.HasEarlyExited)
        {
            //TODO 세이브 기능 만들기 (유저가 Esc 키 눌러서 나간 경우)
            return;
        }
        if (history.HasMentalBroken)
        {
            runDiarySystem.WriteDiary(player.Deck, player.BelongingsBag, false);
            return;
        }

        if (finishedSchedulesCount >= Constant.MAX_SCHEDULE_REPETITION)
        {
            runDiarySystem.WriteDiary(player.Deck, player.BelongingsBag, true);
        }
        else
        {
            scheduleSystem.StartSchdule(player, OnScheduleDataUnsettled); 
        }
    }
}