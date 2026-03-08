using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDatabase : MonoBehaviour, IRunDiaryEnemyDatabaseContext 
{
    [SerializeField] List<EnemyEntity> availableEnemies;
    private Dictionary<string, EnemyEntity> idLookUp = new Dictionary<string, EnemyEntity>();

    public List<EnemyEntity> AvailableEnemies => idLookUp.Values.ToList();

    private void Awake()
    {
        InitializeLookUp();
    }

    private void InitializeLookUp()
    {
        idLookUp.Clear();

        foreach(var enemyData in availableEnemies)
        {
            if (enemyData == null) continue;

            string id = enemyData.Id;
            
            if (string.IsNullOrEmpty(id)) { continue; } 
            
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[EnemyDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = enemyData;
        }
    }

    public EnemyData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out EnemyEntity entity)) { return entity.Data; }

        Debug.LogWarning($"[EnemyDatabase] There's no EnemyData for {id}");
        return null;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableEnemies == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var data in availableEnemies)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[EnemyDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[EnemyDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}