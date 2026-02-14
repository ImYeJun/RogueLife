using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "IncidentDatabase", menuName = "Scriptable Objects/Database/IncidentDatabase")]
public class IncidentDatabase : ScriptableObject, IRunDiaryIncidentDatabaseContext, ISerializationCallbackReceiver {
    [SerializeField] private List<IncidentData> availableIncidents;
    private Dictionary<string, IncidentData> idLookUp = new Dictionary<string, IncidentData>();

    public IncidentData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out IncidentData data)) { return data; }
        
        Debug.LogWarning($"[IncidentDatabase] There's no IncidentData for {id}");
        return null;
    }

    public List<IncidentData> AvailableIncidents => idLookUp.Values.ToList();
    
    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();
        
        foreach(var incidentData in availableIncidents)
        {
            if (incidentData == null) continue;

            string id = incidentData.Id;
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[IncidentDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = incidentData;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableIncidents == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var data in availableIncidents)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[IncidentDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[IncidentDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}