#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleStatusEffectDatabase", menuName = "Scriptable Objects/Database/BattleStatusEffectDatabase")]
public class BattleStatusEffectDatabase : ScriptableObject, IBattleBattleStatusEffectDatabase, ISerializationCallbackReceiver 
{
    [SerializeField] private List<BattleStatusEffectData> buffData = new List<BattleStatusEffectData>();
    [SerializeField] private List<BattleStatusEffectData> debuffData = new List<BattleStatusEffectData>();
    
    private Dictionary<string, BattleStatusEffectData> idLookUp = new Dictionary<string, BattleStatusEffectData>();

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();

        ProcessListForDeserialize(buffData);
        ProcessListForDeserialize(debuffData);
    }

    private void ProcessListForDeserialize(List<BattleStatusEffectData>? list)
    {
        if (list == null) return;

        foreach (var effectData in list)
        {
            if (effectData == null) { continue; }

            string id = effectData.Id;
            
            if (id == null) { continue; }
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[BattleStatusEffectDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = effectData;
        }
    }

    public BattleStatusEffectData? GetRandomData(System.Random random, BattleStatusEffectType type)
    {
        switch (type)
        {
            case BattleStatusEffectType.BUFF:
                return buffData.Count == 0 ? null : buffData[random.Next(buffData.Count)];
            case BattleStatusEffectType.DEBUFF:
                return debuffData.Count == 0 ? null : debuffData[random.Next(debuffData.Count)];
            case BattleStatusEffectType.ANY:
                if (buffData.Count == 0 && debuffData.Count == 0) return null;
                
                if (buffData.Count == 0) return debuffData[random.Next(debuffData.Count)];
                if (debuffData.Count == 0) return buffData[random.Next(buffData.Count)];
                
                var selectedList = random.NextDouble() < 0.5 ? buffData : debuffData;
                return selectedList[random.Next(selectedList.Count)];
            default:
                throw new InvalidOperationException($"[BattleStatusEffectDatabase] {type} is not supported");
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        HashSet<string> checkSet = new HashSet<string>();

        ValidateList(buffData, checkSet, "Buff", BattleStatusEffectType.BUFF);
        ValidateList(debuffData, checkSet, "Debuff", BattleStatusEffectType.DEBUFF);
    }

    private void ValidateList(List<BattleStatusEffectData>? list, HashSet<string> checkSet, string listName, BattleStatusEffectType expectedType)
    {
        if (list == null) return;

        foreach (var data in list)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[BattleStatusEffectDatabase] {listName} 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (data.Type != expectedType)
            {
                Debug.LogError($"[BattleStatusEffectDatabase] 카테고리 오류: ID '{data.Id}'는 {listName} 리스트에 있지만, 실제 설정된 타입은 {data.Type}입니다!", this);
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[BattleStatusEffectDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}