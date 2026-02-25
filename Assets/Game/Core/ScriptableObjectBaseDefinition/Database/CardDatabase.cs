#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Scriptable Objects/Database/CardDatabase")]
public class CardDatabase : ScriptableObject, IFieldCardDatabase, ISerializationCallbackReceiver {
    [SerializeField] private List<CardData> availableCardData;
    private Dictionary<string, CardData> idLookUp = new Dictionary<string, CardData>();

    public CardData? GetData(string id)
    {
        if (idLookUp.TryGetValue(id, out CardData data)) { return data ;}
        
        Debug.LogWarning($"[CardDatabase] There's no CardData for {id}");
        return null;    
    }

    public Card? Materialize(CardData cardData) { return Materialize(cardData.Id); }
    public Card? Materialize(string id)
    {
        if (idLookUp.TryGetValue(id, out CardData data)) { return new Card(data);}

        Debug.LogWarning($"[CardDatabase] There's no CardData for {id}");
        return null;
    }

    public Card? GetRandomCard(System.Random random, List<CardData>? ignoringCardData = null)
    {
        return GetRandomCard(random, CardRarity.ANY, CardType.ANY, CardAttribute.ANY, ignoringCardData);
    }
    public Card? GetRandomCard(System.Random random, CardRarity rarity, CardType type, CardAttribute attribute, List<CardData>? ignoringCardData = null)
    {
        return GetRandomCard(random, rarity, rarity, type, attribute, ignoringCardData);
    }
    public Card? GetRandomCard(System.Random random, CardRarity lowestRarity, CardRarity highestRarity, CardType type, CardAttribute attribute, List<CardData>? ignoringCardData = null)
    {
        lowestRarity = lowestRarity == CardRarity.ANY ? CardRarity.COMMON : lowestRarity;
        highestRarity = highestRarity == CardRarity.ANY ? CardRarity.LEGENDARY : highestRarity;

        var filterdCardData = availableCardData.Where(data =>
            lowestRarity <= data.Rarity && data.Rarity <= highestRarity &&
            (type == CardType.ANY || data.Type == type) &&
            (attribute == CardAttribute.ANY || data.Attribute == attribute) &&
            ((ignoringCardData is null) || !ignoringCardData.Contains(data))
        ).ToList();

        if (filterdCardData.Count <= 0) { 
            Debug.LogWarning($"There's no cards that fullfills the condition (lowestRarity : {lowestRarity}, highestRarity : {highestRarity}, attribute : {attribute}, type : {type})");
            return null;
        }
        var selectedData = filterdCardData[random.Next(filterdCardData.Count)];
        return Materialize(selectedData);
    }

    public void OnBeforeSerialize() { }

    public void OnAfterDeserialize()
    {
        idLookUp.Clear();

        foreach (var cardData in availableCardData)
        {
            if (cardData == null) { continue; }

            string id = cardData.Id;
            if (idLookUp.ContainsKey(id))
            {
                Debug.LogWarning($"[CardDatabase] Duplicate data detected: {id}. the previous data was overwritten.");
            }

            idLookUp[id] = cardData;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (availableCardData == null) { return; }
        HashSet<string> checkSet = new HashSet<string>();

        int i = 0;
        foreach (var data in availableCardData)
        {
            if (data == null) continue;

            if (string.IsNullOrEmpty(data.Id))
            {
                Debug.LogError($"[CardDatabase] 데이터 리스트에 ID가 비어있는 항목이 있습니다! {i}", this);
                continue;
            }

            if (checkSet.Contains(data.Id))
            {
                Debug.LogError($"[CardDatabase] 치명적 오류: ID '{data.Id}'가 중복되었습니다! 수정해주세요.", this);
            }
            else
            {
                checkSet.Add(data.Id);
            }
            i++;
        }
    }
#endif
}