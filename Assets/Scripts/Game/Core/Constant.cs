using System.IO;
using UnityEngine;

public static class Constant
{
    // ---------------------------------------------------------
    // * 플레이어 기본 스탯 (Player Base Stats)
    // ---------------------------------------------------------
    public const int INITIAL_MAX_BATTLE_HEALTH = 50;
    public const int INITIAL_MAX_MENTALITY = 100;
    public const int INITIAL_MAX_ACTION_COST = 10;

    // ---------------------------------------------------------
    // * 덱 및 카드 규칙 (Deck & Card Rules)
    // ---------------------------------------------------------
    public const int BASE_MAX_COPIES_PER_CARD = 3;
    public const int BASE_FIRST_TURN_DRAW_COUNT = 5;
    public const int BASE_START_TURN_DRAW_COUNT = 2;
    public const int BASE_MAX_HAND_ZONE_CARD_COUNT = 7;
    public const int BASE_MAX_DECK_CARD_TYPE_COUNT = 24;
    public const int MAX_MAIN_DECK_CARD_TYPE_COUNT = 8;
    // ---------------------------------------------------------
    // * 소지품 규칙 (Belongings Rules)
    // ---------------------------------------------------------
    public const int MAX_MAIN_BELONINGS_COUNT = 3;

    // ---------------------------------------------------------
    // * 스케줄 생성 및 로직 (Schedule & Generation)
    // ---------------------------------------------------------
    public const int MAX_SCHEDULE_REPETITION = 3;
    public const int SELECTING_SCHEDULE_COUNT = 3;
    public const int MAX_SCHEDULE_SKELETON_GENERATION_ATTEMPTS = 10000;
    public const int MAX_SCHEDULE_GENERATION_ATTEMPTS = 10000;
    public const int MAX_SCHEDULE_NODE_RESOLVE_ATTEMPTS = 10000;

    // ---------------------------------------------------------
    // * 시스템 및 데이터 저장 (System & Data Storage)
    // ---------------------------------------------------------
    public static readonly string DIARY_STORE_PATH = Path.Combine(Application.persistentDataPath, "Diary");
    public const string ENCODE_KEY = "God please make this shit works";

    // ---------------------------------------------------------
    // * 전투 및 적 스탯 (Battle Enemy Constants)
    // ---------------------------------------------------------
    public const int MAX_SPAWNED_ENEMY_COUNT = 5;
    public const int MAX_ACTION_CHOOSE_TRY_COUNT = 100;

    // [적 행동 횟수]
    public const int NORMAL_ENEMY_MIN_BEHAVIOUR_COUNT = 1;
    public const int NORMAL_ENEMY_MAX_BEHAVIOUR_COUNT = 2;
    public const int NORMAL_ENEMY_OVER_BEHAVIOUR_COUNT = 3;

    public const int ELITE_ENEMY_MIN_BEHAVIOUR_COUNT = 2;
    public const int ELITE_ENEMY_MAX_BEHAVIOUR_COUNT = 3;
    public const int ELITE_ENEMY_OVER_BEHAVIOUR_COUNT = 4;

    public const int BOSS_ENEMY_BEHAVIOUR_COUNT = 4;

    // [적 멘탈리티 페널티 수치]
    public const int NORMAL_ENEMY_MENTALITY_PENALTY_AMOUNT = 10;
    public const int ELITE_ENEMY_MENTALITY_PENALTY_AMOUNT = 20;
    public const int BOSS_ENEMY_MENTALITY_PENALTY_AMOUNT = 35;

    // [적 시작 턴 수]
    public const int NORMAL_ENEMY_START_TURN_COUNT = 10;
    public const int ELITE_ENEMY_START_TURN_COUNT = 12;
    public const int BOSS_ENEMY_START_TURN_COUNT = 16;
}