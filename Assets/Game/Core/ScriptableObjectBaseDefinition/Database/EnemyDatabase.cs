using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Scriptable Objects/Database/EnemyDatabase")]
public class EnemyDatabase : ScriptableObject, IRunDiaryEnemyDatabaseContext, ISerializationCallbackReceiver {
    [SerializeField] List<EnemyData> availableEnemies;
    private Dictionary<string, EnemyData> idLookUp = new Dictionary<string, EnemyData>();

    public List<EnemyData> AvailableEnemies => idLookUp.Values.ToList();

    public EnemyData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out EnemyData data)) { return data; }

        Debug.LogWarning($"[EnemyDatabase] There's no EnemyData for {id}");
        return null;
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();

        foreach(var enemyData in availableEnemies)
        {
            if (enemyData == null) continue;

            string id = enemyData.Id;
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[EnemyDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = enemyData;
        }
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