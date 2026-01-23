using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private string description;
    [SerializeField] private string relfectionActivatedDescription;
    [SerializeField] private CardType type;
    [SerializeField] private CardAttribute attribute;
    [SerializeField] private CardRarity rarity;
    [SerializeField] private int actionCost;
    [SerializeField] private List<BattleAction> actions;
    [SerializeField] private List<BattleAction> reflectedActions;

    public string CardName { get => cardName; }
    public string Description { get => description; }
    public string RelfectionActivatedDescription { get => relfectionActivatedDescription; }
    public CardType Type { get => type; }
    public CardAttribute Attribute { get => attribute; }
    public CardRarity Rarity { get => rarity; }
    public int ActionCost { get => actionCost; }

    public void Execute(BattleContext context)
    {
        foreach (BattleAction action in actions)
        {
            action.Execute(context);
        }
    }

    public void ExecuteReflection(BattleContext context)
    {
        foreach (BattleAction action in reflectedActions)
        {
            action.Execute(context);
        }
    }
}