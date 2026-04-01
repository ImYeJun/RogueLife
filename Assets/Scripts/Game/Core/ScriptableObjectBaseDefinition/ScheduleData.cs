using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleData", menuName = "Scriptable Objects/ScheduleData")]
public class ScheduleData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string scheduleName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite usualBackground;
    [SerializeField] private Sprite battleBackground;
    [SerializeField] private AudioData usualBGM;
    [SerializeField] private AudioData battleBGM;
    [SerializeField] private Sprite choiceIdleSprite;
    [SerializeField] private Sprite choiceHoveringSprite;
    [SerializeField] private EnemyEntity bossEntity;
    [SerializeField] private List<EnemyEntity> availableEliteEnemyEntities;
    [SerializeField] private List<EnemyEntity> availableNormalEnemyEntities;
    [SerializeField] private List<IncidentEntity> availableIncidentEntities;

    public string Id { get => id; }
    public string ScheduleName { get => scheduleName; }
    public string Description { get => description; }
    public Sprite UsualBackground { get => usualBackground; }
    public Sprite BattleBackground { get => battleBackground; }
    public AudioData UsualBGM { get => usualBGM; }
    public AudioData BattleBGM { get => battleBGM; }
    public Sprite ChoiceIdleSprite { get => choiceIdleSprite; }
    public Sprite ChoiceHoveringSprite { get => choiceHoveringSprite; }
    public List<EnemyEntity> AvailableNormalEnemyData { get => availableNormalEnemyEntities; }
    public List<EnemyEntity> AvailableEliteEnemyData { get => availableEliteEnemyEntities; }
    public EnemyEntity BossEntity { get => bossEntity; }
    public List<IncidentEntity> AvailableIncidentEntities { get => availableIncidentEntities; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 1. Normal Enemy 검증
        if (availableNormalEnemyEntities != null)
        {
            for (int i = 0; i < availableNormalEnemyEntities.Count; i++)
            {
                var enemy = availableNormalEnemyEntities[i];
                if (enemy == null)
                {
                    Debug.LogError($"[ScheduleData: {name}] Normal Enemy 리스트의 {i}번 항목이 비어있습니다!", this);
                    continue;
                }
                
                if (enemy.Tier != EnemyTier.NORMAL)
                {
                    Debug.LogError($"[ScheduleData: {name}] Normal 리스트에 잘못된 티어의 적이 있습니다: {enemy.name} (Tier: {enemy.Tier})", this);
                }
            }
        }

        // 2. Elite Enemy 검증
        if (availableEliteEnemyEntities != null)
        {
            for (int i = 0; i < availableEliteEnemyEntities.Count; i++)
            {
                var enemy = availableEliteEnemyEntities[i];
                if (enemy == null)
                {
                    Debug.LogError($"[ScheduleData: {name}] Elite Enemy 리스트의 {i}번 항목이 비어있습니다!", this);
                    continue;
                }

                if (enemy.Tier != EnemyTier.ELITE)
                {
                    Debug.LogError($"[ScheduleData: {name}] Elite 리스트에 잘못된 티어의 적이 있습니다: {enemy.name} (Tier: {enemy.Tier})", this);
                }
            }
        }

        // 3. Boss Data 검증
        if (bossEntity != null)
        {
            if (bossEntity.Tier != EnemyTier.BOSS)
            {
                Debug.LogError($"[ScheduleData: {name}] 설정된 Boss Data가 보스 티어가 아닙니다: {bossEntity.name} (Tier: {bossEntity.Tier})", this);
            }
        }
        else
        {
            Debug.LogWarning($"[ScheduleData: {name}] Boss Data가 비어있습니다.", this);
        }

        // 4. Incident Data 검증 (비어있는 항목 체크)
        if (availableIncidentEntities != null)
        {
            for (int i = 0; i < availableIncidentEntities.Count; i++)
            {
                if (availableIncidentEntities[i] == null)
                {
                    Debug.LogError($"[ScheduleData: {name}] Incident 리스트의 {i}번 항목이 비어있습니다 (Null)!", this);
                }
            }
        }
    }
#endif

}
