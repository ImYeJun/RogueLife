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

    // Node Exited
    public const int NodeExit_MovePlayer = 10;

    // Next Node Select Requested
    public const int NodeSelect_OpenPanel = 10;
    public const int NodeSelect_NodeButtonBasePriority = 20;
}