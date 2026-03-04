#nullable enable

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class BelongingsDatabase : MonoBehaviour, IFieldBelongingsDatabase {
    [SerializeField] private List<BelongingsEntity> availableBelongingsEntities = new List<BelongingsEntity>();
    private Dictionary<string, BelongingsEntity> idLookUp;

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        idLookUp = new Dictionary<string, BelongingsEntity>();
        
        foreach(var belongingsEntity in availableBelongingsEntities)
        {
            if (belongingsEntity == null) continue;

            string id = belongingsEntity.Data.Id;

            if (id is null) { continue; }
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[BelongingsDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = belongingsEntity;
        }
    }

    public BelongingsEntity? GetEntity(string id)
    {
        if (idLookUp.TryGetValue(id, out BelongingsEntity entity)) { return entity; }
        
        Debug.LogWarning($"[BelongingsDatabase] There's no BelongingsData for {id}");
        return null;
    }

    public Belongings? GetRandomBelongings(System.Random random, List<Belongings>? ignoring = null)
    {
        var availableData = availableBelongingsEntities;

        if (ignoring is not null)
        {
            var ignoringEntities = ignoring.Select(belongings => belongings.Entity);
            availableData = availableData.Where(entity => !ignoringEntities.Contains(entity)).ToList();
        }

        if (availableData.Count == 0) { return null; }

        var selecetdData = availableData[random.Next(availableData.Count)];
        return Materialize(selecetdData);
    }

    public Belongings? Materialize(BelongingsEntity entity) { return Materialize(entity.Data.Id); } 
    public Belongings? Materialize(string id)
    {
        if (idLookUp.TryGetValue(id, out BelongingsEntity entity)) { return new Belongings(entity); }

        Debug.LogWarning($"[BelongingsDatabase] There's no BelongingsData for {id}");
        return null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableBelongingsEntities == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var entity in availableBelongingsEntities)
        {
            if (entity == null) continue;

            if (string.IsNullOrEmpty(entity.Data.Id))
            {
                Debug.LogError($"[BelongingsDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(entity.Data.Id))
            {
                Debug.LogError($"[BelongingsDatabase] 치명적 오류: ID '{entity.Data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(entity.Data.Id);
            }
        }
    }
#endif
}