using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScheduleDatabase", menuName = "Scriptable Objects/Database/ScheduleDatabase")]
public class ScheduleDatabase : ScriptableObject, ISerializationCallbackReceiver {
    [SerializeField] private List<ScheduleData> availableScheduleData;
    private Dictionary<string, ScheduleData> idLookUp = new Dictionary<string, ScheduleData>();

    public List<ScheduleData> AvailableScheduleData { get => availableScheduleData; }

    public ScheduleData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out ScheduleData data)) { return data; }
        
        Debug.LogWarning($"[ScheduleDatabase] There's no ScheduleData for {id}");
        return null;
    }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();
        
        foreach(var scheduleData in availableScheduleData)
        {
            if (scheduleData == null) continue;

            string id = scheduleData.Id;
            if (id is null) { continue; }
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[ScheduleDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = scheduleData;
        }
    }

    public void OnBeforeSerialize() { }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableScheduleData == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var data in availableScheduleData)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[ScheduleDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[ScheduleDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}