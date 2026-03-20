using System.Collections.Generic;
using UnityEngine;

public class SpecialDiaryDatabase : MonoBehaviour
{
    [SerializeField] private List<SpecialDiaryEntity> availableSpeicalDiaryEntities;
    private Dictionary<string, SpecialDiaryEntity> idLookUp = new Dictionary<string, SpecialDiaryEntity>();

    private void Awake()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        idLookUp.Clear();
        
        if (availableSpeicalDiaryEntities == null) return;

        foreach(var specialDiaryData in availableSpeicalDiaryEntities)
        {
            if (specialDiaryData == null) continue;

            string id = specialDiaryData.Id;

            if (string.IsNullOrEmpty(id)) { continue; }
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[SpecialDiaryDatabase/InitializeDictionary] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = specialDiaryData;
        }
    }

    public SpecialDiaryData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out SpecialDiaryEntity entity)) { return entity.Data; }
        
        Debug.LogWarning($"[SpecialDiaryDatabase/GetData] There's no SpecialDiaryData for {id}");
        return null;
    }

    public bool TryGetData(DiaryContext context, out SpecialDiaryData speicalDiaryData)
    {
        if (availableSpeicalDiaryEntities == null)
        {
            speicalDiaryData = null;
            return false;
        }

        foreach (SpecialDiaryEntity element in availableSpeicalDiaryEntities)
        {
            if (element.AreRequirementsFulfilled(context))
            {
                speicalDiaryData = element.Data;
                return true;
            }
        }

        speicalDiaryData = null;
        return false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableSpeicalDiaryEntities == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var data in availableSpeicalDiaryEntities)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[SpecialDiaryDatabase/OnValidate] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[SpecialDiaryDatabase/OnValidate] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}