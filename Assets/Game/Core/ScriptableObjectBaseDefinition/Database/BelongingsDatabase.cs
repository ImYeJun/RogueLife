using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "BelongingsDatabase", menuName = "Scriptable Objects/Database/BelongingsDatabase")]
public class BelongingsDatabase : ScriptableObject, IFieldBelongingsDatabase, ISerializationCallbackReceiver {
    [SerializeField] private List<BelongingsData> availableBelongingsData;
    private Dictionary<string, BelongingsData> idLookUp = new Dictionary<string, BelongingsData>();

    public BelongingsData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out BelongingsData data)) { return data; }
        
        Debug.LogWarning($"[BelongingsDatabase] There's no BelongingsData for {id}");
        return null;
    }

    public Belongings Materialize(BelongingsData belongingsData) { return Materialize(belongingsData.Id); } 
    public Belongings Materialize(string id)
    {
        if (idLookUp.TryGetValue(id, out BelongingsData data)) { return new Belongings(data); }

        Debug.LogWarning($"[BelongingsDatabase] There's no BelongingsData for {id}");
        return null;
    }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();
        
        foreach(var belongingsData in availableBelongingsData)
        {
            if (belongingsData == null) continue;

            string id = belongingsData.Id;
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[BelongingsDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = belongingsData;
        }
    }

    public void OnBeforeSerialize() { }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableBelongingsData == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var data in availableBelongingsData)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[BelongingsDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[BelongingsDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}