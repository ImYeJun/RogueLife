#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleStatusEffectDatabase : MonoBehaviour, IBattleBattleStatusEffectDatabase 
{
    [SerializeField] private List<BattleStatusEffectEntity> buffEntities = new List<BattleStatusEffectEntity>();
    [SerializeField] private List<BattleStatusEffectEntity> debuffEntities = new List<BattleStatusEffectEntity>();
    
    private Dictionary<string, BattleStatusEffectEntity> idLookUp = new Dictionary<string, BattleStatusEffectEntity>();
    
    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        idLookUp.Clear();

        ProcessList(buffEntities);
        ProcessList(debuffEntities);
    }

    private void ProcessList(List<BattleStatusEffectEntity>? list)
    {
        if (list == null) return;
        

        foreach (var effectEntity in list)
        {
            if (effectEntity == null) { continue; }

            string id = effectEntity.Id;
            
            if (id == null) { continue; }
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[BattleStatusEffectDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = effectEntity;
        }
    }

    public BattleStatusEffectEntity? GetRandomData(System.Random random, BattleStatusEffectType type)
    {
        switch (type)
        {
            case BattleStatusEffectType.BUFF:
                return buffEntities.Count == 0 ? null : buffEntities[random.Next(buffEntities.Count)];
            case BattleStatusEffectType.DEBUFF:
                return debuffEntities.Count == 0 ? null : debuffEntities[random.Next(debuffEntities.Count)];
            case BattleStatusEffectType.ANY:
                if (buffEntities.Count == 0 && debuffEntities.Count == 0) return null;
                
                if (buffEntities.Count == 0) return debuffEntities[random.Next(debuffEntities.Count)];
                if (debuffEntities.Count == 0) return buffEntities[random.Next(buffEntities.Count)];
                
                var selectedList = random.NextDouble() < 0.5 ? buffEntities : debuffEntities;
                return selectedList[random.Next(selectedList.Count)];
            default:
                throw new InvalidOperationException($"[BattleStatusEffectDatabase] {type} is not supported");
        }
    }

    public BattleStatusEffectEntity? GetRandomData(System.Random random, BattleStatusEffectType type, BattleEntityTrait trait)
    {
        var filteredBuffs = buffEntities.Where(e => e.Data.RequiredTraits.HasFlag(trait)).ToList();
        var filteredDebuffs = debuffEntities.Where(e => e.Data.RequiredTraits.HasFlag(trait)).ToList();

        switch (type)
        {
            case BattleStatusEffectType.BUFF:
                return filteredBuffs.Count == 0 ? null : filteredBuffs[random.Next(filteredBuffs.Count)];
            
            case BattleStatusEffectType.DEBUFF:
                return filteredDebuffs.Count == 0 ? null : filteredDebuffs[random.Next(filteredDebuffs.Count)];
            
            case BattleStatusEffectType.ANY:
                if (filteredBuffs.Count == 0 && filteredDebuffs.Count == 0) return null;
                
                if (filteredBuffs.Count == 0) return filteredDebuffs[random.Next(filteredDebuffs.Count)];
                if (filteredDebuffs.Count == 0) return filteredBuffs[random.Next(filteredBuffs.Count)];
                
                var selectedList = random.NextDouble() < 0.5 ? filteredBuffs : filteredDebuffs;
                return selectedList[random.Next(selectedList.Count)];
            
            default:
                throw new InvalidOperationException($"[BattleStatusEffectDatabase] {type} is not supported");
        }
    }

    public BattleStatusEffectData? GetData(string id)
    {
        var entity = idLookUp[id];

        if (entity is null)
        {
            Debug.LogWarning($"[{GetType()}/GetDescription] There's no BattleStatusEffect for {id}");
            return null;
        }
        return entity.Data;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        HashSet<string> checkSet = new HashSet<string>();

        ValidateList(buffEntities, checkSet, "Buff", BattleStatusEffectType.BUFF);
        ValidateList(debuffEntities, checkSet, "Debuff", BattleStatusEffectType.DEBUFF);
    }

    private void ValidateList(List<BattleStatusEffectEntity>? list, HashSet<string> checkSet, string listName, BattleStatusEffectType expectedType)
    {
        if (list == null) return;

        foreach (var entity in list)
        {
            if (entity == null) continue;

            var data = entity?.Data;

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