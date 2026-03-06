using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TransactionChoiceDatabase : MonoBehaviour, IFieldTransactionChoiceDatabase, ISerializationCallbackReceiver
{
    [Header("Choices by Order")]
    [SerializeField] private List<TransactionChoiceEntity> firstChoices;
    [SerializeField] private List<TransactionChoiceEntity> secondChoices;
    [SerializeField] private List<TransactionChoiceEntity> thirdChoices;

    private Dictionary<TransactionChoiceOrder, Dictionary<string, TransactionChoiceEntity>> lookupTable 
        = new Dictionary<TransactionChoiceOrder, Dictionary<string, TransactionChoiceEntity>>();

    public TransactionChoiceEntity GetEntity(TransactionChoiceOrder order, string id)
    {
        if (lookupTable.TryGetValue(order, out var orderLookup))
        {
            if (orderLookup.TryGetValue(id, out var data))
            {
                return data;
            }
        }
        
        Debug.LogWarning($"[TransactionChoiceDatabase] Data not found. Order: {order}, ID: {id}");
        return null;
    }

    public TransactionChoiceEntity GetEntityAnywhere(string id)
    {
        foreach(var orderLookup in lookupTable.Values)
        {
            if (orderLookup.TryGetValue(id, out var data)) return data;
        }
        
        Debug.LogWarning($"[TransactionChoiceDatabase] There's no TransactionChoiceData for {id} in any order.");
        return null;
    }

    public bool TryGetRandomData(FieldContext context, TransactionChoiceOrder order, out TransactionChoiceEntity choiceData)
    {
        var list = GetList(order);
        var filteredList = list.Where(element => element.IsFulfilled(context)).ToList();

        if (filteredList.Count <= 0) { 
            choiceData = null;
            return false;
        }

        choiceData = filteredList[context.Random.Next(filteredList.Count)];
        return true;
    }

    public List<TransactionChoiceEntity> GetList(TransactionChoiceOrder order)
    {
        return order switch
        {
            TransactionChoiceOrder.FIRST => firstChoices,
            TransactionChoiceOrder.SECOND => secondChoices,
            TransactionChoiceOrder.THIRD => thirdChoices,
            _ => null
        };
    }

    public void OnAfterDeserialize()
    {
        lookupTable.Clear();

        InitializeLookup(TransactionChoiceOrder.FIRST, firstChoices);
        InitializeLookup(TransactionChoiceOrder.SECOND, secondChoices);
        InitializeLookup(TransactionChoiceOrder.THIRD, thirdChoices);
    }

    private void InitializeLookup(TransactionChoiceOrder order, List<TransactionChoiceEntity> list)
    {
        var orderDict = new Dictionary<string, TransactionChoiceEntity>();
        lookupTable[order] = orderDict; // 딕셔너리 등록

        if (list == null) return;

        foreach (var data in list)
        {
            if (data == null) continue;
            
            if (data.Id == null) { continue; }
            if (orderDict.ContainsKey(data.Id))
            {
                Debug.LogWarning($"[TransactionChoiceDatabase] Duplicate ID '{data.Id}' in {order}. Overwritten.");
            }
            orderDict[data.Id] = data;
        }
    }

    public void OnBeforeSerialize() { }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // 전체 리스트 통합 검사를 위한 Set
        HashSet<string> globalCheckSet = new HashSet<string>();

        ValidateList(firstChoices, globalCheckSet, TransactionChoiceOrder.FIRST);
        ValidateList(secondChoices, globalCheckSet, TransactionChoiceOrder.SECOND);
        ValidateList(thirdChoices, globalCheckSet, TransactionChoiceOrder.THIRD);
    }

    private void ValidateList(List<TransactionChoiceEntity> list, HashSet<string> checkSet, TransactionChoiceOrder order)
    {
        if (list == null) return;

        foreach (var data in list)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[TransactionChoiceDatabase] {order} 리스트에 ID가 비어있는 항목이 있습니다!", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[TransactionChoiceDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! ({order} 확인 필요)", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
        }
    }
#endif
}