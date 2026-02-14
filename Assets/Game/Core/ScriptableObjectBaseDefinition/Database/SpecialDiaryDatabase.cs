using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialDiaryDatabase", menuName = "Scriptable Objects/Database/SpecialDiaryDatabase")]
public class SpecialDiaryDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<SpecialDiaryData> availableSpeicalDiaryData;
    private Dictionary<string, SpecialDiaryData> idLookUp = new Dictionary<string, SpecialDiaryData>();

    public SpecialDiaryData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out SpecialDiaryData data)) { return data; }
        
        Debug.LogWarning($"[SpecialDiaryDatabase] There's no SpecialDiaryData for {id}");
        return null;
    }

    public bool TryGetData(DiaryContext context, out SpecialDiaryData speicalDiaryData)
    {
        foreach (SpecialDiaryData element in availableSpeicalDiaryData)
        {
            if (element.AreRequirementsFulfilled(context))
            {
                speicalDiaryData = element;
                return true;
            }
        }

        speicalDiaryData = null;
        return false;
    }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();
        
        foreach(var specialDiaryData in availableSpeicalDiaryData)
        {
            if (specialDiaryData == null) continue;

            string id = specialDiaryData.Id;
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[SpecialDiaryDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = specialDiaryData;
        }
    }

    public void OnBeforeSerialize() { }


#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableSpeicalDiaryData == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var data in availableSpeicalDiaryData)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[SpecialDiaryDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[SpecialDiaryDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}