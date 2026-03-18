using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] private string id;
    [SerializeField] private string cardName;
    [SerializeField] private Sprite background;
    [SerializeField, TextArea] private string description;
    [SerializeField] private List<string> relatedStatusEffectIds;
    [SerializeField, TextArea] private string relfectionAppliedDescription;
    [SerializeField] private List<string> reflectionRelatedStatusEffectIds;
    [SerializeField] private CardType type;
    [SerializeField] private CardAttribute attribute;
    [SerializeField] private CardRarity rarity;
    [SerializeField] private int actionCost;

    public string Id { get => id; }
    public string CardName { get => cardName; }
    public string Description { get => description; }
    public string RelfectionActivatedDescription { get => relfectionAppliedDescription; }
    public CardType Type { get => type; }
    public CardAttribute Attribute { get => attribute; }
    public CardRarity Rarity { get => rarity; }
    public int ActionCost { get => actionCost; }
    public Sprite Background { get => background; }
    public List<string> RelatedStatusEffectIds { get => relatedStatusEffectIds; }
    public List<string> ReflectionRelatedStatusEffectIds { get => reflectionRelatedStatusEffectIds; }
}