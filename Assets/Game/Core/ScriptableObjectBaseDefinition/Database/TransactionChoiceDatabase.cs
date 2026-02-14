using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransactionChoiceDatabase", menuName = "Scriptable Objects/Database/TransactionChoiceDatabase")]
public class TransactionChoiceDatabase : ScriptableObject, ISerializationCallbackReceiver {
    [SerializeField] private List<TransactionChoiceData> availableTransactionChoiceData;
    private Dictionary<string, TransactionChoiceData> idLookUp = new Dictionary<string, TransactionChoiceData>();

    public TransactionChoiceData GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out TransactionChoiceData data)) { return data; }
        
        Debug.LogWarning($"[TransactionChoiceDatabase] There's no TransactionChoiceData for {id}");
        return null;
    }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();
        
        foreach(var transactionChoiceData in availableTransactionChoiceData)
        {
            if (transactionChoiceData == null) continue;

            string id = transactionChoiceData.Id;
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[TransactionChoiceDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = transactionChoiceData;
        }
    }

    public void OnBeforeSerialize() { }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableTransactionChoiceData == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        foreach (var data in availableTransactionChoiceData)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[TransactionChoiceDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[TransactionChoiceDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}