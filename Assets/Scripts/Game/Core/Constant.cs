using System.IO;
using UnityEngine;

public static class Constant{
    public const int INITIAL_MAX_BATTLE_HEALTH = 100;
    public const int INITIAL_MAX_MENTALITY = 50;
    public const int INITIAL_MAX_ACTION_COST = 10;
    public const int BASE_MAX_COPIES_PER_CARD = 3;
    public const int BASE_FIRST_TURN_DRAW_COUNT = 5;
    public const int BASE_START_TURN_DRAW_COUNT = 2;
    public const int BASE_MAX_HAND_ZONE_CARD_COUNT = 7;
    public const int BASE_MAX_DECK_CARD_TYPE_COUNT = 24;
    public const int MAX_MAIN_DECK_CARD_TYPE_COUNT = 8;
    public const int MAX_SCHEDULE_REPETITION = 3;
    public static readonly string DIARY_STORE_PATH = Path.Combine(Application.persistentDataPath, "Diary");
    public const string ENCODE_KEY = "God please make this shit works";
    public const int MAX_SCHEDULE_SKELETON_GENERATION_ATTEMPTS = 10000;
    public const int MAX_SCHEDULE_GENERATION_ATTEMPTS = 10000;
    public const int MAX_SCHEDULE_NODE_RESOLVE_ATTEMPTS = 10000;

    public const int SELECING_SCHEUDLE_COUNT = 3;

    //* Battle Enemy Constants
    public const int MAX_SPAWNED_ENEMY_COUNT = 5;
    public const int MAX_ACTION_CHOOSE_TRY_COUNT = 100;

    public const int NORMAL_ENEMY_MIN_BEHAVIOUR_COUNT = 1;
    public const int NORMAL_ENEMY_MAX_BEHAVIOUR_COUNT = 2;
    public const int NORMAL_ENEMY_OVER_BEHAVIOUR_COUNT = 3;
    public const int ELITE_ENEMY_MIN_BEHAVIOUR_COUNT = 2;
    public const int ELITE_ENEMY_MAX_BEHAVIOUR_COUNT = 3;
    public const int ELITE_ENEMY_OVER_BEHAVIOUR_COUNT = 4;
    public const int BOSS_ENEMY_BEHAVIOUR_COUNT = 5;

    public const int NORMAL_ENEMY_MENTALITY_PENALTY_AMOUNT = 10;
    public const int ELITE_ENEMY_MENTALITY_PENALTY_AMOUNT = 20;
    public const int BOSS_ENEMY_MENTALITY_PENALTY_AMOUNT = 35;

    public const int NORMAL_ENEMY_START_PHASE_COUNT = 5;
    public const int ELITE_ENEMY_START_PHASE_COUNT = 6;
    public const int BOSS_ENEMY_START_PHASE_COUNT = 8;

}