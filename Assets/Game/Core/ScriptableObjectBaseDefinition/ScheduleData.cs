using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleData", menuName = "Scriptable Objects/ScheduleData")]
public class ScheduleData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string scheduleName;
    [SerializeField, TextArea] private string description;
    [SerializeField] private Sprite[] usualBackground;
    [SerializeField] private Sprite battleBackground;
    [SerializeField] private AudioClip usualBGM;
    [SerializeField] private AudioClip battleBGM;
    [SerializeField] private Sprite choiceSprite;
    [SerializeField] private EnemyData bossData;
    [SerializeField] private List<EnemyData> availableEliteEnemyData;
    [SerializeField] private List<EnemyData> availableNormalEnemyData;
    [SerializeField] private List<IncidentData> availableIncidentData;

    public string Id { get => id; }
    public string ScheduleName { get => scheduleName; }
    public string Description { get => description; }
    public Sprite[] UsualBackground { get => usualBackground; }
    public Sprite BattleBackground { get => battleBackground; }
    public AudioClip UsualBGM { get => usualBGM; }
    public AudioClip BattleBGM { get => battleBGM; }
    public Sprite ChoiceSprite { get => choiceSprite; }
    public List<EnemyData> AvailableNormalEnemyData { get => availableNormalEnemyData; }
    public List<EnemyData> AvailableEliteEnemyData { get => availableEliteEnemyData; }
    public EnemyData BossData { get => bossData; }
    public List<IncidentData> AvailableIncidentData { get => availableIncidentData; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 1. Normal Enemy 검증
        if (availableNormalEnemyData != null)
        {
            for (int i = 0; i < availableNormalEnemyData.Count; i++)
            {
                var enemy = availableNormalEnemyData[i];
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
        if (availableEliteEnemyData != null)
        {
            for (int i = 0; i < availableEliteEnemyData.Count; i++)
            {
                var enemy = availableEliteEnemyData[i];
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
        if (bossData != null)
        {
            if (bossData.Tier != EnemyTier.BOSS)
            {
                Debug.LogError($"[ScheduleData: {name}] 설정된 Boss Data가 보스 티어가 아닙니다: {bossData.name} (Tier: {bossData.Tier})", this);
            }
        }
        else
        {
            Debug.LogWarning($"[ScheduleData: {name}] Boss Data가 비어있습니다.", this);
        }

        // 4. Incident Data 검증 (비어있는 항목 체크)
        if (availableIncidentData != null)
        {
            for (int i = 0; i < availableIncidentData.Count; i++)
            {
                if (availableIncidentData[i] == null)
                {
                    Debug.LogError($"[ScheduleData: {name}] Incident 리스트의 {i}번 항목이 비어있습니다 (Null)!", this);
                }
            }
        }
    }
#endif

}
