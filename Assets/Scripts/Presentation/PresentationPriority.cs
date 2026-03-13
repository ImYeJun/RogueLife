public static class PresentationPriority
{

    ///* -------------------------
    ///* StartMenu
    ///* -------------------------

    // Start Deck Loaded
    public const int StartDeckLoaded_ViewAppear = 10;
    public const int StartDeckLoaded_BaseDeckPopUp = 20;
    
    // Ready To Start Game
    public const int ReadyToStartGame_FadeIn = 10;
    public const int ReadyToStartGame_SceneTransition = 20;

    ///* -------------------------
    ///* Schedule Selecting
    ///* -------------------------
    
    // Ready To Select Schedule
    public const int ReadyToSelectSchedule_FadeOut = 10;

    //  Schedule Settled
    public const int ScheduleSettled_FadeIn = 10;
    public const int ScheduleSettled_SceneTransition = 20;


    ///* -------------------------
    ///* Schedule
    ///* -------------------------

    // Node Entered
    public const int NodeEnter_MovePlayer = 10;
    public const int NodeEnter_Specific = 20;

    // Node Exited
    public const int NodeExit_MovePlayer = 10;

    // Next Node Select Requested
    public const int NodeSelect_OpenPanel = 10;
    public const int NodeSelect_NodeButtonBasePriority = 20;

    // Battle Engaged
    public const int BattleEngaged_FadeIn = 10;
    public const int BattleEngaged_SceneTransition = 20;

    //Returned From Battle
    public const int ReturnedFromBattle_FadeOut = 10;

    ///* -------------------------
    ///* Battle
    ///* -------------------------
    
    // Battle Started
    public const int BattleStarted_FadeOut = 10;
    public const int BattleStarted_TurnViewShowingDown = 10;

    // Battle Exited
    public const int BattleExited_SceneTransition = 10;
    
    // Player Turn Started
    public const int PlayerTurnStarted_TurnViewShowingDown = 10;
    public const int PlayerTurnStarted_OpenHandDeck = 10;
    public const int PlayerTurnStarted_TurnEndButtonShow = 10;

    //Player Turn Ended
    public const int PlayerTurnEnded_TurnEndButtonDisappear = 10;
    public const int PlayerTurnEnded_CloseHandDeck = 10;
    public const int PlayerTurnEnded_TurnViewDisappearingUp = 10;

    //Enemy Turn Started
    public const int EnemyTurnStarted_TurnViewShowingDown = 10;

    //Enemy Turn Ended
    public const int EnemyTurnEnded_TurnViewDisappearingUp = 10;

    //Cost Consumed
    public const int CostConsumed_CountCost = 10;

    //Cost Restored
    public const int CostRestored_CountCost = 10;

    //Card Drawed
    public const int CardDrawed_HandDeckPresentation = 10;
    public const int CardDrawed_DrawDrawDeckCount = 20;
    
    //Card Discarded
    public const int CardDiscarded_HandDeckPresentation = 10;
    public const int CardDiscarded_DrawGraveDeckCount = 20;

    //Card Restored
    public const int CardRestored_HandDeckPresentation = 10;
    public const int CardRestored_DrawGraveDeckCount = 20;
    public const int CardRestored_DrawDrawDeckCount = 20;
}