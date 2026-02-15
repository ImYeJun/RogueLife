using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransactionChoiceDatabase", menuName = "Scriptable Objects/Database/TransactionChoiceDatabase")]
public class TransactionChoiceDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    [Header("First Choices")]
    [SerializeField] private List<TransactionChoiceData> availableTransactionFirstChoiceData;
    private Dictionary<string, TransactionChoiceData> firstChoiceLookUp = new Dictionary<string, TransactionChoiceData>();

    [Header("Second Choices")]
    [SerializeField] private List<TransactionChoiceData> availableTransactionSecondChoiceData;
    private Dictionary<string, TransactionChoiceData> secondChoiceLookUp = new Dictionary<string, TransactionChoiceData>();

    [Header("Third Choices")]
    [SerializeField] private List<TransactionChoiceData> availableTransactionThirdChoiceData;
    private Dictionary<string, TransactionChoiceData> thirdChoiceLookUp = new Dictionary<string, TransactionChoiceData>();

    public List<TransactionChoiceData> FirstChoices => availableTransactionFirstChoiceData;
    public List<TransactionChoiceData> SecondChoices => availableTransactionSecondChoiceData;
    public List<TransactionChoiceData> ThirdChoices => availableTransactionThirdChoiceData;

    public TransactionChoiceData GetData(string id)
    {
        if (firstChoiceLookUp.TryGetValue(id, out var data)) return data;
        if (secondChoiceLookUp.TryGetValue(id, out data)) return data;
        if (thirdChoiceLookUp.TryGetValue(id, out data)) return data;

        Debug.LogWarning($"[TransactionChoiceDatabase] There's no TransactionChoiceData for {id}");
        return null;
    }

    public TransactionChoiceData GetFirstChoiceData(string id) => GetFromLookup(id, firstChoiceLookUp);
    public TransactionChoiceData GetSecondChoiceData(string id) => GetFromLookup(id, secondChoiceLookUp);
    public TransactionChoiceData GetThirdChoiceData(string id) => GetFromLookup(id, thirdChoiceLookUp);

    private TransactionChoiceData GetFromLookup(string id, Dictionary<string, TransactionChoiceData> lookup)
    {
        if (lookup.TryGetValue(id, out var data)) return data;
        return null;
    }

    public void OnAfterDeserialize()
    {
        firstChoiceLookUp.Clear();
        secondChoiceLookUp.Clear();
        thirdChoiceLookUp.Clear();

        RegisterListToLookup(availableTransactionFirstChoiceData, firstChoiceLookUp, "First");
        RegisterListToLookup(availableTransactionSecondChoiceData, secondChoiceLookUp, "Second");
        RegisterListToLookup(availableTransactionThirdChoiceData, thirdChoiceLookUp, "Third");
    }

    private void RegisterListToLookup(List<TransactionChoiceData> list, Dictionary<string, TransactionChoiceData> lookup, string listName)
    {
        if (list == null) return;

        foreach (var data in list)
        {
            if (data == null) continue;

            string id = data.Id;
            if (lookup.ContainsKey(id))
            {
                Debug.LogWarning($"[TransactionChoiceDatabase] Duplicate data detected in {listName}: {id}. The previous data was overwritten.");
            }
            lookup[id] = data;
        }
    }

    public void OnBeforeSerialize() { }

#if UNITY_EDITOR
    private void OnValidate()
    {
        HashSet<string> checkSet = new HashSet<string>();

        ValidateList(availableTransactionFirstChoiceData, checkSet, "FirstChoice");
        ValidateList(availableTransactionSecondChoiceData, checkSet, "SecondChoice");
        ValidateList(availableTransactionThirdChoiceData, checkSet, "ThirdChoice");
    }

    private void ValidateList(List<TransactionChoiceData> list, HashSet<string> checkSet, string listName)
    {
        if (list == null) return;

        foreach (var data in list)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[TransactionChoiceDatabase] {listName} 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[TransactionChoiceDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! ({listName} 목록 또는 다른 목록과 중복)", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}