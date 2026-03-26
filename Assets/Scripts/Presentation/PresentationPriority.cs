public static class PresentationPriority
{
    //Game Ended
    public const int RunEnded_StopBgm = 10;
    public const int RunEnded_SceneTransition = 20;

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

    // Went To Bed
    public const int WentToBed_FadeOut = 10;


    ///* -------------------------
    ///* Schedule
    ///* -------------------------

    // Node Entered
    public const int NodeEnter_StageSet = 10;
    public const int NodeEnter_MovePlayer = 20;
    public const int NodeEnter_Specific = 30;

    // Node Exited
    public const int NodeExit_MovePlayer = 10;
    public const int NodeExit_StageUnset = 20;

    // Next Node Select Requested
    public const int NodeSelect_OpenPanel = 10;
    public const int NodeSelect_NodeButtonBasePriority = 20;

    //Incident Select Requested
    public const int IncidentSelectRequested_ChoiceAppear = 10;

    //Transaction Select Requested
    public const int TransactionSelectRequested_ChoiceAppear = 10;

    // Battle Engaged
    public const int BattleEngaged_StopBgm = 10;
    public const int BattleEngaged_FadeIn = 20;
    public const int BattleEngaged_SceneTransition = 30;

    //Returned From Battle
    public const int ReturnedFromBattle_FadeOut = 10;
    public const int ReturnedFromBattle_EnemyLine = 20;

    //Battle Reward Select Requested
    public const int BattleRewardSelectRequested_Open = 10;
    public const int BattleRewardSelectRequested_Close = 20;

    //Collection Update
    public const int CollectionUpdate = 10;

    //Player Hurt
    public const int PlayerHurt = 10;

    //Player Healed
    public const int PlayerHealed = 10;

    //Schedule Cleared
    public const int ScheduleCleared_StopBgm = 10;
    public const int ScheduleCleared_SceneTransition = 20;


    ///* -------------------------
    ///* Battle
    ///* -------------------------
    
    // Battle Started
    public const int BattleStarted_FadeOut = 10;
    public const int BattleStarted_PlayBgm = 20;

    // Battle Exited
    public const int BattleExited_StopBgm = 10;
    public const int BattleExited_PlaySFX = 20;
    public const int BattleExited_FadeOut = 30;
    public const int BattleExited_SceneTransition = 40;
    
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
    public const int EnemyTurnEnded_ActionClear = 10;
    public const int EnemyTurnEnded_TurnViewDisappearingUp = 0;

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

    //Card Effect Executed
    public const int CardEffectExecuted_CasterAction = 10;

    //Card Trigger Resolved
    public const int CardTriggerResolved_ExtinguishCardView = 10;

    //Enemy Action Executed
    public const int EnemyActionExecuted_ActorAction = 10;

    //Battle Status Effect Applied
    public const int BattleStatusEffectAplied_IconAction = 10;

    //Battle Status Effect Executed
    public const int BattleStatusEffectExecuted_IconAction = 10;

    //Battle Status Effect Removed
    public const int BattleStatusEffectRemoved_IconAction = 10;

    //Enemy Action Planned 
    public const int EnemyActionPlanned_BaseIconAction = 10;

    //Enemy Action Executed
    public const int EnemyActionExecuted_IconAction = 10;

    //Enemy Action Removed
    public const int EnemyActionRemoved_IconAction = 10;

    //Enemy Hurt
    public const int EnemyHurt_EnemyPresentation = 10;
    public const int EnemyHurt_HealthBarPresentation = 10;

    //Enemy Heal
    public const int EnemyHeal_HealthBarPresentation = 10;
    
    //Enemy Died
    public const int EnemyDied_DiePresentation = 10;

    //Enemy Spawned
    public const int EnemySpawned_PositionSet = 10;

    //Enemy Removed
    public const int EnemyRemoved_PositionSet = 10;

    //Player Hurt
    public const int PlayerHurt_PlayerPresentation = 10;

    //Player Heal
    public const int PlayerHeal_HealthBarPresentation = 10;

    //Phase Increased
    public const int PhaseIncreased_UpdateView = 10;

    //Phase Decreased
    public const int PhaseDecreased_UpdateView = 10;

    //Card Cost Changed
    public const int CardCostChanged_UpdateView = 10;

    //Card Reflection Changed
    public const int CardReflectionChanged_UpdateView = 10;

    ///* -------------------------
    ///* Write Diary
    ///* -------------------------
    
    // Diary Written
    public const int DiaryWritten_FadeIn = 10;
    public const int DiaryWritten_PlayBgm = 20;
    public const int DiaryWritten_CommonPartPresentation = 30;
    public const int DiaryWritten_SpecialPartPresentation = 40;

    // Return To Main Menu Requested
    public const int ReturnToMainMenuRequested_FadeOut = 10;
    public const int ReturnToMainMenuRequested_SceneTransition = 20;
}