using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Scriptable Objects/CardDatabase")]
public class CardDatabase : ScriptableObject {
    [SerializeField] private List<CardData> cardData;

    public Card GetRandomCard(System.Random random)
    {
        return GetRandomCard(random, CardAttribute.ANY, CardType.ANY);
    }

    public Card GetRandomCard(System.Random random, CardAttribute attribute, CardType type)
    {
        var candidates = cardData.Where(data => 
            (attribute == CardAttribute.ANY || (attribute & data.Attribute) == attribute) && 
            (type == CardType.ANY || (type & data.Type) == type)
        ).ToList();

        if (candidates.Count == 0) { 
            Debug.LogWarning($"There's no cards that fullfills the condition (attribute : {attribute}, type : {type})");
            return null;
        }

        var selectedData = candidates[random.Next(candidates.Count)];

        return new Card(selectedData);
    }
}